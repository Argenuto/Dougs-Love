using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingCatScript : MonoBehaviour
{
    public GameObject CatToThrow;
    public float howFar;
    private PlayerManager playerMan;
    private Animator myAnim;
    private Bird localBird;
    private Controller myController;
    private bool alreadyShown;

    // Start is called before the first frame update
    void Awake()
    {
        playerMan = FindObjectOfType<PlayerManager>();
        myAnim = GetComponent<Animator>();
        localBird = playerMan.GetComponent<Bird>();
        myController = FindObjectOfType<Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        myAnim.SetFloat("angle",Vector2.Angle(playerMan.transform.position, transform.position));
        //Debug.Log(Vector2.Angle(playerMan.transform.position, transform.position));
    }
    public void OnBecameVisible()
    {
        //Debug.Log("me ven!");
        alreadyShown = true;
        Prepare();
    }
    void Prepare() {
        if(!playerMan.GetComponent<Bird>().isFlying)
            GetComponent<Animator>().SetTrigger("shot");
    }
    public void Jump() {
        GameObject myCat = Instantiate(CatToThrow, transform.position, transform.rotation,transform).gameObject;
        myCat.tag = "jumping_cat";
        myCat.GetComponent<Rigidbody2D>().AddForce((Vector2.down+(Vector2.right*transform.position.x)) * howFar, ForceMode2D.Impulse);

    }
    public void OnBecameInvisible()
    {
        if (isActiveAndEnabled&&alreadyShown && !localBird.invulnerable)
            myController.black_dodged++;
    }
}
