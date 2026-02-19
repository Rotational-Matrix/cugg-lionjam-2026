using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class VerDicts : ScriptableObject
{
    public static Dictionary<string, (string, Sprite)> EvidenceDict { private set; get; }
    // name should access each (flavourText,image) pair
    // images should be loaded via Resources manoeuvre on scriptable object load
    
    public static Dictionary<string, Sprite> ImageDict { private set; get; }
    // for all the crappy MS Paint images.
    

    
    /*[SerializeField] private TMPro.TMP_Text choiceText; // The actual displayed text in the text box
    [SerializeField] private GameObject selectorImage; // should be a child of the Choice box panel
    //private string internalString = ""; // maybe needed for later to parse commands
    private bool isSelected = false;
    private bool isLocked = false; // for special plot circumstances, locked can be viewed, not chosen

    protected Sprite sprite;
    protected string EvidenceName;
    protected string FlavourText;*/

    //there should be a scriptless object somewhere which contains all the objects' names and FlavourText, perhaps in a dict

    /*

    private void CleanTextContents()
    {
        choiceText.text = "";
    }

    private void OnDisable()
    {
        this.CleanTextContents();
        this.SetSelected(false);
    }

    private void Awake()
    {
        this.CleanTextContents();
        this.SetSelected(false);
    }
    */
    // WILL PROBABLY NOT EXIST AS A CLASS, SEE SCRIPTABLE OBJECT THAT HOLDS INFO
}