using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Ctrl_GameOver : MonoBehaviour
{
    AsyncOperation newEscena;

    public Ctrl_InterfazGameOver interfaz;
    public float velocidad;
    public TextMeshProUGUI maxScore;
    [Space]
    [SerializeField] int repeticionesParaPublicidad;

    private void Awake()
    {
        newEscena = null;

        interfaz.fader.RayTarget(false);
    }

    private void Start()
    {
        maxScore.text = Valores.maxPoints.ToString("D2");
        StartCoroutine(Rutina_Inicio());

    }

    public void FaceBookShare()
    {
        ScreenshotHandler.instance.Share();    }

    public void Btn_Reiniciar ()
    {
        StartCoroutine(Rutina_Reiniciar());
    }

    IEnumerator Rutina_Inicio ()
    {
        yield return null;

        FindObjectOfType<AudioManager>().Play("MusicaGameOver");

        Vector3 newPos = interfaz.panel_gameOver.transform.position;

        newPos.y = 11;

        interfaz.panel_gameOver.transform.position = newPos;

        while (true)
        {
            newPos.y = Mathf.MoveTowards(newPos.y, 0f, velocidad * Time.deltaTime);

            interfaz.panel_gameOver.transform.position = newPos;

            if (Mathf.Approximately(interfaz.panel_gameOver.transform.position.y, 0f))
            {
                newPos.y = 0;

                interfaz.panel_gameOver.transform.position = newPos;

                break;
            }

            yield return null;
        }

        yield return null;
        /*
        if (newEscena == null)
        {
            SceneManager.UnloadSceneAsync("Game_Main");
            newEscena = SceneManager.LoadSceneAsync("Game_Main", LoadSceneMode.Additive);
            newEscena.allowSceneActivation = false;
        }
        
        if (Ctrl_Escenas.main == null)
        {
            SceneManager.UnloadSceneAsync("Game_Main");
            Ctrl_Escenas.main = SceneManager.LoadSceneAsync("Game_Main", LoadSceneMode.Additive);
            Ctrl_Escenas.main.allowSceneActivation = false;
        }
        */
        if (FindObjectOfType<Controller>())
        {
            Controller ctrl_main = FindObjectOfType<Controller>();

            ctrl_main.ResetWithNoUnload();
        }
        else
        {
            Debug.Log("Controlador no encontrado");
        }

        if (FindObjectOfType<Ctrl_InterfazMain>())
        {
            Ctrl_InterfazMain ctrl_InterfazMain = FindObjectOfType<Ctrl_InterfazMain>();

            ctrl_InterfazMain.panel_Continuar.gameObject.SetActive(false);

            ctrl_InterfazMain.fader.RayTarget(false);
        }
        else
        {
            Debug.Log("Controlador interfaz main no encontrado");
        }

        interfaz.fader.RayTarget(false);

        //          LLAMAR INSTERSTITIAL
        /*
        if (Valores.adsActivated && FindObjectOfType<AdsManager>().interstitial.IsLoaded())
        {
            int r = Random.Range(1, 101);

            if (r >= 70)
            {
                FindObjectOfType<AdsManager>().Show_Interstitial();
            }
        }
        */
        if (Valores.adsActivated && FindObjectOfType<AdsManager>().interstitial.IsLoaded() && (Valores.timesPlayed % repeticionesParaPublicidad) == 0)
        {
            FindObjectOfType<AdsManager>().Show_Interstitial();
        }

        yield return null;
        StopCoroutine(Rutina_Inicio());
    }

    IEnumerator Rutina_Reiniciar ()
    {/*
        if (Ctrl_Escenas.main != null)
        {
            Ctrl_Escenas.main.allowSceneActivation = true;
            Ctrl_Escenas.main = null;
        }
        */
        interfaz.fader.RayTarget(true);

        if (Valores.adsActivated)
        {
            AdsManager adsManager = FindObjectOfType<AdsManager>();

            if (adsManager.rewardBasedVideo.IsLoaded())
            {
                adsManager.Destruir_Banner();

                adsManager.Request_Banner();
            }
        }

        Vector3 newPos = interfaz.panel_gameOver.transform.position;

        newPos.y = 0;

        interfaz.panel_gameOver.transform.position = newPos;

        while (true)
        {
            newPos.y = Mathf.MoveTowards(newPos.y, -11f, velocidad * Time.deltaTime);

            interfaz.panel_gameOver.transform.position = newPos;

            if (Mathf.Approximately(interfaz.panel_gameOver.transform.position.y, -11f))
            {
                newPos.y = -11;

                interfaz.panel_gameOver.transform.position = newPos;

                break;
            }

            yield return null;
        }
        /*
        yield return null;
        
        if (newEscena != null)
        {
            newEscena.allowSceneActivation = true;
        }
        */
        yield return null;

        interfaz.fader.RayTarget(false);

        yield return null;

        SceneManager.UnloadSceneAsync("GameOverScene");

        yield return null;
        StopCoroutine(Rutina_Reiniciar());
    }
}
