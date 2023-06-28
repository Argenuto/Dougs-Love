using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaderCircular : MonoBehaviour
{
	public Transform circulo;
	[Space]
	public float tiempo;

	void Awake ()
	{
		if (!GetComponent<SpriteRenderer> ().enabled)
		{
			GetComponent<SpriteRenderer> ().enabled = true;
		}
	}

	void Start ()
	{
		
	}

	void Update ()
	{
		
	}

	public void Set_Tamaño (float t_x, float t_y)
	{
		circulo.localScale = new Vector3 (t_x, t_y);
	}

	public IEnumerator Rutina_Tamaño (float tamañoFinal, float duracion)
	{
		float fadeVel = Mathf.Abs (circulo.localScale.x - tamañoFinal) / duracion;

		while (!Mathf.Approximately (circulo.localScale.x, tamañoFinal))
		{
			float tamañoCirculo_x = circulo.localScale.x;

			tamañoCirculo_x = Mathf.MoveTowards(tamañoCirculo_x, tamañoFinal, fadeVel * Time.unscaledDeltaTime);

			float tamañoCirculo_y = circulo.localScale.y;

			tamañoCirculo_y = Mathf.MoveTowards(tamañoCirculo_y, tamañoFinal, fadeVel * Time.unscaledDeltaTime);

			Set_Tamaño (tamañoCirculo_x, tamañoCirculo_y);

			yield return null;
		}

		yield return null;
		StopCoroutine(Rutina_Tamaño (tamañoFinal, duracion));
	}
}
