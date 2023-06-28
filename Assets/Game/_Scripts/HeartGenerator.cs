using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartGenerator : CoinGenerator
{
  
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Ubicate() {
        //Debug.Log("carazoncito uwu");
        float worldX = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth/2, Camera.main.pixelHeight)).x;
        transform.position = new Vector2(worldX, transform.position.y);
    }
    public override void StopSons()
    {
        foreach (Transform t in transform)
        {
            t.gameObject.GetComponent<CoinScript>().speed = 0;

        }
    }

}
