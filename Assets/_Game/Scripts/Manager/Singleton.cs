using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] bool dontdestry;
    public static T Instance
    {
        get { return _instance; }
    }

    private static T _instance;

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            if (dontdestry)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
