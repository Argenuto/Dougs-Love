using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class Ctrl_InterfazGameOver : MonoBehaviour
{
    public GameObject panel_gameOver, panel_principal, panel_creditos;
    public TextMeshProUGUI txt_monedas, txt_puntuacion, txt_puntuacionMax;

    public Fader fader;

    public void Start()
    {
        Actualizar_Monedas(Valores.coins);

        txt_puntuacion.text = Valores.gameOverScore.ToString();

        txt_puntuacionMax.text = Valores.maxPoints.ToString();
    }

    public void ReproducirSonido (string newSonido)
    {
        FindObjectOfType<AudioManager>().Play(newSonido);
    }

    public void Actualizar_PuntuacionMax (int newPts)
    {
        txt_puntuacionMax.text = newPts.ToString();
    }

    public void Actualizar_Monedas (int newMonedas)
    {
        txt_monedas.text = newMonedas.ToString();
    }
}
