using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveBag : MonoBehaviour,IDragHandler
{
    RectTransform currentRect;//画布坐标名
    private void Awake()
    {
        currentRect = GetComponent<RectTransform>();
    }
    public void OnDrag(PointerEventData eventData)//实现背包页面的移动
    {
        currentRect.anchoredPosition += eventData.delta;//anchoredPosition画布中心点，eventData.delta为鼠标的轻微移动
    }
}
