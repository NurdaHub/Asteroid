using System;
using Player;
using UI;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private ShipController shipController;
    [SerializeField] private AlienSpawner alienSpawner;
    [SerializeField] private AsteroidSpawner asteroidSpawner;
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private PlayerLife playerLife;
    
    private bool isPaused;

    private void Start()
    {
        mainMenu.OnPlayClicked += StartGame;
        mainMenu.OnQuitClicked += Quit;
        mainMenu.OnControlChanged += ChangeControl;
        playerLife.OnLifeEnded += GameOver;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void StartGame()
    {
        Resume();
        PlayerScore.Instance.ResetScore();
        playerLife.ActivateLife();
        shipController.gameObject.SetActive(true);
        shipController.Init();
        alienSpawner.Init();
        asteroidSpawner.Init();
    }

    private void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
        mainMenu.MenuViewShow(true);
    }

    private void Resume()
    {
        isPaused = false;
        Time.timeScale = 1;
        mainMenu.MenuViewShow(false);
    }

    private void GameOver()
    {
        shipController.Deactivate();
        mainMenu.GameOver();
    }

    private void Quit()
    {
        Application.Quit();
    }
    
    private void ChangeControl(bool isOn)
    {
        shipController.SwitchControl(isOn);
    }
}