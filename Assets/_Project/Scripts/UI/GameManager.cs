using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject _deathUI;
    [SerializeField] GameObject _victoryUI;

    public void ExitGamePauseMenu()
    {
        Time.timeScale = 1f;
        AudioManager.Instance.StopAll();
        AudioManager.Instance.Play("MainMenu");
        SceneManager.LoadScene("MainMenu");
    }
    public void ChangeScene(string sceneName)
    {
        Time.timeScale = 1;
        switch (sceneName)
        {
            case "Level1":
                AudioManager.Instance.StopAll();
                AudioManager.Instance.Play(sceneName);
                break;
            case "Level2":
                AudioManager.Instance.StopAll();
                AudioManager.Instance.Play(sceneName);
                break;
            case "MainMenu":
                AudioManager.Instance.StopAll();
                AudioManager.Instance.Play(sceneName);
                break;
        }
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGameMainMenu()
    {
        Application.Quit();
        Debug.Log("Hai chiuso il gioco");
    }

    public void ShowDeathUI()
    {
        _deathUI.SetActive(true);
        AudioManager.Instance.StopAll();
        Time.timeScale = 0f;
    }

    public void ShowVictoryUI()
    {
        _victoryUI.SetActive(true);
        AudioManager.Instance.StopAll();
        Time.timeScale = 0f;
    }
}
