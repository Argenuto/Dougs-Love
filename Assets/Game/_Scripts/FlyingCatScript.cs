using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlyingCatScript : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject hit;
    SpriteRenderer mySR;
    public Sprite hitCat;
    Animator myAnim;
    Rigidbody2D myRb2D;
    public float underThat = -10;

    private void Update()
    {
        if (transform.position.y <= underThat)
            Destroy(gameObject);
    }

    private void Start()
    {
        hit = transform.Find("Hit").gameObject;
        mySR = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        myAnim = transform.Find("Sprite").GetComponent<Animator>();
        myRb2D = GetComponent<Rigidbody2D>();

    }
    public void CatHitByShield(Vector3 dir) {
        hit.GetComponent<ParticleSystem>().Play();
        hit.GetComponent<AudioSource>().Play();
        mySR.sprite = hitCat;
        Debug.Log("empuje normalizado"+dir.normalized);
        myRb2D.velocity = (Vector2)dir*10;
        myAnim.enabled = false;
    }
}
