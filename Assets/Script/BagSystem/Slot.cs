using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item slotItem;
    public Image slotImage;
    public TextMeshProUGUI slotItemNum;

    public GameObject itemInSlot;
    public string slotInfo;
    public int slotID;
    public void ItemOnClicked()
    {
        InventoryManager.ShowItemInfo(slotInfo);
    }
    public void SetSlotItem(Item item)
    {
        if(item == null)
        {
            itemInSlot.SetActive(false);
            return;
        }
        slotImage.sprite = item.itemImag;
        slotItemNum.text = item.itemHeldCount.ToString();
        slotInfo = item.itemInfo;
    }
}
