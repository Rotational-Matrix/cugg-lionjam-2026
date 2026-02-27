using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputHandler : MonoBehaviour
{
    /// <summary>
    /// [Cu]'s documentation
    /// 
    /// Adapted from ToaF, the only file in AAP that should know about user input
    ///     
    /// 
    /// </summary>

    private CatDCM dcManager;
    private bool mrg = true; //master regulatory gene


    // PLEASE: observe unity inputManager 1.15 (or any version, really)
    // I have been using the legacy KeyCode all this time w/o realising it, keyboard and key is so much better!

    //------------------------ keyboard -----------------------------
    public static UnityEngine.InputSystem.Keyboard CurrentKeyboard;

    [SerializeField] public bool DebugKeysActive; //if true, allows dangerous debug actions

    //--------------------- Dialogue Keys ---------------------------

    /* dialogueKey does the following: 
     *  - Progress to the next line of dialogue
     *  - Initiate choice selection (when the dialogue hits a choice)
     */
    public static Key dialogueKey = Key.Space;
    public static Key alterDialogueKey = Key.Enter;

    /* commitChoiceKey does the following:
     *  - Commits the currently selected/highlighted choice
     *  
     *  NOTE: this is currently the SAME key as dialogueKey
     */
    public static Key commitChoiceKey = Key.Space;
    public static Key alterCommitChoiceKey = Key.Enter;

    /* mvSelectUp, mvSelectDown:
     *  - for choice selection, these commands move the selector up and down
     *      - note that 'up' and 'down' refer to how the user perceives up and down
     *      - also note that the functions behind this movement are not as consistent
     *          - i.e. ChoiceCanvas Increment means Down, but GridSelector Increment means up
     *              - I should probably change this, but regardless, these issues are documented as they appear below
     */
    public static Key mvSelectUp = Key.UpArrow;
    public static Key altMvSelectUp = Key.W;

    public static Key mvSelectDown = Key.DownArrow;
    public static Key altMvSelectDown = Key.S;

    public static Key mvSelectLeft = Key.LeftArrow;
    public static Key altMvSelectLeft = Key.A;

    public static Key mvSelectRight = Key.RightArrow;
    public static Key altMvSelectRight = Key.D;

    //------------------- Pause Menu Keys ---------------------------

    public static Key openConfigKey = Key.Tab; //won't do anything in AAP
    public static Key exitKey = Key.Escape;    //won't do anything in AAP

    //------------------- Roam State Keys ---------------------------

    public static Key interactKey = Key.Space; //also nothing to interact w/

    //--------------------- Debug Keys ------------------------------

    /* debug_forceStartDialogue does the following:
     *  - opens the dialogue and inkstory at wherever it currently is.
     *  
     * debug_forceJumpDialogue does the following:
     *  - opens the dialogue and inkstory at specified knotName
     *      - knotName can be written in the inspector
     *  
     * NOTE: 
     *  - *not* buttons players should have access to.
     *  - can (and will) break dialogue if used haphazardly
     *  - forced jumping will blow up if a non-real knotName is used
     */
    private static Key debug_forceStartDialogue = Key.RightBracket;
    private static Key debug_forceJumpDialogue = Key.O;
    //public static Key debug_moveSariel = Key.Q; AUGH I WISH I HAD REMOVED THIS SOONER!
    [SerializeField] public string forceJumpKnotName;


    private void Start() //has to be start to guarantee it occurs after StateManager.Awake()
    {
        dcManager = AAPSingleton.dcm;
    }


    private void Awake()
    {
        CurrentKeyboard = UnityEngine.InputSystem.Keyboard.current;
        AAPSingleton.inputHandler = this;
    }

    //------------------ The Update function ------------------------
    private void Update()
    {
        if (CurrentKeyboard.anyKey.wasPressedThisFrame && mrg)
        {
            foreach (KeyControl key in Keyboard.current.allKeys)
            {
                if ((!object.Equals(key, null)) && key.wasPressedThisFrame)
                {
                    DistributeInput(key.keyCode);
                    //Debug.Log("Key pressed: " + key.keyCode);
                }
            }
        }
    }

    public void ForceInputActive(bool value)
    {
        mrg = value;
    }

    private void DistributeInput(Key keyPressed)
    {
        DialogueInputHandler(keyPressed); //handles dialogue and choice states
    }


    //all cases of non menuStack 'pseudo menus' (choice/dialogue)
    private void DialogueInputHandler(Key keyPressed)
    {
        /* Why does this look so stupid? Observe:
         *  - OnGui gets called every time an input happens, and the code reaches here every keyboard input
         *      - this is the least taxing when non-relevant keypresses only have to check == with a few keys
         *  - This is not in switchcase because the keys might be assigned to the same button (esp choose & dialogue)
         * 
         */
        if (dcManager.IsChoiceActive())
        {
            if (keyPressed == mvSelectUp || keyPressed == altMvSelectUp)
                dcManager.MoveSelectorUp();
            else if (keyPressed == mvSelectDown || keyPressed == altMvSelectDown)
                dcManager.MoveSelectorDown();
            else if (keyPressed == mvSelectLeft || keyPressed == altMvSelectLeft)
                dcManager.MoveSelectorLeft();
            else if (keyPressed == mvSelectRight || keyPressed == altMvSelectRight)
                dcManager.MoveSelectorRight();
            else if (keyPressed == commitChoiceKey || keyPressed == alterCommitChoiceKey)
            {
                dcManager.Choose();
                dcManager.AttemptContinue();
            }
        }
        else if (keyPressed == dialogueKey || keyPressed == alterDialogueKey) //choice cannot have started active
        {
            if (!dcManager.IsChoiceActive())
            {
                if (!dcManager.AttemptContinue())
                    dcManager.InitiateChoices();
            }
        }
    }







}