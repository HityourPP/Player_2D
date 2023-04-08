using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveBag : MonoBehaviour,IDragHandler
{
    RectTransform currentRect;//����������
    private void Awake()
    {
        currentRect = GetComponent<RectTransform>();
    }
    public void OnDrag(PointerEventData eventData)//ʵ�ֱ���ҳ����ƶ�
    {
        currentRect.anchoredPosition += eventData.delta;//anchoredPosition�������ĵ㣬eventData.deltaΪ������΢�ƶ�
    }
}
