using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fondo : MonoBehaviour
{
    public WallsManager wm;

    public FondoObj[] objs;

    private void Start()
    {
        foreach (FondoObj o in objs)
        {
            o.posInicial = o.obj.transform.localPosition;
        }
    }

    private void Update()
    {
        foreach (FondoObj o in objs)
        {
            o.obj.transform.Translate(-Vector3.up * (wm.speed / o.distancia) * Time.deltaTime);
        }
    }

    public void Reiniciar ()
    {
        foreach (FondoObj o in objs)
        {
            o.obj.transform.localPosition = o.posInicial;
        }
    }
}
