using UnityEngine;

public class CharacterInteractionScript : MonoBehaviour
{
    Camera _CharacterCamera;
    Vector3 mousePos;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            mousePos = Input.mousePosition;
            Interact();
        }

    }
    public virtual void Interact()
    {
        _CharacterCamera = Camera.main;
        RaycastHit _CharacterInteractionRayHit;

        if (Physics.Raycast(transform.position, Input.mousePosition, out _CharacterInteractionRayHit, 1.5f))
        {
            _ = _CharacterInteractionRayHit.transform.GetComponent<PickupScript>()._Item;
        }
    }

}
