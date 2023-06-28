using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum ButtonType { share,rate}
public class MultiFunctionScript : MonoBehaviour
{
    public Sprite[] buttonImage;
    private Image myImage;
    private Button myButton;
    
    // Start is called before the first frame update
    void Start()
    {
        myImage = GetComponent<Image>();
        myButton = GetComponent<Button>();
        ButtonType myBT =(ButtonType) Random.Range(0, 2);
        myImage.sprite = buttonImage[(int)myBT];
        switch (myBT)
        {
            case ButtonType.share:
                myButton.onClick.AddListener(GetComponent<RateUsScript>().Rate);
                break;
            case ButtonType.rate:
                myButton.onClick.AddListener(FindObjectOfType<Ctrl_GameOver>().FaceBookShare);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
