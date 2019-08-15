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
    private Vector2 mouseDirectionCurr;
    private List<Vector2> mouseDirections = new List<Vector2>();
    private int mouseDirectionsIndex = -1; // This should be started from -1
    private int mouseDirectionsMaxNumber = 60;
    //private int mouseDirectionsCurrNumber = -1;  
    //private float mouseSpiningSpeed = 0;
    private bool isSpiningLeft = false;

    [SerializeField] private float force_throwing = 1;
    [SerializeField] private float force_spining = 1;

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
        Vector3 position = cam.ScreenToWorldPoint( new Vector3(mousePosition.x, mousePosition.y, cam.nearClipPlane+0.01f ) );
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
                Vector3 throwForce = mouseDirectionFromVec2(MouseThrowingForce(5)) * force_throwing;
                float spinningSpeed = MouseSpinningSpeed(mouseDirectionsMaxNumber) * force_spining;
                Debug.Log("[Throw] force: " + throwForce + ", spin: " + spinningSpeed);
                currTossingObject.Throw(throwForce, spinningSpeed);
                currTossingObject = null;
                CleanupMouseDirections();
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


    

    private Vector3 mouseDirectionFromVec2(Vector2 _dir) {
        return cam.transform.right*_dir.x + cam.transform.up*_dir.y + cam.transform.forward * Mathf.Sqrt(_dir.x*_dir.x + _dir.y*_dir.y);
    }

    private void StoreMouseDirections() {

        // get current index
        mouseDirectionsIndex++;
        int index = mouseDirectionsIndex = (mouseDirectionsIndex) % mouseDirectionsMaxNumber;

        // attenuation
        for(int i=0; i<mouseDirections.Count; i++) {
            mouseDirections[i] *= 0.95f;
        }

        // Store new velocity
        mouseDirectionCurr = (mousePosition - pMousePosition) * Time.deltaTime;
        //mouseDirections.Add(direction);
        if(mouseDirections.Count < mouseDirectionsMaxNumber) {
            mouseDirections.Add(mouseDirectionCurr);
        } else {
            mouseDirections[index] = mouseDirectionCurr;
        }

        //// Store spinningSpeed
        //AddMouseSpinningSpeed();
        
        // Store previous MousePosition
        pMousePosition = mousePosition;
    }

    private void CleanupMouseDirections() {
        mouseDirections.Clear();
        mouseDirectionCurr = Vector2.zero;
        mouseDirectionsIndex = -1;  // This should be started from -1
    }

    private Vector3 MouseThrowingForce(int _sampleCount) {
        int sampleCount = Mathf.Min(_sampleCount, mouseDirections.Count);
        Vector2 mouseDirs = Vector2.zero;
        for(int i = 0; i < sampleCount; i++) {
            int index = mouseDirectionsIndex - i;
            if(index < 0) {
                index += mouseDirections.Count;
            }
            mouseDirs += mouseDirections[index];
        }
        mouseDirs /= sampleCount;
        return mouseDirectionFromVec2(mouseDirs);
    }

    private Vector3 MouseSpiningForce() {
        Vector2 mouseDirs = Vector2.zero;
        for(int i = 0; i < mouseDirections.Count; i++) {
            mouseDirs += mouseDirections[i];
        }
        mouseDirs /= mouseDirections.Count;
        return mouseDirectionFromVec2(mouseDirs);
        //return cam.transform.right*mouseDirs.x + cam.transform.up*mouseDirs.y;
    }

    /*private float MouseSpiningSpeed() {
        float speed = 0;
        for(int i = 0; i < mouseDirections.Count; i++) {
            speed += Mathf.Sqrt(Vector2.SqrMagnitude(mouseDirections[i]));
        }
        return speed;
        //return cam.transform.right*mouseDirs.x + cam.transform.up*mouseDirs.y;
    }*/

    /*private void AddMouseSpinningSpeed() {
        int prvIndex = mouseDirectionsIndex - 1;
        if(prvIndex < 0) {
            prvIndex += maximumMouseDirections;
        }
        bool isLeft = isVectorsLeft(mouseDirectionCurr, mouseDirections[prvIndex]);
        if(isSpiningLeft != isLeft) {
            isSpiningLeft = isLeft;
            mouseSpiningSpeed = 0;
        } else {
            mouseSpiningSpeed += Vector2.SqrMagnitude(mouseDirections[prvIndex]);
        }
        mouseSpiningSpeed = Mathf.Max(mouseSpiningSpeed, 200);
    }*/

    private float MouseSpinningSpeed(int _sampleCount) {
        int sampleCount = Mathf.Min(_sampleCount, mouseDirections.Count);
        float speed = 0;
        for(int i = 0; i < sampleCount; i++) {
            int index = mouseDirectionsIndex - i;
            if(index < 0) {
                index += mouseDirections.Count;
            }
            int indexPrv = index - 1;
            if(indexPrv < 0) {
                indexPrv += mouseDirections.Count;
            }
            bool isLeft = IsVectorsLeft(mouseDirections[index], mouseDirections[indexPrv]);
            float dist = Vector2.SqrMagnitude(mouseDirections[index]);
            float dot = Vector2.Dot(mouseDirections[index].normalized, mouseDirections[indexPrv].normalized);
            dot = dot * 0.5f + 0.5f;
            if(isSpiningLeft != isLeft) {
                isSpiningLeft = isLeft;
                speed -= dot*dist;
            } else {
                speed += dot*dist;
            }
        }
        return (isSpiningLeft) ? speed : -speed;
    }
    
    private bool IsVectorsLeft(Vector2 A, Vector2 B) {
        return (-A.x * B.y + A.y * B.x) < 0;
    }

    #endregion
}
