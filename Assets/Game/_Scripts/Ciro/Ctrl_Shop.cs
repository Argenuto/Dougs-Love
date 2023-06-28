using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using TMPro;

public class Ctrl_Shop : MonoBehaviour
{
    public RectTransform panel_rect, panel_perros, panel_cashShop;
    public TextMeshProUGUI txt_money;
    public Fader fader;
    [Space]
    public int monedas;
    [Space]
    public float vel_panelEntrada;

    void Start ()
    {
        monedas = Valores.coins;

        Actualizar_TxtMoney();

        //          BORRAR<<<<<<<<<<<<<<<<<<<<<<<<<<<
        Valores.personajeDesbloqueado[0] = true;
        /*
        for (int i = 1; i < Valores.personajeDesbloqueado.Length; i++)
        {
            Valores.personajeDesbloqueado[i] = false;
        }*/
        //          BORRAR<<<<<<<<<<<<<<<<<<<<<<<<<<<

        StartCoroutine(Rutina_Inicio());
    }

    public void Actualizar_TxtMoney()
    {
        txt_money.text = monedas.ToString();
    }

    public void ReproducirSonido (string newNombre)
    {
        FindObjectOfType<AudioManager>().Play(newNombre);
    }

    public void Detener_ReproduccionSonido (string newNombre)
    {
        FindObjectOfType<AudioManager>().Stop(newNombre);
    }

    public void Btn_AbrirCashShopPanel()
    {
        StartCoroutine(Rutina_AbrirPanelCashShop());
    }

    public void Btn_CerrarCashPanelShop()
    {
        StartCoroutine(Rutina_CerrarPanelCashShop());
    }

    public void Btn_CerrarEscena()
    {
        //StartCoroutine(Rutina_Salir());
        SceneManager.UnloadSceneAsync("Shop");
    }

    IEnumerator Rutina_Inicio ()
    {
        yield return null;

        FindObjectOfType<AudioManager>().Play("MusicaTienda");
        /*
        fader.RayTarget(true);
        
        Vector3 posInicial = panel_rect.transform.position;

        panel_rect.transform.position = new Vector3(panel_rect.transform.position.x, 40, panel_rect.transform.position.z);

        while (true)
        {
            yield return null;

            Vector3 panelPos = panel_rect.transform.position;

            panelPos.y = Mathf.MoveTowards(panelPos.y, posInicial.y, vel_panelEntrada * Time.deltaTime);

            panel_rect.transform.position = panelPos;

            if (Mathf.Approximately(panel_rect.transform.position.y, posInicial.y))
            {
                panel_rect.transform.position = posInicial;

                break;
            }
        }

        fader.RayTarget(false);
        */
        yield return null;
        StopCoroutine(Rutina_Inicio());
    }

    IEnumerator Rutina_AbrirPanelCashShop ()
    {
        fader.RayTarget(true);

        Vector3 posFinal_1 = panel_perros.transform.position, posFinal_2 = panel_cashShop.transform.position;

        posFinal_1.x -= Mathf.Abs(posFinal_1.x - posFinal_2.x);

        posFinal_2.x = 0;

        while (true)
        {
            yield return null;

            Vector3 pos1 = panel_perros.transform.position, pos2 = panel_cashShop.transform.position;

            pos1.x = Mathf.MoveTowards(pos1.x, posFinal_1.x, vel_panelEntrada * Time.deltaTime);

            pos2.x = Mathf.MoveTowards(pos2.x, posFinal_2.x, vel_panelEntrada * Time.deltaTime);

            panel_perros.transform.position = pos1;

            panel_cashShop.transform.position = pos2;

            if (Mathf.Approximately(panel_cashShop.transform.position.x, posFinal_2.x))
            {
                panel_cashShop.transform.position = posFinal_2;

                panel_perros.transform.position = posFinal_1;

                break;
            }
        }

        fader.RayTarget(false);

        yield return null;
        StopCoroutine(Rutina_AbrirPanelCashShop());
    }

    IEnumerator Rutina_CerrarPanelCashShop()
    {
        fader.RayTarget(true);

        Vector3 posFinal_1 = panel_perros.transform.position, posFinal_2 = panel_cashShop.transform.position;

        posFinal_2.x += Mathf.Abs(posFinal_1.x - posFinal_2.x);

        posFinal_1.x = 0;
        
        while (true)
        {
            yield return null;

            Vector3 pos1 = panel_perros.transform.position, pos2 = panel_cashShop.transform.position;

            pos1.x = Mathf.MoveTowards(pos1.x, posFinal_1.x, vel_panelEntrada * Time.deltaTime);

            pos2.x = Mathf.MoveTowards(pos2.x, posFinal_2.x, vel_panelEntrada * Time.deltaTime);

            panel_perros.transform.position = pos1;

            panel_cashShop.transform.position = pos2;

            if (Mathf.Approximately(panel_perros.transform.position.x, posFinal_1.x))
            {
                panel_cashShop.transform.position = posFinal_2;

                panel_perros.transform.position = posFinal_1;

                break;
            }
        }

        fader.RayTarget(false);

        yield return null;
        StopCoroutine(Rutina_CerrarPanelCashShop());
    }

    IEnumerator Rutina_Salir ()
    {
        fader.RayTarget(true);

        Vector3 posFinal = new Vector3(panel_rect.transform.position.x, 40, panel_rect.transform.position.z);
        
        while (true)
        {
            yield return null;

            Vector3 panelPos = panel_rect.transform.position;

            panelPos.y = Mathf.MoveTowards(panelPos.y, posFinal.y, vel_panelEntrada * Time.deltaTime);

            panel_rect.transform.position = panelPos;

            if (Mathf.Approximately(panel_rect.transform.position.y, posFinal.y))
            {
                panel_rect.transform.position = posFinal;

                break;
            }
        }

        fader.RayTarget(false);

        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Shop").buildIndex);

        yield return null;
        StopCoroutine(Rutina_Salir());
    }
}
