using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartScript : CoinScript
{
    public float speedBooster;
    public float secs;

    public override void Activate(ref Collider2D other)
    {
        
      
        Bird myBird= other.GetComponent<Bird>();
        WallsManager myWallsM = FindObjectOfType<WallsManager>();
        myWallsM.BoosterActivated = true;
        myWallsM.speed += speedBooster;
        myWallsM.MoveWalls();
        myBird.GottaGoFast(secs);
        //myBird.Anim.SetBool("jumping", true);

        Debug.Log("right: " + Right + "--- Left: " + Left);
    }

    public override void Movement()
    {
        transform.Translate(Vector2.right * Direction*Time.deltaTime);
        if ((Right - transform.position.x) < 0.01)
            Direction *= -1;
        if ((transform.position.x - Left) < 0.01)
            Direction *= -1;
    }
    public override void SetInitialMovement()
    {
        rb2d.gravityScale = 0.05f;
        speed = 1;
    }
}
