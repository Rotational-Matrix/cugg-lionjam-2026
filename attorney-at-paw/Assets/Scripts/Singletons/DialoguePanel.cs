using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Windows;
using System.Text;

public class DialoguePanel : MonoBehaviour
{
    // The following fields are expected to be children of the dialogue panel (or attached to children)
    [SerializeField] private TMPro.TMP_Text bodyText;
    [SerializeField] private GameObject headerPanel;
    [SerializeField] private TMPro.TMP_Text headerText;
    private Coroutine textCrawlCoroutine;

    // the events in the inspector is a Broadcaster-Only thing (listeners have listener components)
    [Header("Events")]
    [SerializeField] private GameEvent TextCrawlStateChange;
    

    private Color32 defaultTextColour = new(255, 255, 255, 255);
    private Color32 greyTextColour = new(255, 255, 255, 255); //new (180, 180, 180, 255); not grey!

    private readonly float CHAR_INTERVAL = 0.05f;

    // reminder that ink script should broadcast textCrawlStateChange(sender, true)
    // and only input handler/this file should broadcast textCrawlStateChange(sender, false)
    private string coroutineOnCreationText;

    //private bool isTextCrawlOn = true; //not implemented, I can implement this later, but not for the proto.
    //if text crawl is added, will have to hold an intermediate string.



    // this has been repurposed from ToaF!


    // there could also be an sprite to appear to indicate the need to hit the enter button

    //there will have to be a hefty 'on disable' call

    //should be done just prior to enabling the DPanel


    //overloaded version in case one wants to ignore the Sprites
    public bool AttemptProgressDialogue(string bodyText, string headerText)
    {
        if (InstantKillTextCrawl())
        {
            return false;
        }
        else
        {
            SetBodyText(bodyText);
            SetHeaderText(headerText);
            return true;
        }
    }


    public void SetBodyText(string bodyText)
    {
        textCrawlCoroutine = StartCoroutine(TextCrawl(bodyText)); //FIXXX THIS SHOULD FLIP
                                             //A SWITCH ELSEWHERE THAT TELLS SPEAKER TO CONSTANTLY MEOW WHILE ACTIVE
    }
    private IEnumerator TextCrawl(string bodyText)
    {

        // should broadcast beginning of textcrawl FIXXXXXX
        coroutineOnCreationText = bodyText; //added only to prevent edgecase.
        BroadcastTextCrawlStart(); // tells the audio to start occurring

        char[] bodyTextCharArr = bodyText.ToCharArray();

        StringBuilder sb = new StringBuilder();

        //yield on a new YieldInstruction that waits for 5 seconds.
        //time progresses regardless of menu state, but backdrop being on or not is not that big of a deal
        for(int i = 0; i < bodyTextCharArr.Length; i++)
        {
            if (bodyTextCharArr[i] == '<') //rich text incoming
            {
                int richTextLen = LengthToIncludeChar(bodyTextCharArr, i, '>');
                if (richTextLen != -1)
                {
                    sb.Append(bodyTextCharArr, i, richTextLen);
                    i += richTextLen; //to basically skip all said cycles
                    if (i == bodyTextCharArr.Length)
                    {
                        break; // since the end has already been hit
                    }
                }
                
            }
            yield return new WaitForSeconds(CHAR_INTERVAL);
            sb.Append(bodyTextCharArr[i]);
            this.bodyText.text = sb.ToString();
        }
        TextCrawlEndBehavior();
    }

    private void TextCrawlEndBehavior()
    {
        BroadcastTextCrawlEnd();
        this.bodyText.text = coroutineOnCreationText;
        textCrawlCoroutine = null;
    }

    //killing on update was foolish.
    public bool InstantKillTextCrawl()
    {
        if (textCrawlCoroutine != null)
        {
            StopCoroutine(textCrawlCoroutine);
            TextCrawlEndBehavior();
            return true;
        }
        return false;
    }

    //the text crawl looks really stupid whenever I put in rich text, this solves that
    //returns the number of characters including the one at currIndex to the one at sought
    // e.g. let arr be a char array: qz<i>xw, then
    //      LengthToIncludeChar(arr, 2, '>') -> outputs 3 (bc it is referrring to '<','i','>')
    private int LengthToIncludeChar(char[] arr, int currIndex, char sought)
    {
        int found = -1;
        for (int i = 1;  i < arr.Length - currIndex; i++) //length of 0 not permitted
        {
            if(arr[currIndex + i] == sought)
            {
                found = i + 1; //bc it has to return number of characters inclusive
                break;
            }
        }
        return found;
    } // FUTURE REFERENCE: Array.IndexOf() is a function!

    /*public void SetBodyText(string bodyText)
    {
        this.bodyText.text = bodyText;
    }*/


    // this file also listens to OnTextCrawlStateChange NO IT DOESN'T, GET OUT OF MY HEAD
    /*
    private void OnTextCrawlStateChange(Component sender, object data) //data expected to be bool
    {
        if (sender is DialoguePanel) //preferred if this file ignores its own broadcasts
        {
            return;
        }
        if (data is bool) //which, by the way, it should be
        {
            bool value = (bool)data;
            if (value && )
            {

            }
        }
    }*/


    // these should strt containing audio!
    private void BroadcastTextCrawlEnd()
    {
        TextCrawlStateChange.Raise(this, false);
    }
    private void BroadcastTextCrawlStart()
    {
        TextCrawlStateChange.Raise(this, true);
    }






    //SetHeaderText(headerText) is also responsible for the active state of the header panel
    //  - receiving "" or "NO_SPEAKER" will disable the header panel
    //  - receiving anything else will activate the header panel and set the headerText

    public void SetHeaderText(string headerText)
    {
        this.headerText.text = headerText;
        if (headerText.Equals("") || headerText.Equals("NO_SPEAKER"))
        {
            this.headerText.text = "";
            this.headerPanel.SetActive(false);
        }
        else
        {
            this.headerText.text = headerText;
            this.headerPanel.SetActive(true);
        }
    }

    //for convenience, specifically restores default on passing false as param
    public void GreyOutText(bool becomeGrey)
    {
        if (becomeGrey)
            bodyText.color = greyTextColour;
        else
            bodyText.color = defaultTextColour;
    }


    private void CleanDPanel()
    {
        //This is done so that, once DPanel is re-enabled, its sprites are not shown unless they should be.
        SetBodyText("");
        SetHeaderText("");
        bodyText.color = defaultTextColour;
    }




    private void OnDisable()
    {
        CleanDPanel();
    }

}
