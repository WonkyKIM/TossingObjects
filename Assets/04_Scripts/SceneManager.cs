using UnityEngine;

public class SceneManager : MonoBehaviour {

    [SerializeField] private Transform cam;
    [SerializeField] private TossingObject[] tossingObjects;

    private Vector3 posCam;
    private Quaternion rotCam;
    private Vector3[] posObjects;
    private Quaternion[] rotObjects;
    
    private void Start() {
        Cursor.visible = false;
        posCam = cam.position;
        rotCam = cam.rotation;
        posObjects = new Vector3[tossingObjects.Length];
        rotObjects = new Quaternion[tossingObjects.Length];
        for(int i=0; i<tossingObjects.Length; i++) {
            posObjects[i] = tossingObjects[i].transform.position;
            rotObjects[i] = tossingObjects[i].transform.rotation;
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.R)) {
            cam.SetPositionAndRotation(posCam, rotCam);
            for(int i = 0; i < tossingObjects.Length; i++) {
                tossingObjects[i].transform.SetPositionAndRotation(posObjects[i], rotObjects[i]);
            }
        }
    }

}
