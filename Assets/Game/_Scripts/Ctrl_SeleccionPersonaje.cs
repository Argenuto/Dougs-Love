using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Ctrl_SeleccionPersonaje : MonoBehaviour
{
    public Text txt_nombre;
    public Image img;
    public Text txt_descripcion;
   
    [Space]
    public List<Personaje> personajes = new List<Personaje>();
    [Space]
    public int index;

    void Start ()
    {
        index = Valores.personajeSeleccioando - 1;

        Actualizar();
    }

    void Actualizar ()
    {
        txt_nombre.text = personajes[index].name;

        img.sprite = personajes[index].imagen;

        txt_descripcion.text = personajes[index].descripcion;
    }

    public void Btn_Siguiente ()
    {
        if (index < personajes.Count - 1)
        {
            index++;
        }
        else
        {
            index = 0;
        }

        Actualizar();
    }

    public void Btn_Anterior ()
    {
        if (index > 0)
        {
            index--;
        }
        else
        {
            index = personajes.Count - 1;
        }

        Actualizar();
    }

    public void Btn_Seleccionar ()
    {
        Valores.personajeSeleccioando = index + 1;
        Valores.SelectedCharFile = personajes[index].fileName;
        Debug.Log("Personaje " + Valores.personajeSeleccioando + " seleccionado");
    }
}
