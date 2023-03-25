using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCardState{
        drawpile,
        mine,
        target,
        discard
    }

public enum eCardType{
    normal,
    silver,
    gold
}
public class CardProspector : Card
{
    public eCardState state = eCardState.drawpile;
    public eCardType type = eCardType.normal;

    public List<CardProspector> hiddenBy = new List<CardProspector>();
    public int layoutID;
    public JSonLayoutSlot layoutSlot;

    public override void OnMouseUpAsButton()
    {
        Prospector.CARD_CLICKED(this);
    }

    public void ConvertToSilver(){
        type = eCardType.silver;
        back.GetComponent<SpriteRenderer>().sprite = CardSpritesSO.S.cardBackSilver;
        GetComponent<SpriteRenderer>().sprite = CardSpritesSO.S.cardFrontSilver;
    }
    public void ConvertToGold(){
        type = eCardType.gold;
        back.GetComponent<SpriteRenderer>().sprite = CardSpritesSO.S.cardBackGold;
        GetComponent<SpriteRenderer>().sprite = CardSpritesSO.S.cardFrontGold;

    }
}
