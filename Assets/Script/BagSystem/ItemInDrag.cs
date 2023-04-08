using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Inventory bag;
   
    private Transform originParent;
    private int currentItemID;
    public void OnBeginDrag(PointerEventData eventData)//开始拖拽
    {
        originParent = transform.parent;
        currentItemID = originParent.GetComponent<Slot>().slotID;
        transform.SetParent(transform.parent.parent);
        transform.position = eventData.position;//拖拽时物体的位置与鼠标位置相同
        transform.GetComponent<CanvasGroup>().blocksRaycasts = false;//关闭射线遮挡
    }
    public void OnDrag(PointerEventData eventData)//拖拽过程
    {
        transform.position = eventData.position;
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
    }
    public void OnEndDrag(PointerEventData eventData)//结束拖拽
    {
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            if(eventData.pointerCurrentRaycast.gameObject.name=="Item Image")
            {
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position;

                var temp = bag.items[currentItemID];//临时变量存储原来的物品ID
                //将鼠标点击的对应的位置的物品ID赋值到要拖拽的物品的ID上                                         
                bag.items[currentItemID] = bag.items[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID];
                bag.items[currentItemID] = temp;

                eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originParent.position;
                eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(originParent);//将二者子对象互换
                transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
            if (eventData.pointerCurrentRaycast.gameObject.name == "Slot(Clone)")
            {
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
                //物品栏为空时
                bag.items[eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID] = bag.items[currentItemID];
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotID != currentItemID)
                {
                    bag.items[currentItemID] = null;
                }
                transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
            transform.SetParent(originParent.transform);
            transform.position = originParent.transform.position;
            transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else
        {
            transform.SetParent(originParent.transform);
            transform.position = originParent.transform.position;
            transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }
}
