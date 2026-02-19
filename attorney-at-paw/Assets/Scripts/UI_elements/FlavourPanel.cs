using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlavourPanel : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text flavourText; // The actual displayed text in the text box
    [SerializeField] private GameObject panel; //panel that arises upon selection.

    public void SetFlavourText(string fText)
    {
        flavourText.text = fText;
    }

    public void SetPanelActive(bool value)
    {
        panel.SetActive(value);
    }

    public void Clean()
    {
        flavourText.text = "";
        SetPanelActive(false);
    }

    //change of fText expected on choiceLoad
    
    

}
