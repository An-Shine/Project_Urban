using UnityEngine;

class GameManager : Singleton<GameManager>
{
    [SerializeField] private Player player;

    public Player Player => player;
}