using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class ShopController : MonoBehaviour
{
    public Button noAdsButton;
    private Ctrl_Shop myCtrlShop;
    private AdsManager myAdsManager;
    private Ctrl_Ads myCtrlAds;

    public void UpdatePurchasing() {
        Valores.coins = myCtrlShop.monedas;
        Valores.UpdatePurchasedCharacter(new SqliteDatabase("doug777.db"));
    }
    private void Start()
    {
        myCtrlShop = GetComponent<Ctrl_Shop>();

        myAdsManager = FindObjectOfType<AdsManager>();
        if (Valores.mySqlDB == null)
        {
            Valores.mySqlDB = new SqliteDatabase("doug777.db");
            Valores.LoadBoolField(Valores.mySqlDB.ExecuteQuery("SELECT has_ads FROM Player"), ref Valores.adsActivated, "has_ads");
        }
        if (!Valores.adsActivated)
        {
            noAdsButton.interactable = false;
            noAdsButton.GetComponent<RectTransform>().sizeDelta = new Vector2(400,112);
            noAdsButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Purchased";
            noAdsButton.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(400, 112);
        }
    }
    public void UpdateSelectedChar()
    {
        SqliteDatabase sqlitedb = new SqliteDatabase("doug777.db");
        Valores.SaveCharId(ref sqlitedb);
    }
    public void ChargeCoins(Product purchased) {
        
        PayoutDefinition coin = purchased.definition.payouts.ElementAt<PayoutDefinition>(0);
        Valores.coins += (int)coin.quantity;
        Valores.SaveCoins();
        myCtrlShop.monedas = Valores.coins;
        myCtrlShop.Actualizar_TxtMoney();
        Debug.Log(coin.quantity); 
    }
    public void UpdateNoAds() {
        noAdsButton.interactable = false;
        noAdsButton.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 112);
        noAdsButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Purchased";
        noAdsButton.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(400, 112);
        myCtrlAds = FindObjectOfType<Ctrl_Ads>();
        Valores.adsActivated = false;
        Valores.UpdateIntField(ref Valores.mySqlDB, 0, "Player", "has_ads");
        Destroy(myCtrlAds.adsManager.gameObject);
    }
}
