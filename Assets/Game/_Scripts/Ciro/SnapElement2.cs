using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SnapElement2 : MonoBehaviour
{
    Vector2 tamañoInicial;

    public SnapObject snapObject;
    [Space]
    public RectTransform rect;

    public ScrollRect scroll;

    public Color colorBloqueado;
    public GameObject candado;

    public Animator anim;

    public Image img;

    public float factorDisminucion = 50f;

    void Start ()
    {
        tamañoInicial = new Vector2(rect.rect.width, rect.rect.height);

        //img.sprite = snapObject.sprite;

        //img.preserveAspect = true;

        if (snapObject.ID > 1)
        {
            if (!Valores.personajeDesbloqueado[snapObject.ID - 1])
            {
                img.color = colorBloqueado;
            }
            else
            {
                candado.SetActive(false);
            }
        }
        
        anim.SetFloat("Personaje Seleccionado", snapObject.ID);

        Actualizar_Tamaño();
    }

    void Update ()
    {
        Actualizar_Tamaño();
    }

    public void Actualizar_Tamaño()
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
}
