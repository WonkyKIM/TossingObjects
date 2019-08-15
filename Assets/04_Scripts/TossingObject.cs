using UnityEngine;

public class TossingObject : MonoBehaviour {

    [SerializeField] private TossingObject_Visibility visibiliy;
    [SerializeField] private TossingObject_Physics physics;

    private void Awake() {
        if(visibiliy == null) {
            visibiliy = this.GetComponent<TossingObject_Visibility>();
        }
        if(physics == null) {
            physics = this.GetComponent<TossingObject_Physics>();
        }
    }

    private void Start() {
        visibiliy.Invisible();
    }

    public void Hover() {
        visibiliy.Visible(ColorsLibrary.inst.Green, 1);
    }
    public void HoverExit() {
        visibiliy.Invisible();
    }

    public void OnGrab(Vector3 _target) {
        physics.MoveToTarget(_target);
        visibiliy.SetColor(ColorsLibrary.inst.Yellow, 1);
        visibiliy.Impact(100);
    }
    public void StayGrab(Vector3 _target) {
        physics.MoveToTarget(_target);
    }

    public void Throw(Vector3 _forceThrow, float _angularVelocity) {
        physics.AddForce(_forceThrow, _angularVelocity);
        visibiliy.Invisible();
    }





}
