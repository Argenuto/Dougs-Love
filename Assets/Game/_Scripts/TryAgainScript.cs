using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using GoogleMobileAds.Api;

public class TryAgainScript : MonoBehaviour
{
    public uint secs;
    TextMeshProUGUI myText;
    Controller myController;
    private AdsManager myAdsM;
    public Button byCoins, byVideo;
    public int coins = 50;
    private Bird myBird;
    // Update is called once per frame
    void Update()
    {
        if (myAdsM.videoRecompensado)
        {
            myBird.Resurrect();
            myAdsM.videoRecompensado = false;
            GetComponent<HideAndShow>().Hide();
        }
    }

    private void OnEnable()
    {
        myText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        myController = FindObjectOfType<Controller>();
        myAdsM = FindObjectOfType<AdsManager>();
        myBird = FindObjectOfType<Bird>();
       // StartCoroutine("Countdown");
    }

    private void Give()
    {
        myBird.Resurrect();
        gameObject.SetActive(false);
    }

    public void OnBecameVisible()
    {
        Debug.Log("soy digno!!");
        //myAdsM.rewardBasedVideo.OnAdRewarded += Give;
        bool hasCoins = Valores.coins >= coins ? true : false,hasVideo = myAdsM.rewardBasedVideo.IsLoaded()?true:false;
        byCoins.interactable = hasCoins;
        byVideo.interactable = hasVideo;
        //Debug.Log("eventos antes:" + byVideo.onClick.GetPersistentEventCount());
        //byVideo.onClick.AddListener(()=> {myAdsM.Show_RewardAd();});
        if (!hasVideo && !hasCoins)
            myController.LoadGameOver();
        //Debug.Log("eventos ahora:"+ byVideo.onClick.GetPersistentEventCount());
    }

    IEnumerator Countdown() {
        Debug.Log("activao");
        for (int i =(int)secs; i>=0; i--) {
            myText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        myController.LoadGameOver();
    }

    public void ShowVideo()
    {
        if (Valores.adsActivated && FindObjectOfType<AdsManager>().rewardBasedVideo.IsLoaded())
        {
            FindObjectOfType<AdsManager>().Show_RewardAd();
        }
    }

    public void Charge() {
        Valores.coins -= coins;
        Debug.Log("actual coins");
        myBird.Resurrect();
    }
}
