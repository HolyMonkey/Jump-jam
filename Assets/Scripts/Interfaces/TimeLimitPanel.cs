using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpJam
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TimeLimitPanel : MonoBehaviour
    {
        private CanvasGroup _group;

        private void Awake()
        {
            _group = GetComponent<CanvasGroup>();
        }

        public void StartFadeIn()
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
    }
}
