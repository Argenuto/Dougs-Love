using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public bool isWalkingToCenter = false;
    public float secsDelay = 5;
    public float walkingSpeed;
    public UnityEvent PlayerLose,PlayerResurrect;
    private int tempChar;
    private Animator myAnim;
    float floor,celing;
    public SnapObject[] dogsAnims;
    Vector2 pos;
    private bool invoked=false;
    private bool playerLoseIsInvoked;
    UnityEvent charSelected;
    Bird myBird;
    Rigidbody2D myrb2d;
    internal static PlayerManager thisPM;
    public bool PlayerLoseIsInvoked { get => playerLoseIsInvoked; set => playerLoseIsInvoked = value; }
    internal CapsuleCollider2D myCapColl;

    // Start is called before the first frame update
    private void OnEnable()
    {
        thisPM = this;
    }
    void Start()
    {
        pos = transform.position;
        charSelected = new UnityEvent();
        tempChar = Valores.personajeSeleccioando;
        Debug.Log("Personaje Seleccionad: " + Valores.personajeSeleccioando);
        myAnim = transform.Find("Sprite").GetComponent<Animator>();
        SelectDog();
        // myAnim.SetFloat("Personaje", Valores.personajeSeleccioando);
        //charSelected.AddListener(() => myAnim.SetFloat("Personaje", Valores.personajeSeleccioando));
        // PlayerLose.AddListener(FindObjectOfType<AdsManager>().Request_Banner);
        celing = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight)).y;
        floor =Camera.main.ScreenToWorldPoint(Vector2.zero).y;
        PlayerLose.AddListener(()=>GetComponent<Rigidbody2D>().gravityScale =0);
        PlayerLose.AddListener(()=>GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static);
        PlayerLose.AddListener(()=> transform.position = transform.position = new Vector2(0, transform.position.y));
      //  PlayerLose.AddListener(()=>FindObjectOfType<Controller>().LoadGameOver());
        Debug.Log("Veces Jugadas: "+Valores.timesPlayed);
        //Debug.Log("floor: " + floor);
        //Debug.Log("ceiling: " + celing);
        myBird = GetComponent<Bird>();
        myrb2d = GetComponent<Rigidbody2D>();
        myCapColl = GetComponent<CapsuleCollider2D>();
    }

    public void SelectDog()
    {
        for (int i = 0; i < dogsAnims.Length; i++)
        {
            if (Valores.personajeSeleccioando == dogsAnims[i].ID)
            {
                myAnim.runtimeAnimatorController = dogsAnims[i].animatorControler;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
       
        if (!IsInthere(floor-1,celing,transform.position.y)&& !playerLoseIsInvoked)
        {
            myBird.invulnerable = false;
            PlayerLose.Invoke();
            playerLoseIsInvoked = true;
            
            // Debug.Log( PlayerLose.GetPersistentEventCount());
        }

        if (isWalkingToCenter && Time.time>=secsDelay&&!invoked)
        {
            transform.position = Vector3.MoveTowards(transform.position, Vector2.down * 4.2f, walkingSpeed * Time.deltaTime);
            transform.GetComponentInChildren<Animator>().SetFloat("distance", Vector2.Distance(transform.position, Vector2.down * 4.2f));
            
        }
        if (tempChar != Valores.personajeSeleccioando)
        {
            charSelected.Invoke();
            tempChar = Valores.personajeSeleccioando;
        }
        
    }
    private void LateUpdate()
    {

    }
    bool IsInthere(float min,float max,float x) {
        if (x >= min && x < max)
            return true;
        else {
            return false; }
            
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
    }
    public void ResetInitials() {
        transform.position = pos;
        transform.rotation = Quaternion.identity;
        myAnim.SetTrigger("reset");
        myAnim.SetBool("jumping", false);
        myBird.IsDead = false;
        myrb2d.bodyType = RigidbodyType2D.Static;
        myrb2d.gravityScale = 0;
        myrb2d.bodyType = RigidbodyType2D.Dynamic;
        myBird.vertDelta = 0;
        myBird.Direction = 1;
        myBird.firstJumpMultiplier = 1;
        myAnim.GetComponent<Rotation>().activated = false;
        myBird.Anim.transform.rotation = Quaternion.identity;
        myBird.enabled = false;
        myAnim.SetFloat("vert_delta",-9999);
        playerLoseIsInvoked = false; 
    }
}
