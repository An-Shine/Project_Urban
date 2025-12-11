// 0 ~ 99 무속성카드
// 100 ~ 199 화속성카드
// 200 ~ 299 얼음속성카드
// 300 ~ 399 풀속성카드
 
public enum CardName
{
    // 0 ~ 99: 무속성 카드
    Punch = 0,          // 타격
    Shooting,           // 사격
    Strike,             // 강타
    VileAttack,         // 비열한 공격
    Assault,            // 기습
    Guard,              // 방어
    Rollout,            // 구르기
    Maintenance,        // 정비
    Dummy,              // 더미
    
    // 100 ~ 199: 화속성 카드
    Inferno = 100,      // 백염
    BlazingStrike,      // 불타는 일격
    Embers,             // 불씨
    Ignition,           // 점화
    Backdraft,          // 백드리프트
    BlazeBarrier,       // 화염 방벽
    HeatExchange,       // 열교환
    Incendiary,         // 소이탄
    HeatUp,             // 열기
    Overheat,           // 과열
    Cinder,             // 잔불
    Stigma,             // 낙인
    OilSplash,          // 기름 뿌리기
    
    // 200 ~ 299: 얼음 속성 카드
    EnergyNeedle = 200, // 에너지 송곳
    KineticGrasp,       // 염력 손아귀
    Pulse,              // 파동
    ElectricArrow,      // 전기 화살
    GlacialWedge,       // 빙하 쐐기
    IceShield,          // 얼음 방패
    ElectricField,      // 전자기장
    AccelConcoction,    // 가속 화합물
    Superconductor,     // 초전도체
    Anxiolytic,         // 신경 안정제
    CryoPowder,         // 초저온 분말
    Disturb,            // 방해
    
    // 300 ~ 399: 풀 속성 카드
    DoubleEdged = 300,  // 양날의 검
    Plague,             // 역병
    SpikyBush,          // 가시 덤블
    AbsorbingStrike,    // 흡수의 일격
    DistortedSlay,      // 뒤틀린 일격
    ThornWhip,          // 가시 채찍
    ElasticBarrier,     // 탄성 장벽
    Blooming,           // 개화
    SurgingLife,        // 맥동하는 생명
    CellChange,         // 체조직 교환
    Cycle,              // 순환
    EnfeebleSludge,      // 약화 점액
}