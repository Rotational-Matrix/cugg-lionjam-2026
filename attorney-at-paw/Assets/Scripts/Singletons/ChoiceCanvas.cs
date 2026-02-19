
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;



// Stolen from ChoiceCanvasHandler to instead navigate evidence options
// Evidence options are in an array of sorts, so this has to be accounted for (up,down,left,right)
// reminder that evidence options are boxes to choose from, but evidence items fill said boxes

//ink will give mere array of 'choices' which will just be evidence names (legitimate evidence names in VerDicts)

public class ChoiceCanvas : MonoBehaviour
{
    [SerializeField] private List<EvidenceOption> evidenceOptions = new List<EvidenceOption>();
    //[SerializeField] private GameObject perish;

    private int selectorIndex = -1;
    private int choicesAvailable = 0;
    private readonly int MAX_POSS_CHOICES = 0x7fffffff; //largest signed integer
    private readonly int ROW_LENGTH = 5; //unknown currently FIXXX
    private readonly HorizBoundaryBehaviour horizBounds = HorizBoundaryBehaviour.Wrap;
    private readonly VertBoundaryBehaviour vertBounds = VertBoundaryBehaviour.Stop;

    private enum HorizBoundaryBehaviour
    {
        Wrap, //as in Torus behaviour, horizontal movement into edges wrap to other side (no vertical change)
        Crlf, //as in going to right wraps and goes down one row (I have left doing the opposite) provided it is possible
        Stop  //as in horizantal movement is stopped at edges
    }
    private enum VertBoundaryBehaviour
    {
        Wrap, //as in Torus behaviour, vertical edges wrap to other side (no vertical change)
        Stop  //as in vertical movement is stopped at edges
    }

    //InitiateChoices is how choice options are set up, and it returns a bool indicating its success

    //unlike ChoiceCanvasManager, this option must give options in a 2d array (for <^v> mvmnt)
    //grids will fill top left across, row by row, like in the following example (letting row length be 5)
    /* 0 1 2 3 4
     * 5 6 7 8 9 
     * a b c d e 
     * f
     */
    //This is assumed as a precondition, so we can say:
    /* up:    index -= ROW_LENGTH
     * down:  index += ROW_LENGTH
     * left:  index += 1
     * right: index 
     */




    public bool InitiateEvidenceSelection(string[] choiceArr)
    {
        if (choiceArr.Length > MAX_POSS_CHOICES || choiceArr.Length == 0 ||
            choicesAvailable != 0)
        {
            //if faulty array is passed, or another choice is already active:
            return false;
        }

        choicesAvailable = choiceArr.Length;
        selectorIndex = 0;

        for (int i = 0; i < choiceArr.Length; i++)
        {
            
            evidenceOptions[i].gameObject.SetActive(true);
            EvidenceOption evidOption = evidenceOptions[i];
            evidOption.SetEvidence(choiceArr[i]);
            if (i == 0) //set the first option selected by default
                evidOption.SetSelected(true);
        }

        return true; // to indicate success
    }

    //attempts to 'Increment' the choice selection. now decrements the index to 'move up'
    private void MoveSelector(int newIndex)
    {
        if (newIndex != selectorIndex)
        {
            evidenceOptions[selectorIndex].SetSelected(false); //unselect original index
            evidenceOptions[newIndex].SetSelected(true); // select new index
            selectorIndex = newIndex; //inform selector index of its new position
        }
    }
    public void MoveSelectorUp() //genuinely is up
    {
        if (selectorIndex < choicesAvailable && selectorIndex >= 0)
        {
            int newIndex = selectorIndex - ROW_LENGTH;
            if (newIndex < 0)
            {
                switch (vertBounds)
                {
                    case VertBoundaryBehaviour.Wrap: // I'm not explaining this, the math works out - [Cu]
                        newIndex = choicesAvailable - (choicesAvailable % ROW_LENGTH) + (selectorIndex % ROW_LENGTH);
                        if (newIndex >= choicesAvailable)
                            newIndex -= ROW_LENGTH;
                        break;
                    case VertBoundaryBehaviour.Stop:
                        newIndex = selectorIndex; // do not move
                        break;
                }
            }
            MoveSelector(newIndex);
        }
    }

    public void MoveSelectorDown() //genuinely is down
    {
        if (selectorIndex < choicesAvailable && selectorIndex >= 0)
        {
            int newIndex = selectorIndex + ROW_LENGTH;
            if (newIndex >= choicesAvailable)
            {
                switch (vertBounds)
                {
                    case VertBoundaryBehaviour.Wrap:
                        newIndex %= ROW_LENGTH;
                        break;
                    case VertBoundaryBehaviour.Stop:
                        newIndex = selectorIndex; // do not move
                        break;
                }
            }
            MoveSelector(newIndex);
        }
    }
    public void MoveSelectorLeft() //genuinely is left
    {
        if (selectorIndex < choicesAvailable && selectorIndex >= 0)
        {
            int newIndex = selectorIndex - 1;
            if (selectorIndex % ROW_LENGTH == 0) // if 'left boundary' would be hit
            {
                switch (horizBounds)
                {
                    case HorizBoundaryBehaviour.Wrap:
                        newIndex += ROW_LENGTH; //moves to rightmost option on same row (may be out of bounds)
                        if (newIndex >= choicesAvailable)
                            newIndex = choicesAvailable - 1; // if out of bounds, set to within bounds
                        break;
                    case HorizBoundaryBehaviour.Crlf:
                        if (newIndex < 0) //only causes problem when at first index.
                            newIndex = 0;
                        break;
                    case HorizBoundaryBehaviour.Stop:
                        newIndex = selectorIndex; // do not move
                        break;
                }
            }
            MoveSelector(newIndex);
        }
    }
    public void MoveSelectorRight() //genuinely is right
    {
        if (selectorIndex < choicesAvailable && selectorIndex >= 0)
        {
            int newIndex = selectorIndex + 1;
            if (newIndex % ROW_LENGTH == 0 || newIndex >= choicesAvailable) // if 'right boundary' was hit
            {
                switch (horizBounds)
                {
                    case HorizBoundaryBehaviour.Wrap:
                        newIndex -= ROW_LENGTH; //moves leftmost option on same row (unless on uneven final row)
                        if (newIndex % ROW_LENGTH != 0)
                            newIndex += ROW_LENGTH - (choicesAvailable % ROW_LENGTH); // if out of bounds, set to within bounds
                        break;
                    case HorizBoundaryBehaviour.Crlf:
                        if (newIndex >= choicesAvailable) //only causes problem when at last index.
                            newIndex = choicesAvailable - 1;
                        break;
                    case HorizBoundaryBehaviour.Stop:
                        newIndex = selectorIndex; // do not move
                        break;
                }
            }
            MoveSelector(newIndex);
        }
    }

    public int Choose()
    {
        int retIndex = selectorIndex;
        RemoveChoices();
        return retIndex; //this returns the 0-indexed verion of the index
    }

    public bool IsDisplaying()
    {
        return choicesAvailable != 0;
    }

    private void RemoveChoices()
    {
        if (choicesAvailable != 0)
        {
            foreach (EvidenceOption evidOpt in evidenceOptions)
            {
                evidOpt.SetEvidence("");
            }
            choicesAvailable = 0;
            selectorIndex = -1;
        }
    }

    private void Awake()
    {
        foreach (EvidenceOption evidOpt in evidenceOptions)
        {
            evidOpt.SetEvidence("");
        }
        choicesAvailable = 0;
        selectorIndex = -1;
    }




}
