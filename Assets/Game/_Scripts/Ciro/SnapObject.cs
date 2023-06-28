using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopInfo", menuName = "Shop Info")]
public class SnapObject : ScriptableObject
{
    public int ID;
    public string nombre;
    public Sprite sprite;
    public int precio;
    public RuntimeAnimatorController animatorControler;
}
