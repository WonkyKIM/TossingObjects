using UnityEngine;

public class Camera_Navigate : MonoBehaviour {
    
    private Transform tr;
    private Vector3 mouseCache;
    private Vector3 rotEulerCache;
    [SerializeField, Range(0,1)] private float sensitivity = 0.5f;

    void Start() {
        tr = this.transform;
    }
    
    void Update() {
        if(Input.GetKeyDown(KeyCode.LeftAlt)) {
            mouseCache = Input.mousePosition;
            rotEulerCache = tr.transform.localEulerAngles;
        }
        if(Input.GetKey(KeyCode.LeftAlt)) {
            Vector3 mouse = Input.mousePosition - mouseCache;
            tr.localEulerAngles = rotEulerCache + (new Vector3(-mouse.y, mouse.x, 0) * sensitivity*0.5f);
        }
    }
}
