using UnityEngine;

public class CursorObject : MonoBehaviour {


    public Vector3 Position {
        set { this.transform.position = value; }
    }

    private MeshRenderer mr;
    private int shaderProp_size;
    private int shaderProp_color;

    private float cursorSize_target = 0.3f;
    private float cursorSize = 0.3f;
    private float cursorSizeVelocity;
    private bool isCursorSizeChanging = false;



    #region MonoBehaviour
    private void Awake() {
        mr = this.GetComponent<MeshRenderer>();
        shaderProp_size = Shader.PropertyToID( "_CursorSize" );
        shaderProp_color = Shader.PropertyToID( "_MainColor" );
    }

    private void Start() {
        UpdateCursorStart( 0.3f, Color.gray );
    }

    private void Update() {

        if (isCursorSizeChanging) {
            UpdateCursor();
        }

    }
    #endregion



    #region Private Methods




    private void UpdateCursorStart( float? _target = null, Color? _color = null ) {
        if (_target != null) {
            cursorSize_target = (float)_target;
        }
        if (_color != null) {
            mr.material.SetColor( shaderProp_color, (Color)_color );
        }
        isCursorSizeChanging = true;
        timer_update = 0;
    }

    float timer_update = 0;
    private void UpdateCursor() {
        cursorSizeVelocity += ( cursorSize_target - cursorSize ) * Time.deltaTime * 5;
        cursorSize += cursorSizeVelocity;
        //if( Mathf.Abs(cursorSize_target - cursorSize) < 0.0001f ) {
        //if ( cursorSizeVelocity < 0.01f ) {
        //cursorSize = cursorSize_target;
        //cursorSizeVelocity = 0.0f;
        //isCursorSizeChanging = false;
        //} else {
        cursorSizeVelocity *= 0.9f;
        //}
        mr.material.SetFloat( shaderProp_size, cursorSize );

        timer_update += Time.deltaTime;
        if (timer_update > 1) {
            timer_update = 0;
            isCursorSizeChanging = false;
        }
    }
    #endregion




}

