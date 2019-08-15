using UnityEngine;

/// <summary>
/// 
/// </summary>
public class TossingObject_Physics : MonoBehaviour {


    [SerializeField] private Transform tr_this;
    
    private Vector3 velocity;
    private float angularVelocity;

    private Vector3 rotation_direction;
    private float rotation_strength;
    
    private Vector3 target;
    private bool isToTarget = false;

    [SerializeField, Range(0.1f, 1)] private float drag = 0.94f;
    [SerializeField, Range(0.1f, 1)] private float drag_angular = 0.96f;
    [SerializeField, Range(0.1f, 1)] private float drag_rotation = 0.98f;

    [SerializeField] private float rotationSensitivity = 2;

    public float Mass {
        get { return mass; }
        set {
            float m = Mathf.Max(value, 0.01f);
            mass = m;
        }
    }
    [SerializeField] private float mass = 1;


    public void AddForce(Vector3 _forceThrow, float _angularVel) {
        isToTarget = false;
        Vector3 f1 = Vector3.Min(_forceThrow, Vector3.one*2) / mass;
        velocity += f1;
        //Vector3 f2 = (_forceSpin - _forceThrow) / mass;
        //Vector3 f2 = (tr_this.InverseTransformVector(_forceSpin) - tr_this.TransformVector(_forceThrow)) / mass;
        angularVelocity = _angularVel;
        ForceToRotation(f1);
    }

    public void MoveToTarget(Vector3 _target) {
        isToTarget = true;
        //target = tr_this.InverseTransformPoint(_target);
        target = _target;
        //ForceToRotation(_target - tr_this.position);
    }
    
    private void ForceToRotation(Vector3 _force) {
        float rotX = Vector3.Dot(tr_this.forward, _force) * 0.2f;
        float rotY = Vector3.Dot(tr_this.up, _force);
        float rotZ = Vector3.Dot(-tr_this.right, _force);
        rotation_direction = new Vector3(rotX, rotY, rotZ);
        rotation_strength = Vector3.Magnitude(_force);
    }


    private void Update() {
        
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.A)) {
            AddForce(Camera.main.transform.forward * 1, 0);
        }
        if(Input.GetKeyDown(KeyCode.S)) {
            float[] a = new float[3];
            for(int i=0; i<3; i++) {
                a[i] = Random.Range(0f, 0.5f);
            }
            MoveToTarget(new Vector3(a[0], a[1], a[2]));
        }
#endif
        //velocity += tr_this.TransformVector(additionalVelocity) * Time.deltaTime;
        velocity = Quaternion.AngleAxis(angularVelocity, Camera.main.transform.forward) * velocity;

        if(isToTarget) {
            Vector3 vel = (target - tr_this.position) / mass * 0.2f;
            tr_this.Translate(tr_this.InverseTransformVector(vel));
            if(Vector3.Distance(target, tr_this.position) < 0.01f) {
                isToTarget = false;
                velocity = Vector3.zero;
                //rotation_strength = 0;
            }
        }
    }
    
    private void LateUpdate() {
        //float localRotX = Vector3.Dot(tr_this.forward, velocity);
        //float localRotY = Vector3.Dot(tr_this.up, velocity);
        //float localRotZ = Vector3.Dot(-tr_this.right, velocity);

        tr_this.Translate(tr_this.InverseTransformVector(velocity) * Time.deltaTime);
        //tr_this.Rotate(new Vector3(localRotX, localRotY, localRotZ) * rotationSensitivity);
        tr_this.Rotate(tr_this.InverseTransformDirection(rotation_direction) * rotation_strength * rotationSensitivity);

        velocity *= drag;
        angularVelocity *= drag_angular;
        rotation_strength *= drag_rotation;
    }


    // Collision Event From Manager
    public void OnCollisionWithOther(TossingObject_Physics _coll, float _dist) {
        Vector3 normal = this.transform.position - _coll.transform.position;
        normal.Normalize();
        
        _coll.AddForce(normal * Vector3.Dot(velocity, normal), 0);
        
        this.transform.position = _coll.transform.position + normal * _dist;
        velocity += Vector3.Reflect(velocity, normal) / mass;
    }




}
