using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Inventory bag;
   
    private Transform originParent;
    private int currentItemID;
    public void OnBeginDrag(PointerEventData eventData)//��ʼ��ק
    {
        originParent = transform.parent;
        currentItemID = originParent.GetComponent<Slot>().slotID;
        transform.SetParent(transform.parent.parent);
        transform.position = eventData.position;//��קʱ�����λ�������λ����ͬ
        transform.GetComponent<CanvasGroup>().blocksRaycasts = false;//�ر������ڵ�
    }
    public void OnDrag(PointerEventData eventData)//��ק����
    {
        transform.position = eventData.position;
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
    }
    public void OnEndDrag(PointerEventData eventData)//������ק
    {
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            if(eventData.pointerCurrentRaycast.gameObject.name=="Item Image")
            {
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position;

                var temp = bag.items[currentItemID];//��ʱ�����洢ԭ������ƷID
                //��������Ķ�Ӧ��λ�õ���ƷID��ֵ��Ҫ��ק����Ʒ��ID��                                         
                bag.items[currentItemID] = bag.items[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID];
                bag.items[currentItemID] = temp;

                eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originParent.position;
                eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(originParent);//�������Ӷ��󻥻�
                transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
            if (eventData.pointerCurrentRaycast.gameObject.name == "Slot(Clone)")
            {
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
                //��Ʒ��Ϊ��ʱ
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
