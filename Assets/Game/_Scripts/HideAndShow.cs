using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HideAndShow : MonoBehaviour
{

    RectTransform myRT;
    public UnityEvent OnShow, OnHide;
    // Start is called before the first frame update
    void Start()
    {
        myRT = GetComponent<RectTransform>();

    }

    // Update is called once per frame
    public void Hide() {
        myRT.anchoredPosition = new Vector2(-9999, -9999);
        OnHide.Invoke();
    }
    public void Show() {
        myRT.anchoredPosition = Vector2.zero;
        Debug.Log("se mostro");
        OnShow.Invoke();
    }

}
