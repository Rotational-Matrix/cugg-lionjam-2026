using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class VerDicts : ScriptableObject
{
    public static Dictionary<string, (string, Sprite)> EvidenceDict { private set; get; } = new();
    // name should access each (flavourText,image) pair
    // images should be loaded via Resources manoeuvre on scriptable object load

    public static Dictionary<string, Sprite> ImageDict { private set; get; } = new();
    // for all the crappy MS Paint images.

    public static Dictionary<string, CatObject> CatDict { private set; get; } = new();

    /*
    public static Dictionary<string, AudioClip> CatVoices { private set; get; }

    public static Dictionary<(string,string), Sprite> CatSprites { private set; get; }
    */

    private void Awake()
    {
        InstantiateCatData();
    }

    //Cat names
    /*
     * paldo
     * waldo
     * aaa
     * chaircat
     * legislator1
     * legislator2
     */
    private void InstantiateCatData()
    {
        CatDict.Add("PALDO", new CatObject("Paldo"));
        CatDict.Add("WALDO", new CatObject("Waldo"));
        CatDict.Add("AAA", new CatObject("AAA"));
        CatDict.Add("CHAIRCAT", new CatObject("Chaircat"));
        CatDict.Add("LEGISLATOR1", new CatObject("Non-Local-CongressPerson"));
        CatDict.Add("LEGISLATOR2", new CatObject("Random Legislator Cat"));
    }




    /*
    private void InitSprites()
    {
        // note that there will be a lot of Resources.Load<Sprite>(filepath) calls
        string recpath = "Overlays/";

        dialogueSprites.Add((_e, "DEFAULT"), Resources.Load<Sprite>(recpath + "eve_new"));
        dialogueSprites.Add((_e, "CRY"), Resources.Load<Sprite>(recpath + "eve_cry"));
        dialogueSprites.Add((_e, "SAD"), Resources.Load<Sprite>(recpath + "eve_new_sad"));
        dialogueSprites.Add((_e, "SILHOUETTE"), Resources.Load<Sprite>(recpath + "evesilhouette"));

        dialogueSprites.Add((_s, "DEFAULT"), Resources.Load<Sprite>(recpath + "sariel_new"));
        dialogueSprites.Add((_s, "SMILE"), Resources.Load<Sprite>(recpath + "sariel_new_smile"));
        dialogueSprites.Add((_s, "DISAPPOINTED"), Resources.Load<Sprite>(recpath + "Sariel_disappointed"));
        dialogueSprites.Add((_s, "LAUGH"), Resources.Load<Sprite>(recpath + "sariel_new_smile")); //doesn't exist
        dialogueSprites.Add((_s, "SILHOUETTE"), Resources.Load<Sprite>(recpath + "sarielsilhouette"));
    }
    */

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