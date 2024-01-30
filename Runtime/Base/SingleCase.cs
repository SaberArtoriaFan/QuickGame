using UnityEngine;

public abstract class SingleCase<T>  where T : SingleCase<T>,new ()
{
    protected static T instance;
    public static T Instance { get { 
            if(instance == null)
                instance = new T();
            return instance;
        } }

    public SingleCase()
    {
        Init();
    }
    public static bool IsInitialized { get { return instance != null; } }
    protected virtual void Init()
    {

    }
    protected virtual void OnDestroy()
    {

    }
    public void ShutDown()
    {
        if (instance == this)
            instance = null;
        OnDestroy();
    }
}