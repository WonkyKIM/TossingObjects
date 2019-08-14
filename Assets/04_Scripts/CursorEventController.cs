using System.Collections.Generic;
using UnityEngine;


public class CursorEventController : MonoBehaviour {

    
    private TossingObject currTossingObject;
    private bool isHovered = false;
    private bool isClicked = false;
    
    [SerializeField] private CursorObject cursorObject;
    private Vector3 position;

    private Vector2 mousePosition;
    private Vector2 pMousePosition;
    private List<Vector2> mouseDirections = new List<Vector2>();
    private int maximumMouseDirections = 60;
    private int mouseDirectionsCurrNumber = -1;  // This should be started from -1

    [SerializeField] private float throwingForce = 5;

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
        mousePosition = Input.mousePosition;
        Vector3 position = cam.ScreenToWorldPoint( new Vector3(mousePosition.x, mousePosition.y, cam.nearClipPlane ) );
        cursorObject.Position = position;

        // Check Mouse Hover
        Ray ray = cam.ScreenPointToRay(mousePosition);
        if(!isClicked) {
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit)) {
                if(currTossingObject == null) {
                    currTossingObject = hit.transform.GetComponent<TossingObject>();
                    if(currTossingObject != null) {
                        currTossingObject.Hover();
                    }
                }
            } else {
                if(currTossingObject != null) {
                    currTossingObject.HoverExit();
                    currTossingObject = null;
                }
            }
            isHovered = (currTossingObject != null);
        }

        // Mouse Click & Tossing Events
        if(isHovered && Input.GetMouseButtonDown(0)) {
            isClicked = true;
            currTossingObject.OnGrab(position + ray.direction * 0.5f);
        }
        if(isClicked) {
            currTossingObject.StayGrab(position + ray.direction * 0.5f);
            StoreMouseDirections();
            if(Input.GetMouseButtonUp(0)) {
                isClicked = false;
                currTossingObject.Throw(MouseThrowingForce() * throwingForce, Vector3.zero);
                currTossingObject = null;
            }
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
                    cursorObject.UpdateCursor(0.3f, ColorsLibrary.inst.Grey);
                    break;
                case CursorStatus.Hover:
                    cursorObject.UpdateCursor(0.3f, ColorsLibrary.inst.Green);
                    break;
                case CursorStatus.Clicked:
                    cursorObject.UpdateCursor(0.8f, ColorsLibrary.inst.White);
                    break;
            }
        }

    }



    private Vector2 calcMouseDirection() {
        return (mousePosition - pMousePosition) * Time.deltaTime;
    }

    private void StoreMouseDirections() {

        // get current index
        mouseDirectionsCurrNumber++;
        int index = (mouseDirectionsCurrNumber) % maximumMouseDirections;

        // attenuation
        for(int i=0; i<mouseDirections.Count; i++) {
            mouseDirections[i] *= 0.95f;
        }

        // Store new velocity
        Vector2 direction = calcMouseDirection();
        //mouseDirections.Add(direction);
        if(mouseDirections.Count <= maximumMouseDirections) {
            mouseDirections.Add(direction);
        } else {
            mouseDirections[index] = direction;
        }

        // Store previous MousePosition
        pMousePosition = mousePosition;
    }

    private Vector3 MouseThrowingForce() {
        Vector2 mouseDirs = Vector2.zero;
        for(int i = 0; i < mouseDirections.Count; i++) {
            mouseDirs += mouseDirections[i];
        }
        mouseDirs /= mouseDirections.Count;
        return new Vector3(mouseDirs.x, mouseDirs.y, 0) + cam.transform.forward * (mouseDirs.x*mouseDirs.x + mouseDirs.y*mouseDirs.y)*3;
    }


    #endregion
}
