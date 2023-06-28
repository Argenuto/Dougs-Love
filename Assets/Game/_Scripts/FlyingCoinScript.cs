using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCoinScript : CoinScript
{
    bool catched = false;
    Vector3 coinsPlace;
    public float flySpeed=1;
    private float step = 0;
    bool moved = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!catched)
        {
            try
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, (transform.parent.GetComponent<CoinGenerator>().flip ? Vector2.right : Vector2.left), 5, LayerMask.GetMask("Default"));

                if (hit.collider.tag == "box" || hit.collider.tag == "LaserBeam" || hit.collider.tag == "cat")
                {
                    Destroy(gameObject);
                }
            }
            catch (NullReferenceException nre)
            {
                Debug.Log(nre.Message);


            }
        }
    }

    public override void Catch()
    {
       
        catched = true;  
    }

    public override void Movement()
    {
        if (!catched)
            base.Movement();
        else
        {
           step += flySpeed * Time.fixedDeltaTime;
           transform.position = Vector2.Lerp(transform.position, coinsPlace, step);
            if (Vector2.Distance(transform.position, coinsPlace) < 0.009)
            {
                FindObjectOfType<Controller>().MonedasActuales += Point;
                Valores.coins += Point;
                base.Catch();

            }
        }
       
    }
    public override void SetInitialMovement()
    {
        base.SetInitialMovement();
        if(!FindObjectOfType<Bird>().IsDead)
            coinsPlace = GameObject.Find("CoinsLine/Sprite/Guide").GetComponent<RectTransform>().position;
       
    }

    public override void Activate(ref Collider2D other)
    {
        
    }
}
