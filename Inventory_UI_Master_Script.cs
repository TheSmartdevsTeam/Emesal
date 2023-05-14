using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_UI_Master_Script : MonoBehaviour
{
    [SerializeField] private _MouseLook _Cursor;
    public Transform _CharacterTransform;
    public Camera _CharacterCamera;
    public Animator _CharacterAnimator;
    public Transform _CharacterSheets;
    public bool _MouseLookBool;
    //GameObject _Inventory;
    public Transform _SlotsParent;
    InventorySlot_Script[] _slots;
    GameObject _Inventory;
    public GameObject _SkillsGameObject;
    InventoryScript inventory;
    InventorySlot_Script iss = new InventorySlot_Script();
    public Material _FireMaterial;
    public CanvasRenderer _CanvasRendererSkills;
    public GameObject _FireBackground;
    public GameObject _FireSkillTree;
    public GameObject _EarthSkillTree;
    public GameObject _WaterSkillTree;
    public GameObject _AirSkillTree;
    public GameObject _SkillsElements;
    void Start()
    {
        #region Cursor SetUp
        _Cursor = gameObject.AddComponent<_MouseLook>();
        
        _Cursor.XSensitivity = 2;
        _Cursor.YSensitivity = 2;
        _Cursor.MinimumX = -70;
        _Cursor.MaximumX = 70;
        _Cursor.clampVerticalRotation = true;
        _Cursor.smooth = false;
        _Cursor.smoothTime = 5;

        //_Cursor.lockCursor = true;
        //_Cursor.SetCursorLock(true);

        //_CursorLastLocation = Input.mousePosition;
        _Cursor.lockCursor = true;
        _Cursor.SetCursorLock(true);
        #endregion

        _CharacterCamera = Camera.main;
        _Inventory = transform.GetChild(0).gameObject;
        _SkillsGameObject = transform.GetChild(2).gameObject;

        inventory = InventoryScript.instance;
        inventory.onItemChangedCallBack += UpdateUI;
        _SlotsParent = transform.GetChild(0).GetChild(1).GetChild(3);
        _slots = _SlotsParent.GetComponentsInChildren<InventorySlot_Script>();
        _FireSkillTree = transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).gameObject;
        _EarthSkillTree = transform.GetChild(2).GetChild(0).GetChild(0).GetChild(3).gameObject;
        _WaterSkillTree = transform.GetChild(2).GetChild(0).GetChild(0).GetChild(4).gameObject;
        _AirSkillTree = transform.GetChild(2).GetChild(0).GetChild(0).GetChild(5).gameObject;
        _SkillsElements = _SkillsGameObject.transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
    }

    private void UpdateUI()
    {
        Debug.Log("UPDATING UI");
        for (int i=0;i<_slots.Length;i++)
        {
            Debug.Log("Items in inventory: " + inventory.items.Count);
            if(i<inventory.items.Count)
            {
                _slots[i].AddItem(inventory.items[i]);
                if (_slots[i].transform.GetChild(0).GetChild(3).gameObject.activeSelf)
                {
                    _slots[i].SetActivePanel(true);
                }
                else
                {
                    _slots[i].SetActivePanel(false);
                }
            }
            else
            {
                if (_slots[i].GetItem())
                {
                    _slots[i].ClearSlot();
                }
            }

        }
    }

    void Update()
    {
        CheckInventory();
    }

    public void CheckInventory()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && _Inventory.activeSelf == false)
        {
            
            transform.gameObject.GetComponentInChildren<InventoryScript>().gameObject.SetActive(true);
            _CharacterAnimator.SetBool("Backpack", true);
            Debug.Log("Inventory ON");
            _Inventory.gameObject.SetActive(true);
            _CharacterSheets.gameObject.SetActive(false);
            _SkillsGameObject.gameObject.SetActive(false);
            //Cursor.visible = true;
            //_Cursor.Init(transform, Camera.main.transform);
            //_Cursor.LookRotation(transform, Camera.main.transform);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;


            _MouseLookBool = true;
            _CharacterCamera.cullingMask = LayerMask.NameToLayer("UI");
            
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            _CharacterAnimator.SetBool("Backpack", false);
            Debug.Log("Inventory OFF");
            _Inventory.gameObject.SetActive(false);
            //_Cursor.SetCursorLock(true);
            //Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;


            _MouseLookBool = false;
            _CharacterCamera.cullingMask = LayerMask.NameToLayer("Everything");
            
            iss.InventoryClosed();
            

        }
        else if(_Inventory.gameObject.activeInHierarchy == true)
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                _Inventory.gameObject.SetActive(false);
                _CharacterSheets.gameObject.SetActive(false);
                _SkillsGameObject.gameObject.SetActive(true);

            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                _Inventory.gameObject.SetActive(false);
                _CharacterSheets.gameObject.SetActive(true);
                _SkillsGameObject.gameObject.SetActive(false);
            }
        }
        else if (_CharacterSheets.gameObject.activeInHierarchy == true)
        {

            if (Input.GetKeyDown(KeyCode.Q))
            {
                _Inventory.gameObject.SetActive(true);
                _CharacterSheets.gameObject.SetActive(false);
                _SkillsGameObject.gameObject.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                _Inventory.gameObject.SetActive(false);
                _CharacterSheets.gameObject.SetActive(false);
                _SkillsGameObject.gameObject.SetActive(true);
            }
        }
        else if (_SkillsGameObject.gameObject.activeInHierarchy == true)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _Inventory.gameObject.SetActive(false);
                _CharacterSheets.gameObject.SetActive(true);
                _SkillsGameObject.gameObject.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                _Inventory.gameObject.SetActive(true);
                _CharacterSheets.gameObject.SetActive(false);
                _SkillsGameObject.gameObject.SetActive(false);
            }
        }
        else
        {
            _Inventory.SetActive(false);
        }


    }

    public void CheckSwapInventory()
    {
        

        if (_CharacterSheets.gameObject.activeInHierarchy == true)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                _Inventory.gameObject.SetActive(false);
                _CharacterSheets.gameObject.SetActive(false);
                _SkillsGameObject.gameObject.SetActive(true);

            }
        }
        if (_SkillsGameObject.gameObject.activeInHierarchy == true)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                _Inventory.gameObject.SetActive(true);
                _CharacterSheets.gameObject.SetActive(false);
                _SkillsGameObject.gameObject.SetActive(false);

            }
        }
    }

    public  bool _GetMouseLookBool()
    {
        return _MouseLookBool;
    }

    private void RotateView()
    {
        _Cursor.Init(_CharacterTransform, _CharacterCamera.transform);
        _Cursor.LookRotation(_CharacterTransform, _CharacterCamera.transform);
    }

    public void ClickFireSymbol()
    {
        _FireSkillTree.SetActive(true);
        _SkillsElements.SetActive(false);
    }

    public void ClickEarthSymbol()
    {
        _EarthSkillTree.SetActive(true);
        _SkillsElements.SetActive(false);
    }

    public void ClickWaterSymbol()
    {
        _WaterSkillTree.SetActive(true);
        _SkillsElements.SetActive(false);
    }

    public void ClickAirSymbol()
    {
        _AirSkillTree.SetActive(true);
        _SkillsElements.SetActive(false);
    }



}

