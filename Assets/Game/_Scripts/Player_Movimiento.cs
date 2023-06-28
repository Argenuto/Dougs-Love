using UnityEngine;
using UnityEngine.UI;

public class Player_Movimiento : MonoBehaviour 
{
	public Touch touchPad;
	public float velocidad;
	Ray camRay;
	RaycastHit floorHit;
	int floorMask = 666;
    private float nextx;
    private Vector2 actionArea;
    Vector3 init;

    void OnValidate()
    {
        float xmin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0)).x;
        float xmax = Camera.main.ViewportToWorldPoint(new Vector3(1, 0)).x;
        actionArea = new Vector2(xmin, xmax);
    }
    void Awake()
	{
		floorMask = LayerMask.GetMask ("Background");
        Debug.Log(floorMask);
        Debug.Log(Mathf.Cos( Vector2.Angle(new Vector2(0,0),new Vector2(1,0))));
        
        
    }

    private void Start()
    {
        OnValidate();
        touchPad.Sevent.AddListener(Listnr);
        init = transform.position;

    }
    void Listnr(float f,Vector2 direc) {       
        nextx = Mathf.Clamp(transform.position.x+f*direc.x,actionArea.x,actionArea.y);
        Debug.Log(nextx);
        //FindObjectOfType<Text>().text = nextx.ToString() + "\n" + f.ToString(); ;
       
    }
    void Update ()
	{
        switch (touchPad.movementType) {
            case Move_mode.Ciro:
                if (touchPad.touched)
                {
                    Debug.Log("touched");
                    camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(camRay, out floorHit, 999f, floorMask))
                    {
                        Vector3 newPos = Vector3.Lerp(transform.position, new Vector3(floorHit.point.x, floorHit.point.y, 0), velocidad * Time.deltaTime);

                        transform.position = new Vector3(newPos.x, transform.position.y, transform.position.z);
                    }
                }
                break;
            case Move_mode.Argenis:
                Vector3 npos = Vector3.Lerp(transform.position, new Vector3(nextx,transform.position.y, 0), velocidad * Time.deltaTime);

                transform.position = new Vector3(npos.x, transform.position.y, transform.position.z);

                break;
            default:break; }
       
	}

    public void ResetPosition()
    {
        transform.position = init;
    }

}
