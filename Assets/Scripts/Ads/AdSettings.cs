using UnityEngine;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using UnityEngine.Events;

namespace JumpJam
{
    public class AdSettings : MonoBehaviour, IRewardedVideoAdListener, IInterstitialAdListener
    {
        [SerializeField] private Player _player;
        [SerializeField] private GameOverScreen _gameOverScreen;

        private const string AppKey = "df0363ceb63b83a1a35ec2d308ab8aedd81c3fc41f6fb0d4";
        private const string ResurrectionRewarded = "ResurrectionRewarded";
        private const string LevelInterstitial = "LevelInterstitial";

        private const string ShowViewAd = "ShowedViewAd";

        public event UnityAction RewardedVideoFinished;

        private void Start()
        {
            int adTypes = Appodeal.INTERSTITIAL | Appodeal.REWARDED_VIDEO;
            Appodeal.initialize(AppKey, adTypes, true);

            Appodeal.setInterstitialCallbacks(this);
            Appodeal.setRewardedVideoCallbacks(this);
        }

        public void ShowInterstitial()
        {
            if (Appodeal.canShow(Appodeal.INTERSTITIAL, LevelInterstitial) && !Appodeal.isPrecache(Appodeal.INTERSTITIAL))
                Appodeal.show(Appodeal.INTERSTITIAL, LevelInterstitial);
        }

        public void ShowRewardedVideo()
        {
            if (Appodeal.canShow(Appodeal.REWARDED_VIDEO, ResurrectionRewarded) && !Appodeal.isPrecache(Appodeal.REWARDED_VIDEO))
                Appodeal.show(Appodeal.REWARDED_VIDEO, ResurrectionRewarded);
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

        public void onInterstitialLoaded(bool isPrecache)
        {
        }

        public void onInterstitialFailedToLoad()
        {
        }

        public void onInterstitialShowFailed()
        {
        }

        public void onInterstitialShown()
        {
            string paremeters = "{\"Appodeal\":\"Interstitial\"}";
            AppMetrica.Instance.ReportEvent(ShowViewAd, paremeters);
        }

        public void onInterstitialClosed()
        {
        }

        public void onInterstitialClicked()
        {
        }

        public void onInterstitialExpired()
        {
        }
    }
}
