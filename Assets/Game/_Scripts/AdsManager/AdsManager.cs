using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;

public class AdsManager : MonoBehaviour
{
	public static AdsManager instance;

	[HideInInspector] public BannerView bannerView;
	[HideInInspector] public InterstitialAd interstitial;
    [HideInInspector] public RewardBasedVideoAd rewardBasedVideo;

    [SerializeField] string appID = "";
    [Space]
	[SerializeField] string bannerID = "";
	[SerializeField] string interstitialID = "";
    [SerializeField] string rewardBasedVideoID = "";
    [Space]
    public bool testAds = true;

    [Space]
    public bool videoRecompensado = false;

    void Awake ()
	{
		if (instance == null)
        {
            instance = this;
        }
		else
		{
			Destroy (gameObject);
			return;
		}

		DontDestroyOnLoad (gameObject);

		MobileAds.Initialize (appID);
	}

	void Start ()
	{
        //this.Request_Banner();

        this.Request_InterstitialAd();
        this.rewardBasedVideo = RewardBasedVideoAd.Instance;
        // Called when an ad request has successfully loaded.
        rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
        // Called when an ad request failed to load.
        rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        // Called when an ad is shown.
        rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
        // Called when the ad starts to play.
        rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
        // Called when the user should be rewarded for watching a video.
        rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        // Called when the ad is closed.
        rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        // Called when the ad click caused the user to leave the application.
        rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;

        this.Request_RewardBasedVideo();
    }

	public void Request_Banner ()
	{
        if (testAds)
        {
        #if UNITY_ANDROID
            bannerID = "ca-app-pub-3940256099942544/6300978111";
        #elif UNITY_IPHONE
		    bannerID = "ca-app-pub-3940256099942544/2934735716";
        #else
		    bannerID = "unexpected_platform";
        #endif
        }

        // Create banner
        //this.bannerView = new BannerView(bannerID, AdSize.SmartBanner, AdPosition.Bottom);
        this.bannerView = new BannerView(bannerID, AdSize.Banner, AdPosition.Bottom);

        // Called when an ad request has successfully loaded.
        this.bannerView.OnAdLoaded += HandleOnBannerAdLoaded;
        // Called when an ad request failed to load.
        this.bannerView.OnAdFailedToLoad += HandleOnBannerAdFailedToLoad;
        // Called when an ad is clicked.
        this.bannerView.OnAdOpening += HandleOnBannerAdOpened;
        // Called when the user returned from the app after an ad click.
        this.bannerView.OnAdClosed += HandleOnBannerAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.bannerView.OnAdLeavingApplication += HandleOnBannerAdLeavingApplication;

        // Create an empty ad request.
        AdRequest request;

        if (testAds)
        {
            request = new AdRequest.Builder().AddTestDevice("1FC5980990D7AB0404386CAFEB34C7A3").AddTestDevice("AB1920B812365BCA490030A5FBE1F7D0").Build();
        }
        else
        {
            request = new AdRequest.Builder().Build();
        }

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
	}

	public void Request_InterstitialAd ()
	{
        if (testAds)
        {
        #if UNITY_ANDROID
            interstitialID = "ca-app-pub-3940256099942544/1033173712";
        #elif UNITY_IPHONE
            interstitialID = "ca-app-pub-3940256099942544/4411468910";
        #else
            interstitialID = "unexpected_platform";
        #endif
        }

        this.interstitial = new InterstitialAd (interstitialID);

        // Called when an ad request has successfully loaded.
        this.interstitial.OnAdLoaded += HandleOnInterstitialAdLoaded;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnInterstitialAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitial.OnAdOpening += HandleOnInterstitialAdOpened;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnInterstitialAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.interstitial.OnAdLeavingApplication += HandleOnInterstitialAdLeavingApplication;

        // Create an empty ad request
        AdRequest request;

        if (testAds)
        {
            request = new AdRequest.Builder().AddTestDevice("1FC5980990D7AB0404386CAFEB34C7A3").AddTestDevice("AB1920B812365BCA490030A5FBE1F7D0").Build();
        }
        else
        {
            request = new AdRequest.Builder().Build();
        }

        // Load the interstitial with the request
        this.interstitial.LoadAd(request);
    }

    public void Request_RewardBasedVideo ()
    {
        if (testAds)
        {
        #if UNITY_ANDROID
            rewardBasedVideoID = "ca-app-pub-3940256099942544/5224354917";
        #elif UNITY_IOS
            rewardBasedVideoID = "ca-app-pub-3940256099942544/1712485313";
        #else
            //rewardVideoID = "unexpected_platform";
        #endif
        }

        // Create an empty ad request.
        AdRequest request;

        if (testAds)
        {
            request = new AdRequest.Builder().AddTestDevice("1FC5980990D7AB0404386CAFEB34C7A3").AddTestDevice("AB1920B812365BCA490030A5FBE1F7D0").Build();
        }
        else
        {
            request = new AdRequest.Builder().Build();
        }

        // Load the rewarded video ad with the request.
       this.rewardBasedVideo.LoadAd(request, rewardBasedVideoID);
    }

    public void Show_Banner ()
	{/*
        if (this.bannerView != null)
        {
            this.bannerView.Show();
        }
        */
        this.Destruir_Banner();

        this.Request_Banner();
    }

	public void Show_Interstitial ()
	{
		if (this.interstitial != null && this.interstitial.IsLoaded())
		{
			this.interstitial.Show();
		}
	}

    public void Show_RewardAd ()
    {
        if (this.rewardBasedVideo != null && this.rewardBasedVideo.IsLoaded())
        {
            this.rewardBasedVideo.Show();
        }
    }

	public void Destruir_Banner ()
	{
        if (this.bannerView != null)
        {
            this.bannerView.Destroy();
        }
	}

    #region Banner

    public void HandleOnBannerAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnBannerAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.Message);
    }

    public void HandleOnBannerAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnBannerAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleOnBannerAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

    #endregion

    #region Interstitial

    public void HandleOnInterstitialAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnInterstitialAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.Message);
    }

    public void HandleOnInterstitialAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnInterstitialAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");

        this.interstitial.Destroy();

        this.Request_InterstitialAd();
    }

    public void HandleOnInterstitialAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

    #endregion

     #region RewardVideo

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoFailedToLoad event received with message: "+ args.Message);
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoClosed event received");

        this.Request_RewardBasedVideo();
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print("HandleRewardBasedVideoRewarded event received for " + amount.ToString() + " " + type);
        videoRecompensado = true;
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
    }

    #endregion
}
