using UnityEngine;

public class Card4 : DebuffCard
{
    public Card4() : base(_debuffAmount: 1, _duration: 1, _cost: 1, _element: CardElement.None)
    {
    }

    public static Card4 GetCard()
    {
        return new Card4();
    }
}