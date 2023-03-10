using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Deck : MonoBehaviour
{
    //cards in deck
    //to move these cards call the gameobject
    public List<Card> allCardsInDeck;
    public List<Card> cardLayer1 = new List<Card>(10);
    public List<Card> cardLayer2 = new List<Card>(9);
    public List<Card> cardLayer3 = new List<Card>(6);
    public List<Card> cardLayer4 = new List<Card>(3);
    public List<Card> extraCards;
    public GameObject centerCard;

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
