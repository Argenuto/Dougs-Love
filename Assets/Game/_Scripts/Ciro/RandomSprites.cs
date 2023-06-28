using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomSprites : MonoBehaviour
{
    SpriteRenderer sprRend;

    public List<Sprite> sprs = new List<Sprite>();
    public List<Sprite> sprs_pisosBajos = new List<Sprite>();

    public bool actualizarAtStart = true;
    public bool pisosBajos;

    void Start ()
    {
        sprRend = GetComponent<SpriteRenderer>();

        if (actualizarAtStart)
            Actualizar_SpriteAleatorio();
    }

    public void Actualizar_SpriteAleatorio ()
    {
        if (sprs.Count > 0)
        {
            if (pisosBajos)
            {
                int r = Random.Range(0, sprs_pisosBajos.Count);

                sprRend.sprite = sprs_pisosBajos[r];
            }
            else
            {
                int r = Random.Range(0, sprs.Count);

                sprRend.sprite = sprs[r];
            }
        }
    }
}
