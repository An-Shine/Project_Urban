using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    static private T instance;

    static public T Instacne => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }
}