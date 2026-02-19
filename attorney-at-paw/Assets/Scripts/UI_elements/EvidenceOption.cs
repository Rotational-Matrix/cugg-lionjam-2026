using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EvidenceOption : MonoBehaviour
{
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private FlavourPanel flavourPanel; //panel that arises upon selection.
    [SerializeField] private GameObject selectorImage; // should be a child of the Choice box panel
    [SerializeField] private SpriteRenderer evidenceSR; // associated image, obtained from scriptable object
    [SerializeField] private TMPro.TMP_Text evidenceNameTextBox;


    private string currEvidenceName = ""; //acts as ID for current held object.

    //unlike ChoiceBoxHandler, this 'option' simply responds, and technically doesn't store any information
    

    //unless [Cu] is behaving stupid, really only 


    public void SetSelected(bool setSelected)
    {
        selectorImage.SetActive(setSelected);
        flavourPanel.SetPanelActive(setSelected);
    }
    

    public void SetEvidence(string evidenceName)
    {
        if (string.Equals(evidenceName, null) || string.Equals(evidenceName, ""))
        {
            ClearEvidence();
        }
        else
        {
            try
            {
                (string, Sprite) evidenceData = VerDicts.EvidenceDict[evidenceName];
                currEvidenceName = evidenceName;
                evidenceNameTextBox.text = evidenceName;
                flavourPanel.SetFlavourText(evidenceData.Item1);
                evidenceSR.sprite = evidenceData.Item2;
                optionPanel.SetActive(true);
            }
            catch (KeyNotFoundException)
            {
                Debug.Log("Attempted to set illegitimate evidence using name: " + evidenceName);
                ClearEvidence();
            }
        }
    }

    private void ClearEvidence()
    {
        SetSelected(false);
        flavourPanel.Clean();
        currEvidenceName = "";
        evidenceNameTextBox.text = "";
        evidenceSR.sprite = null;
        optionPanel.SetActive(false);
    }

    /*private void OnDisable() //disabling the evidence box 
    {
        ClearEvidence();
    }*/ 

    private void Awake()
    {
        ClearEvidence();
    }


}