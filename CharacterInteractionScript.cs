using UnityEngine;

public class CharacterInteractionScript : MonoBehaviour
{
    Camera _CharacterCamera;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
        
    }
    public virtual void Interact()
    {
        _CharacterCamera = Camera.main;
        RaycastHit _CharacterInteractionRayHit;
        if (Physics.Raycast(transform.position, _CharacterCamera.transform.forward, out _CharacterInteractionRayHit, 1))
        {
            Item _Item = _CharacterInteractionRayHit.transform.GetComponent<PickupScript>()._Item;
        }
    }

}
