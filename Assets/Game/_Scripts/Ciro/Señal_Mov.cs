using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Señal_Mov : MonoBehaviour
{
    RectTransform rectTrans;

    public float velocidad;
    public float direccion;
    public AudioClip shot;
    [Space]
    public float tiempo;
    float temp = 0;
    [Space]
    public GameObject gato;
    Animator myAnim;
    bool dropped=false;
    float shotTime = 1;
    AudioSource myAS;
    internal Transform catParent;

    void Start ()
    {
        catParent = FindObjectOfType<Invocador_Gato1>().transform;
        rectTrans = transform.GetComponent<RectTransform>();
        myAnim = GetComponent<Animator>();
        shotTime = shot.length*0.20f;
        int r = Random.Range(0, 2);
        myAS = gameObject.GetComponent<AudioSource>();
        switch (r)
        {
            case 0:
                direccion = 1;
                break;

            case 1:
                direccion = -1;
                break;
        }
    }

    void Update ()
    {
        temp += Time.deltaTime;

        if (temp >= tiempo && !dropped)
        {
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);

            Rigidbody2D gatoEnEscena = Instantiate(gato, transform.position, Quaternion.identity,catParent).GetComponent<Rigidbody2D>();
            gatoEnEscena.AddForce(Vector2.down * 2.5f,ForceMode2D.Impulse);
            myAnim.SetTrigger("shot");
            myAS.clip = shot;
            myAS.Play();
            dropped = true;


        }
        if(!dropped && (tiempo-temp)>0.35f)
            rectTrans.position = new Vector3(rectTrans.position.x + (velocidad * direccion * Time.deltaTime), rectTrans.position.y);
        if (temp >= tiempo + shotTime && dropped)
        {
            Destroy(gameObject);
            myAS.Stop();
        }
        if (direccion > 0 && rectTrans.anchoredPosition.x > 350)
        {
            direccion *= -1;
        }
        else if (direccion < 0 && rectTrans.anchoredPosition.x < -350)
        {
            direccion *= -1;
        }
    }
}
