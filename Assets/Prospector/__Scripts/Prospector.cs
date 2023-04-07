using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Prospector : MonoBehaviour
{

    public enum GameState{
        Prospector,
        Golf
    }

    public static GameState currentGame;

    private static Prospector S;
    public float roundDelay = 2f;
    public List<CardProspector> drawPile;
    public List<CardProspector> discardPile;
    public List<CardProspector> mine;

    public List<CardProspector> potentialSpecialCards;

    public List<float> silverCardChances;
    public List<float> goldCardChances;
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
        SpecialCards();
        SpecialCardsGold();
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
            potentialSpecialCards.Add(cp);
        }
        
        return (listCP);
    }

    void SpecialCards(){
        int cardsToMake = 0;
        float random = Random.value;
        for (int i = 0; i < silverCardChances.Count; i++)
        {
            if (random <= silverCardChances[i]) cardsToMake++;
            Debug.Log(cardsToMake);
        }

        for (int i = 0; i < cardsToMake; i++)
        {
            int rand = Random.Range(0, potentialSpecialCards.Count);
            potentialSpecialCards[rand].ConvertToSilver();
            potentialSpecialCards.RemoveAt(rand);
        }

    }

    void SpecialCardsGold(){
        int cardsToMake = 0;
        float random = Random.value;
        for (int i = 0; i < goldCardChances.Count; i++)
        {
            if (random <= goldCardChances[i]) cardsToMake++;   
            Debug.Log(cardsToMake);
        }

        for (int i = 0; i < cardsToMake; i++)
        {
            int rand = Random.Range(0, potentialSpecialCards.Count);
            potentialSpecialCards[rand].ConvertToGold();
            potentialSpecialCards.RemoveAt(rand);
        }
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
            jsonLayout.multiplier.x*jsonLayout.discardpile.x,
            jsonLayout.multiplier.y*jsonLayout.discardpile.y,
            0)
        );
        cp.faceUp=true;
        cp.SetSpriteSortingLayer(jsonLayout.discardpile.layer);
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

    void CheckForGameOver(){
        if (mine.Count == 0)
        {
            GameOver(true);
            return;
        }
        if(drawPile.Count>0) return;
        foreach (var cp in mine)
        {
            if(target.AdjacentTo(cp)) return;
        }
        GameOver(false);
    }

    void GameOver(bool won){
        if(won) ScoreManager.TALLY(eScoreEvent.gameWin);
        else ScoreManager.TALLY(eScoreEvent.gameLoss);
        CardSpritesSO.RESET();
        Invoke("ReloadLevel",roundDelay);
        UITextManager.GAME_OVER_UI(won);
    }

    void ReloadLevel(){
        SceneManager.LoadScene("__Prospector_Scene_0");
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
                    if (cp.type == eCardType.silver)
                    {
                        ScoreManager.TALLY(eScoreEvent.mine);
                    }
                    S.mine.Remove(cp);
                    S.MoveToTarget(cp);
                    if (currentGame == GameState.Prospector)
                    {
                        S.SetMineFaceUps();
                    }
                    if (currentGame == GameState.Golf){
                        S.SetMineFaceUps();
                    }   
                }
                break;
        }
        S.CheckForGameOver();
    }
}
