using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour 
{
	public Image imgFader;

	public float duracionFade;

	public void Set_Alfa (float nuevoAlfa)
	{
		imgFader.color = new Color (imgFader.color.r, imgFader.color.g, imgFader.color.b, nuevoAlfa);
	}

	public void RayTarget (bool nuevoValor)
	{
		imgFader.raycastTarget = nuevoValor;
	}

	public IEnumerator Fade (float alfaFinal, float duracion)
	{
		float fadeVel = Mathf.Abs (imgFader.color.a - alfaFinal) / duracion;

		while (!Mathf.Approximately (imgFader.color.a, alfaFinal))
		{
			float fadeAlfa = imgFader.color.a;

			fadeAlfa = Mathf.MoveTowards(fadeAlfa, alfaFinal, fadeVel * Time.unscaledDeltaTime);

			Set_Alfa (fadeAlfa);

			yield return null;
		}

		yield return null;
		StopCoroutine(Fade(alfaFinal, duracion));
	}
}
