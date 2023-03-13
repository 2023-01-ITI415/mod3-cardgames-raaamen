using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{

    public CardSpritesSO cardSprites;
    public GameObject prefabCard;
    public GameObject prefabSprite;
    public bool startFaceUp = true;

    public Transform deckAnchor;
    public List<Card> cards;

    private JsonParseDeck jsonDeck;

    public static GameObject SPRITE_PREFAB{ get; private set;}
    // Start is called before the first frame update
    void Start()
    {
        //InitDeck();
        //Shuffle(ref cards);
    }

    public static void Shuffle(ref List<Card> refCards)
    {
        List<Card> tCards = new List<Card>();
        int ndx;
        while (refCards.Count>0)
        {
            ndx = UnityEngine.Random.Range(0,refCards.Count);
            tCards.Add(refCards[ndx]);
            refCards.RemoveAt(ndx);
        }
        refCards=tCards;
    }

    public void InitDeck()
    {
        SPRITE_PREFAB = prefabSprite;
        cardSprites.Init();
        jsonDeck = GetComponent<JsonParseDeck>();

        if (GameObject.Find("_Deck") == null)
        {
            GameObject anchorGO = new GameObject("_Deck");
            deckAnchor = anchorGO.transform;
        }
        MakeCards();

    }

    private void MakeCards()
    {
        cards = new List<Card>();
        Card c;
        string suits = "CDHS";
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j <= 13; j++)
            {
                c = MakeCard(suits[i], j);
                cards.Add(c);
                c.transform.position = new Vector3(
                    (j-7)*3,
                    (i-1.5f)*4,
                    0
                );
            }
        }
    }

    private Card MakeCard(char suit, int rank)
    {
        GameObject go = Instantiate<GameObject>(prefabCard, deckAnchor);
        Card card = go.GetComponent<Card>();
        card.Init(suit,rank,startFaceUp);
        return card;
    }



}
