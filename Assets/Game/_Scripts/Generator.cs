using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Generator : MonoBehaviour
{
    public GameObject Default;
    public float init_time = 0.5f;
    public float end_time = 2;
    float rythm = 1;
    string pattern;

    public float Rythm { get => rythm; set => rythm = value; }
    public string Pattern { get => pattern; set => pattern = value; }

    // Start is called before the first frame update
    void OnEnable()
    {
        // StartCoroutine("CorrutineGenerateMomos");
        StartCoroutine("CorroutineCoolPatern");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  
    IEnumerator CorrutineGenerateMomos()
    {
        yield return new WaitForSeconds(Random.Range(init_time, end_time));
        while (true)
        {
            Instantiate(Default,transform.position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(init_time, end_time));
        }
    }

    public IEnumerator CorroutineCoolPatern() {
        yield return new WaitForSeconds(3f);
        int i= 0;
        while (true)
        {

            char move = pattern[i];
            if(move == '1')
                Instantiate(Default, transform.position, Quaternion.identity);
            i++;
            if (i > pattern.Length)
                i = 0;
            yield return new WaitForSeconds(rythm);
        }

    }
}
