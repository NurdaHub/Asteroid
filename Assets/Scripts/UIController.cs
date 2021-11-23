using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static int currentScore = 0;
    
    [SerializeField] private ShipController shipController;
    [SerializeField] private AlienSpawner alienSpawner;
    [SerializeField] private AsteroidSpawner asteroidSpawner;
    [SerializeField] private Button playButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Toggle controlToggle;
    [SerializeField] private GameObject menuGO;
    [SerializeField] private GameObject gameOverGO;

    private bool isPaused;

    private void Awake()
    {
        playButton.onClick.AddListener(StartGame);
        resumeButton.onClick.AddListener(ResumeGame);
        controlToggle.onValueChanged.AddListener(SwitchControl);
        quitButton.onClick.AddListener(QuitGame);

        shipController.OnGameOver += GameOver;
    }

    private void Update()
    {
        scoreText.text = currentScore.ToString();
        
        if (Input.GetKeyDown(KeyCode.Escape))
            ResumeGame();
    }

    private void StartGame()
    {
        Time.timeScale = 1;
        currentScore = 0;

        MenuViewShow(false);
        resumeButton.gameObject.SetActive(true);
        shipController.gameObject.SetActive(true);
        shipController.Init();
        alienSpawner.Init();
        asteroidSpawner.Init();
    }

    private void ResumeGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            MenuViewShow(true);
            isPaused = true;
        }
        else
        {
            MenuViewShow(false);
            isPaused = false;
            Time.timeScale = 1;
        }
    }
    
    private void QuitGame()
    {
        Application.Quit();
    }

    private void SwitchControl(bool isOn)
    {
        shipController.SwitchControl(isOn);
    }

    private void SwitchSound(bool isOn)
    {
        AudioListener.volume = isOn ? 1f : 0f;
    }

    private void MenuViewShow(bool isActive)
    {
        menuGO.SetActive(isActive);
    }

    private void GameOver()
    {
        resumeButton.gameObject.SetActive(false);
        gameOverGO.SetActive(true);

        StartCoroutine(AfterGameOver());
    }

    private IEnumerator AfterGameOver()
    {
        yield return new WaitForSeconds(5);
        gameOverGO.SetActive(false);
        MenuViewShow(true);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(StartGame);
        resumeButton.onClick.RemoveListener(ResumeGame);
        controlToggle.onValueChanged.RemoveListener(SwitchControl);
        quitButton.onClick.RemoveListener(QuitGame);
    }
}
