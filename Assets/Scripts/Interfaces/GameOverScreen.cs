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
        [SerializeField] private Button _restartButton;
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
            _player.Destroyed += OnPlayerDestroyed;
            _restartButton.onClick.AddListener(OnRestartButtonClick);
            _exitButton.onClick.AddListener(OnExitButtonClick);
        }

        private void OnDisable()
        {
            _player.Destroyed -= OnPlayerDestroyed;
            _restartButton.onClick.RemoveListener(OnRestartButtonClick);
            _exitButton.onClick.RemoveListener(OnExitButtonClick);
        }

        private void OnPlayerDestroyed()
        {
            StartFadeIn();
        }

        private void OnRestartButtonClick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnExitButtonClick()
        {
            SceneManager.LoadScene(MainMenu);
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
