using UnityEngine; 
using System.Collections; 
 
public class Rotation : MonoBehaviour { 
 
    public float degreesPerSec = 360f;
    public bool activated=false;

    void Start() { 
    }

    void Update() {
        float rotAmount = degreesPerSec * Time.deltaTime;
        float curRot = transform.localRotation.eulerAngles.z;
        if(activated)
        transform.localRotation = Quaternion.Euler(new Vector3(0,0,curRot+rotAmount)); 
    } 
 
}