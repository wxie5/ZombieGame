using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is create and wrote by Jiacheng Sun
public class OpenPauseUI : MonoBehaviour
{
	[SerializeField] private GameObject pauseUI;
	[SerializeField] private GameObject pauseMainUI;
	[SerializeField] private GameObject helpUI;
	void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (!pauseUI.activeSelf)
			{
				PauseGame();
				pauseUI.SetActive(true);
				pauseMainUI.SetActive(true);
				helpUI.SetActive(false);
			}
			else
			{
				ContinueGame();
				pauseUI.SetActive(false);
			}
		}
	}

	public void PauseGame()
    {
		Time.timeScale = 0;
	}

	public void ContinueGame()
    {
		Time.timeScale = 1;
	}
}
