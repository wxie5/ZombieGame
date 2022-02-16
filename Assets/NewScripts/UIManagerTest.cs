using UnityEngine;

public class UIManagerTest : MonoBehaviour
{
    private static UIManagerTest instance;
    public static UIManagerTest Instance
    {
        get
        {
            if (instance != null) { return instance; }
            return null;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("More than one instance!!!");
        }
    }

    public void HealthChange()
    {
        print("Change Health");
    }
}
