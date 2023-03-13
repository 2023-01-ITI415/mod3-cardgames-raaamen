using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCardState{
        drawpile,
        mine,
        target,
        discard
    }
public class CardProspector : Card
{
    public eCardState state = eCardState.drawpile;

    public List<CardProspector> hiddenBy = new List<CardProspector>();
    public int layoutID;
    public JSonLayoutSlot layoutSlot;

    public override void OnMouseUpAsButton()
    {
        Prospector.CARD_CLICKED(this);
    }
}
