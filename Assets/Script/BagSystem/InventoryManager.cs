using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    static InventoryManager instance;

    public Inventory bag;
    public GameObject slotGird;
    public GameObject slot;
    public TextMeshProUGUI description;

    public List<GameObject> slots = new List<GameObject>();
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
    }
    private void OnEnable()
    {
        GenerateItem();
    }
    //public static void CreateNewItem(Item item)//生成新的格子
    //{
    //    Slot newItem = Instantiate(instance.slot, instance.slotGird.transform.position, Quaternion.identity);
    //    newItem.gameObject.transform.SetParent(instance.slotGird.transform);
    //    newItem.slotItem = item;
    //    newItem.slotImage.sprite = item.itemImag;
    //    newItem.slotItemNum.text = item.itemHeldCount.ToString();
    //}
    public static void GenerateItem()//根据背包中的物品数目更新UI中的信息
    {
        for (int i = 0; i < instance.slotGird.transform.childCount; i++)
        {
            Destroy(instance.slotGird.transform.GetChild(i).gameObject);
            instance.slots.Clear();//再次打开背包时需要将其清空再重新生成
        }
        for (int i = 0; i < instance.bag.items.Count; i++)
        {
            instance.slots.Add(Instantiate(instance.slot));
            instance.slots[i].transform.SetParent(instance.slotGird.transform);
            instance.slots[i].GetComponent<Slot>().SetSlotItem(instance.bag.items[i]);
            instance.slots[i].GetComponent<Slot>().slotID = i;
        }
    }
    public static void ShowItemInfo(string description)//显示物品的描述信息
    {
        instance.description.text = description;
    }
}
