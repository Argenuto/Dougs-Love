using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.Events;
using System.Threading.Tasks;
using UnityEngine.Audio;

public class Controller : MonoBehaviour
{
    public Ctrl_InterfazMain interfaz;
    [HideInInspector]
    public static Controller thisController;
    [Space]
    public TextMeshProUGUI Score, myCoins;
    public int monedasActuales = 0;
    public int valorParaContinuar;
    public WallsManager myWallsM;
    public SpriteRenderer myBackground;
    public Canvas myMainCanvas;
    public Button pauseButton;
    public float verticalDifficulty = 1;
    public Toggle[] music, sound;
    // public AdmobManager myAdMobManager;
    public int MonedasActuales { get => monedasActuales; set => monedasActuales = value; }
    [HideInInspector]
    internal SqliteDatabase sqlDB;
    public AudioMixer myAM;
    public DataTable PlayerDT { get; private set; }
    internal bool tried;
    private DataTable CharacterDT,achievementsDT;
    public DataTable mySettings;
    //public ScrollingUVs_PDS mysuvs;
    public GameObject startbutton, gottaGetFastBG, killingCatController, tryAgain;
    public UnityEvent gamePaused, gameUnpaused,gameReseted;
    private PlayerManager myPlayerM;
    internal int orange_dodged;
    internal int black_dodged;
    internal int jumper_dodged;

    /*void OnValidate(){
myBackground.transform.position =new Vector3(myBackground.transform.position.x,myBackground.transform.position.y,myMainCanvas.transform.position.z + 1);

}*/
    // Use this for initialization

    private void OnEnable()
    {
        thisController = this;
        sqlDB = new SqliteDatabase("doug777.db");
        Valores.mySqlDB = sqlDB;
        mySettings = sqlDB.ExecuteQuery("SELECT * FROM Config");
        PlayerDT = sqlDB.ExecuteQuery("SELECT * FROM Player");
        CharacterDT = sqlDB.ExecuteQuery("SELECT * FROM Character");
        achievementsDT = sqlDB.ExecuteQuery("SELECT * FROM Achievements");
        Valores.LoadCharId(PlayerDT);
        Valores.LoadConfig(mySettings);
        Valores.LoadMaxScore(PlayerDT);
        Valores.LoadCoins(PlayerDT);
        Valores.LoadCharacters(CharacterDT);
        Valores.LoadIntField(PlayerDT, ref Valores.timesPlayed, "times_played");
        Valores.LoadCompleteDT(achievementsDT,ref Valores.achievementDT);
        Valores.timesShocked = Mathf.RoundToInt(float.Parse(Valores.achievementDT[0]["goal"].ToString()) *float.Parse(Valores.achievementDT[0]["progress"].ToString()));
        
        // mysuvs.transform.localScale = Vector2.one* Mathf.Max(Camera.main.pixelWidth,Camera.main.pixelHeight);
        //FindObjectOfType<SplashChecker>().SplashCheck();

        myPlayerM = FindObjectOfType<PlayerManager>();

    }
    void Start()
    {
        //	OnValidate ();
        // maxScore.text = Valores.maxPoints.ToString();
        monedasActuales = 0;
        orange_dodged = 0;
        black_dodged = 0;
        jumper_dodged = 0;
        myPlayerM.PlayerLose.AddListener(() => Valores.gameOverScore = int.Parse(Score.text));
        /*
        myPlayerM.PlayerLose.AddListener(() =>
        {
            if (tried) {

                LoadGameOver();
                tried = false;
            } else
                tryAgain.GetComponent<HideAndShow>().Show();
        });
        */
        //AGREGADO POR CIRO
        myPlayerM.PlayerLose.AddListener(GameOver);
        //+++++++++++++++++++++++++++++++++++++++++
        //pauseButton.onClick.AddListener(PauseTheGame);
        myPlayerM.PlayerLose.AddListener(() => { Valores.timesPlayed++; Valores.UpdateIntField(ref sqlDB, Valores.timesPlayed, "Player", "times_played"); Debug.Log("actualizada: " + Valores.timesPlayed); });

        InitializeMuteButton(Valores.hasMusic, Valores.hasSound);
        StartTheGame();
        Debug.Log(Mathf.RoundToInt(float.Parse(Valores.achievementDT[0]["goal"].ToString()))+" - "+Valores.timesShocked);

        if (Valores.adsActivated)
        {
            StartCoroutine(Rutina_CargarBanner());
        }
    }

