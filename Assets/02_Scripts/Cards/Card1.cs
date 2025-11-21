using UnityEngine;

public class Card1 : AttackCard
{ 
    public Card1() : base(_damage: 1, _cost: 1, _element: CardElement.None)
    {
        
    }

    public static Card1 GetCard()
    {
        return new Card1();
    }
}