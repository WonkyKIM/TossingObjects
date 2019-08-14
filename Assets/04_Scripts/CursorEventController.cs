using UnityEngine;


public class CursorEventController : MonoBehaviour {


    [SerializeField] private CursorObject cursorObject;
    private Vector3 position;

    #region Private Variables
    public enum CursorStatus {
        normal,
        hover,
        clicked
    }
    private CursorStatus currCursorStatus;

    private Camera cam;


    private bool isHovered = false;
    #endregion


    #region MonoBehaviour
    private void Awake() {
        if(cursorObject == null) {
        }
        cam = Camera.main;
    }


    private void Update() {


    }
    #endregion



    #region Private Methods
    private void MouseBehaviour() {
        Vector2 mousePos = Input.mousePosition;
        position = cam.ScreenToWorldPoint( new Vector3( mousePos.x, mousePos.y, cam.nearClipPlane ) );
        cursorObject.Position = position;

        if (Input.GetMouseButtonDown( 0 )) {
            //ChangeStatus( CursorStatus.clicked );
        }
        if (Input.GetMouseButtonUp( 0 )) {
            if (isHovered) {
                //ChangeStatus( CursorStatus.hover );
            } else {
                //ChangeStatus( CursorStatus.normal );
            }
        }
    }



    private void ChangeStatus( CursorStatus _status ) {
        if (currCursorStatus == _status) {
            return;
        } else {
            currCursorStatus = _status;
            switch (currCursorStatus) {
                case CursorStatus.normal:
                    //cursorSize_target = 0.3f;
                    //mr.material.SetColor( shaderProp_color, Color.gray );
                    break;
                case CursorStatus.hover:
                    //cursorSize_target = 0.3f;
                    //mr.material.SetColor( shaderProp_color, Color.white );
                    break;
                case CursorStatus.clicked:
                    //cursorSize_target = 0.8f;
                    //mr.material.SetColor( shaderProp_color, Color.white );
                    break;
            }
            //UpdateCursorStart();
        }
    }
    #endregion
}
