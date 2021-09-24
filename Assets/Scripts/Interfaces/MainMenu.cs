using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JumpJam
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _battleRoyaleButton;
        [SerializeField] private Button _timeLimitButton;
        [SerializeField] private TimeLimitPanel _timeLimitPanel;
        [SerializeField] private AdSettings _adSettings;

        private const string GameMode = "GameMode";

        public event UnityAction Disappearing;

        private void Start()
        {
            Time.timeScale = 0;

            _adSettings.ShowInterstitial();
        }

        private void OnEnable()
        {
            _battleRoyaleButton.onClick.AddListener(OnBattleRoyaleButtonClick);
            _timeLimitButton.onClick.AddListener(OnTimeLimitButtonClick);
        }

        private void OnDisable()
        {
            _battleRoyaleButton.onClick.RemoveListener(OnBattleRoyaleButtonClick);
            _timeLimitButton.onClick.RemoveListener(OnTimeLimitButtonClick);

            Disappearing?.Invoke();
            Time.timeScale = 1;
        }

        private void OnBattleRoyaleButtonClick()
        {
            gameObject.SetActive(false);

            string parameters = "{\"Name\":\"BattleRoyale\"}";
            AppMetrica.Instance.ReportEvent(GameMode, parameters);
        }

        private void OnTimeLimitButtonClick()
        {
            gameObject.SetActive(false);
            _timeLimitPanel.gameObject.SetActive(true);
            _timeLimitPanel.StartFadeIn();

            string parameters = "{\"Name\":\"TimeLimit\"}";
            AppMetrica.Instance.ReportEvent(GameMode, parameters);
        }
    }
}
