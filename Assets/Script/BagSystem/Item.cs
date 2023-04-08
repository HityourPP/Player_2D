using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item",menuName ="Inventory/New Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemImag;
    public int itemHeldCount;
    [TextArea]
    public string itemInfo;//ŒÔ∆∑√Ë ˆ
    public bool canEquip;
}
