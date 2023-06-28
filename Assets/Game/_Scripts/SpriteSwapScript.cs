using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwapScript : MonoBehaviour
{
    AudioManager _am;
    private void Start()
    {
        _am = FindObjectOfType<AudioManager>();
    }
    public GameObject toggleTrueImage, toggleFalseImage;
    
    public void Swap(bool tb)
    {
        toggleTrueImage.SetActive(tb);
        toggleFalseImage.SetActive(!tb);
    }
    public void ClickSound() {
        Debug.Log("sonado");
        _am.Play("BotonTap");
    }
}
