using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] GameObject UIParent;
    [SerializeField] TextMeshProUGUI messageText;

    public void Show(string message)
    {
        if (UIParent.activeSelf == false)
        {
            EnemySpawner.Instance.StopSpawning();
            UIParent.SetActive(true);
            messageText.text = message;
        }
    }

    public void OnRestartButtonClick() => GameManager.Instance.OnRestart();
    public void OnMenuButtonClick() => GameManager.Instance.OnMenu();
}
