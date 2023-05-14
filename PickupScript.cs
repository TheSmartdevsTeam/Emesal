using UnityEngine;

public class PickupScript : CharacterInteractionScript
{
    public Item _Item;
    public bool _DetectedItem;
   
    public override void Interact()
    {
        if(_DetectedItem == true)
        {
            base.Interact();
            PickUp();
        }
    }

    void PickUp()
    {
        
        bool wasPickedUp = InventoryScript.instance.Add(_Item);
        

        if (wasPickedUp)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        _Item = GetComponent<PickupScript>()._Item;
        _DetectedItem = true;
    }

    private void OnTriggerStay(Collider other)
    {
        _DetectedItem = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _DetectedItem = false;

    }
}