    private void StartTheGame()
    {
        GameObject.Find("MainCanvas/Title_Logo").GetComponent<Animator>().SetBool("splashed", true);
        FindObjectOfType<Controller>().startbutton.SetActive(true);
    }

    private void InitializeMuteButton(bool hasMusic, bool hasSound)
    {
        foreach (Toggle tgl in music)
        {
            tgl.isOn = hasMusic;
            tgl.onValueChanged.Invoke(tgl.isOn);
        }
        foreach (Toggle tgl in sound)
        {
            tgl.isOn = hasSound;
            tgl.onValueChanged.Invoke(tgl.isOn);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            Score.text = Mathf.RoundToInt(myWallsM.Score).ToString("D2");
            myCoins.text = monedasActuales.ToString("D2");
        }

        if (myWallsM.Score > Valores.maxPoints)
        {
            Valores.maxPoints = (int)myWallsM.Score;
        }
    }

    public void MuteMusic(bool hasSound)
    {
        if (hasSound)
            myAM.SetFloat("MusicVol", 0f);
        else
            myAM.SetFloat("MusicVol", -80f);
        Valores.hasMusic = hasSound;
        Valores.SaveConfig(ref sqlDB);
    }

    public void MuteSound(bool hasSound)
    {
        if (hasSound)
            myAM.SetFloat("SoundVol", 0f);
        else
            myAM.SetFloat("SoundVol", -80f);
        Valores.hasSound = hasSound;
        Valores.SaveConfig(ref sqlDB);
    }

    public void Reset()
    {
        Time.timeScale = 1;
        // FindObjectOfType<AdsManager>().Destruir_Banner();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public async void RetryReset()
    {
        Scene s = SceneManager.GetActiveScene();
        AsyncOperation loads = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        loads.allowSceneActivation = false;
        await Task.Delay(250);
        loads.allowSceneActivation = true;
    }
    //          EDITADO POR CIRO
    public void Btn_PauseOn ()
    {
        gamePaused.Invoke();

        StartCoroutine(Rutina_PauseOn());
    }

    public void Btn_PauseOff ()
    {
        StartCoroutine(Rutina_PauseOff());
    }

    public void Btn_PauseReiniciar ()
    {
        ResetWithNoUnload();

        StartCoroutine(Rutina_PauseReiniciar());
    }

    public void Actualizar_TimeScale (int newScale)
    {
        if (newScale < 0)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = newScale;
        }
    }
    //+++++++++++++++++++++++++++
    public void SendGameOverCoins()
    {
        Valores.gameOverCoins = monedasActuales.ToString();
    }
    //          CREADO POR CIRO
    public void GameOver()
    {
        var adsManager = FindObjectOfType<AdsManager>();
        bool rewardVideoLoaded = false;
        if (adsManager != null)
             rewardVideoLoaded = adsManager.rewardBasedVideo.IsLoaded();

            if (!tried && (Valores.adsActivated && rewardVideoLoaded || Valores.coins >= valorParaContinuar))
            {
                Debug.Log("Abrir panel continuar");

                FindObjectOfType<Ctrl_InterfazMain>().panel_Continuar.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("GameOver");

                LoadGameOver();
            }
    }
    //+++++++++++++++++++++++++++++++++++++
    public void LoadGameOver()
    {
        Valores.SaveMaxScore(ref sqlDB);
        Valores.SaveCoins();
        SceneManager.LoadSceneAsync("GameOverScene", LoadSceneMode.Additive);
    }

