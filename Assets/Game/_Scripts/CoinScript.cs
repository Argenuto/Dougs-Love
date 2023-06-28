using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public float speed=0;
    protected Rigidbody2D rb2d;
    int point=1,direction=1;
    public ParticleSystem CatchParticle;
    public AudioClip takenClip;
    AudioSource myAS;
    public int Point { get => point; set => point = value; }
    public float Left { get => left; set => left = value; }
    public float Right { get => right; set => right = value; }
    public int Direction { get => direction; set => direction = value; }
    public AudioSource MyAS { get => myAS; set => myAS = value; }
    internal bool isMagnet = false;
    public float magnetDelay = 1,magnetSpeed=1;
    private float left,right;
    internal float tempMD = 0;
    private void Awake()
    {
        left = Camera.main.ViewportToWorldPoint(new Vector2(0.2f, 0)).x;
        right = Camera.main.ViewportToWorldPoint(new Vector2(0.8f, 0)).x;
        myAS = GetComponent<AudioSource>();
        myAS.clip = takenClip;
    }
    private void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
        SetInitialMovement();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }
    // Update is called once per frame
    public virtual void Movement()
    {
        if (speed != 0)
        {
            rb2d.MovePosition((Vector2)transform.position + Vector2.down * speed * Time.fixedDeltaTime);
        }
        if (isMagnet)
            MagnetMovement();
        
    }

    private void MagnetMovement()
    {
        if (tempMD >= magnetDelay)
        {
            transform.position= Vector2.Lerp(transform.position,Bird.instance.transform.position,Time.fixedDeltaTime*magnetSpeed);
            Debug.Log("Accion turbo");
        }
        tempMD += Time.fixedDeltaTime;
    }

    void OnBecameInvisible()
    {
        //Debug.Log("desapareci");
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !other.GetComponent<Bird>().IsDead)
        {
            Activate(ref other);
            Catch();
        }
    }

   public virtual void Catch()
    {
        Debug.Log("tomado");
        myAS.clip = takenClip;
        GameObject go = new GameObject("cling");
        go.AddComponent<AudioSource>();
        go.GetComponent<AudioSource>().outputAudioMixerGroup = myAS.outputAudioMixerGroup;
        go.GetComponent<AudioSource>().clip = myAS.clip;
        go.GetComponent<AudioSource>().playOnAwake = true;
        Destroy(go, 3f);
        GameObject instant = Instantiate(CatchParticle, transform.position, transform.rotation).gameObject;
        instant.name = "instant";
        //instant.GetComponent<CoinScript>().speed = 0;
        Destroy(instant, 2f);
        Destroy(gameObject);
    }

    public virtual  void Activate(ref Collider2D other)
    {
        //Debug.Log("monedisha");
        FindObjectOfType<Controller>().MonedasActuales += point;
        Valores.coins += point;
        // Debug.Log (FindObjectOfType<Controller>().Coins);
    }

    public virtual void SetInitialMovement()
    {
        
        isMagnet = Bird.instance.isFlying;
        if (isMagnet)
            transform.parent = null;
        //rb2d.gravityScale = 0.05f;
        speed = 0;
    }

}

