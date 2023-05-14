using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuButtonScript : MonoBehaviour
{
    public Button _Button;

    public static bool _ClickSound;
    void Start()
    {
        _ClickSound = false;
        _Button = GetComponent<Button>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
       
    }

    public void onStartClick()
    {
        Debug.Log("Clicked");
        _ClickSound = true;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        
    }

    public void onSettingsClick()
    {
        _ClickSound = true;
        //this.gameObject.transform.parent.parent.gameObject.SetActive(false);
    }

    public void onNoClick()
    {
        _ClickSound = true;
        gameObject.transform.parent.gameObject.SetActive(false);
        
    }

    public void onConfirmStartClick()
    {
        _ClickSound = true;
        gameObject.transform.parent.gameObject.SetActive(false);
        //SceneManager.LoadScene(1);
    }

    public void onConfirmExitClick()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
        //Application.Quit();
    }

    public void onExitClick()
    {
        _ClickSound = true;
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        
    }

    public void onSettingsExitClick()
    {
        _ClickSound = true;
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void onClickPlayClickAudio()
    {
        _ClickSound = true;
    }

    
    

}