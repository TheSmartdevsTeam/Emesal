using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string _Name;
    public Sprite _Icon = null;
    public bool _IsDefaultItem = false;
    public int _ItemWeigth;
    public bool _CanBeCombined;
    public int _CombineNumber;
    public int _RequiredSlots;
    public virtual void Use()
    {
        Debug.Log("Using " + _Name);
    }

    public virtual void Combine(Item item)
    {
        Debug.Log("Combining with " + item._Name);
    }

}


