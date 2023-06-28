using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;

public class GameOverInitializer : MonoBehaviour
{
    private AsyncOperation newGame;
    //private bool played=false;
    /*
    void Awake()
    {
        transform.Find("MainPanel/GoScore").GetComponent<TextMeshProUGUI>().text = Valores.gameOverScore.ToString();
        transform.Find("MainPanel/Coins_Found/Text").GetComponent<TextMeshProUGUI>().text = Valores.gameOverCoins;
        if (!played) {
            GetComponent<AudioSource>().Play();
            
            played = true;

        }
        
    }
    */
    public void Close()
    {
        if (newGame == null)
        {
            SceneManager.UnloadSceneAsync("Game_Main");
            newGame = SceneManager.LoadSceneAsync("Game_Main", LoadSceneMode.Additive);
            newGame.allowSceneActivation = false;
            newGame.completed += GoodBye;
        }
        //GetComponent<Animator>().enabled = false;

    }

    private async void GoodBye(AsyncOperation obj)
    {
        await Task.Delay(1000);
        SceneManager.UnloadSceneAsync("GameOverScene");
    }

    public  void Restart()
    {   
        newGame.allowSceneActivation = true;
    }

    public void AddShopScene()
    {
        SceneManager.LoadScene("Shop", LoadSceneMode.Additive);
    }
    
}
