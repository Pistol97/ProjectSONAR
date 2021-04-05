﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public GameObject m_item;//획득한 아이템
    public Image m_itemImage;//아이템 이미지

    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private GameObject go_CountImage;

    private void SetColor(float alpha)
    {
        Color color = m_itemImage.color;
        color.a = alpha;
        m_itemImage.color = color;
    }

    public void AddItem(GameObject item, int count = 1)
    {
        m_item = item;

        go_CountImage.SetActive(true);

        SetColor(1);
    }
}
