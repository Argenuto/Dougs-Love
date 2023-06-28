using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public SpriteRenderer close;
    [Range(0, 1)]
    public float closeSpeeed=1;
    public SpriteRenderer mid;
    [Range(0, 1)]
    public float midSpeeed=0.5f;
    public SpriteRenderer mid2;
    [Range(0, 1)]
    public float mid2Speeed = 0.5f;
    public SpriteRenderer far;
    [Range(0, 1)]
    public float farSpeeed=0.25f;
    float speed;

    public float sizedif { get; private set; }

    // Start is called before the first frame update
    private void Awake()
    {
        double width = close.sprite.bounds.size.x;
        double worldScreenHeight = Camera.main.orthographicSize * 2.0;
        double worldScreenWidth = worldScreenHeight * Camera.main.aspect;
         sizedif=  (float)(worldScreenWidth / width);
    }
    void Start()
    {
       FindObjectOfType<WallsManager>().OnSpeedChangedF.AddListener(ChangeSpeed);
        
             
        close.transform.localScale = new Vector2(1, 1) * sizedif;
       // Debug.Log(close.transform.position.y + close.sprite.bounds.min.y);
        close.transform.Translate(new Vector2(0, -(close.transform.position.y + close.sprite.bounds.min.y*sizedif) + Camera.main.ScreenToWorldPoint(Vector2.zero).y));
    }

    private void ChangeSpeed(float spd)
    {
        speed = spd;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            close.transform.Translate(Vector2.down * speed * Time.deltaTime*closeSpeeed);
            mid.transform.Translate(Vector2.down * speed * Time.deltaTime *midSpeeed);
            try
            {
                far.transform.Translate(Vector2.down * speed * Time.deltaTime *farSpeeed);
            }
            catch (UnassignedReferenceException e) {
                //Debug.Log(e.Message);
            }
        }
    }
}
