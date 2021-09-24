using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace JumpJam
{
    [RequireComponent(typeof(CanvasGroup))]
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private ReviveButton _reviveButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private TimeLimitPanel _timeLimitPanel;

        private CanvasGroup _group;

        private const string BattleRoyaleLoss = "BattleRoyaleLoss";
        private const string TimeLimitLoss = "TimeLimitLoss";

        private const string ShowViewAd = "ShowedViewAd";

        private void Start()
        {
            _group = GetComponent<CanvasGroup>();
            _group.alpha = 0;
            _group.blocksRaycasts = false;
        }

        private void OnEnable()
        {
            _player.Destroyed += OnPlayerDestroyed;
            _reviveButton.ShowedRewardedAd += OnShowedRewardedAd;

            _restartButton.onClick.AddListener(OnRestartButtonClick);
            _exitButton.onClick.AddListener(OnExitButtonClick);
        }

        private void OnDisable()
        {
            _player.Destroyed -= OnPlayerDestroyed;
            _reviveButton.ShowedRewardedAd -= OnShowedRewardedAd;

            _restartButton.onClick.RemoveListener(OnRestartButtonClick);
            _exitButton.onClick.RemoveListener(OnExitButtonClick);
        }

        private void OnPlayerDestroyed()
        {
            StartFadeIn();

            string gameMode = CheckGameMode();
            string playerLevel = _player.GetComponent<MonsterTruck>().CurrentSize.ToString();
            string parameters = "{\"Level\":\"" + playerLevel + "\"}";
            AppMetrica.Instance.ReportEvent(gameMode, parameters);
        }

        private string CheckGameMode()
        {
            if (_timeLimitPanel.gameObject.activeSelf == true)
            {
                return TimeLimitLoss;
            }
            else
            {
                return BattleRoyaleLoss;
            }
        }

        private void OnShowedRewardedAd()
        {
            string gameMode = CheckGameMode();

            string parameters = "{\"Appodeal\":\"Rewarded\", \"GameMode\":\"" + gameMode + "\"}";
            AppMetrica.Instance.ReportEvent(ShowViewAd, parameters);

            Time.timeScale = 1;
        }

        private void OnRestartButtonClick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnExitButtonClick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        private void StartFadeIn()
        {
            StartCoroutine(FadeIn());
        }

        private IEnumerator FadeIn()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            float alpha = 0;

            for (int i = 0; i < 30; i++)
            {
                alpha = 0f + (1f / 30f * i);
                _group.alpha = alpha;

                yield return waitForFixedUpdate;
            }

            _group.alpha = 1;
            _group.blocksRaycasts = true;
            Time.timeScale = 0;
        }

        private IEnumerator FadeOut()
        {
            var waitForFixedUpdate = new WaitForFixedUpdate();
            float alpha = 1;

            for (int i = 0; i < 30; i++)
            {
                alpha = 1f - (1f / 30f * i);
                _group.alpha = alpha;

                yield return waitForFixedUpdate;
            }

            _group.alpha = 0;
            _group.blocksRaycasts = false;
        }

        public void StartFadeOut()
        {
            StartCoroutine(FadeOut());
        }
    }
}
