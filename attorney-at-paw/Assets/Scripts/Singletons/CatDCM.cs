//using Ink.Parsed;
using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Windows;

public class CatDCM : MonoBehaviour
{
    /// <summary>
    /// [Cu]'s Documentation
    /// 
    /// Adapted from ToaF to AAP
    /// 
    /// </summary>


    [SerializeField] private DialoguePanel dPanel;
    [SerializeField] private ChoiceCanvas cCanvas;

    [SerializeField] private CatPodium leftPodium;
    [SerializeField] private CatPodium rightPodium;

    //this should be set to the compiled json asset (not the ink itself)
    public TextAsset inkAsset;

    //The ink story we're wrapping,
    Story _inkStory;

    //Dialogue intermediates (stored here to be processed or applied all at once)
    private Sprite currLeftPodiumSprite = null;
    private Sprite currRightPodiumSprite = null;
    private string currHeader = "";
    private string intermediateBodyText = "";
    private List<string> currTags = new List<string>();
    //informs how to read text
    private bool readAsStageLines = true;

    private Dictionary<string, Func<string[], bool>> colonLineCommands = new();
    // dialogue sprites are associated with a character (of course, null ones may exist)
    private Dictionary<(GameObject, string), Sprite> dialogueSprites = new();

    

    private void Awake()
    {
        InitInk(); // creates inkstory, and 
        SetDialogueState(true); //it automatically makes sure it is turned off at start. (YOU CAN'T LEAVE IT (AAP))
        AAPSingleton.dcm = this;
    }

    // creates the inkstory whilst also properly informing the newgame save state and providing it with proper external vals
    private void InitInk()
    {
        _inkStory = new Story(inkAsset.text);
        colonLineCommands.Add("SET_INPUT", SetInputCmd);
        colonLineCommands.Add("CUTSCENE", CutSceneCmd);
        /*
        colonLineCommands.Add("FORCED_MOVE", ForcedMoveCmd);
        colonLineCommands.Add("AUTOSAVE", AutosaveCmd);
        colonLineCommands.Add("LEASH_SET", LeashSetActiveCmd);
        //colonLineCommands.Add("LEASH_COEF", LeashSetCoefCmd); merged into LEASH_SET
        colonLineCommands.Add("BACKDROP_SET", BackdropSetCmd);
        colonLineCommands.Add("HIDDEN_FLOWER_ACTIVE", HiddenFlowerCmd);
        colonLineCommands.Add("SARIEL_INSTANT_INTERACT", SetSarielCanInteractCmd);
        colonLineCommands.Add("TELEPORT", TeleportCmd);
        colonLineCommands.Add("SARIEL_DIST_TRIGGER", SetSarielDistTriggerCmd);
        colonLineCommands.Add("FLOWERPOT_SET", FlowerPotSpriteSetCmd);
        colonLineCommands.Add("BACKDROP_TIMER_TRANSITION", BackdropTimerTransitionCmd);
        colonLineCommands.Add("ENDING", EndingCmd);
        colonLineCommands.Add("BACKDOOR", BackdoorCmd);
        */
    }

    private bool SetInputCmd(string[] argv)
    {
        if (argv.Length != 2)
            return false;
        AAPSingleton.inputHandler.ForceInputActive(CapsToBool(argv[1]));
        return true;
    }
    private bool CutSceneCmd(string[] argv)
    {
        if (argv.Length != 2)
            return false;
        if (string.Equals(argv[1], "CLEAR"))
            AAPSingleton.cutScene.ClearCutScenes();
        else
            AAPSingleton.cutScene.InitiateCutScene(VerDicts.ImageDict[argv[1]]);
        return true;
    }
    

    public void ResponseToLoadSave()
    {
        if (GetInkVar<bool>("is_start_save"))
            InitiateDialogueState("save_load_knot");
    }

    public bool InitiateDialogueState(string knotName) //also "knotName.stitchName" is valid
    {
        bool divertBlocked = false;
        if (!Equals(knotName, null))
        {
            divertBlocked = DivertTo(knotName); //in case one wishes to intentionally not jump, but return to dialogue state
        }
        if (!divertBlocked)
        {
            SetDialogueState(true);
            return AttemptContinue();
        }
        return false;  //mimics failing, although divert blocking is acceptable
    }

