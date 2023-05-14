using TMPro;
using UnityEngine;
using Screen = UnityEngine.Screen;

public class SettingsMenuScript : MonoBehaviour
{
    public int _VideoAspect;
    public GameObject _4x3ResoultionDropdown;
    public GameObject _16x9ResoultionDropdown;
    public GameObject _16x10ResoultionDropdown;
    public GameObject _21x9ResoultionDropdown;
    public GameObject _32x9ResoultionDropdown;
    public int _ResolutionValue;
    public Resolution _CurrentResolution;
    public Resolution[] _Resolutions; 
    void Start()
    {
        //transform.GetChild(2).GetChild(1).GetChild(0).GetChild(1).gameObject.SetActive(true);
        Screen.SetResolution(1366, 768, false);
        Screen.fullScreen = Screen.fullScreen;

        #region SetUpGameObjectVariables
        _VideoAspect = transform.GetChild(0).GetChild(1).GetChild(2).GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_Dropdown>().value;
        //4:3_ResolutionDropdown
        _4x3ResoultionDropdown = transform.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(1).GetChild(2).GetChild(0).gameObject;
        _4x3ResoultionDropdown.SetActive(true);
        //16:9_ResolutionDropdown
        _16x9ResoultionDropdown = transform.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(1).GetChild(2).GetChild(1).gameObject;
        _16x9ResoultionDropdown.SetActive(false);
        //16:10_ResolutionDropdown
        _16x10ResoultionDropdown = transform.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(1).GetChild(2).GetChild(2).gameObject;
        _16x10ResoultionDropdown.SetActive(false);
        //21:9_ResolutionDropdown
        _21x9ResoultionDropdown = transform.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(1).GetChild(2).GetChild(3).gameObject;
        _21x9ResoultionDropdown.SetActive(false);
        //32:9_ResolutionDropdown
        _32x9ResoultionDropdown = transform.transform.GetChild(0).GetChild(1).GetChild(2).GetChild(1).GetChild(2).GetChild(4).gameObject;
        _32x9ResoultionDropdown.SetActive(false);
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        DropDownControler();

    }

    private void DropDownControler()
    {
        if (_VideoAspect == 0)
        {
            _4x3ResoultionDropdown.SetActive(true);
            _16x9ResoultionDropdown.SetActive(false);
            _16x10ResoultionDropdown.SetActive(false);
            _21x9ResoultionDropdown.SetActive(false);
            _32x9ResoultionDropdown.SetActive(false);
        }
        if (_VideoAspect == 1)
        {
            _4x3ResoultionDropdown.SetActive(false);
            _16x9ResoultionDropdown.SetActive(true);
            _16x10ResoultionDropdown.SetActive(false);
            _21x9ResoultionDropdown.SetActive(false);
            _32x9ResoultionDropdown.SetActive(false);
        }
        if (_VideoAspect == 2)
        {
            _4x3ResoultionDropdown.SetActive(false);
            _16x9ResoultionDropdown.SetActive(false);
            _16x10ResoultionDropdown.SetActive(true);
            _21x9ResoultionDropdown.SetActive(false);
            _32x9ResoultionDropdown.SetActive(false);
        }
        if (_VideoAspect == 3)
        {
            _4x3ResoultionDropdown.SetActive(false);
            _16x9ResoultionDropdown.SetActive(false);
            _16x10ResoultionDropdown.SetActive(false);
            _21x9ResoultionDropdown.SetActive(true);
            _32x9ResoultionDropdown.SetActive(false);
        }
        if (_VideoAspect == 4)
        {
            _4x3ResoultionDropdown.SetActive(false);
            _16x9ResoultionDropdown.SetActive(false);
            _16x10ResoultionDropdown.SetActive(false);
            _21x9ResoultionDropdown.SetActive(false);
            _32x9ResoultionDropdown.SetActive(true);
        }
    }
}
