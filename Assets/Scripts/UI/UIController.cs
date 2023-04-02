using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        public static int CurrentScore = 0;
    
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
        [SerializeField] private List<GameObject> lifeImgList;

        private float playTimeScale = 1;
        private float pauseTimeScale = 0;
        private int startScore = 0;
        private bool isPaused;

        private void Awake()
        {
            playButton.onClick.AddListener(StartGame);
            resumeButton.onClick.AddListener(ResumeGame);
            controlToggle.onValueChanged.AddListener(SwitchControl);
            quitButton.onClick.AddListener(QuitGame);

            //shipController.OnShipDestroyed += ShipDestroyed;
            //shipController.OnGameOver += GameOver;
        }

        private void Update()
        {
            scoreText.text = CurrentScore.ToString();
        
            if (Input.GetKeyDown(KeyCode.Escape))
                ResumeGame();
        }

        private void StartGame()
        {
            Time.timeScale = playTimeScale;
            CurrentScore = startScore;

            ActivateShipLife();
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
                Time.timeScale = pauseTimeScale;
                MenuViewShow(true);
                isPaused = true;
            }
            else
            {
                MenuViewShow(false);
                isPaused = false;
                Time.timeScale = playTimeScale;
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
    
        private void MenuViewShow(bool isActive)
        {
            menuGO.SetActive(isActive);
        }

        private void ShipDestroyed(int shipLife)
        {
            lifeImgList[shipLife].SetActive(false);
        }

        private void ActivateShipLife()
        {
            foreach (var lifeImg in lifeImgList)
            {
                lifeImg.SetActive(true);
            }
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
}
