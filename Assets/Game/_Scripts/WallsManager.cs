using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
[System.Serializable]
public class MyfloatEvent : UnityEvent<float>
{
}
public class WallsManager : MonoBehaviour {
	UnityEvent onSpeedChanged;
    MyfloatEvent onSpeedChangedF;
	[Range(0,20)]
	public float speed =2;
	float tempSpeed = 0;
    [Range(0, 2)]
    public float modifier;
    GameObject myDog;
    public float score = 0,tempJS=0;
    private bool neighbourCreatedASpike;
    [Range(0, 2)]
    public float vertPosition=0.45f;
    private Bird myBird;
    private bool boosterActivated;
    private double WorldScreenWidth;
    public float levelUpLapse = 10, jumpScale = 4;
    public bool testing;
    private bool jumped;
    private WallGenerator[] myWgs;

    private void Awake()
    {
        OnSpeedChanged = new UnityEvent();
        OnSpeedChangedF = new MyfloatEvent();
    }
    // Use this for initialization
    void Start () {
        Floor = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y;
        Ceiling = Camera.main.ScreenToWorldPoint(new Vector2(0, Camera.main.pixelHeight)).y;
        myDog = GameObject.FindGameObjectWithTag("Player");
        tempSpeed = speed;
        FindObjectOfType<PlayerManager>().PlayerLose.AddListener(() => speed = 0);
        myWgs = transform.GetComponentsInChildren<WallGenerator>();
         FindDoug();
        foreach (Transform t in transform) {
			WallGenerator wg = t.GetComponent<WallGenerator> ();
			wg.Speed = speed;
           
			wg.ChangeWallSpeed ();
			OnSpeedChanged.AddListener (wg.ChangeWallSpeed);

		}
        foreach (CoinGenerator cg in FindObjectsOfType<CoinGenerator>()) 
            OnSpeedChanged.AddListener(cg.ChangeSpeed);

        double worldScreenHeight = Camera.main.orthographicSize * 2.0;
        WorldScreenWidth = worldScreenHeight * Camera.main.aspect;
        /*float sd = /*FindObjectOfType<BackgroundManager>().sizedif;1;
        foreach (Objeto go in GetComponent<RandomGenerator>().elementos) 
            foreach (SpriteRenderer sr in go.go.GetComponentsInChildren<SpriteRenderer>(true))
                sr.transform.localScale = new Vector2(0.5f, 0.5f)*sd;*/
            

        
      
    }
	
	// Update is called once per frame
	void Update () {
     

            if (!myBird.IsDead)
        {
            if (!boosterActivated && !testing)
                speed = Mathf.Clamp(JumpSpeed(), -0.5f, Mathf.Infinity);
            // speed = Mathf.Clamp(myBird.vertDelta*(1+ 1.5f*NormalizedInversePoint(myBird.transform.position.y)), -0.5f, Mathf.Infinity);
            if ((tempSpeed != speed && Time.timeScale != 0 && NormalizedInversePoint(myBird.transform.position.y) >= 0) || boosterActivated || testing)
            {
                MoveWalls();
                //	Debug.Log (speed.ToString("###.00000")); 
                score += Time.deltaTime * speed * modifier;
                foreach (RandomGenerator tp in transform.GetComponentsInChildren<RandomGenerator>())
                    tp.AcumVariable = score;
            }
            else if(NormalizedInversePoint(myBird.transform.position.y) < 0.2)
            {
                speed = 0;
                MoveWalls();
            }

            //Debug.Log (NeighbourCreatedASpike);

        }
        else
        {
            //Debug.Log("estoy morido no jodas");
            speed = 0;
            MoveWalls();}
            
	}
    public float JumpSpeed() {
        if (Input.GetMouseButtonDown(0))
            tempJS = 0;
        if (jumped)
        {
            tempJS += Mathf.PI * Time.deltaTime;
            //Debug.Log(tempJS);
        }
        /*
        if (tempJS > (Mathf.PI * 1.15))
        {
            jumped = false;
            tempJS = 0;
        }
        return Mathf.Sin(tempJS) * jumpScale;*/
        float _wallSpeed= MathFuncs.WeibullFunc(1.3f, .7f, tempJS) * jumpScale;
        if (tempJS > 0 && _wallSpeed < 0.099 && myBird.vertDelta < 0)
            return myBird.vertDelta;
        else
            return _wallSpeed;

    }
    public void MoveWalls()
    {
        foreach (WallGenerator wg in myWgs)
            wg.Speed = speed;
        OnSpeedChanged.Invoke();
        onSpeedChangedF.Invoke(speed);
        tempSpeed = speed;
    }



	public float Speed {
		get {
			return speed;
		}
		set {
			speed = value;
		}
	}

    public bool NeighbourCreatedASpike { get => neighbourCreatedASpike; set => neighbourCreatedASpike = value; }
    public bool BoosterActivated { get => boosterActivated; set => boosterActivated = value; }
    public UnityEvent OnSpeedChanged { get => onSpeedChanged; set => onSpeedChanged = value; }
    public MyfloatEvent OnSpeedChangedF { get => onSpeedChangedF; set => onSpeedChangedF = value; }
 

    public void StartMoving(){
		OnSpeedChanged.Invoke ();
	}
	public void SetSpeed(float f){
		speed = f;
	}
    IEnumerator SpeedProgresion() {

        WallGenerator[] myWalls = GetComponentsInChildren<WallGenerator>();

        while (true) {
            foreach (WallGenerator wg in myWalls)
                if (wg.distancia>=0)
                  wg.k -= 1;

            
            yield return new WaitForSeconds(levelUpLapse);
        }
      
    }

    public void FindDoug() {
       myBird = FindObjectOfType<Bird>();
    }
    public float Ceiling { get; private set; }
    public float Floor { get; private set; }
    public bool Jumped { get => jumped; set => jumped = value; }
    public float Score { get => score; set => score = value; }

    public float NormalizedPoint(float actualPoint)
    {
        return Mathf.Clamp01((actualPoint - Ceiling) / (-Ceiling + Floor));
    }
    public float NormalizedInversePoint(float y)
    {
        return 1 - Mathf.Clamp01((y - Ceiling) / (-Ceiling + Floor));
    }
    public void ClearAll() {
        foreach (WallGenerator wg in myWgs)
            wg.DestroyWalls();
    }
    public void InitiateAll() {
        speed = 0;
        tempJS = 0;
        jumped = false;
        MoveWalls();
        foreach (WallGenerator wg in myWgs)
            wg.StartCoroutine("CreateInitialWalls");
    }
}
