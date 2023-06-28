using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag) {

            /*case "wall":
                transform.parent.parent.GetComponent<Bird>().ChangeOrientation();
                break;*/
            case "jumping_cat":
                collision.gameObject.GetComponent<FlyingCatScript>().CatHitByShield(collision.transform.position - transform.position);
                collision.GetComponent<Collider2D>().enabled = false;
                Debug.Log("tocado el "+collision.gameObject.name);
                break;
            default: Debug.Log(collision.gameObject.name);
                break;
    }
    }
}
