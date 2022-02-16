using UnityEngine;

public class TestSingle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManagerTest.Instance.HealthChange();
    }
}
