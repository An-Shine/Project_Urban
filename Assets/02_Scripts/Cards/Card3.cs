using UnityEngine;

public class Card3 : BuffCard
{
    // BuffCard 생성자: (_buffAmount, _duration, _cost, _element)
    public Card3() : base(_buffAmount: 1, _duration: 1, _cost: 1, _element: CardElement.None)
    {
    }

    public static Card3 GetCard()
    {
        return new Card3();
    }
}