using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NuevoPersonaje", menuName = "Personaje")]
public class Personaje : ScriptableObject
{
    public Sprite imagen;    
    public string fileName,nombre;
    [Space]
    [TextArea]
    public string descripcion;
}