    public void SetDialogueState(bool setActive)
    {
        dPanel.gameObject.SetActive(setActive);
    }




    //attempts to continue and returns if it failed.
    //this is done because we may desire to allow choice selection at this point
    //however, it is normally a good idea to separate the commands for coninue & choose
    public bool AttemptContinue()
    {
        bool canContinue = _inkStory.canContinue;
        if (canContinue)
        {
            intermediateBodyText = _inkStory.Continue();
            bool validText = ContinueDialogue(); //if text is invalid (as in 'encontered line command'), recurse!

            //the recursion allows AttemptContinue to process each line command,
            //while ultimately skipping past all of them from the user's perspective 
            if (!validText)
                canContinue = AttemptContinue();
        }
        return canContinue;
    }


    //The following are the choice handler methods:
    public bool InitiateChoices()
    {
        if (_inkStory.currentChoices.Count > 0)
        {
            string[] arr = new string[_inkStory.currentChoices.Count];
            for (int i = 0; i < _inkStory.currentChoices.Count; ++i)
            {
                Choice choice = _inkStory.currentChoices[i];
                arr[i] = choice.text;
            }
            return cCanvas.InitiateEvidenceSelection(arr);
        }
        else
            return false;
    }

    public void Choose()
    {
        _inkStory.ChooseChoiceIndex(cCanvas.Choose());
    }
    public void MoveSelectorUp() { cCanvas.MoveSelectorUp(); }
    public void MoveSelectorDown() { cCanvas.MoveSelectorDown(); }
    public void MoveSelectorLeft() { cCanvas.MoveSelectorLeft(); }
    public void MoveSelectorRight() { cCanvas.MoveSelectorRight(); }
    public bool IsChoiceActive()
    {
        return cCanvas.IsDisplaying();
    }


    public bool DivertTo(string knotName) //or knotName.stitchName
    {
        //Debug.Log("Diverting to knot: " + knotName);
        bool incurBlock = HandleKnotTags(knotName);
        _inkStory.ChoosePathString(knotName);
        return incurBlock;
    }

   
    

    public void SetInkVar<T>(string variableName, T newVal) //literally the name of the variable as it appears in the inkstory
    {
        _inkStory.variablesState[variableName] = newVal; //it...allows this (pls make newVal type match the variable)
    }
    public T GetInkVar<T>(string variableName)
    {
        return (T)(_inkStory.variablesState[variableName]); //forceful cast... please match types!
    }


    //##### only on DCM initiated diverting are KnotTags handled #####
    // returns true if divert is blocked
    private bool HandleKnotTags(string knotName) //or knotName.stitchName
    {
        List<string> knotTagList = _inkStory.TagsForContentAtPath(knotName);
        if (object.Equals(knotTagList, null) || knotTagList.Count == 0)
            return false; // quick exit in case the tagList either doesn't exist or is empty
        foreach (string tag in _inkStory.TagsForContentAtPath(knotName))
        {
            int result = CheckDivertBlockTags(tag);
            if (result == -1)
                return true; //encountered
            else if (result == 0)
                HandleColonKeyTags(tag);
            // skip handling the tag via colon tags in 'case: 1'
        }
        return false;
    }

    // ContinueDialogue returns a bool
    //  - returns true if its parsed text is valid
    //  - returns false if its parsed text is invalid (encountered line command)
    //      - if encountered the stop command, null is considered valid parsed text
    private bool ContinueDialogue()
    {
        string parsedText = ParseCommands(intermediateBodyText);
        if (!Equals(parsedText, null)) //null is passed by ParseCommands for line commands
        {
            HandleLineTags(); //note that line tags are handled after all actions of ParseCommands
            dPanel.AttemptProgressDialogue(parsedText, currHeader);
            PlacePodiumSprites(currLeftPodiumSprite, currRightPodiumSprite);
            return true;
        }
        else
            return false;//!StateManager.GetDialogueStatus(); //returns false unless the dialogue has been stopped
        //AAP: Dialogue can't stop won't stop

    }
    private void PlacePodiumSprites(Sprite leftPodSpr, Sprite rightPodSpr)
    {
        leftPodium.SetCatSprite(leftPodSpr);
        rightPodium.SetCatSprite(rightPodSpr);
    }

