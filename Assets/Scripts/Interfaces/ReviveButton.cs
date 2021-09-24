using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JumpJam
{
    [RequireComponent(typeof(Button))]
    public class ReviveButton : MonoBehaviour
    {
        [SerializeField] private AdSettings _adSettings;

        private Button _button;

        public event UnityAction ShowedRewardedAd;
            
        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(_adSettings.ShowRewardedVideo);

            _adSettings.RewardedVideoFinished += OnRewardedVideoFinished;
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(_adSettings.ShowRewardedVideo);

            _adSettings.RewardedVideoFinished -= OnRewardedVideoFinished;
        }

        private void OnRewardedVideoFinished()
        {
            ShowedRewardedAd?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
