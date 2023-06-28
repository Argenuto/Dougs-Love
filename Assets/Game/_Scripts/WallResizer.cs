using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RatioFix { expand,shrink}
public class WallResizer : MonoBehaviour
{
    [Range(-1.75f, 2)]
    public float difference;
    public bool flip;
    public RatioFix actualRatioFix;
    int ratioExpander=1;
    // Start is called before the first frame update
    void Start()
    {

        if (actualRatioFix == RatioFix.shrink)
            ratioExpander = -1;
        else
            ratioExpander = 1;
        float worldX = flip ? Camera.main.ViewportToWorldPoint(new Vector2(1, 1)).x + (difference * Camera.main.aspect * ratioExpander) : Camera.main.ViewportToWorldPoint(Vector2.zero).x - (difference * Camera.main.aspect * ratioExpander);

        transform.position = new Vector2(worldX, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnValidate()
    {

        


    }
}
