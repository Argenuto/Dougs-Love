using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class Panel_Continuar : MonoBehaviour
{
    Controller ctrl;
    AdsManager adsManager;

    public Button btn_video, btn_monedas;
    public TextMeshProUGUI txt_monedas;

    private void OnEnable()
    {
        Debug.Log("Panel continuar activado");



        adsManager = FindObjectOfType<AdsManager>();

        if (adsManager != null)
            if (Valores.adsActivated && adsManager.rewardBasedVideo.IsLoaded())
            {
                btn_video.interactable = true;
            }
            else
            {
                btn_video.interactable = false;
            }

        if (ctrl == null)
        {
            ctrl = FindObjectOfType<Controller>();

            txt_monedas.text = ctrl.valorParaContinuar.ToString();
        }

        if (Valores.coins >= ctrl.valorParaContinuar)
        {
            btn_monedas.interactable = true;

            txt_monedas.color = new Color(txt_monedas.color.r, txt_monedas.color.g, txt_monedas.color.b, 1f);
        }
        else
        {
            btn_monedas.interactable = false;

            txt_monedas.color = new Color(txt_monedas.color.r, txt_monedas.color.g, txt_monedas.color.b, 0.5f);
        }
    }

    private void Update()
    {
        if (adsManager != null)
            if (Valores.adsActivated && adsManager.videoRecompensado)
            {
                Debug.Log("Visto video para continuar");

                adsManager.videoRecompensado = false;

                FindObjectOfType<Bird>().Resurrect();

                ctrl.tried = true;

                gameObject.SetActive(false);
            }
    }

    public void Btn_VerVideo ()
    {
        Debug.Log("Activar video recompensado");

        if (Valores.adsActivated && adsManager.rewardBasedVideo.IsLoaded())
        {
            adsManager.Show_RewardAd();
        }
    }

    public void Btn_PagarParaContinuar ()
    {
        Debug.Log("Pagar para continuar");

        Valores.coins -= ctrl.valorParaContinuar;

        Valores.SaveCoins();

        FindObjectOfType<Bird>().Resurrect();

        ctrl.tried = true;

        gameObject.SetActive(false);
    }

    public void Btn_NoContinuar()
    {
        Debug.Log("No Continuar");

        FindObjectOfType<Ctrl_InterfazMain>().fader.RayTarget(true);

        ctrl.LoadGameOver();
    }

    public void ReproducirSonido (string nombreSonido)
    {
        FindObjectOfType<AudioManager>().Play(nombreSonido);
    }
}
