using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "CardSprites", menuName = "ScriptableObjects/CardSpritesSO")]
public class CardSpritesSO : ScriptableObject
{
    public static CardSpritesSO S;

     public Sprite cardBack;
     public Sprite cardBackGold;
     public Sprite cardFront;
    public Sprite cardFrontGold;
    public Sprite cardFrontSilver;
    public Sprite cardBackSilver;


    public Sprite suitDiamond;
    public Sprite suitClub;
    public Sprite suitHeart;
    public Sprite suitSpade;

    public List<Sprite> faceSprites;
    public List<Sprite> rankSprites;

    public static Dictionary<char, Sprite> SUITS { get; private set;}

    public void Init(){
        INIT_STATICS(this);
    }

    static void INIT_STATICS(CardSpritesSO cSSO){
        if (S!=null)
        {
            return;
        }
        S = cSSO;


        SUITS = new Dictionary<char, Sprite>{
            {'C', S.suitClub},
            {'D', S.suitDiamond},
            {'H', S.suitHeart},
            {'S', S.suitSpade}
        };
    }

    public static List<Sprite> RANKS{
        get{ return S.rankSprites;}
    }

    public static Sprite GET_FACE(string name){
        foreach (Sprite item in S.faceSprites)
        {
            if (item.name == name) return item;
        }
        return null;
    }

    public static Sprite BACK{
        get { return S.cardBack; }
    }

    public static void RESET(){
        S = null;
    }

}
