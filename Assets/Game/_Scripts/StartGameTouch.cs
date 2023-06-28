using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StartGameTouch : MonoBehaviour, IPointerDownHandler
{ 
    public UnityEvent GameStarted;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
       float x = Camera.main.ScreenToWorldPoint(eventData.position).x;
        PlayerManager myPlayer = FindObjectOfType<PlayerManager>();
        if (x < myPlayer.transform.position.x)
        {
            myPlayer.GetComponent<Bird>().Direction *= -1;

        }
        else {
            myPlayer.transform.RotateAround(transform.position, Vector2.up, -180);
        }
        //Debug.Log("pinshado");
        StartAndSelfDestroy();
    }

    public void StartAndSelfDestroy()
    {
        GameStarted.Invoke();
        transform.parent.gameObject.SetActive(false);
       // Destroy(transform.parent.gameObject);
    }

    public void JustSelfDestroy()
    {
        //Destroy(transform.parent.gameObject);
        transform.parent.gameObject.SetActive(false);
    }

    void Start()
    {
        GameStarted.AddListener(() => FindObjectOfType<WallsManager>().StartCoroutine("SpeedProgresion"));
    }
}
