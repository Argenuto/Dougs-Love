using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarScript : CoinScript
{
    // Start is called before the first frame update
    public int invSecs;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Activate(ref Collider2D other)
    {
        Bird myBird = FindObjectOfType<Bird>();
        myBird.CantTouchThis(invSecs);
    }
    public override void SetInitialMovement()
    {
        rb2d.gravityScale = 0.05f;
        speed = 1;
    }
    public override void Movement()
    {
        transform.Translate(Vector2.right * Direction * Time.deltaTime);
        if ((Right - transform.position.x) < 0.01)
            Direction *= -1;
        if ((transform.position.x - Left) < 0.01)
            Direction *= -1;
    }
}