    private string ParseCommands(string input)
    {
        //should handle line commands and inline commands.
        if (input.Length >= 3 && input.Substring(0, 3).Equals(">>>")) //hardcoded, bc >>> is expected
        {
            string lineCommand = input.Substring(3).Trim(); //removes >>> and then whitespace at the start and end
            HandleLineCommands(lineCommand);
            return null; //all line commands will return null (even invalid line commands)
        }
        else
            return HandleInlineCommands(input); //no need to even set bucketString = input
    }

    private void HandleLineCommands(string command)
    {
        bool handled = true;
        switch (command)
        {
            case "STOP_DIALOGUE": //wipes dialogue panel
                SetDialogueState(false);
                break;
            case "START_DIALOGUE": //phony, done to allow knot tags to work
                break;
            default:
                handled = false;
                break;
        }
        if (!handled)
        {
            if (!LineColonCommands(command))
                Debug.Log("Line command was not recognised: " + command);
        }
    }

    //returns success at handling command
    private bool LineColonCommands(string command)
    {
        //protocol for these line commands are: COMMAND:ARG1,ARG2,ARG3,...,ARGN
        //all parts of any command are expected to be case insensitive
        int colonIndex = command.IndexOf(':'); // should != -1
        if (colonIndex == -1 || command.IndexOf(':', colonIndex + 1) != -1)
            return false; // a quick check to guard against accidents. Can be altered if double colons are allowed

        //now time to generate argv
        char[] separators = new char[] { ':', ',' };
        string[] argv = command.ToUpper().Split(separators);
        for (int i = 0; i < argv.Length; i++) // I bet C programmers love seeing 'argv.Length'
            argv[i] = argv[i].Trim();
        try
        {
            return colonLineCommands[argv[0]](argv);
        }
        catch (KeyNotFoundException)
        {
            return false;
        }
    }
    private void DebugLogCmd(string[] argv) //not an actual cmd, will never be called by ink
    {
        int argc = argv.Length;
        if (argc == 0) return; //who would pass an empty array?
        StringBuilder sb = new StringBuilder();
        sb.Append(argv[0]);
        sb.Append(':');
        if (argc > 1) // if cmd composed of merely executable (I'm so AP pilled)
        {
            int i = 1;
            sb.Append(argv[i++]); //first arg
            while (i < argc)
            {
                sb.Append(',');
                sb.Append(argv[i++]);
            }
        }
        Debug.Log(sb.ToString());
    }


    /* Handle Inline Commands
     * Handling rich text has been removed as a fn, '$' now used as indicator of introducing L or S
     */
    private string HandleInlineCommands(string input)
    {
        if (readAsStageLines)
            return HandleInlineSpeaker(input);
        else
            return input;
    }



    // new protocol is being added, expected [speaker],[character],[emotion]
    // note that if [speaker] is a character, the 2nd option need not exist (but can)
    // if [emotion] DNE, assume default.
    // will read according to number of args
    // will always have character, and no sprite will be a character

