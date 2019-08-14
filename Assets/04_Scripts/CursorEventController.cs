using UnityEngine;


public class CursorEventController : MonoBehaviour {

    
    private TossingObject currTossingObject;
    private bool isHovered = false;
    private bool isClicked = false;
    
    [SerializeField] private CursorObject cursorObject;
    private Vector3 position;

    #region Private Variables
    public enum CursorStatus {
        Normal,
        Hover,
        Clicked
    }
    private CursorStatus currCursorStatus;
    private CursorStatus currCursorStatus_cached;

    private Camera cam;
    #endregion
    
    #region MonoBehaviour
    private void Awake() {
        if(cursorObject == null) {
            cursorObject = FindObjectOfType<CursorObject>();
        }
        cam = Camera.main;
    }
    private void Update() {
        MouseBehaviour();
    }
    #endregion
    

    #region Private Methods
    private void MouseBehaviour() {
        
        // Update Cursor Position
        Vector2 mousePos = Input.mousePosition;
        position = cam.ScreenToWorldPoint( new Vector3( mousePos.x, mousePos.y, cam.nearClipPlane ) );
        cursorObject.Position = position;

        // Check Mouse Hover
        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit)) {
            if(currTossingObject == null) {
                currTossingObject = hit.transform.GetComponent<TossingObject>();
            }
        } else {
            currTossingObject = null;
        }
        isHovered = (currTossingObject != null);

        // Mouse Click Event
        if(isHovered && Input.GetMouseButtonDown(0)) {
            // Click Event
            isClicked = true;
        }
        if(isClicked && Input.GetMouseButtonUp(0)) {
            // Throw Event
            isClicked = false;
        }
        
        // Update Status
        if(isClicked) {
            currCursorStatus = CursorStatus.Clicked;
        } else {
            currCursorStatus = (isHovered) ? CursorStatus.Hover : CursorStatus.Normal;
        }
        
        // Update Cursor
        if(currCursorStatus != currCursorStatus_cached) {
            currCursorStatus_cached = currCursorStatus;
            switch(currCursorStatus) {
                case CursorStatus.Normal:
                    cursorObject.UpdateCursor(0.3f, Color.gray);
                    break;
                case CursorStatus.Hover:
                    cursorObject.UpdateCursor(0.3f, Color.green);
                    break;
                case CursorStatus.Clicked:
                    cursorObject.UpdateCursor(0.8f, Color.white);
                    break;
            }
        }

    }
    #endregion
}
