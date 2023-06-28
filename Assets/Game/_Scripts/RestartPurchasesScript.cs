using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
public class RestartPurchasesScript : MonoBehaviour
{
    public void UpdateNoAds(Product purchased)
    {
        Debug.Log("estoy aqui en renovar");
        Debug.Log("item para renovar: " + purchased.definition.id);
        string noAdsID = "NO_ADS";       
        if (purchased.definition.id == noAdsID)
        {
            Debug.Log("renove esto");
            Ctrl_Ads myCtrlAds = FindObjectOfType<Ctrl_Ads>();
            Valores.adsActivated = false;
            Valores.UpdateIntField(ref Valores.mySqlDB, 0, "Player", "has_ads");
            Destroy(myCtrlAds.adsManager.gameObject);
        }
    }
    
}
