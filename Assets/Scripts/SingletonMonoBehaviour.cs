using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T _instance;

    public static T Instance => _instance;

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}