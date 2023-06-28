using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public enum Move_mode { Ciro,Argenis,Phisx_Argenis}
[System.Serializable] public class SwipeEvent : UnityEvent<float,Vector2> { }
public class Touch : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
   
    public Vector2 origin;
	public Vector2 direction; 
	public bool touched,isDrag;
	public int pointerID;
    public Move_mode movementType;
	public Vector2 currentPos;
    SwipeEvent sevent;

    public SwipeEvent Sevent { get => sevent; set => sevent = value; }

    void Awake()
	{
		direction = Vector2.zero;
		touched = false;
        Debug.Log("Here");
        sevent = new SwipeEvent();
	}

    //----------Set our start point
    public void OnPointerDown(PointerEventData data)
    {

        if (!touched)
        {
            touched = true;
            pointerID = data.pointerId;
            origin = data.position;
            //Debug.Log(data.position);
        }
        isDrag = false;
    }   
        
        
	

	//----------Compare diference betwen our start point and current pointer pos
	public void OnDrag (PointerEventData data)
	{
        switch (movementType)
        {

            case Move_mode.Ciro:
                if (data.pointerId == pointerID)
                {
                    currentPos = data.position;
                    Vector2 directionRaw = currentPos - origin;
                    direction = directionRaw.normalized;

                    Debug.Log("Direccion y: " + direction);
                }
                break;
            case Move_mode.Argenis:
                isDrag = true;
                break;
            default: break;
        }
	}

	//----------Reset everything
	public void OnPointerUp (PointerEventData data)
	{
        switch (movementType)
        {

            case Move_mode.Ciro:
                //Debug.Log("toque");
                if (data.pointerId == pointerID)
                {
                    direction = Vector2.zero;
                   
                    Debug.Log(direction);
                }
                break;
            case Move_mode.Argenis:     
                
                currentPos = data.position;
                //Debug.Log(currentPos);
                Vector2 wp_origin = Camera.main.ScreenToWorldPoint(origin);
                Vector2 wp_curp = Camera.main.ScreenToWorldPoint(currentPos);
                float angle = Vector2.Angle(wp_origin, wp_curp);
                Debug.Log(angle);
               
                if (isDrag) {
                if (wp_origin.x<wp_curp.x)
                sevent.Invoke(Vector2.Distance(wp_origin, wp_curp),Vector2.right);
                if (wp_origin.x > wp_curp.x)
                 sevent.Invoke(Vector2.Distance(wp_origin, wp_curp), Vector2.left);
                }
               
                break;
            default: break;
        }
        touched = false;
        isDrag = false;
        origin = Vector2.zero;
        currentPos = Vector2.zero;
    }

	public Vector2 GetDirection()
	{
		return direction;
	}

	public Vector2 GetPosition()
	{
		return currentPos;
	}

    public void ModoArgenis() {
        movementType = Move_mode.Argenis;
    }
    public void ModoCiro() {
        movementType = Move_mode.Ciro;
    }
}
