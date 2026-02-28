using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPodium : MonoBehaviour
{
    [SerializeField] private SpriteRenderer catSR;
    [SerializeField] private GameObject podiumBackground;

    [SerializeField] private bool isLeft;

    private readonly Vector3 paldoSizedPos = new Vector3(0, 0, 0);
    private readonly Vector3 paldoSizedScl = new Vector3(1, 1, 1);
    private readonly Vector3 msPaintSizedPos = new Vector3(0, 0.07f, 0);
    private readonly Vector3 msPaintSizedScl = new Vector3(0.25f, 0.25f, 1);

    private void Awake()
    {
        if (isLeft)
        {
            AAPSingleton.catPodL = this;
        }
        else
            AAPSingleton.catPodR = this;
    }
    public void SetCatSprite(Sprite newSprite)
    {
        if(object.Equals(newSprite,null))
        {
            catSR.gameObject.SetActive(false);
        }
        else
        {
            catSR.sprite = newSprite;
            catSR.gameObject.SetActive(true);
        }
    }

    //will take string and perform protocol itself so that it can distinguish 
    //     paldo and non-paldo
    // paldo is mapped properly and requires 1x scale w/ 0,0,0 coords
    // no one else is, and they require 0.25x scale w/ 0,0.07,0 coords
    public void TimeCrunchSetCatSprite(Sprite sprite, bool isPaldoSized)
    {
        if (object.Equals(sprite, null))
        {
            catSR.gameObject.SetActive(false);
        }
        else
        {
            Vector3 locPosition = isPaldoSized ? paldoSizedPos : msPaintSizedPos;
            Vector3 locScale = isPaldoSized ? paldoSizedScl : msPaintSizedScl;
            catSR.transform.localPosition = locPosition;
            catSR.transform.localScale = locScale;
            catSR.sprite = sprite;
            catSR.gameObject.SetActive(true);
        }
    }

    public Vector3 GetBackgroundPos()
    {
        return podiumBackground.transform.position;
    }



}
