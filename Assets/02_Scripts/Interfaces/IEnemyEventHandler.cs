/// <summary>
/// 적 이벤트를 처리하는 핸들러 인터페이스
/// Player가 구현하여 적 상호작용 로직을 처리합니다.
/// </summary>
public interface IEnemyEventHandler
{
    /// <summary>
    /// 마우스가 적에 진입했을 때 호출됩니다.
    /// </summary>
    void OnEnemyEnter(Enemy enemy);

    /// <summary>
    /// 마우스가 적에서 벗어났을 때 호출됩니다.
    /// </summary>
    void OnEnemyExit(Enemy enemy);

    /// <summary>
    /// 적이 클릭되었을 때 호출됩니다.
    /// </summary>
    void OnEnemyClick(Enemy enemy);
}
