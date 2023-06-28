using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;


public class Ctrl_Intro : MonoBehaviour
{
    public GameObject Doug, Heart, HeartGenerator1, effectHeart,reestablecido;
    public PacedBarkScript helpText;
    FaderCircular myFadeC;
    private SqliteDatabase sqlDB;
    public bool testing = true;
    public Animator villainAnim;
    AudioSource myAS;
    public AudioClip heartFlying,touchedHeart;
    Animator dougAnim;
    Ctrl_Ads myCtrlAds;
    private AsyncOperation myAO;
    [Space]
    public Fader fader;

    // Start is called before the first frame update
    private void Awake()
    {


        if (Valores.hasIntro&&!testing)
            SceneManager.LoadScene("Game_Main");

    }
    void Start()
    {
        myFadeC = FindObjectOfType<FaderCircular>();
        myFadeC.Set_Tamaño(0, 0);
        //myFadeC.transform.position = villainAnim.transform.position;
        dougAnim = Doug.transform.Find("Visual").GetComponent<Animator>();
        myAS = GetComponent<AudioSource>();
        myCtrlAds = FindObjectOfType<Ctrl_Ads>();
        StartCoroutine("IntroCR");
        sqlDB = new SqliteDatabase("doug777.db");
        Valores.LoadHasIntro(sqlDB.ExecuteQuery("SELECT IntroHappened FROM Config"));
        Valores.LoadBoolField(sqlDB.ExecuteQuery("SELECT has_ads FROM Player"), ref Valores.adsActivated, "has_ads");
        if (!Valores.adsActivated)
            Destroy(myCtrlAds.adsManager.gameObject);
        
    }

    IEnumerator IntroCR() {
        myAS.clip = heartFlying;
        myAO = SceneManager.LoadSceneAsync("Game_Main");
        myAO.allowSceneActivation = false;
        //se abre el hueco
        yield return myFadeC.Rutina_Tamaño(2, 2.5f);
        helpText.isLooping = true;
        yield return new WaitForSeconds(2.25f);
        yield return myFadeC.Rutina_Tamaño(25, 1);
        helpText.isLooping = false;
        yield return new WaitForSeconds(1f);
        villainAnim.Play("Intro_Villain_S02");
        //buah buah llora la perrita :'v
        //suelta corazoncito
        yield return new WaitForSeconds(1.3f);
        villainAnim.Play("Intro_Villain");
        yield return new WaitForSeconds(.45f);
        GameObject firstHeart =Instantiate(Heart, HeartGenerator1.transform.position,transform.rotation);
        firstHeart.GetComponent<MoveTowards>().velocidad = 15f;
        Camera.main.GetComponent<MoveTowards>().velocidad = 19;
        myAS.PlayDelayed(1.9f);
        yield return new WaitForSeconds(1.7f);
        firstHeart.GetComponent<MoveTowards>().velocidad = 42;
        while (Camera.main.GetComponent<MoveTowards>().velocidad <= 39)
        {
            Camera.main.GetComponent<MoveTowards>().velocidad += Time.deltaTime*10;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(() => firstHeart.transform.position == firstHeart.GetComponent<MoveTowards>().posFinal);
        yield return new WaitForSeconds(0.3f);
        firstHeart.GetComponent<MoveTowards>().posFinal = Doug.transform.position + (Vector3.up * .5f);
        myAS.clip = touchedHeart;
        firstHeart.GetComponent<MoveTowards>().velocidad = 5f;
        yield return new WaitUntil(() => Camera.main.transform.position == Camera.main.GetComponent<MoveTowards>().posFinal);
        //centre el circulito en medio del perritu
        myFadeC.transform.Find("Mascara").position = Doug.transform.position;
        yield return new WaitUntil(() => firstHeart.transform.position == firstHeart.GetComponent<MoveTowards>().posFinal);
        Destroy(firstHeart);
        myAS.Play();
        Instantiate(effectHeart, Doug.transform.position + (Vector3.up * .5f), effectHeart.transform.rotation);
        yield return new WaitForSeconds(1);
        Doug.transform.Find("Visual").GetComponent<Animator>().SetTrigger("touchedBytheHeart");
        yield return new WaitForSeconds(dougAnim.GetCurrentAnimatorStateInfo(0).length+3f);
        //cierra circulito
        yield return myFadeC.Rutina_Tamaño(2.61f, 0.71f);
        yield return new WaitForSeconds(1.1f);
        yield return myFadeC.Rutina_Tamaño(0, 0.7f);
        Valores.hasIntro = true;
        Valores.SaveHasIntro(ref sqlDB);
        myAO.allowSceneActivation = true;
    }
    public void Interrupt()
    {
        StopAllCoroutines();

        StartCoroutine(Rutina_Fade());
    }

    IEnumerator Rutina_Fade ()
    {
        fader.RayTarget(true);

        yield return fader.Fade(1, fader.duracionFade);

        yield return null;

        myAO.allowSceneActivation = true;

        yield return null;
        StopCoroutine(Rutina_Fade());
    }

}
