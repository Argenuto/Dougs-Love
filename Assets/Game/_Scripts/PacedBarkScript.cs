using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacedBarkScript : MonoBehaviour
{
    public float secs;
    float i = 0;
    public bool isLooping = false;
    AudioSource barkSource;
    private ParticleSystem barkParticle;

    // Start is called before the first frame update
    void Start()
    {
        barkSource = GetComponent<AudioSource>();
        barkParticle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLooping&&i>=(secs+Mathf.Min( barkSource.clip.length,barkParticle.main.duration))) {
            i = 0;
            barkSource.Play();
            barkParticle.Play();
        }
        i += Time.deltaTime;
    }
}
