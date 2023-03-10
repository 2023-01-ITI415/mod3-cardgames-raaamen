using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardSpace{
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CardScriptableObj")]
public class CardObj : ScriptableObject
{
    public string cardName;

    public CardSuit cardSuit;
    public enum CardSuit{
        Spades,
        Cloves,
        Hearts,
        Diamonds
    }
    public bool faceCard;

    public List<Sprite> cardSprites;


    //0 position is front, 1 position is back
    public List<Sprite> frontBackList;
   
}
}
