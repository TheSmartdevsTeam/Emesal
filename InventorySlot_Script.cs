using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventorySlot_Script : MonoBehaviour, IDropHandler
{
    Item item;
    public Image icon;
    public Color _EmptySlotColor;
    public Sprite _EmptyIconSprite;
    Color _DisabledSlotColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
    public GameObject _PropertiesPanel;
    public GameObject _PropertiesMenuParent;
    public InventorySlot_Script[] _Slots;
    public static bool _CombineFlag;
    public static bool _ActivePanel;

    public void AddItem (Item newItem)
    {
        item = newItem;
        icon.color = new Color(1, 1, 1, 1);
        icon.sprite = item._Icon;
        //icon.enabled = true;
        
    }

    public void ClearSlot()
    {
        icon.sprite = _EmptyIconSprite;
        icon.color = _EmptySlotColor;
        item = null;
    }

    public void OnDropButtonClick()
    {
        InventoryScript.instance.Remove(item);
        _PropertiesPanel.SetActive(false);
        _ActivePanel = false;
        Debug.Log("DropedItem");
    }

    public void OnPropertyButtonClick()
    {
        if (item != null)
        {
            if(_PropertiesPanel.activeInHierarchy == true)
            {
                _ActivePanel = true;

            }
            if (_CombineFlag == true)
            {
                _CombineFlag = false;
                Debug.Log("COMBINED!");
                _ActivePanel = false;
            }
            if (_CombineFlag == false && _ActivePanel == false)
            {
                _PropertiesPanel.SetActive(true);
                _ActivePanel = true;
            }
        }   
    }

    public void OnCombineConfirmButtonClick()
    {
        _PropertiesMenuParent.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
        _PropertiesMenuParent.transform.parent.gameObject.SetActive(false);
        //select item to combine
        if (item != null)
        {
            item.Combine(item);
            _CombineFlag = true;
            
            for (int i = 0; i < _Slots.Length; i++)
            {
                if (_Slots[i].item._CanBeCombined == false || _Slots[i].item._CombineNumber != item._CombineNumber || _Slots[i].item != null)
                {
                    //_Slots[i].icon.color = _DisabledSlotColor;
                    Debug.Log("DiSABLED");
                }
            }
        } 
    }

    public void UseItem()
    {
        if(item != null)
        {
            _PropertiesPanel.SetActive(false);
            item.Use();
            Debug.Log("UsedItem");
        }
    }

    public void InventoryClosed()
    {
        _CombineFlag = false;
        _ActivePanel = false;
        
    }

    private void OnEnable()
    {
        if(_PropertiesPanel != null)
        {
            _PropertiesPanel.SetActive(false);
            _PropertiesMenuParent.transform.parent.gameObject.SetActive(false);
        }
    }

    public Item GetItem()
    {
        return item;
    }

    public void SetActivePanel(bool value)
    {
        _ActivePanel = value; 
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        //Debug.Log(transform.GetChild(0).GetChild(2).GetComponent<Image>().name);
        if(transform.GetChild(0).GetChild(0).GetComponent<Image>().enabled == true) { 
            GameObject dropped = eventData.pointerDrag;
            DraggableScript draggableItem = dropped.GetComponent<DraggableScript>();
            draggableItem._ParentAfterDrag = transform;
        }
    }
}
