using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace JumpJam
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TimeISUpPanel : MonoBehaviour
    {
        [SerializeField] private TimeCounter _timer;
        [SerializeField] private Button _exitButton;

        [SerializeField] private Image[] _iconsUI;
        [SerializeField] private Image[] _targetIconsUI;

        [SerializeField] private TMP_Text[] _labels;
        [SerializeField] private TMP_Text[] _targetLabels;

        [SerializeField] private CanvasGroup _scoreBoard;
        [SerializeField] private RectTransform _player;

        private CanvasGroup _group;

        private const string TimeLimitVictory = "TimeLimitVictory";

        private void Start()
        {
            _group = GetComponent<CanvasGroup>();
            _group.alpha = 0;
            _group.blocksRaycasts = false;
        }

        private void OnEnable()
        {
            _timer.Stopped += OnTimerStopped;
            _exitButton.onClick.AddListener(OnExitButtonClick);
        }

        private void OnDisable()
        {
            _timer.Stopped -= OnTimerStopped;
            _exitButton.onClick.RemoveListener(OnExitButtonClick);
        }

        private void OnExitButtonClick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnTimerStopped()
        {
            ApplyIconsUI();
            ApplyLabelsUI();

            _scoreBoard.alpha = 1;

            StartFadeIn();

            string playerPlace = (_player.GetSiblingIndex() + 1).ToString();
            string playerLevel = _player.GetComponent<ScoreCounter>().Score.ToString();
            string parametersPlace = "{\"Place\":\"" + playerPlace + "\", \"Level\":\"" + playerLevel + "\"}";
            AppMetrica.Instance.ReportEvent(TimeLimitVictory, parametersPlace);
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

        private void ApplyIconsUI()
        {
            for (int i = 0; i < _iconsUI.Length; i++)
            {
                _iconsUI[i].sprite = _targetIconsUI[i].sprite;
            }
        }

        private void ApplyLabelsUI()
        {
            for (int i = 0; i < _labels.Length; i++)
            {
                _labels[i].text = _targetLabels[i].text;
            }
        }
    }
}
