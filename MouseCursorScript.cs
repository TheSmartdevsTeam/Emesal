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

    void Update()
    {

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
