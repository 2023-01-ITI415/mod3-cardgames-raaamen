using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Deck : MonoBehaviour
{
    //cards in deck
    //to move these cards call the gameobject
    public List<Card> allCardsInDeck;
    public List<Card> cardLayer1;
    public List<Card> cardLayer2;
    public List<Card> cardLayer3;
    public List<Card> cardLayer4;
    public List<Card> extraCards;

    //prefabs
    public GameObject cardPrefab;

    //components


    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    void AddCardToDeck(){

    }
    void RemoveCardFromDeck(){

    }
    void ClearDeck(){

    }
    void UpdateVisualDeck(){

    }
    public Card MakeCard(){
        Card card = null;
        return card;
    }
}
