using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UI_TamañoPorDistancia : MonoBehaviour
{
    RectTransform rect;
    Vector2 tamañoInicial;

    public Transform referencia;

    public float factorDisminucion;
    
    void Start ()
    {
        rect = GetComponent<RectTransform>();

        tamañoInicial = new Vector2(rect.rect.width, rect.rect.height);

        Actualizar_Tamaño();
    }

    void Update ()
    {
        Actualizar_Tamaño();
    }

    public void Actualizar_Tamaño()
    {
        float distancia = Mathf.Abs(transform.position.x - referencia.position.x);

        if (!Mathf.Approximately(transform.position.x, referencia.position.x))
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
        else
        {

        }
    }
}
