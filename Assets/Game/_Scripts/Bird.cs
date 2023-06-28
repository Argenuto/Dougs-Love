using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bird : MonoBehaviour
{
    internal static Bird thisBird;
    public float upForce, sideForce;                   //Upward force of the "flap".
    public bool isDead = false;            //Has the player collided with a wall?
    int direction = 1;
    public float firstJumpMultiplier;
    private Animator anim;                  //Reference to the Animator component.
    private Rigidbody2D rb2d;               //Holds a reference to the Rigidbody2D component of the bird.
    private AudioSource myAs;
    public float vertDelta = 0;
    float tempY = 0;
    public int Direction { get => direction; set => direction = value; }
    public Animator Anim { get => anim; set => anim = value; }
    public bool IsDead { get => isDead; set => isDead = value; }
    public AudioClip doink, jump;
    public bool invulnerable = false;
    private MeshRenderer shield;
    private WallsManager myWallsM;
    [Range(0.01f, 1)]
    public float freezedInScreenY = 0.85f;
    internal static Bird instance;
    public void Resurrect()
    {
        StartCoroutine("ResurrectCR");
    }

    private float initialFISTY;
    private Vector3 center;
    [HideInInspector]
    public bool isFlying;
    Rotation myRotation;
    private bool isGettingGoFast=false;
    private bool isIntouchable=false;

    void OnEnable()
    {
        instance = this;
        center = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
        Debug.Log(center);
        tempY = transform.position.y;
        //Get reference to the Animator component attached to this GameObject.
        Anim = GetComponentInChildren<Animator>();
        //Get and store a reference to the Rigidbody2D attached to this GameObject.
        rb2d = GetComponentInParent<Rigidbody2D>();
        myAs = GetComponent<AudioSource>();
        PlayerManager.thisPM.PlayerLose.AddListener(() => vertDelta = 0);
        PlayerManager.thisPM.PlayerLose.AddListener(() => isDead = true);
        myWallsM = FindObjectOfType<WallsManager>();
        myRotation = anim.GetComponent<Rotation>();

    }
    private void Start()
    {
        initialFISTY = freezedInScreenY;
        thisBird = this;

    }
    public void OnValidate()
    {
        shield = transform.Find("Sprite/Shield").GetComponent<MeshRenderer>();
        shield.sortingOrder = 2;
    }
    void Update()
    {
        freezedInScreenY = initialFISTY + myWallsM.Score * 0.01f * Controller.thisController.verticalDifficulty * Time.deltaTime;

        switch (Equation())
        {
            case 0: vertDelta = 999;
                break;
            default:
                if (Time.deltaTime != 0)
                    vertDelta = (transform.position.y - tempY) / Time.deltaTime;
                break;
        }


        //Don't allow control if the bird has died.
        if (!IsDead)
        {
            Anim.SetFloat("vert_delta",vertDelta);
            //Look for input to trigger a "flap".
            if (Input.GetMouseButtonDown(0)&&(!isFlying||!isGettingGoFast))
            {
                myWallsM.Jumped = true;
                //...tell the animator about it and then...
                // anim.SetTrigger("Flap");
                //...zero out the birds current y velocity before...
                rb2d.velocity = Vector2.zero;
                //  new Vector2(rb2d.velocity.x, 0);
                //..giving the bird some upward force.

                rb2d.AddForce(new Vector2(sideForce * Direction, upForce * Equation()));
                rb2d.gravityScale = 1;
                myAs.Play();
                /* Debug.Log("Normal inverse: "+myWallsM.NormalizedInversePoint(transform.position.y));
                 Debug.Log("Normal: "+myWallsM.NormalizedPoint(transform.position.y));
                 Debug.Log("Equation: " +((freezedInScreenY- myWallsM.NormalizedInversePoint(transform.position.y)) /freezedInScreenY)*100);*/
            }
            if (isGettingGoFast || isIntouchable)
                invulnerable = true;
            else
                invulnerable = false;
        }

        tempY = transform.position.y;
    }

    private float Equation()
    {
        if (((freezedInScreenY - myWallsM.NormalizedInversePoint(transform.position.y)) / freezedInScreenY) <= 0.01)
            return 0;
        else
            return 1;
        //return (freezedInScreenY - myWallsM.NormalizedInversePoint(transform.position.y)) / freezedInScreenY;
    }

    void OnCollisionEnter2D(Collision2D other)
    {

        switch (other.gameObject.tag)
        {
            case "jumping_cat":
                if (invulnerable)
                    other.gameObject.GetComponent<FlyingCatScript>().CatHitByShield(other.transform.position - transform.position);
                else
                    Die(other);
                Destroy(other.gameObject);
                break;
            case "throwed_object":
                if (!invulnerable)
                    Die(other);
                break;
            case "wall":
                ChangeOrientation();
                break;
            default:
                Debug.Log("nada");
                break;
        }

        /*
        //Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "wall")
        {
            //Debug.Log(other.gameObject.name);
            ChangeOrientation();
        }
        else if ((other.gameObject.tag == "throwed_object" || other.gameObject.tag == "jumping_cat") && !invulnerable)
        {

            Die(other);
        }
        else if (invulnerable)
        {
            switch (other.gameObject.tag)
            {
                case "jumping_cat":

                    other.gameObject.GetComponent<FlyingCatScript>().CatHitByShield(other.transform.position - transform.position);

                    break;
                default:
                    Debug.Log("nada");
                    break;
            }
        }
        else
        {
            Debug.Log(other.gameObject.tag);
            //Debug.Log("ouch");
            // Zero out the bird's velocity
            //rb2d.velocity = Vector2.zero;

            //...and tell the game control about it.
            // GameControl.instance.BirdDied();
        }*/

    }

    public void ChangeOrientation()
    {
        Direction *= -1;
        transform.transform.RotateAround(transform.position, Vector2.up, -180);
        rb2d.gravityScale += 1f;
    }

    private void Die(Collision2D other)
    {
        myRotation.activated = true;
        if (isDead)
        {
            myAs.Play();
        }
        else
        {
            IsDead = true;
            Transform hit = Instantiate(other.gameObject.transform.Find("Hit"), transform.position, Quaternion.identity);
            hit.name = "cataplun";
            hit.GetComponent<ParticleSystem>().Play(true);
            hit.GetComponent<AudioSource>().Play();
            myWallsM.Speed = 0;
            // If the bird collides with something set it to dead...
            FindObjectOfType<CameraShake>().ShakeDuration = 1;
            rb2d.AddForce(new Vector2(direction * sideForce, 0));
            myAs.clip = doink;
            //...tell the Animator about it...
            Anim.SetTrigger("Die");
            ScreenshotHandler.TakeScreenshotDelayed(Random.Range(0.1f,1));            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(gameObject.name);
        if (!invulnerable)
        {
            if (other.tag == "box" || other.tag == "Gas" || other.tag == "cat" || other.tag == "jumping_cat" || other.tag == "electric_box")
            {
                switch (other.tag) {

                    case "cat":
                        other.GetComponent<Animator>().SetTrigger("slash");
                        break;
                    case "electric_box":
                        Debug.Log("electrocutado");
                        if (Valores.timesShocked < 4 && !isDead)
                        {
                            Valores.timesShocked++;
                            Valores.UpdateFloatField(ref Controller.thisController.sqlDB, (float)Valores.timesShocked / float.Parse(Valores.achievementDT[0]["goal"].ToString()), "Achievements", "progress", 1);
                        }
                        break;
                    default:
                        break;

                }

                Die(other);
            }
            else if (other.tag == "LaserBeam")
            {
                Debug.Log("nombre: " + other.name);
                if (other.transform.Find("Ray").GetComponent<ParticleSystem>().isPlaying)
                {
                    Debug.Log("Rashado");
                    Die(other);

                }
            }
        }


    }


    private void Die(Collider2D other)
    {
        myRotation.activated = true;
        if (isDead)
        {


            myAs.Play();
        }
        else
        {
            Transform hit;
            try
            {
                hit = other.gameObject.transform.parent.Find("Hit");
                hit.GetComponent<ParticleSystem>().Play(true);
                hit.GetComponent<AudioSource>().Play();
            }
            catch (Exception e) {
                if (other.tag == "LaserBeam")
                    transform.Find("Sprite/Zap").GetComponent<ParticleSystem>().Play(true);
                transform.Find("Sprite/Zap").GetComponent<AudioSource>().Play();
            }
            myWallsM.Speed = 0;
            // If the bird collides with something set it to dead...
            IsDead = true;
            FindObjectOfType<CameraShake>().ShakeDuration = 1;
            rb2d.AddForce(new Vector2(direction * sideForce, 0));
            myAs.clip = doink;
            Debug.Log("golpeado por " + other.name);
            //...tell the Animator about it...
            Anim.SetTrigger("Die");
            ScreenshotHandler.TakeScreenshotDelayed(Random.Range(0.1f, 0.5f));
        }
    }


    public void FirstJump() {

        invulnerable = false;
        isDead = false;
        rb2d.velocity = Vector2.zero;
        rb2d.AddForce(new Vector2(sideForce * Direction, upForce) * (firstJumpMultiplier));
        GameObject go = new GameObject("ladrido");
        go.AddComponent<AudioSource>();
        go.GetComponent<AudioSource>().outputAudioMixerGroup = myAs.outputAudioMixerGroup;
        go.GetComponent<AudioSource>().clip = myAs.clip;
        go.GetComponent<AudioSource>().playOnAwake = true;
        Destroy(Instantiate(go), 4f);
        Destroy(go, 3);
        myAs.clip = jump;
        Debug.Log("first jump");
        ScreenshotHandler.TakeScreenshotDelayed(Random.Range(0.1f,1));
    }
    public void SecondBreath(float upForce, float sideForce) {
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        direction = -1;
        rb2d.gravityScale = 1;
        rb2d.velocity = Vector2.zero;
        rb2d.AddForce(new Vector2(sideForce * Direction, upForce) * (firstJumpMultiplier));
        isDead = false;

    }
    public async Task GottaGoFast(float secs) {
        isFlying = true;
        Vector2 initialPos = transform.position;
        transform.Find("Sprite/Trail_Boost").gameObject.SetActive(true);
        rb2d.bodyType = RigidbodyType2D.Static;
        isGettingGoFast = true;
        Controller.thisController.gottaGetFastBG.SetActive(true);
        Controller.thisController.killingCatController.SetActive(false);
        float tempSecs = 0;
        while (tempSecs < secs) {
            await Task.Delay(Mathf.RoundToInt(Time.deltaTime * 1000));
            //Debug.Log(Time.time);
            tempSecs += Time.deltaTime;
            transform.position = Vector2.Lerp(initialPos, center, tempSecs / secs);
        }
        Controller.thisController.killingCatController.SetActive(true);
        WallsManager wm = myWallsM;
        wm.BoosterActivated = false;
        wm.speed = 0;
        wm.MoveWalls();
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        transform.Find("Sprite/Trail_Boost").gameObject.SetActive(false);
        transform.Find("Sprite/Trail").gameObject.SetActive(true);
        Controller.thisController.gottaGetFastBG.SetActive(false);
        isFlying = false;
        await Task.Delay(3000);
        isGettingGoFast = false;
    }
    async public void CantTouchThis(int invSecs)
    {
        float i = 0;
        GameObject shield = transform.Find("Sprite/Shield").gameObject;
        float initialtime = Time.time;
        do
        {
            isIntouchable = true;
            shield.SetActive(true);
            await Task.Delay(Mathf.RoundToInt(Time.deltaTime * 1000));
            i += Time.deltaTime;
            Debug.Log("milisegundos acumulados hasta ahora: "+i);
        } while (i < (invSecs*0.55f));
        if (!isDead)
            transform.Find("Sprite/Shield").gameObject.SetActive(false);

        isIntouchable = false;
        Debug.Log("time passed "+(Time.time - initialtime));
    }
    public IEnumerator ResurrectCR() {
        Debug.Log("second breath");
        myRotation.activated = false;
        myRotation.transform.localRotation = Quaternion.identity;
        Controller.thisController.tried = true;
        PlayerManager.thisPM.PlayerResurrect.Invoke();
        anim.SetTrigger("continue");
        transform.rotation = Quaternion.identity;
        firstJumpMultiplier = 4;
        SecondBreath(88, 41);
        CantTouchThis(3);
        yield return new WaitForSeconds(2);
        PlayerManager.thisPM.PlayerLoseIsInvoked = false;

    }
    
}
