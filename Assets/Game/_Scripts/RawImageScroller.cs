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
	public Vector2 resetPositionAt = new(250,250);
	float distanceThreshold;
    private void Start()
    {
		distanceThreshold = resetPositionAt.magnitude;
        rawImg = GetComponent<RawImage>();
    }

    void FixedUpdate ()
	{
		Vector2 uvPosition =(rawImg.uvRect.position.magnitude < distanceThreshold)? 
			new (rawImg.uvRect.x + (velocidad * direccionX), rawImg.uvRect.y + (velocidad * direccionY)):Vector2.zero;
		rawImg.uvRect = new Rect (uvPosition.x,uvPosition.y, rawImg.uvRect.width, rawImg.uvRect.height);		
	}
}
