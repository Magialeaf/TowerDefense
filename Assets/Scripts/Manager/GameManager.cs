using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameOverUI gameOverUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        UpdateTime();
    }

    public void Fail()
    {
        gameOverUI.Show("游戏失败");
        FreezeTime();
    }
    public void Success()
    {
        gameOverUI.Show("游戏胜利");
        FreezeTime();
    }

    private void UpdateTime() { }
    private void FreezeTime() { }

    public void OnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMenu()
    {
        SceneManager.LoadScene(0);
    }
}
