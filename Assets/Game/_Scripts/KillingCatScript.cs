using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillingCatScript : CoinScript
{
    private float right = 0;
    private float left = 0;
    private float top = 0;
    private readonly float distance = 1;
    public GameObject catArrow;
    private AlertArrowScript myCatArrow;

    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
     
    public override void SetInitialMovement()
    {
        top = Camera.main.ScreenToWorldPoint(Vector2.up*Camera.main.pixelHeight).y - catArrow.GetComponent<SpriteRenderer>().bounds.extents.y;
        left = Camera.main.ScreenToWorldPoint(Vector2.zero).x +distance;
        right = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth,0)).x - distance;
        transform.position = new Vector2(Random.Range(left,right),transform.position.y);
        rb2d.gravityScale = 2f;
        speed = 1;
        myCatArrow = Instantiate(catArrow, new Vector2(Random.Range(left, right), top), transform.rotation).GetComponent<AlertArrowScript>();
        myCatArrow.MyCat = gameObject;

        Debug.Log("left: " + left);
        Debug.Log("right: " + right);
    }
    private void OnBecameVisible()
    {
        Destroy(myCatArrow.gameObject);
    }
}
