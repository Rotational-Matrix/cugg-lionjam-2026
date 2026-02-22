using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPodium : MonoBehaviour
{
    [SerializeField] private SpriteRenderer catSR;
    [SerializeField] private GameObject podiumGameObject;


    private void Awake()
    {
        
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

}
