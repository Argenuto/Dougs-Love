using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class RawImageScroller : MonoBehaviour 
{
	private RawImage rawImg;
	[Space]
	public float velocidad;
	[Range(-1, 1)]
	public float direccionX, direccionY;

    private void Start()
    {
        rawImg = GetComponent<RawImage>();
    }

    void FixedUpdate ()
	{
		rawImg.uvRect = new Rect (rawImg.uvRect.x + (velocidad * direccionX), rawImg.uvRect.y + (velocidad * direccionY), rawImg.uvRect.width, rawImg.uvRect.height);
	}
}
