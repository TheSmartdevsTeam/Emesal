using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSkillsScript : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{
    Image SlotBackgroundImage;
    Image SlotBorderImage;
    Image TalentImage;
    public Image RequiredConnectionImage;
    public Image ConnectionImage1;
    public Image ConnectionImage2;
    public int _TalentIndex;
    public bool _Active;
    int Talent11index = 11;
    int Talent12index = 12;
    GameObject TalentTooltip;
    Material _DefaultMaterial;
    Material _TalentMaterial;

    // Start is called before the first frame update
    private void Start()
    {
        SlotBackgroundImage = transform.GetChild(0).GetComponent<Image>();
        SlotBorderImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        TalentImage = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        _TalentIndex = transform.GetComponent<CharacterSkillsScript>()._TalentIndex;

        if (transform.GetChild(0).childCount == 3)
        {
            TalentTooltip = transform.GetChild(0).GetChild(2).gameObject;
        }

        
        if (TalentImage != null)
        {
            SlotBackgroundImage.material = _DefaultMaterial;
            SlotBorderImage.material = _DefaultMaterial;
            TalentImage.material = _DefaultMaterial;
        }
    }
    private void Update()
    {
        SetDefaultMaterial();
    }
    public void SetDefaultMaterial()
    {
        if (RequiredConnectionImage != null)
        {
            if (RequiredConnectionImage.material.name != "FCA_Material" && RequiredConnectionImage.material.name != "AlchCon_Material" && RequiredConnectionImage.material.name != "InsCon_Material")
            {

                if (TalentImage != null)
                {
                    SlotBackgroundImage.material = _DefaultMaterial;
                    SlotBorderImage.material = _DefaultMaterial;
                    TalentImage.material = _DefaultMaterial;

                }
                if (ConnectionImage1 != null)
                {
                    ConnectionImage1.material = _DefaultMaterial;
                }
                if (ConnectionImage2 != null)
                {
                    ConnectionImage2.material = _DefaultMaterial;
                }

            }
        }
    }
    public bool CheckTalentActivation()
    {
        if (RequiredConnectionImage != null && RequiredConnectionImage.material.name == "FCA_Material" || (RequiredConnectionImage != null && RequiredConnectionImage.material.name == "AlchCon_Material") || (RequiredConnectionImage != null && RequiredConnectionImage.material.name == "InsCon_Material"))
        {
            return true;
        }
        else if (_TalentIndex == Talent11index)
        {
            return true;
        }
        else if (_TalentIndex == Talent12index)
        {
            return true;
        }
        else if(_Active)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Act_Talent(Material newMaterial)
    {
        if (CheckTalentActivation())
        {
            if (TalentImage.material != newMaterial)
            {
                TalentImage.material = newMaterial;
                _Active = true;
            }
            else
            {
                TalentImage.material = null;
                _Active = false;
            }
        }
    }
    public void Act_Background(Material newMaterial)
    {
        if (CheckTalentActivation())
        {
            if (SlotBackgroundImage.material != newMaterial)
            {
                SlotBackgroundImage.material = newMaterial;
            }
            else
            {
                SlotBackgroundImage.material = null;
            }
        }
    }
    public void Act_Border(Material newMaterial)
    {
        if (CheckTalentActivation())
        {
            if (SlotBorderImage.material != newMaterial)
            {
                SlotBorderImage.material = newMaterial;
            }
            else
            {
                SlotBorderImage.material = null;
            }
        }
    }
    public void Act_Connection(Material newMaterial)
    {
        if (CheckTalentActivation() && ConnectionImage1 != null)
        {
            if (ConnectionImage1.material != newMaterial)
            {
                ConnectionImage1.material = newMaterial;
            }
            else
            {
                ConnectionImage1.material = null;
            }
        }
        if (CheckTalentActivation() && ConnectionImage2 != null)
        {
            if (ConnectionImage2.material != newMaterial)
            {
                ConnectionImage2.material = newMaterial;
            }
            else
            {
                ConnectionImage2.material = null;
            }
        }
    }
    public void OnTalentsAcceptButtonClick()
    {
        //save changes
        Debug.Log("SAVED");
    }
    public void OnTalentsExitButtonClick(GameObject ObjectToActivateOnExit)
    {
        //save changes
        transform.parent.parent.gameObject.SetActive(false);
        ObjectToActivateOnExit.SetActive(true);

    }
    // When highlighted with mouse.
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Do something.
        //Debug.Log("<color=red>Event:</color> Completed mouse highlight.");
        if (TalentTooltip)
        {
            TalentTooltip.SetActive(true);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (TalentTooltip)
        {
            TalentTooltip.SetActive(false);
        }
    }
    // When selected.
    public void OnSelect(BaseEventData eventData)
    {
        // Do something.
        //Debug.Log("<color=red>Event:</color> Completed selection.");
    }

   
}
