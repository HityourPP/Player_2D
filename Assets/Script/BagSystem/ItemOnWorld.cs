using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public Item item;
    public Inventory inventory;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            AddItem();
            Destroy(gameObject);
        }
    }
    private void AddItem()
    {
        if (inventory.items.Contains(item))
        {
            item.itemHeldCount += 1;
        }
        else
        {
            for (int i = 0; i < inventory.items.Count; i++)//寻找空位置放新物品
            {
                if (inventory.items[i] == null)
                {
                    inventory.items[i] = item;
                    break;
                }
            }
            //inventory.items.Add(item);
            //InventoryManager.CreateNewItem(item);
        }
        InventoryManager.GenerateItem();
    }
}
