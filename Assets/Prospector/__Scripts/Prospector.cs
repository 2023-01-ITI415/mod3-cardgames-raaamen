using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prospector : MonoBehaviour
{
    private static Prospector S;
    public List<CardProspector> drawPile;
    public List<CardProspector> discardPile;
    public List<CardProspector> mine;
    public CardProspector target;
    private Transform layoutAnchor;
    private Deck deck;
    private JsonLayout jsonLayout;
    private Dictionary<int, CardProspector> mineIdToCardDict;
    // Start is called before the first frame update
    void Start()
    {
        if (S != null) Debug.LogError("Attempted to set S more than once!");
        S = this;
        jsonLayout = GetComponent<JsonParseLayout>().layout;
        deck = GetComponent<Deck>();
        deck.InitDeck();
        Deck.Shuffle(ref deck.cards);
        drawPile =ConvertCardsToCardProspectors(deck.cards);
        LayoutMine();
        MoveToTarget(Draw());
        UpdateDrawPile();
    }

    CardProspector Draw(){
        CardProspector cp = drawPile[0];
        drawPile.RemoveAt(0);
        return cp;
    }

    List<CardProspector> ConvertCardsToCardProspectors(List<Card> listCard){
        List<CardProspector> listCP = new List<CardProspector>();
        CardProspector cp;
        foreach (Card card in listCard)
        {
            cp = card as CardProspector;
            listCP.Add(cp);
        }
        return (listCP);
    }

    void LayoutMine(){
        if (layoutAnchor==null)
        {
            GameObject tGO = new GameObject("_LayoutAnchor");
            layoutAnchor = tGO.transform;
        }
        CardProspector cp;
        mineIdToCardDict = new Dictionary<int, CardProspector>();

        foreach (var slot in jsonLayout.slots)
        {
            cp = Draw();
            cp.faceUp = slot.faceUp;
            cp.transform.SetParent(layoutAnchor);
            int z = int.Parse(slot.layer[slot.layer.Length -1].ToString());
            cp.SetLocalPos( new Vector3(
                jsonLayout.multiplier.x * slot.x,
                jsonLayout.multiplier.y * slot.y,
                -z));
                cp.layoutID = slot.id;
                cp.layoutSlot = slot;
                cp.state= eCardState.mine;
                cp.SetSpriteSortingLayer(slot.layer);
                mine.Add(cp);
                mineIdToCardDict.Add(slot.id, cp);
        }

    }

    void MoveToDiscard(CardProspector cp){
        cp.state=eCardState.discard;
        discardPile.Add(cp);
        cp.transform.SetParent(layoutAnchor);
        cp.SetLocalPos(new Vector3(
            jsonLayout.multiplier.x*jsonLayout.discordpile.x,
            jsonLayout.multiplier.y*jsonLayout.discordpile.y,
            0)
        );
        cp.faceUp=true;
        cp.SetSpriteSortingLayer(jsonLayout.discordpile.layer);
        cp.SetSortingOrder(-200+(discardPile.Count*3));
    }
    void MoveToTarget(CardProspector cp){
        if(target!=null) MoveToDiscard(target);
        MoveToDiscard(cp);
        target = cp;
        cp.state = eCardState.target;
        cp.SetSpriteSortingLayer("Target");
        cp.SetSortingOrder(0);
    }
    void UpdateDrawPile(){
        CardProspector cp;
        for (int i = 0; i < drawPile.Count; i++)
        {
            cp = drawPile[i];
            cp.transform.SetParent(layoutAnchor);
            Vector3 cpPos = new Vector3();
            cpPos.x = jsonLayout.multiplier.x * jsonLayout.drawPile.x;
            cpPos.x += jsonLayout.drawPile.xStagger * i;
            cpPos.y = jsonLayout.multiplier.y *jsonLayout.drawPile.y;
            cpPos.z = 0.1f * i;
            cp.SetLocalPos(cpPos);
            cp.faceUp = false;
            cp.state = eCardState.drawpile;
            cp.SetSpriteSortingLayer(jsonLayout.drawPile.layer);
            cp.SetSortingOrder(-10 * i);
        }
    }

    public void SetMineFaceUps(){
        CardProspector coverCP;
        foreach (CardProspector cp in mine)
        {
            bool faceUp = true;
            foreach (var coverID in cp.layoutSlot.hiddenBy)
            {
                coverCP = mineIdToCardDict[coverID];
                if (coverCP==null||coverCP.state == eCardState.mine)
                {
                    faceUp = false;
                }
            }
            cp.faceUp = faceUp;
        }
    }

    public static void CARD_CLICKED(CardProspector cp){
        switch (cp.state)
        {
            case eCardState.target:
                break;
            case eCardState.drawpile:
                S.MoveToTarget(S.Draw());
                S.UpdateDrawPile();
                break;
            case eCardState.mine:
                bool validMatch = true;
                if (!cp.faceUp) validMatch = false;
                if (!cp.AdjacentTo(S.target)) validMatch=false;
                if (validMatch)
                {
                    S.mine.Remove(cp);
                    S.MoveToTarget(cp);
                    S.SetMineFaceUps();
                }
                break;
        }
    }
}