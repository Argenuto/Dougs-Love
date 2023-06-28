using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
public class RateUsScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("NOMBRE DE LA APLICACION: "+Application.identifier);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Rate() {
#if UNITY_ANDROID
        Application.OpenURL("market://details?id="+ Application.identifier);
#elif UNITY_IOS
        Device.RequestStoreReview();
#endif
    }
}
