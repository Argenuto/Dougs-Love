﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gato_Mov : MonoBehaviour
{
    public float velocidad;
    [HideInInspector]

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - (velocidad * Time.deltaTime), transform.position.z);
    }

    private void OnBecameInvisible()
    {

    }
}
