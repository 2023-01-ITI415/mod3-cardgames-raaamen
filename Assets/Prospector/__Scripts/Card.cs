using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardSpace;

public class Card : MonoBehaviour
{
    public CardObj cardObject;
    public CardObj.CardSuit cardsSuit;
    private bool _frontFacing;
    public bool frontFacing {
        get {
            return _frontFacing;
        }
        set {
            switch (value)
            {  
                case true:
                    //change sprite renderer to front facing
                    SwapFace(0);
                    _frontFacing = true;
                    break;
                case false:
                    //change sprite renderer to front facing
                    //maybe include animation in here
                    SwapFace(1);
                    _frontFacing = false;
                    break;
            }
        }
    }

    //card gameobjects
    public GameObject face;
    public GameObject frontback;
    public GameObject suit;
    public GameObject rank;


    //component references
    public SpriteRenderer spriteRender;
    public Vector3 faceSpritePosition = Vector2.zero;
    public Vector3 suitSpritePosition;
    private void Awake() {
        spriteRender = GetComponent<SpriteRenderer>();
        InitializeCard();
    }

    void InitializeCard(){
        this.name = cardObject.name;
        this.cardsSuit = cardObject.cardSuit;
        Debug.Log("Instantiating "+ this.name + "of "+ this.cardsSuit);

        var faceObj = Instantiate(new GameObject(), faceSpritePosition, Quaternion.identity);
        faceObj.transform.parent = gameObject.transform;

    }


    void SwapFace(int value){
        spriteRender.sprite = cardObject.frontBackList[value];
        //play animation
    }
}
