using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour
{
    public enum sceneNmae
    {
        GameStartUI,
        EndlessMode,
        MultiplayerEndlessMode,
        StoryMode1,
        StoryMode2,
        AIMultiplayerEndlessMode
    }
    [SerializeField] private bool playSoundEffect;
    [SerializeField] private AudioClip soundEffect;
    [SerializeField] private sceneNmae m_sceneName;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        Time.timeScale = 1;
        StartCoroutine(load());
    }

    IEnumerator load()
    {
        if (playSoundEffect)
        {
            GameSetting.Instance.Save();
            AudioSource.PlayClipAtPoint(soundEffect, new Vector3(0, 0, 0));
            yield return new WaitForSeconds(1f);
        }
        SceneManager.LoadScene(m_sceneName.ToString());
    }
}
