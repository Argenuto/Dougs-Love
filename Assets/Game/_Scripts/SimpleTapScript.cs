using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleTapScript : MonoBehaviour
{
    bool taped = false;
    public UnityEvent OnTaped;

    // Start is called before the first frame update
    void Start()
    {
        OnTaped.AddListener(()=>taped = true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !taped)
        {
            OnTaped.Invoke();
        } 
    }
}
