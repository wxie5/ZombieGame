using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//This script is wrote by Jiacheng Sun
public class SwicthToStoryMode : MonoBehaviour
{
    [SerializeField] private AudioClip zombie_roar;
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        StartCoroutine(load());
    }

    IEnumerator load()
    {
        AudioSource.PlayClipAtPoint(zombie_roar, new Vector3(0, 0, 0));
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Story Mode");
    }
}
