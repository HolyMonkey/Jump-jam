using UnityEngine;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using UnityEngine.Events;

namespace JumpJam
{
    public class AdSettings : MonoBehaviour, IRewardedVideoAdListener
    {
        [SerializeField] private Player _player;
        [SerializeField] private GameOverScreen _gameOverScreen;

        private const string AppKey = "9642cd21e7186e8719b3b1e4f1a233aa0532045ae5d53fef";
        private const string Placement = "ResurrectionRewarded";

        public event UnityAction RewardedVideoFinished;

        private void Start()
        {
            int adTypes = Appodeal.REWARDED_VIDEO;
            Appodeal.initialize(AppKey, adTypes, true);

            Appodeal.setRewardedVideoCallbacks(this);
        }

        public void ShowRewardedVideo()
        {
            if (Appodeal.canShow(Appodeal.REWARDED_VIDEO, Placement) && !Appodeal.isPrecache(Appodeal.REWARDED_VIDEO))
                Appodeal.show(Appodeal.REWARDED_VIDEO, Placement);
        }

        public void onRewardedVideoClicked()
        {
        }

        public void onRewardedVideoClosed(bool finished)
        {
        }

        public void onRewardedVideoExpired()
        {
        }

        public void onRewardedVideoFailedToLoad()
        {
        }

        public void onRewardedVideoFinished(double amount, string name)
        {
            _player.gameObject.SetActive(true);
            _gameOverScreen.StartFadeOut();
            RewardedVideoFinished?.Invoke();
        }

        public void onRewardedVideoLoaded(bool precache)
        {
        }

        public void onRewardedVideoShowFailed()
        {
        }

        public void onRewardedVideoShown()
        {
        }
    }
}
