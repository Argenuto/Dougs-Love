using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;

    // How long the object should shake for.
    private float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;
	
	Vector3 originalPos;

    public float ShakeDuration { get => shakeDuration; set => shakeDuration = value; }

    void Awake()
	{
		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}
	
	void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}

	void Update()
	{
		if (ShakeDuration > 0)
		{
			camTransform.localPosition = originalPos + Vector3.right*Random.Range(-1,1) * shakeAmount;
			
			ShakeDuration -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			ShakeDuration = 0f;
			camTransform.localPosition = originalPos;
		}
	}
}
