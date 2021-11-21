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
    [SerializeField] private Button settingsButton;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Toggle controlToggle;
    [SerializeField] private Toggle soundToggle;
    [SerializeField] private GameObject settingsGO;

    private bool isPaused;

    private void OnEnable()
    {
        playButton.onClick.AddListener(StartGame);
        settingsButton.onClick.AddListener(SettingsPanel);
        controlToggle.onValueChanged.AddListener(SwitchControl);
        soundToggle.onValueChanged.AddListener(SwitchSound);
    }

    private void Update()
    {
        scoreText.text = currentScore.ToString();
        
        if (Input.GetKey(KeyCode.Escape))
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
    }

    private void StartGame()
    {
        settingsGO.SetActive(false);
        MenuViewShow(false);
        shipController.gameObject.SetActive(true);
        alienSpawner.Init();
        asteroidSpawner.Init();
    }

    private void SettingsPanel()
    {
        settingsGO.SetActive(true);
    }

    private void SwitchControl(bool isOn)
    {
        shipController.SwitchControl(isOn);
    }

    private void SwitchSound(bool isOn)
    {
        
    }

    private void MenuViewShow(bool isActive)
    {
        isPaused = isActive;
        playButton.gameObject.SetActive(isActive);
        settingsButton.gameObject.SetActive(isActive);
    }

    private void OnDisable()
    {
        
    }
}
