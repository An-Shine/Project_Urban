using UnityEngine;

public class Card2 : DefenseCard
{
    // DefenseCard 생성자: (_armor, _cost, _element)
    public Card2() : base(_armor: 1, _cost: 1, _element: CardElement.None)
    {
    }

    public static Card2 GetCard()
    {
        return new Card2();
    }
}