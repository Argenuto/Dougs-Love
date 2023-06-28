using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NekokazeCatScript : MonoBehaviour
{
    private Controller myController;

    // Start is called before the first frame update
    private void Awake()
    {
        myController = FindObjectOfType<Controller>();
    }

    private void OnBecameInvisible()
    {
        myController.jumper_dodged++;
        Destroy(gameObject);
    }
}
