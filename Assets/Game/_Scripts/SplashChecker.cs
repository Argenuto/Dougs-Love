using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashChecker : MonoBehaviour
{
    public bool SplashInitialized;
    private SplashChecker instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

        public void SplashCheck() {

        if (!SplashInitialized)
            GameObject.FindGameObjectWithTag("Player").transform.Translate(Vector2.left * 5);
        else
        {
          //  GameObject.Find("MainCanvas/Logo_Taga").SetActive(false);
            GameObject.Find("StartButtonCanvas/Pattern").SetActive(false);
            GameObject.Find("MainCanvas/Title_Logo").GetComponent<Animator>().SetBool("splashed", true);
            FindObjectOfType<Controller>().startbutton.SetActive(true);
           // GetComponent<AdsManager>().Destruir_Banner();

        }
    }
}
