﻿using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("이동 관련 변수")]
    [Tooltip("기본이동속도")]//유니티 인스펙터에서 아래 변수에 마우스를 올렸을때 문자열이 뜸
    public float MoveSpeed = 2.0f; //이동속도
    public float DirectionRotateSpeed = 100.0f;//이동방향을 변경하기 위한 속도
    public float BodyRotateSpeed = 2.0f;//몸통의 방향을 변경하기 위한 속도

    [Range(0.01f, 5.0f)]//밑의 변수는 Range()안의 범위의 수만 가질 수 있다
    public float VelocityChangeSpeed = 0.01f;//속도가 변경되기 위한 속도(0이되면 안됌)
    private Vector3 CurrentVelocity = Vector3.zero;
    private Vector3 MoveDirection = Vector3.zero;//이동방향
    private CharacterController myCharacterController = null;
    private CollisionFlags collisionFlags = CollisionFlags.None;
    private float gravity = 9.8f; //중력값

    [SerializeField]
    private Transform cameraTransform;//카메라 transform 컴포넌트
    [SerializeField]
    private SelectionManager selectionMgr;

    // Start is called before the first frame update
    void Start()
    {
        myCharacterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //이동
        NewMove();
        //회전
        BodyDirectionChange();
        //중력적용
        ApplyGravity();

    }
    /// <summary>
    /// 캐릭터 이동 함수
    /// </summary>
    private void NewMove()
    {
        float vertical = Input.GetAxis("Vertical");//키보드의 위,아래,w,s, -1~1
        float horizontal = Input.GetAxis("Horizontal");//키보드의 좌,우,a,d, -1~1
        Vector3 direction=new Vector3(horizontal,0,vertical);

        Vector3 movedis = cameraTransform.rotation * direction;
        MoveDirection = new Vector3(movedis.x, MoveDirection.y, movedis.z);
        myCharacterController.Move(MoveDirection * MoveSpeed * Time.deltaTime);
    }
    /// <summary>
    /// 캐릭터의 이동 관련 변수 화면에 표시
    /// </summary>
    private void OnGUI()
    {
        //충돌정보
        GUILayout.Label("충돌 :" + collisionFlags.ToString());

        GUILayout.Label("현재 속도 :" + GetVelocitySpeed().ToString());

        if (myCharacterController != null && myCharacterController.velocity != Vector3.zero)
        {
            //현재 내 캐릭터가 이동하는 방향(+크기)
            GUILayout.Label("current Velocity Vector :" + myCharacterController.velocity.ToString());
            //현재 내 속도
            GUILayout.Label("current Velocity Magnitude :" + myCharacterController.velocity.magnitude.ToString());
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawRay(transform.position, transform.forward * 10);
    }
    /// <summary>
    /// 현재 내 캐릭터의 이속을 얻어온다.
    /// </summary>
    /// <returns></returns>
    public float GetVelocitySpeed()
    {
        //멈춰있다면
        if (myCharacterController.velocity == Vector3.zero)
        {
            //현재 속도 = 0
            CurrentVelocity = Vector3.zero;
        }
        else
        {
            Vector3 goalVelocity = myCharacterController.velocity;
            goalVelocity.y = 0.0f;
            CurrentVelocity = Vector3.Lerp(CurrentVelocity, goalVelocity, VelocityChangeSpeed * Time.deltaTime);
        }

        //currentVelocity의 크기 리턴
        return CurrentVelocity.magnitude;
    }
    /// <summary>
    /// 몸통의 방향을 이동방향으로 바꾼다.
    /// </summary>
    void BodyDirectionChange()
    {
        //transform.LookAt(selectionMgr.GetFront());
        Vector3 targetDirection = selectionMgr.GetFront();
        Vector3 front = new Vector3(targetDirection.x, 0, targetDirection.z);
        Quaternion rotation = Quaternion.LookRotation(front.normalized);
        transform.rotation = rotation;
    }
    /// <summary>
    /// 중력 적용
    /// </summary>
    void ApplyGravity()
    {
        //CollidedBelow가 세팅되었다면(바닥에 붙었다면)
        if ((collisionFlags & CollisionFlags.CollidedBelow) != 0)
        {
            MoveDirection.y = 0.0f;
        }
        else
        {
            MoveDirection.y -= gravity * Time.deltaTime;
        }
    }
}
