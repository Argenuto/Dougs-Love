using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ShootingCatScript : MonoBehaviour
{
    public GameObject plant;
    private bool alreadyShown;
    [HideInInspector]
    public bool shot = false;
    private PlayerManager playerMan;
    Animator myAnimator;
    private Collider2D playerColl;
    private Controller myController;
    public float howFar = 2;
    public float secs=1;
    float i = 0;
    private bool alreadyShot=false;

    // Start is called before the first frame update
    void Awake()
    {
        playerMan = FindObjectOfType<PlayerManager>();
        myAnimator = transform.parent.GetComponentInChildren<Animator>();
        playerColl = playerMan.GetComponent<Collider2D>();
        myController = FindObjectOfType<Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        i += Time.deltaTime;
        if (i >= secs&&Canshoot())
            {
            i = 0;
            Debug.Log(Vector2.Angle(playerColl.transform.position, transform.position));
            myAnimator.SetTrigger("shot");
            alreadyShot = true;
        }
    }

    private bool Canshoot()
    {
        Vector2 diference = playerColl.transform.position - transform.position + Vector3.up * FindObjectOfType<Bird>().vertDelta;
        Debug.Log(transform.parent.name+" - "+diference);
        bool canshoot=true;
        canshoot &= !alreadyShot;
        canshoot &= shot;
        canshoot &= MathFuncs.IsInThere(-0.25f, Mathf.Infinity, transform.position.y - PlayerManager.thisPM.transform.position.y);
        canshoot &= !Bird.thisBird.IsDead;
        return canshoot;
    }

    public void ShootPlant()
    {
            GameObject myPlant = Instantiate(plant, transform.position, transform.rotation, transform).gameObject;
            myPlant.GetComponent<Rigidbody2D>().AddForce((Vector2)(playerColl.transform.position - transform.position + Vector3.up * FindObjectOfType<Bird>().vertDelta) * howFar, ForceMode2D.Impulse);
    }
    private void OnBecameVisible()
    {
        alreadyShown = true;
        shot = true;
    }
    private void OnBecameInvisible()
    {
        shot = false;
        if (alreadyShown && !Bird.thisBird.invulnerable)
            myController.orange_dodged++;
    }

}
