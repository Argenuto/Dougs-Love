using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertArrowScript : MonoBehaviour
{
    GameObject myCat;

    public GameObject MyCat { get => myCat; set => myCat = value; }

    // Start is called before the first frame update
    void OnEnable()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(myCat != null)
            transform.position = new Vector2 (Mathf.Lerp(transform.position.x,myCat.transform.position.x,Time.deltaTime),transform.position.y);
    }
}
