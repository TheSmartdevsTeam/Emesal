using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    #region Singleton
    public static InventoryScript instance;
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallBack;
    void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }
        instance = this;
    }
    #endregion

    public List<Item> items = new List<Item>();
    public int space;

    private void Start()
    {
        space = 62;
    }
    public bool Add(Item item)
    {
        Debug.Log("Adding item " + item._Name + " to inventory");
        if (items.Count >= space)
        {
            Debug.Log("Not enough room in inventory!");
            return false;
        }
        else
        {
            items.Add(item);
            if(onItemChangedCallBack != null)
            {
                onItemChangedCallBack.Invoke();
            }
        }
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        if (onItemChangedCallBack != null)
        {
            onItemChangedCallBack.Invoke();
        }
    }

    public List<Item> GetItemList()
    {
        return items;
    }

    public bool IsFull()
    {
        if(items.Count == space)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
