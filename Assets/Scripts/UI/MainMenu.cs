using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject menuGO;
        [SerializeField] private GameObject gameOverGO;
        [SerializeField] private Button playButton;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Toggle controlToggle;
        [SerializeField] private float delay = 5;
        
        public Action OnPlayClicked { get; set; }
        public Action OnQuitClicked { get; set; }
        public Action<bool> OnControlChanged { get; set; }

        private void Start()
        {
            playButton.onClick.AddListener(() => OnPlayClicked?.Invoke());
            quitButton.onClick.AddListener(() => OnQuitClicked?.Invoke());
            controlToggle.onValueChanged.AddListener((isOn) => OnControlChanged?.Invoke(isOn));
        }

        public void MenuViewShow(bool isActive)
        {
            menuGO.SetActive(isActive);
        }
        
        public void GameOver()
        {
            resumeButton.gameObject.SetActive(false);
            gameOverGO.SetActive(true);

            StartCoroutine(AfterGameOver());
        }

        private IEnumerator AfterGameOver()
        {
            yield return new WaitForSeconds(delay);
            gameOverGO.SetActive(false);
            MenuViewShow(true);
        }

        private void OnDisable()
        {
            playButton.onClick.RemoveListener(() => OnPlayClicked?.Invoke());
            quitButton.onClick.RemoveListener(() => OnQuitClicked?.Invoke());
            controlToggle.onValueChanged.RemoveListener((isOn) => OnControlChanged?.Invoke(isOn));
        }
    }
}