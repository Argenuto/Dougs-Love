using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SnapElement : MonoBehaviour
{
    RectTransform rect;

    public SnapScroll scroll;

    public Image img;

    public Vector3 tamañoInicial;
    
    public Vector2 posFinal;
    
    public float velocidad = 10;
    
    public float factorDisminucion = 50;

    public bool mover;
    
    // Start is called before the first frame update
    void Start()
    {
        mover = false;

        rect = GetComponent<RectTransform>();

        tamañoInicial = new Vector2(rect.rect.width, rect.rect.height);

        Actualizar_Tamaño();
    }

    void FixedUpdate()
    {
        if (mover)
        {
            Actualizar_Tamaño();
        }
    }

    public void Mover (Vector3 newPos)
    {
        posFinal = newPos;

        mover = true;

        StartCoroutine(Rutina_Mover());
    }

    public void Actualizar_Tamaño ()
    {
        float distancia = Vector2.Distance(transform.position, scroll.transform.position);

        if (!Mathf.Approximately(transform.position.x, scroll.transform.position.x))
        {
            Vector2 newTam = tamañoInicial;

            distancia *= factorDisminucion;

            if (newTam.x > distancia)
            {
                newTam.x -= distancia;
            }

            if (newTam.y > distancia)
            {
                newTam.y -= distancia;
            }
            
            rect.sizeDelta = newTam;
        }
    }

    IEnumerator Rutina_Mover ()
    {
        while (true)
        {
            //transform.position = Vector3.MoveTowards(transform.position, posFinal, velocidad * Time.deltaTime);

            Vector3 pos = transform.position;

            pos.x = Mathf.MoveTowards(pos.x, posFinal.x, velocidad * Time.deltaTime);

            transform.position = pos;

            yield return null;

            if (Mathf.Approximately(transform.position.x, posFinal.x))
            {
                transform.position = new Vector3(posFinal.x, transform.position.y, transform.position.z);
                
                mover = false;

                break;
            }
        }

        yield return null;
        StopCoroutine(Rutina_Mover());
    }
}
