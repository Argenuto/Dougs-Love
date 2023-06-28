using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invocador_Gato1 : MonoBehaviour
{
    Vector2 camMin, camMax;
    public float floorToStart=80, minInterval = 8, maxInterval = 15;
    public int catsInAction = 1;
    public GameObject señal;
    Transform catParent;
    GameObject señalInvocada;
    [Space]
    public bool invoqueOnAwake;
    WallsManager myWM;
    Bird myBird;
    private float acumFloor;

    public void OnEnable()
    {
        StopAllCoroutines();
        acumFloor = 0;
        myWM = FindObjectOfType<WallsManager>();
        StartCoroutine("GenerateCatCR");
    }
    void Start ()
    {
        camMin = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        catParent = GameObject.FindGameObjectWithTag("Canvas").transform.Find("CatZone");
        camMax = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        myBird = FindObjectOfType<Bird>();
        if (invoqueOnAwake)
        {
            Invocar_Gato();
        }
    }

    public void Invocar_Gato ()
    {
        Vector3 posInvo = Camera.main.WorldToViewportPoint(new Vector3(0, camMax.y, 0));

        señalInvocada = (GameObject)Instantiate(señal, posInvo, Quaternion.identity, catParent);

        RectTransform sirt = señalInvocada.GetComponent<RectTransform>();

        sirt.anchoredPosition3D = new Vector3(0, -100, 0);
    }
    IEnumerator GenerateCatCR() {
        //Debug.Log("corutina activada");
        acumFloor = floorToStart;
        yield return new WaitUntil(()=>myWM.Score > floorToStart);
        yield return new WaitForSeconds(Random.Range(1, 5));
        while (true)
        {
            if (myWM.speed <= 15 && transform.childCount < catsInAction && catParent.childCount < catsInAction)
                Invocar_Gato();
            else
                yield return new WaitForSeconds(Random.Range(5,10));
            yield return new WaitUntil(() => myWM.Score > acumFloor);
            acumFloor += Random.Range(minInterval, maxInterval);
            Debug.Log("AcumFloor: " + acumFloor + "WallsManager Floors: "+myWM.score);
        }

    }
    public void Clean() {
        if(catParent.childCount>0)
            foreach (Transform item in catParent.transform)
              Destroy(item.gameObject);
            
        if(transform.childCount > 0)
            foreach (Transform item in transform)
             Destroy(item.gameObject);
        
    }
}
