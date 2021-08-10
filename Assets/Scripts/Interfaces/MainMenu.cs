using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace JumpJam
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _battleRoyaleButton;
        [SerializeField] private Button _timeLimitButton;

        private const string BattleRoyale = "BattleRoyale";
        private const string TimeLimit = "TimeLimit";

        private void OnEnable()
        {
            _battleRoyaleButton.onClick.AddListener(OnBattleRoyaleButtonClick);
            _timeLimitButton.onClick.AddListener(OnTimeLimitButtonClick);
        }

        private void OnDisable()
        {
            _battleRoyaleButton.onClick.RemoveListener(OnBattleRoyaleButtonClick);
            _timeLimitButton.onClick.RemoveListener(OnTimeLimitButtonClick);
        }

        private void OnBattleRoyaleButtonClick()
        {
            SceneManager.LoadScene(BattleRoyale);
        }

        private void OnTimeLimitButtonClick()
        {
            SceneManager.LoadScene(TimeLimit);
        }
    }
}
