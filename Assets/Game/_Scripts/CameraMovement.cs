using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Camera farCamera;
    [Range(0, 1)]
    private float speed;
    public float farCameraSpeeed=1;
    [HideInInspector]
    public Vector3 myInitialPosition;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<WallsManager>().OnSpeedChangedF.AddListener(ChangeSpeed);
        myInitialPosition = farCamera.transform.localPosition;
        Debug.Log(myInitialPosition);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        
            farCamera.transform.Translate(Vector2.up * speed * Time.deltaTime * farCameraSpeeed);
        }
    private void ChangeSpeed(float spd)
    {
        speed = spd;
    }

}
