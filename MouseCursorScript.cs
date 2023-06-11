using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorScript : MonoBehaviour
{
    Vector3 _CursorPosition;
    GameObject _NormalCursor;
    GameObject _ClickedCursor;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        _NormalCursor = transform.GetChild(0).gameObject;
        _ClickedCursor = transform.GetChild(1).gameObject;

        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        _CursorPosition.x = Input.mousePosition.x;
        _CursorPosition.y = Input.mousePosition.y;
        _CursorPosition.z = Camera.main.nearClipPlane;
        
        _CursorPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        */
        //_CursorPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        _CursorPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        transform.position = _CursorPosition;
        CursorClick();
    }

    void CursorClick()
    {
        if(Input.GetKey(KeyCode.Mouse0))
        {
            _NormalCursor.SetActive(false);
            _ClickedCursor.SetActive(true);
        }
        else
        {
            _NormalCursor.SetActive(true);
            _ClickedCursor.SetActive(false);
        }
    }

    

}
