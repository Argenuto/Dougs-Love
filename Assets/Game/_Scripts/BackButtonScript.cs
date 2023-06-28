using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BackButtonScript : MonoBehaviour
{
    public UnityEvent GoBackPressed;

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Invoke();
        }
    }

    public void Invoke ()
    {
        GoBackPressed.Invoke();
    }
}
