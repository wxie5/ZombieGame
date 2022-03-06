using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = (T)this;
        }
        else
        {
            Debug.Log(this.gameObject.name);
            Debug.LogError("More Than One Instance of Singleton!");
        }
    }
}
