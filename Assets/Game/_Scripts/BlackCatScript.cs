using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackCatScript : MonoBehaviour
{
    private Controller myController;

    // Start is called before the first frame update
    private void Awake()
    {
        myController = FindObjectOfType<Controller>();
    }

    private void OnBecameInvisible()
    {
        myController.black_dodged++;
        Destroy(gameObject);
    }
}
