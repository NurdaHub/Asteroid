using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerScore : MonoBehaviour
    {
        public static PlayerScore Instance { get; set; }

        [SerializeField] private TextMeshProUGUI scoreText;
        
        private int currentScore = 0;
        private int startScore = 0;

        private void Awake()
        {
            Instance = this;
        }

        private void SetScoreText()
        {
            scoreText.text = currentScore.ToString();
        }

        public void UpdateScore(int score)
        {
            currentScore += score;
            SetScoreText();
        }

        public void ResetScore()
        {
            currentScore = startScore;
        }
    }
}