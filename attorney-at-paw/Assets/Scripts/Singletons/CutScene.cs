using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image fullscreenImage0;
    [SerializeField] private UnityEngine.UI.Image fullscreenImage1;
    private UnityEngine.UI.Image[] fsi = new UnityEngine.UI.Image[2];
    private int nextfsiIndex = 0;
    private readonly float alphaIncrement = 0.05f;
    // in the interest of time, enums aren't real.
    // 0 = off, 1 = turning on, 2 = on, 3 = turning off
    private int[] fsiState = {0,0};

    private void Awake()
    {
        fsi[0] = fullscreenImage0;
        fsi[1] = fullscreenImage1;
        Color new0clr = new Color(
            fullscreenImage0.color.r,
            fullscreenImage0.color.g,
            fullscreenImage0.color.b,
            0
            );
        fullscreenImage0.color = new0clr;

        Color new1clr = new Color(
            fullscreenImage0.color.r,
            fullscreenImage0.color.g,
            fullscreenImage0.color.b,
            0
            );
        fullscreenImage1.color = new1clr;
        AAPSingleton.cutScene = this;
    }

    private void Update()
    {
        AlphaChange(alphaIncrement, 0);
        AlphaChange(alphaIncrement, 1);
    }


    //ugh, fsiState is hardcoded protocol to 0,1,2,3 : off, turningOn, On, turningOff
    private void AlphaChange(float alphaIncremUnit, int fsiNum)
    {
        //float currAlpha = blackBackdrop.color.a;
        //blackBackdrop.color = currAlpha;
        Color clr = fsi[fsiNum].color;
        //float alpha = clr.a + alphaIncremUnit;
        float alpha;
        switch (fsiState[fsiNum])
        {
            case 0: // off
                return;
            case 1: // turning on
                alpha = clr.a + alphaIncremUnit;
                if (alpha < 1)
                {
                    fsi[fsiNum].color = new Color(clr.r, clr.g, clr.b, alpha);
                }
                else
                {
                    fsi[fsiNum].color = new Color(clr.r, clr.g, clr.b, 1);
                    fsiState[fsiNum] = 2; // now on
                }
                break;
            case 2: // on
                return;
            case 3: // turning off
                alpha = clr.a - alphaIncremUnit;
                if (alpha > 0)
                {
                    fsi[fsiNum].color = new Color(clr.r, clr.g, clr.b, alpha);
                }
                else
                {
                    fsi[fsiNum].color = new Color(clr.r, clr.g, clr.b, 0);
                    fsiState[fsiNum] = 0; // now off
                }
                break;
        }
    }
    public void InitiateCutScene(Sprite image)
    {
        if (fsiState[nextfsiIndex] != 0) //if not off
        {
            Color clr = fsi[nextfsiIndex].color;
            fsi[nextfsiIndex].color = new Color(clr.r, clr.g, clr.b, 0);
            fsiState[nextfsiIndex] = 0; // now off (technically useless here, but whatever
        }
        fsi[nextfsiIndex].sprite = image; //set image
        fsiState[nextfsiIndex] = 1; //turning on
        int notNextIndex = 0x1 ^ nextfsiIndex; // we LOVE bitwise XOR (note there should only be indexes 0,1)
        fsiState[notNextIndex] = 3; // turning off
        nextfsiIndex = notNextIndex; // change next next fsi
    }
    public void ClearCutScenes()
    {
        for (int i = 0; i < fsiState.Length; i++)
        {
            fsiState[i] = (fsiState[i] == 0) ? 0 : 3; // set it to off or turning off
        }
    }
}
