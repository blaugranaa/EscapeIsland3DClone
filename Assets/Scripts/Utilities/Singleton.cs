using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    [SerializeField] protected bool dontDestroy = true;

    private static T _instance;

    #region Mono
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            if (dontDestroy)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }
    #endregion


    #region Pattern

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject t = new GameObject();
                    t.name = typeof(T).Name;
                    _instance = t.AddComponent<T>();
                }
            }

            return _instance;
        }
    }


    #endregion

}

