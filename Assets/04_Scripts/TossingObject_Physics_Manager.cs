using UnityEngine;

public class TossingObject_Physics_Manager : MonoBehaviour {


    private TossingObject_Physics[] objects;
    [SerializeField] private float objectRadius = 0.08f;
    
    private void Awake() {
        objects = FindObjectsOfType<TossingObject_Physics>();
        Debug.Log("There are (" + objects.Length + ") numbers of physics objects.");
    }


    private void Update() {
        CheckCollision();
    }


    private void CheckCollision() {
        for(int i=0; i<objects.Length; i++) {
            Vector3 a = objects[i].transform.position;
            float ra = objectRadius * objects[i].transform.localScale.x;
            for(int j=0; j<objects.Length; j++) {
                if(i == j) continue;
                Vector3 b = objects[j].transform.position;
                float rb = objectRadius * objects[i].transform.localScale.x;
                float distance = (ra + rb) * 0.5f;
                if(Vector3.Distance(a, b) < distance) {
                    objects[j].OnCollisionWithOther(objects[i], distance);
                }
            }
        }
    }
    
}
