using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("RCollider").position = new Vector2(Camera.main.ScreenToWorldPoint(Vector2.zero).x, transform.position.y);
        transform.Find("LCollider").position = new Vector2(Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight)).x, transform.position.y);
        transform.Find("RCollider").GetComponent<Collider2D>();

        //float worldX = transform.Find("LCollider") ? : Camera.main.ScreenToWorldPoint(Vector2.zero).x;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
