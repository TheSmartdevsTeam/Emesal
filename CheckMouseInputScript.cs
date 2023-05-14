using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CheckMouseInputScript : MonoBehaviour, IPointerClickHandler
{

    public UnityEvent leftClick;
    public bool _LeftClickFlag;
    public UnityEvent middleClick;
    public bool _MiddleClickFlag;
    public UnityEvent rightClick;
    public bool _RightClickFlag;


    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Left click");
            _LeftClickFlag = true;
        }
        else
        {
            Debug.Log("Left click off");
            _LeftClickFlag = false;
        }
            
        if (eventData.button == PointerEventData.InputButton.Middle) 
        {
            Debug.Log("Middle click");
            _MiddleClickFlag = true;
        }
        else
        {
            Debug.Log("Middle click off");
            _MiddleClickFlag = false;
        }
        if (eventData.button == PointerEventData.InputButton.Right)  
        {
            Debug.Log("Right click");
            _RightClickFlag = true;
        }
        else
        {
            Debug.Log("Right click off");
            _RightClickFlag = false;
        }
    }
}
