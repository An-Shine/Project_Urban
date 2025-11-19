using UnityEngine;

public class SceneSingleton<T> : MonoBehaviour where T : Component
{
    static private T instance;

    static public T Instacne => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this as T;
        else
            Destroy(gameObject);
    }
}