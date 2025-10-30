using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
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
                GameSession.Instance.ResetCoins();
                break;
        }
        DOTween.KillAll();
        SaveSystem.ClearCheckpoint();
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Hai chiuso il gioco");
    }

    private void OnApplicationQuit()
    {
        SaveSystem.ClearCheckpoint();
    }
}
