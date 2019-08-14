using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class TossingObject_Physics : MonoBehaviour {


    [SerializeField] private Transform tr_this;
    
    private Vector3 velocity;

    private Vector3 rotation_direction;
    private float rotation_strength;
    
    private Vector3 target;
    private bool isToTarget = false;

    [SerializeField, Range(0.1f, 1)] private float drag = 0.96f;
    [SerializeField, Range(0.1f, 1)] private float drag_rotation = 0.96f;

    [SerializeField] private float rotationSensitivity = 2;

    public float Mass {
        get { return mass; }
        set {
            float m = Mathf.Max(value, 0.01f);
            mass = m;
        }
    }
    private float mass = 1;


    public void AddForce(Vector3 _force, Vector3 _force2) {
        isToTarget = false;
        Vector3 f = Vector3.Min(_force, Vector3.one*10) / mass;
        velocity += f;
        ForceToRotation(f);
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
            AddForce(Camera.main.transform.forward * 1, Vector3.zero);
        }
        if(Input.GetKeyDown(KeyCode.S)) {
            float[] a = new float[3];
            for(int i=0; i<3; i++) {
                a[i] = Random.Range(0f, 0.5f);
            }
            MoveToTarget(new Vector3(a[0], a[1], a[2]));
        }
#endif

        if(isToTarget) {
            Vector3 vel = (target - tr_this.position) / mass * 0.2f;
            tr_this.Translate(tr_this.InverseTransformVector(vel));
            if(Vector3.Distance(target, tr_this.position) < 0.01f) {
                isToTarget = false;
                velocity = Vector3.zero;
                //rotation_strength = 0;
            }
        }

        //float localRotX = Vector3.Dot(tr_this.forward, velocity);
        //float localRotY = Vector3.Dot(tr_this.up, velocity);
        //float localRotZ = Vector3.Dot(-tr_this.right, velocity);

        tr_this.Translate(tr_this.InverseTransformVector(velocity) * Time.deltaTime);
        //tr_this.Rotate(new Vector3(localRotX, localRotY, localRotZ) * rotationSensitivity);
        tr_this.Rotate(tr_this.InverseTransformDirection(rotation_direction) * rotation_strength * rotationSensitivity);

        velocity *= drag;
        rotation_strength *= drag_rotation;
    }



}