    public void loadShop()
    {
        SceneManager.LoadScene("Shop", LoadSceneMode.Additive);
    }

    public void ClearNumbers()
    {
        monedasActuales = 0;
        Score.text = 0.ToString("D2");
        myCoins.text = 0.ToString("D2");
        myWallsM.score = 0;
    }

    public void ResetWithNoUnload()
    {
        Time.timeScale = 0;
        ClearNumbers();//se van a cero todas las puntuaciones;
        myWallsM.ClearAll();//borro Todas las paredes y reseteo las probabilidades de aparacion
        myPlayerM.ResetInitials();//Doug resetea su "estado", ubicación y física
        Camera.main.GetComponent<CameraMovement>().farCamera.transform.localPosition = Camera.main.GetComponent<CameraMovement>().myInitialPosition;//se reinicia el parallax
        startbutton.transform.parent.gameObject.SetActive(true);//se activa el boton start
        myWallsM.InitiateAll();//se ponen paredes iniciales y se configura la velocidad a 0
        gameReseted.Invoke();//Todo lo demas
        //myMainCanvas.transform.Find("PausePanel").GetComponent<Animator>().SetBool("paused", false);
        Time.timeScale = 1;
        //AGREGADO POR CIRO
        tried = false;
    }

    #region CORRUTINAS

    IEnumerator Rutina_CargarBanner ()
    {
        yield return null;
        var adsManager = FindObjectOfType<AdsManager>();
        if(adsManager != null)
            adsManager.Request_Banner();
        yield return null;
        StopCoroutine(Rutina_CargarBanner());
        yield break;
    }

    IEnumerator Rutina_PauseOn ()
    {
        interfaz.fader.RayTarget(true);

        Vector3 newPos = interfaz.panel_pause.transform.position;
        
        newPos.y = 11;

        interfaz.panel_pause.transform.position = newPos;
        
        while (true)
        {
            newPos.y = Mathf.MoveTowards(newPos.y, 0, 20f * Time.unscaledDeltaTime);

            interfaz.panel_pause.transform.position = newPos;

            if (Mathf.Approximately(interfaz.panel_pause.transform.position.y, 0))
            {
                newPos.y = 0;

                interfaz.panel_pause.transform.position = newPos;

                break;
            }

            yield return null;
        }

        interfaz.fader.RayTarget(false);

        yield return null;
        StopCoroutine(Rutina_PauseOn());
    }

    IEnumerator Rutina_PauseOff ()
    {
        interfaz.fader.RayTarget(true);

        Vector3 newPos = interfaz.panel_pause.transform.position;
        
        while (true)
        {
            newPos.y = Mathf.MoveTowards(newPos.y, -11f, 20f * Time.unscaledDeltaTime);

            interfaz.panel_pause.transform.position = newPos;

            if (Mathf.Approximately(interfaz.panel_pause.transform.position.y, -11f))
            {
                newPos.y = -11f;

                interfaz.panel_pause.transform.position = newPos;

                break;
            }

            yield return null;
        }

        yield return null;

        gameUnpaused.Invoke();

        interfaz.fader.RayTarget(false);

        yield return null;
        StopCoroutine(Rutina_PauseOff());
    }

    IEnumerator Rutina_PauseReiniciar ()
    {
        interfaz.fader.RayTarget(true);

        Vector3 newPos = interfaz.panel_pause.transform.position;

        while (true)
        {
            newPos.y = Mathf.MoveTowards(newPos.y, -11f, 20f * Time.unscaledDeltaTime);

            interfaz.panel_pause.transform.position = newPos;

            if (Mathf.Approximately(interfaz.panel_pause.transform.position.y, -11f))
            {
                newPos.y = -11f;

                interfaz.panel_pause.transform.position = newPos;

                break;
            }

            yield return null;
        }

        yield return null;

        interfaz.fader.RayTarget(false);

        yield return null;
        StopCoroutine(Rutina_PauseReiniciar());
    }

    #endregion
}
