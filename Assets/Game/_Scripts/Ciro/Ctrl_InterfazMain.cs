using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ctrl_InterfazMain : MonoBehaviour
{
    public RectTransform panel_Continuar, panel_pause;
    public Fader fader;

    public void ReproducirSonido (string newSonido)
    {
        FindObjectOfType<AudioManager>().Play(newSonido);
    }
}