    private string HandleInlineSpeaker(string input)
    {
        int colonIndex = input.IndexOf(':');
        if (colonIndex == -1)
        {
            HandleSpeakerTag("NO_SPEAKER");
            //HandleSpriteTag("NONE", true); does NOT remove sprites on narration for AAP
            dPanel.GreyOutText(true);
            return "<i>" + input + "</i>";
        }
        string preColon = input.Substring(0, colonIndex).Trim(); //doesn't include the colon
        string postColon = input.Substring(colonIndex + 1).Trim();
        //new protocol, split by commas
        string[] preColonArgs = preColon.Split(',');
        //if (argv.Length == 1) // must be speaker only
        HandleSpeakerTag(preColonArgs[0]); // speaker will always be first
        if (string.Equals(preColonArgs[0], "NO_SPEAKER"))
        {
            postColon = "<i>" + postColon + "</i>";
            dPanel.GreyOutText(true);
            //HandleSpriteTag("NONE", true); //AAP doesn't eliminate sprites via speakers
            return postColon;
        }
        string[] preColonCaps = new string[preColonArgs.Length];
        for (int i = 0; i < preColonArgs.Length; i++)
        {
            preColonCaps[i] = preColonArgs[i].Trim().ToUpper();
        }
        (CatObject, bool) catPosBucket;
        if (preColonArgs.Length == 1)
        {
            catPosBucket = CapsToCatExtended(preColonCaps[0]);
            HandleSpriteTag((catPosBucket.Item1, "DEFAULT"), catPosBucket.Item2);
        }
        else if (preColonArgs.Length == 2) //then either [spkr + char],[emo] or [spkr],[char + silent default]
        {
            catPosBucket = CapsToCatExtended(preColonCaps[1]);
            if (!object.Equals(catPosBucket.Item1, null)) // i.e. [spkr],[char + silent default]
                HandleSpriteTag((catPosBucket.Item1, "DEFAULT"), catPosBucket.Item2);
            else // must be [spkr + char],[emo]
            {
                catPosBucket = CapsToCatExtended(preColonCaps[0]);
                HandleSpriteTag((catPosBucket.Item1, preColonCaps[1]), catPosBucket.Item2); //bucket being null is legal here
            }
        }
        else //assert (preColonArgs.Length == 3) // must be [spkr],[char],[emo]
        {
            catPosBucket = CapsToCatExtended(preColonCaps[1]);
            HandleSpriteTag((catPosBucket.Item1, preColonCaps[2]), catPosBucket.Item2);
        }
        dPanel.GreyOutText(false); //reverts to default
        return postColon;
    }


    /* Note that HandleLineTags actually queries the inkstory for if there are ink tags on its call.
     *  - currTags are ONLY updated with new tags from the inkstory when this is called
     *      - this is because currTags are meant for regular lines of text
     *      - ParseCommands line commands may call _ink.Story.currentTags, but won't touch currTags
     *  - These features are intended for ParseCommands being called prior to HandleLineTags when reading text
     *      - ParseCommands will likely call _inkStory.Continue() for line commands
     *          - in this case, HandleLineTags should get the tags for the new line, not the old one
     */
    private void HandleLineTags()
    {
        currTags = _inkStory.currentTags;
        foreach (string tag in currTags)
        {
            if (!HandleColonKeyTags(tag))
            {
                /* only colon key tags are being handled right now.
                 *  - other types of handlers would go here
                 *  - this area is only reached when a tag is NOT a colon key tag
                 */
            }
        }
    }


    // for the sub tag interpret methods
    // if finds tag and handles it, returns true
    // else returns false
    private bool HandleColonKeyTags(string tag)
    {
        //checks a single string for a colon,
        // if it has it, checks if the terms before match
        bool isHandled = true;
        int colonIndex = tag.IndexOf(':');
        if (colonIndex != -1)
        {
            string preColon = tag.Substring(0, colonIndex); //doesn't include the colon
            string postColon = tag.Substring(colonIndex + 1).Trim();
            switch (preColon) //this could always be ToUpper-ed to allow for case insensitivity
            {
                case "speaker":
                    HandleSpeakerTag(postColon);
                    break;
                case "sprite": //this intentionally doesn't break and falls to the lSprite case
                case "lSprite":
                    HandleSpriteTag(postColon, true);
                    break;
                case "rSprite":
                    HandleSpriteTag(postColon, false);
                    break;
                case "audio": //not implemented!
                    HandleAudioTag(postColon);
                    break;
                case "READ_AS_STAGE_LINES":
                    if (postColon.ToUpper().Equals("TRUE"))
                        readAsStageLines = true;
                    else if (postColon.ToUpper().Equals("FALSE"))
                        readAsStageLines = false;
                    else
                        throw new System.ArgumentException("Screwed up READ_AS_STAGE_LINES tag");
                    break;
                default:
                    isHandled = false;
                    break;
            }
        }
        else
            isHandled = false;
        return isHandled;
    }

