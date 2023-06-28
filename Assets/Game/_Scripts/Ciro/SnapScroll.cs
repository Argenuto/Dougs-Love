using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class SnapScroll : MonoBehaviour
{
    Ctrl_Shop ctrl;

    Animator anim;

    float t;
    [SerializeField]
    int index = 0;

    public ScrollRect scroll;

    public SnapScroll snapScroll;

    public TextMeshProUGUI txt_nombre, txt_btnPrecio;

    public GameObject prefab_elemento;

    public List<SnapObject> lista_objetos = new List<SnapObject>();

    public List<SnapElement> lista_elementos = new List<SnapElement>();

    public bool enMovimiento, elementosEnPosicion;

    public float distancia, duracion = 0.5f;

    public int cantElementos = 4;

    void Start ()
    {
        ctrl = FindObjectOfType<Ctrl_Shop>();

        anim = GetComponent<Animator>();

        enMovimiento = false;

        elementosEnPosicion = true;

        //+++++++++++++++++++++++++++

        Crear_Elementos();
    }

    void Update ()
    {
        if (scroll.velocity.normalized.x != 0 && !enMovimiento)
        {
            if (scroll.velocity.normalized.x < 0 && index < lista_elementos.Count - 1 || scroll.velocity.normalized.x > 0 && index > 0)
            {
                Swipe();
            }
        }

        if (enMovimiento)
        {
            t -= Time.deltaTime;

            if (t <= 0 && scroll.velocity.normalized.x == 0)
            {
                enMovimiento = false;
            }
        }
    }

    void Crear_Elementos ()
    {
        Vector3 posInvo = transform.position;

        GameObject elementoInvocado = null;

        SnapElement snapElement = null;

        for (int i = 0; i < lista_objetos.Count; i++)
        {
            elementoInvocado = (GameObject)Instantiate(prefab_elemento, posInvo, Quaternion.identity, transform);

            snapElement = elementoInvocado.GetComponent<SnapElement>();

            snapElement.scroll = snapScroll;

            snapElement.img.sprite = lista_objetos[i].sprite;

            lista_elementos.Add(snapElement);

            posInvo.x += distancia;
        }

        Actializar_Textos();
    }

    void Actializar_Textos ()
    {
        txt_nombre.text = lista_objetos[index].nombre;

        txt_btnPrecio.text = lista_objetos[index].precio.ToString();
    }

    public void Swipe ()
    {
        if (!enMovimiento)
        {
            enMovimiento = true;

            t = duracion;

            index -= (int)scroll.velocity.normalized.x;

            Actializar_Textos();

            if (elementosEnPosicion)
            {
                StartCoroutine(Rutina_MoverElementos());
            }
        }
    }

    public void Comprar_Elemento ()
    {

    }

    IEnumerator Rutina_MoverElementos()
    {
        float d = distancia * scroll.velocity.normalized.x;

        elementosEnPosicion = false;

        foreach (SnapElement e in lista_elementos)
        {
            Vector3 newPos = e.transform.position;
            
            newPos.x += d;

            e.Mover(newPos);
        }

        yield return null;

        while (enMovimiento)
        {
            for (int i = 0; i < lista_elementos.Count; i++)
            {
                yield return null;

                if (lista_elementos[i].mover)
                {
                    break;
                }

                if (i == lista_elementos.Count - 1)
                {
                    elementosEnPosicion = true;
                }
            }
        }

        yield return null;
        StopCoroutine(Rutina_MoverElementos());
    }
}
