using UnityEngine;

public class UIMasterScript : MonoBehaviour, IMyDataManager
{
    [SerializeField] private _MouseLook _Cursor;
    public GameObject _Player;
    public Transform _CharacterTransform;
    public Camera _CharacterCamera;
    GameObject _CharacterPreviewObject;
    public Animator _CharacterAnimator;
    GameObject _CharacterSheets;
    public bool _MouseLookBool;
    public bool _Subtitles;
    GameObject _CursorCanvas;
    Transform _SlotsParent;
    InventorySlot_Script[] _slots;
    GameObject _Inventory;
    GameObject _SkillsGameObject;
    InventoryScript inventory;
    InventorySlot_Script iss = new InventorySlot_Script();
    GameObject _FireSkillTree;
    GameObject _EarthSkillTree;
    GameObject _WaterSkillTree;
    GameObject _AirSkillTree;
    GameObject _SkillsElements;
    void Start()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");
        #region Cursor SetUp
        _Cursor = gameObject.AddComponent<_MouseLook>();
        _Cursor.lockCursor = true;
        _Cursor.SetCursorLock(true);
        Cursor.visible = false;
        _CursorCanvas = transform.parent.GetChild(3).gameObject;
        #endregion

        _CharacterCamera = Camera.main;
        _Inventory = transform.GetChild(0).gameObject;
        _SkillsGameObject = transform.GetChild(2).gameObject;

        inventory = InventoryScript.instance;
        inventory.onItemChangedCallBack += UpdateUI;
        _SlotsParent = transform.GetChild(0).GetChild(1).GetChild(0);
        _slots = _SlotsParent.GetComponentsInChildren<InventorySlot_Script>();
        _FireSkillTree = transform.GetChild(2).GetChild(0).GetChild(0).GetChild(2).gameObject;
        _EarthSkillTree = transform.GetChild(2).GetChild(0).GetChild(0).GetChild(3).gameObject;
        _WaterSkillTree = transform.GetChild(2).GetChild(0).GetChild(0).GetChild(4).gameObject;
        _AirSkillTree = transform.GetChild(2).GetChild(0).GetChild(0).GetChild(5).gameObject;
        _SkillsElements = _SkillsGameObject.transform.GetChild(0).GetChild(0).GetChild(1).gameObject;
        _CharacterSheets = transform.GetChild(1).gameObject;
        _CharacterPreviewObject = transform.parent.parent.parent.GetChild(1).gameObject;

        
    }

    private void UpdateUI()
    {
        Debug.Log("UPDATING UI");
        for (int i=0;i<_slots.Length-1;i++)
        {
            Debug.Log("Items in inventory: " + inventory.items.Count);
            if(i<inventory.items.Count)
            {
                if (inventory.items[i] != null)
                {
                    _slots[i].AddItem(inventory.items[i]);
                }
                
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
        CheckUIActive();
        if (CheckUIActive())
        {
            EnableCursor();
            UICullingMask();
            _CharacterPreviewObject.gameObject.SetActive(true);
        }
        else
        {
            DisableCursor();
            EverythingCullingMask();
            _CharacterPreviewObject.gameObject.SetActive(false);
        }
    }
    public bool CheckUIActive()
    {
        bool var = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                var = true;
            }
        }
        return var;
    }
    public void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        _CursorCanvas.SetActive(true);
    }
    public void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _CursorCanvas.SetActive(false);
    }
    public void UICullingMask()
    {
        _CharacterCamera.cullingMask = LayerMask.NameToLayer("UI");
    }
    public void EverythingCullingMask()
    {
        _CharacterCamera.cullingMask = LayerMask.NameToLayer("Everything");
    }
    public void CheckInventory()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && _Inventory.activeSelf == false)
        {
            transform.gameObject.GetComponentInChildren<InventoryScript>().gameObject.SetActive(true);
            _CharacterAnimator.SetBool("Backpack", true);
            _Inventory.gameObject.SetActive(true);
            _CharacterSheets.gameObject.SetActive(false);
            _SkillsGameObject.gameObject.SetActive(false);
            _Player.transform.GetChild(2).gameObject.SetActive(false); ;
            _MouseLookBool = true;
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            _CharacterAnimator.SetBool("Backpack", false);
            _Inventory.gameObject.SetActive(false);
            _Player.transform.GetChild(2).gameObject.SetActive(true);
            _MouseLookBool = false;
            iss.InventoryClosed();
        }
        else if (_Inventory.gameObject.activeInHierarchy == true)
        {
            if (Input.GetKeyDown(KeyCode.Q))
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
    public void SetParameter(bool flag)
    {
        _Subtitles = flag;
    }
    void IMyDataManager.SaveData(ref GameData data)
    {
        
    }
    void IMyDataManager.LoadData(GameData data)
    {
        throw new System.NotImplementedException();
    }
}

