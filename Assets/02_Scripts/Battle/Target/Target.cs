using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HpController))]
[RequireComponent(typeof(ShieldController))]
abstract public class Target : MonoBehaviour
{
    protected HpController hpController;
    protected ShieldController shieldController;

    public HpController HpController => hpController;
    public ShieldController ShieldController => shieldController;

    public UnityEvent OnDie = new();
    public UnityEvent<bool> OnDamaged = new();

    private void Awake()
    {
        Init();
        Reset();
    }

    public void Damage(int hitPoint)
    {
        // 방패로 데미지 흡수
        int shieldPoints = shieldController.CurrentPoint;
        int damageToShield = Mathf.Min(shieldPoints, hitPoint);
        int remainingDamage = hitPoint - damageToShield;

        // 방패에 데미지 적용
        if (damageToShield > 0)
            shieldController.Decrease(damageToShield);

        // 남은 데미지를 체력에 적용
        if (remainingDamage > 0)
            hpController.Decrease(remainingDamage);

        if (hpController.CurrentPoint <= 0)
            OnDie?.Invoke();
        else
            OnDamaged?.Invoke(shieldController.CurrentPoint > 0);

        Debug.Log($"{gameObject.name} : {hpController.CurrentPoint}, hitPoint : {hitPoint}");
    }

    public void AddShield(int shieldPoint)
    {
        shieldController.Increase(shieldPoint);
    }

    [SerializeField] protected List<DoTDamage> dotList = new List<DoTDamage>();
    [System.Serializable]
    public class DoTDamage
    {
        public int damage;   // 틱당 데미지
        public int turns;    // 남은 턴 수

        public DoTDamage(int dmg, int t)
        {
            damage = dmg;
            turns = t;
        }
    }

    public void AddDoT(int damage, int turns)
    {
        dotList.Add(new DoTDamage(damage, turns));       
    }

    // 턴 종료 시 호출해서 데미지 처리 (BattleManager에서)
    public void OnTurnEnd()
    {        
        for (int i = dotList.Count - 1; i >= 0; i--)
        {
            DoTDamage dot = dotList[i];

            // 데미지 적용
            Damage(dot.damage);

            // 턴 차감
            dot.turns--;

            // 끝난 도트 데미지는 리스트에서 삭제
            if (dot.turns <= 0)
            {
                dotList.RemoveAt(i);
            }
        }
    }
    
    protected virtual void Init()
    {
        hpController = GetComponent<HpController>();
        shieldController = GetComponent<ShieldController>();
    }

    protected virtual void Reset()
    {
        hpController.Reset();
        shieldController.Reset();
        dotList.Clear();
    }
}
