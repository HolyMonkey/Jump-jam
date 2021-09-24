using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace JumpJam
{
    [RequireComponent(typeof(CanvasGroup))]
    public class LevelCompletedScreen : MonoBehaviour
    {
        [SerializeField] private EnemiesStateChecker _enemiesState;
        [SerializeField] private Button _exitButton;

        private CanvasGroup _group;
        private const string BattleRoyaleVictory = "BattleRoyaleVictory";

        private void Start()
        {
            _group = GetComponent<CanvasGroup>();
            _group.alpha = 0;
            _group.blocksRaycasts = false;
        }

        private void OnEnable()
        {
            _enemiesState.EnemiesDestroyed += OnEnemiesDestroyed;
            _exitButton.onClick.AddListener(OnExitButtonClick);
        }

        private void OnDisable()
        {
            _enemiesState.EnemiesDestroyed -= OnEnemiesDestroyed;
            _exitButton.onClick.RemoveListener(OnExitButtonClick);
        }

        private void OnExitButtonClick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnEnemiesDestroyed()
        {
            StartFadeIn();

            string playerLevel = _enemiesState.GetComponent<MonsterTruck>().CurrentSize.ToString();
            string parameters = "{\"Level\":\"" + playerLevel + "\"}";
            AppMetrica.Instance.ReportEvent(BattleRoyaleVictory, parameters);
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
