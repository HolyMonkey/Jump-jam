using System.Collections;
using System.Collections.Generic;
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

        private CanvasGroup _group;

        private const string MainMenu = "MainMenu";

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
            SceneManager.LoadScene(MainMenu);
        }

        private void OnTimerStopped()
        {
            StartFadeIn();
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
    }
}
