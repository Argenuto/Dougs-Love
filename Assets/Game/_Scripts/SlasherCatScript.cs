using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlasherCatScript : MonoBehaviour
{
    private PlayerManager dog;
    private Animator myAnim;
    public float distance = 1;

    private void OnEnable()
    {
        dog = FindObjectOfType<PlayerManager>();
        myAnim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!dog.PlayerLoseIsInvoked)
        {
            if (Vector2.Distance(transform.position, dog.transform.position) < distance)
                myAnim.SetTrigger("close");
            else
                myAnim.SetTrigger("far");
        }
        //Debug.Log("distancia entra gato y "+transform.parent.name+": "+ Vector2.Distance(transform.position, dog.transform.position));
    }


}
