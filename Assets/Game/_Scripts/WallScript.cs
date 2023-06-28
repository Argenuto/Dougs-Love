using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[Serializable]
public class WallScript : MonoBehaviour {

    
	public float speed = 2;
    public bool isASpike, isCoin;
    Rigidbody2D rb2d;
    private float h;
    BoxCollider2D myColl2D;
    Bounds myBounds;
    private GameObject GasLeak = null;
    private ParticleSystem Ray = null;

    public AudioSource RaySound { get; private set; }

    float limit,tempTimer=0;
    [HideInInspector]
    public bool isTween;
    public bool mustHide = true;
    [HideInInspector]
   public  WallScript tweenWall;
    [HideInInspector]
    public float delayShoot;
    private float fitSize;

    // Use this for initialization
    private void Awake()
    {
        h = transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
        myColl2D = transform.Find("Sprite").GetComponent<BoxCollider2D>();
        try
        {
            GasLeak = transform.Find("GasLeak").gameObject;
        }
        catch (NullReferenceException nre) {
          //  Debug.Log(nre.Message);
            GasLeak = null;

        }
        if (GasLeak != null)
            StartCoroutine("GasLeakCR");
        myBounds = transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().bounds;

        try
        {
            Ray = transform.Find("Ray").GetComponent<ParticleSystem>();
            RaySound = GetComponent<AudioSource>();
        }
        catch (NullReferenceException nre)
        {
           // Debug.Log(nre.Message);
            Ray = null;
            RaySound = null;

        }
    }

   
    IEnumerator GasLeakCR() {
        //Debug.Log("jumito jumito :3");
        while (true) {
            
            GasLeak.SetActive(!GasLeak.activeInHierarchy);
            myColl2D.enabled = !myColl2D.enabled;
            yield return new WaitForSeconds(3);

        }
    }
    private void Update()
    {
        if (isTween&& tempTimer >= delayShoot && !Ray.isPlaying&&!Bird.thisBird.isDead)
        {
            Ray.GetComponent<ParticleSystem>().Play();
            if(RaySound!=null)
                RaySound.Play();
            tempTimer = 0;
        }
        tempTimer += Time.deltaTime;
    }

    void Start ()
	{
		rb2d = GetComponent<Rigidbody2D> ();
        limit = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y-3f;
       // Debug.Log(name+"__"+transform.GetSiblingIndex());

    }

	// Update is called once per frame
	void FixedUpdate ()
	{
        if (speed != 0 && Time.timeScale ==1)
        {
            transform.Translate(Vector2.down * speed * Time.fixedDeltaTime);
        }
        if ((transform.GetSiblingIndex() == transform.parent.childCount - 1) && (transform.parent.position.y - transform.position.y) >= h)
        {
            float diff = 1f - (transform.parent.position.y - transform.position.y) / h;
            transform.parent.GetComponent<WallGenerator>().CreateWall(diff);
            //Debug.Log(1 - (transform.parent.position.y - transform.position.y) / h);
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

 
    public WallScript TweenWall { get => tweenWall; set => tweenWall = value; }
    public float H { get => h; set => h = value; }

    public void RenderQueue(int i) {
        Debug.Log(transform.GetChild(0).GetComponent<SpriteRenderer>().material.renderQueue = i);
    }

	void OnBecameInvisible(){
        //Debug.Log("desapareci");
        if(mustHide)
            Destroy (gameObject);
	}


    private void LateUpdate()
    {
        if (transform.position.y < limit)
            OnBecameInvisible();
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(WallScript))]
[CanEditMultipleObjects]
public class WallScriptEditor : Editor {

   
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();
        var myWallS = target as WallScript;
         EditorGUILayout.PropertyField(serializedObject.FindProperty("isTween"));
        if (myWallS.isTween)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("tweenWall"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("delayShoot"));

        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
