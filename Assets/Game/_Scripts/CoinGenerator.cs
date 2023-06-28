
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    public GameObject coin;
    [Range(1, 100)]
    public float minSec=2, maxSec=6,secsExtra=0;
    public int point;
    public bool flip;
    public float diff;

    private WallsManager myWallsM;

    private void OnEnable()
    {
        myWallsM = FindObjectOfType<WallsManager>();
        StartCoroutine("GenerateCoin");
    }
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(name);
        
        Ubicate();
        
        FindObjectOfType<PlayerManager>().PlayerLose.AddListener(StopSons);
        myWallsM.OnSpeedChanged.AddListener(ChangeSpeed);

    }


    public virtual void StopSons()
    {
        foreach (Transform t in transform)
        {
            t.gameObject.GetComponent<CoinScript>().speed = 0;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GenerateCoin() {
        float tempMaxSec = maxSec;
        while (true) {
            yield return new WaitWhile(() => myWallsM.Speed >= 15);
            Debug.Log("verificado, siguiente nivel");
            yield return new WaitForSeconds(Random.Range(minSec,tempMaxSec));
            yield return new WaitWhile(()=>Bird.thisBird.IsDead);
            CoinScript myCoin = Instantiate(coin, transform).GetComponent<CoinScript>();
            tempMaxSec += secsExtra;
            myCoin.speed = 0;
            myCoin.Point = point;

        }
        
        }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    public virtual void Ubicate() {
       // Debug.Log("monedisha owo");
        float worldX = flip ? Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight)).x - diff : Camera.main.ScreenToWorldPoint(Vector2.zero).x + diff;
        transform.position = new Vector2(worldX, transform.position.y);
    }
    public void ChangeSpeed() {
       float wgs = myWallsM.Speed;
       // Debug.Log(wgs);
        foreach (Transform t in transform)
            t.GetComponent<CoinScript>().speed = wgs;

    }
}
