using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DougJumping : MonoBehaviour
{
    
    private bool jumping;
    public float jumpTime;
    public Vector2 jumpVector;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !jumping)
        {
            jumping = true;
            StartCoroutine(JumpRoutine());
        }
    }

    IEnumerator JumpRoutine()
    {
        //Set the gravity to zero and apply the force once
        float startGravity = GetComponent<Rigidbody2D>().gravityScale;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        GetComponent<Rigidbody2D>().velocity = jumpVector;
        float timer = 0f;

        while (timer < jumpTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        //Set gravity back to normal at the end of the jump
        GetComponent<Rigidbody2D>().gravityScale = startGravity;
        jumping = false;
    }
}
