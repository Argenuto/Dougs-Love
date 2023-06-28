using UnityEngine;
using System.Collections;

public class ScrollingUVs_PDS : MonoBehaviour 
{
	public int materialIndex = 0;
	public Vector2 uvAnimationRate = new Vector2( 1.0f, 0.0f );
	public string textureName = "_MainTex";
	
	Vector2 uvOffset = Vector2.zero;
    public int sortingOrder=11;

    private void OnEnable()
    {
        GetComponent<Renderer>().sortingOrder = sortingOrder;
     //   gameObject.GetComponent<RectTransform>().localScale = Vector2.one * transform.parent.GetComponent<RectTransform>().rect.height;
    }
        void LateUpdate() 
	{
		uvOffset += ( uvAnimationRate * Time.unscaledDeltaTime );
		if( GetComponent<Renderer>().enabled )
		{
			GetComponent<Renderer>().materials[ materialIndex ].SetTextureOffset( textureName, uvOffset );
		}
	}
}