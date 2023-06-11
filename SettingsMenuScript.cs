using TMPro;
using UnityEngine;
using Screen = UnityEngine.Screen;

public class SettingsMenuScript : MonoBehaviour
{
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
    }

    // Update is called once per frame
    void Update()
    {
        

    }

}
