using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ctrl_Ads : MonoBehaviour
{
    [HideInInspector]
    public static Ctrl_Ads instance;

    public AdsManager adsManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        if (Valores.adsActivated)
        {
            adsManager.gameObject.SetActive(true);
        }
    }
}
