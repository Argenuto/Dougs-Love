using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    public float velocidad;

    public Vector3 posFinal;

    void Update ()
    {
        transform.position = Vector3.MoveTowards(transform.position, posFinal, velocidad * Time.deltaTime);
    }
}