    private void HandleSpeakerTag(string tag)
    {
        //note that there will be many instances of non-character text
        //we may choose to have "" or "..." or whatever the protags name is
        currHeader = tag;
    }

    /* note that for non-speaker tags, the tags should be FILEPATHS
     *  - the filepaths start as if they originate at the Resources folder
     *  - the filepaths ultimately do NOT include the extension
     */
    private void HandleSpriteTag(string tag, bool isLeft)
    {
        //for errors here, note that file order must be preserved in Resources subfiles
        //  - so check there!
        if (isLeft)
        {
            if (tag.Equals("NONE"))
                currLeftPodiumSprite = null;
            else
                currLeftPodiumSprite = Resources.Load<Sprite>(tag);
        }
        else
        {
            if (tag.Equals("NONE"))
                currRightPodiumSprite = null;
            else
                currRightPodiumSprite = Resources.Load<Sprite>(tag);
        }
    }
    private void HandleSpriteTag(Sprite sprite, bool isLeft)
    {
        if (isLeft)
            currLeftPodiumSprite = sprite;
        else
            currRightPodiumSprite = sprite;
    }
    //this last overload 
    private void HandleSpriteTag((CatObject, string) key, bool isLeft)
    {
        Sprite sprite;
        try
        {
            sprite = key.Item1.SpriteDict[key.Item2];
        }
        catch (KeyNotFoundException)
        {
            sprite = null;
        }
        HandleSpriteTag(sprite, isLeft);
    }

    private void HandleAudioTag(string tag)
    {
        throw new System.NotImplementedException("HandleAudioTag isn't implemented yet");
    }



    /* returns 1 on finding a tag and not getting blocked 
     * returns 0 on not finding a tag
     * returns -1 on getting blocked
     */
    private int CheckDivertBlockTags(string tag)
    {
        int result = 0;
        int colonIndex = tag.IndexOf(':');
        if (colonIndex != -1)
        {
            string preColon = tag.Substring(0, colonIndex).ToUpper(); //doesn't include the colon
            string postColon = tag.Substring(colonIndex + 1).Trim();
            switch (preColon) //this could always be ToUpper-ed to allow for case insensitivity
            {
                case "BLOCK_IF_TRUE":
                    if (GetInkVar<bool>(postColon))
                        result = -1;//blocked by tag
                    else
                        result = 1; // found tag, but not blocked
                    break;
                case "BLOCK_IF_FALSE":
                    if (!GetInkVar<bool>(postColon))
                        result = -1; //blocked by tag
                    else
                        result = 1; // found tag, but not blocked
                    break;
                default:
                    break; //did not encounter blocker
            }
        }
        return result;
    }

    private bool CapsToBool(string str)
    {
        if (string.Equals(str, "TRUE"))
            return true;
        else if (string.Equals(str, "FALSE") || string.Equals(str, "NONE")) //allows NONE, but meant for FALSE
            return false;
        else
            throw new ArgumentException("CapsToBool received:" + str);
    }

    private CatObject CapsToCat(string str)
    {
        try
        {
            return VerDicts.CatDict[str];
        }
        catch (KeyNotFoundException)
        {
            return null;
        }
    }


    // as quick protocol, at the end of a cat name, $s affixed to end indicate extra commands
    // for instance, Paldo$L,doom places paldo on left podium with the doom sprite (not actual sprite name prolly)
    // so far only have $L and $R
    private (CatObject, bool) CapsToCatExtended(string str)
    {
        // check for podium placement
        int dollarIndex = str.IndexOf('$');
        if (dollarIndex == -1)
            return (CapsToCat(str), true); //assumes default (left)
        else
        {
            string preDollar = str.Substring(0, dollarIndex).Trim(); //doesn't include the '$'
            string postDollar = str.Substring(dollarIndex + 1).Trim();
            bool isLeft;
            if (postDollar[0] == 'L')
                isLeft = true;
            else if (postDollar[0] == 'R')
                isLeft = false;
            else
            {
                throw new ArgumentException("Invalid $-command: " + str);
            }
            return (CapsToCat(preDollar), isLeft);
        }
    }


}