using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HocicoSimulator : MonoBehaviour
{
    public AudioClip[] barks;
    AudioSource myAudioS;
    // Start is called before the first frame update
    void Start()
    {
        myAudioS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0) && !myAudioS.isPlaying)
        {
            myAudioS.clip = barks[Random.Range(0, barks.Length)];
           
            myAudioS.Play();
        };
    }
}
