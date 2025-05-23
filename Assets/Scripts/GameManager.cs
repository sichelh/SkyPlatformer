using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Player player;

    protected override void Awake()
    {
        base.Awake();

        player = FindObjectOfType<Player>();
    }

    public void UIOpened()
    {
        Time.timeScale = 0f;
    }

    public void UIClosed()
    {
        Time.timeScale = 1f;
    }

    public void ReplayeGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}
