using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Scroll : MonoBehaviour
{
    [SerializeField]
    Vector3 posFinal;

    public ScrollRect scrollRect;

    public int index;

    public List<Transform> lista_contenido = new List<Transform>();

    public float velocidadMin;
    public float velocidad;

    private void Start()
    {
        Calcular_IndexElementoMasCercano();

        //Calcular_PosFinal();
    }

    private void Update()
    {
        Vector3 newPos = scrollRect.content.transform.position;

        newPos.x = Mathf.MoveTowards(newPos.x, scrollRect.content.transform.position.x - lista_contenido[index].position.x, velocidad * Time.deltaTime);

        scrollRect.content.transform.position = newPos;

        //Debug.Log("Pos x: " + scrollRect.content.transform.position.x);
    }

    void Calcular_IndexElementoMasCercano ()
    {
        Transform elementoMasCercano = lista_contenido[index];

        for (int i = 0; i < lista_contenido.Count; i++)
        {
            if (Mathf.Abs(transform.position.x - lista_contenido[i].position.x) < Mathf.Abs(transform.position.x - elementoMasCercano.position.x))
            {
                elementoMasCercano = lista_contenido[i];

                index = i;
            }
        }
    }

    void Calcular_PosFinal ()
    {

    }

    public void Scrolling ()
    {/*
        Calcular_IndexElementoMasCercano();

        if (Mathf.Abs(scrollRect.velocity.x) <= velocidadMin)
        {
            Vector3 newPosContent = scrollRect.content.transform.position;

            float distancia = Mathf.Abs(transform.position.x - lista_contenido[index].position.x);

            if (lista_contenido[index].position.x < transform.position.x)
            {
                distancia *= -1;
            }

            newPosContent.x -= distancia;

            scrollRect.content.position = newPosContent;
        }*/

        if (index < lista_contenido.Count - 1 && scrollRect.velocity.x < 0)
        {
            index++;
        }
        else if (index > 0 && scrollRect.velocity.x > 0)
        {
            index--;
        }
    }
}
