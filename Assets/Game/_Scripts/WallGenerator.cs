using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
[RequireComponent(typeof(RandomGenerator))]
public class WallGenerator : MonoBehaviour {

    // Use this for initialization

    private float speed;
    public int distancia;
    private int i;
    private int j;
    private Transform uncle;
    public bool flip;
    private RandomGenerator myRanGen;
    float limit;
    float bottom;
    internal int k;

    void Start () {
        myRanGen = GetComponent<RandomGenerator>();
        myRanGen.CreateAliasTables();

        //FindObjectOfType<Controller>().gamePaused.AddListener(FitSiblings);
        uncle = transform.parent.GetChild((transform.GetSiblingIndex() + 1) % 2);
        limit = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight)).y;
        bottom = Camera.main.ScreenToWorldPoint(Vector2.zero).y;
        StartCoroutine ("CreateInitialWalls");      
	}
	
	// Update is called once per frame
 
    IEnumerator CreateInitialWalls(){
        foreach (Objeto o in myRanGen.elementos)
            o.go.GetComponent<WallScript>().speed = 0;
        i = 0;
        j =3;
        k = distancia;
        float y = 0;
        float h=0,w=0;
        GameObject actualWall=  myRanGen.TakeElement().go; 
        GameObject myWall;
        //Debug.Log(FindObjectOfType<BackgroundManager>().close.sprite.bounds.max);

        while (bottom + y <= limit - (h * .5f))
        {

            do
            {
                //Debug.Log(name+"-"+Distancia + "-" + distancia + "-" + j);
                myWall = myRanGen.TakeElement().go;
            }
            while ((myWall.GetComponent<WallScript>().isASpike|| myWall.GetComponent<WallScript>().isCoin) && j > 0);
            if (myWall.GetComponent<WallScript>().isASpike)
                j = k;
            y += h = myWall.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
            w = !flip ? myWall.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().bounds.extents.x : -myWall.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().bounds.extents.x;
            actualWall = Instantiate(myWall, new Vector2(transform.position.x, bottom + (h * (i + 0.5f))), transform.rotation, transform);
            if (!flip)
                actualWall.transform.localScale = new Vector3(-1, 1, 1);
            actualWall.transform.Translate(new Vector2(w, 0));
            actualWall.name = "Wall" + i.ToString("D2") + "_By_" + name;
            i++;
            j--;
            Debug.Log("h: " + h+"-"+name);
        }

        //yield return new WaitForSeconds (3);
        //  Debug.Log(a+" - "+transform.GetChild(a).name);

        yield return new WaitForEndOfFrame();		
		
	}
    public void CreateWall(float diff) {
       
         
        GameObject myWall, actualWall;
        // FitSiblings();
        do
            {
                //Debug.Log(name+"-"+Distancia + "-" + distancia + "-" + j);
                myWall = myRanGen.TakeElement().go;
            }
            while (myWall.GetComponent<WallScript>().isASpike && j > 0);
        if (myWall.GetComponent<WallScript>().isASpike)
        {
            j = k;
            uncle.GetComponent<WallGenerator>().J+=2;
        }
            myWall.GetComponent<WallScript>().Speed = speed;
        float h = myWall.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
        float w = !flip ? myWall.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().bounds.extents.x : -myWall.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().bounds.extents.x;
            actualWall = Instantiate(myWall, new Vector2(transform.position.x+w, transform.position.y + h*diff), transform.rotation,transform);
            if (!flip)
                actualWall.transform.localScale = new Vector3(-1, 1, 1);
            actualWall.name = "WAll" + i.ToString("D2") + "_By_" + name;  
       
            if (actualWall.GetComponent<WallScript>().isTween)
            { 
                WallScript nephew;
                GameObject mtw = null;
            if (uncle.childCount >= transform.childCount)
            {
                nephew = uncle.GetChild(actualWall.transform.GetSiblingIndex()).GetComponent<WallScript>();
                if (!nephew.isTween)
                {
                    mtw = Instantiate(myWall.GetComponent<WallScript>().tweenWall, nephew.transform.position, nephew.transform.rotation, uncle).gameObject;

                    Destroy(nephew.gameObject);

                }
            }
            else if (uncle.childCount < transform.childCount)
            {
                nephew = uncle.transform.GetChild(uncle.childCount - 1).GetComponent<WallScript>();
                mtw = Instantiate(myWall.GetComponent<WallScript>().tweenWall, new Vector2(nephew.transform.position.x,actualWall.transform.position.y), nephew.transform.rotation, uncle).gameObject;

            }
         
            mtw.name = "Tween Wall" + i.ToString("D2") + "_By_" + uncle.name;
            mtw.GetComponent<WallScript>().speed = speed;
            if (flip)
                mtw.transform.localScale = new Vector3(-1, 1, 1);
            else
                mtw.transform.localScale = new Vector3(1, 1, 1);
        }

            j--;
            i++;
        

    }
	
	public void StopWalls(){
		foreach (WallScript ws in transform.GetComponentsInChildren<WallScript>()) {
			ws.Speed = 0;
		}
	}
	public void ChangeWallSpeed(){
		
		foreach (WallScript ws in transform.GetComponentsInChildren<WallScript>()) {
			ws.Speed = speed;
		}

	}

	public float Speed {
		get {
			return speed;
		}
		set {
			speed = value;
		}
	}

    public int Distancia { get => distancia; set => distancia = value; }
    public double WorldScreenWidth { get; private set; }
    public int J { get => j; set => j = value; }
    public void DestroyWalls() {
        myRanGen.ResetNumbers();
        foreach (Transform ct in transform)
            GameObject.Destroy(ct.gameObject);
    }
}
