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


    // the events in the inspector is a Broadcaster-Only thing (listeners have listener components)
    [Header("Events")]
    [SerializeField] private GameEvent TextCrawlStateChange;
    

    private Color32 defaultTextColour = new(255, 255, 255, 255);
    private Color32 greyTextColour = new(180, 180, 180, 255);

    private readonly float CHAR_INTERVAL = 0.05f;

    // reminder that ink script should broadcast textCrawlStateChange(sender, true)
    // and only input handler/this file should broadcast textCrawlStateChange(sender, false)
    private bool textCrawlActive = false; //internally informs if textCrawl is active
    private bool textCrawlMarkedForDeath = false; // lets textcrawl be killed at coroutine update


    //private bool isTextCrawlOn = true; //not implemented, I can implement this later, but not for the proto.
    //if text crawl is added, will have to hold an intermediate string.



    // this has been repurposed from ToaF!


    // there could also be an sprite to appear to indicate the need to hit the enter button

    //there will have to be a hefty 'on disable' call

    //should be done just prior to enabling the DPanel


    //overloaded version in case one wants to ignore the Sprites
    public bool AttemptProgressDialogue(string bodyText, string headerText)
    {
        if (textCrawlActive)
        {
            textCrawlMarkedForDeath = true;
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
        StartCoroutine(TextCrawl(bodyText)); //FIXXX THIS SHOULD FLIP
                                             //A SWITCH ELSEWHERE THAT TELLS SPEAKER TO CONSTANTLY MEOW WHILE ACTIVE
    }
    private IEnumerator TextCrawl(string bodyText)
    {

        // should broadcast beginning of textcrawl FIXXXXXX

        BroadcastTextCrawlStart(); // tells the audio to start occurring

        char[] bodyTextCharArr = bodyText.ToCharArray();

        StringBuilder sb = new StringBuilder();

        //yield on a new YieldInstruction that waits for 5 seconds.
        //time progresses regardless of menu state, but backdrop being on or not is not that big of a deal
        for(int i = 0; i < bodyTextCharArr.Length; i++)
        {
            yield return new WaitForSeconds(CHAR_INTERVAL);
            sb.Append(bodyTextCharArr[i]);
            this.bodyText.text = sb.ToString();

            //if() MARKED FOR DEATH RESPONSE FIXXX
            if (textCrawlMarkedForDeath)
            {
                break;
            }
        }
        if (!textCrawlMarkedForDeath) //i.e. if the textcrawl naturally finishes
        {
            BroadcastTextCrawlEnd();
        }
        this.bodyText.text = bodyText;
        textCrawlMarkedForDeath = false;
    }

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
