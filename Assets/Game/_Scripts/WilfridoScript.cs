using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WilfridoScript : MonoBehaviour
{
    public AudioClip /*pushStartM,*/ inGameM;
    AudioSource myAS;
    // Start is called before the first frame update
    private void Awake()
    {
        
    }
    void Start()
    {
        myAS = GetComponent<AudioSource>();
       // myAS.clip = pushStartM;
        //myAS.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeToIngame() {
        myAS.clip = inGameM;
       // myAS.volume -= 0.25f;
        myAS.Play();
    }

}
