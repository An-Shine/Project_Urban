/// <summary>
/// 카드 이벤트를 처리하는 핸들러 인터페이스
/// Player가 구현하여 카드 상호작용 로직을 처리합니다.
/// </summary>
public interface ICardEventHandler
{
    /// <summary>
    /// 마우스가 카드에 진입했을 때 호출됩니다.
    /// </summary>
    void OnCardEnter(Card card);

    /// <summary>
    /// 마우스가 카드에서 벗어났을 때 호출됩니다.
    /// </summary>
    void OnCardExit(Card card);

    /// <summary>
    /// 카드가 클릭되었을 때 호출됩니다.
    /// </summary>
    void OnCardClick(Card card);
}
