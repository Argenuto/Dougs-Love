using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBE : MonoBehaviour
{
    Rigidbody myrb;
    public float gravedad=0;
    // Start is called before the first frame update
    void Start()
    {
        myrb =GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
         Vector3 velocidad = myrb.velocity;
        velocidad.y -= gravedad;
        myrb.velocity = velocidad;
        Debug.Log(myrb.velocity.y);
    }
}
