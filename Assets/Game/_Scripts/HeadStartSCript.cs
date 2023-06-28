using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class HeadStartSCript : MonoBehaviour
{
    private Button myButton;
    private Controller myController;
    public float speedBooster=10,secs=5;
    public int cost = 50;
    public Image bubble;

    // Start is called before the first frame update
    private void OnEnable()
    {
        myButton = GetComponent<Button>();

        if (Valores.coins < cost)
            myButton.interactable = false;
        if (Valores.timesPlayed >= 3)
            bubble.gameObject.SetActive(false);
    }
    void Start()
    {
        Bird.thisBird = FindObjectOfType<Bird>();
        myController = FindObjectOfType<Controller>();


    }

    // Update is called once per frame
    public async void Activate() {
        Bird.thisBird.enabled = true;
        Valores.coins -= cost;
        WallsManager myWallsM = FindObjectOfType<WallsManager>();
        myWallsM.BoosterActivated = true;
        myWallsM.speed += speedBooster;
        myWallsM.MoveWalls();
        Valores.SaveCoins();
        await Bird.thisBird.GottaGoFast(secs);
        
       
    }
    public async void ButtonSwitcher() {
        myController.startbutton.GetComponent<Image>().enabled = false;
        await Task.Delay((int)(secs*1.42f * 1000));
        Bird.thisBird.GetComponent<Rigidbody2D>().gravityScale = 1;
        myController.startbutton.GetComponent<Image>().enabled = true;
    }
}
