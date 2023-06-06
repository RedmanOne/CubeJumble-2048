using UnityEngine.EventSystems;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public CubeController CubeController { get; set; } //controller script on current cube
    public CubeSpawner CubeSpawner { get; set; }

    private Vector3 inputPosition, lastInputPosition;
    private float velocity;
    private bool runningOnMobile = false;
    private KeyCode movementInput = KeyCode.Mouse0; //input for desktop/editor
    private bool touchIsOverUI = false; //used because TouchInput doesn`t work correctly with IsPointerOverGameObject()

    private void Awake()
    {
        //setting controls depending on platform
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            runningOnMobile = true;
    }

    private void Update()
    {
        if (!CubeController && !GameManager.Instance.roundIsOver) //disable the controls if there is no current cube
            return;
        runningOnMobile = true;
        if (runningOnMobile)
        {
            TouchControls();
        }
        else //running in editor or desktop
        {
            MouseControls();
        }
    }


    private void TouchControls()
    {
        if (Input.touchCount < 1) //break the function if there is no touch
            return;

        Touch touch = Input.GetTouch(0);
        inputPosition = touch.position;

        if (touch.phase == TouchPhase.Began) //finger down
        {
            touchIsOverUI = false;

            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                touchIsOverUI = true;

            if(CubeController)
                CubeController.Move(0);
            lastInputPosition = touch.position;
        }
        else if(touch.phase == TouchPhase.Moved && CubeController) //finger move
        {
            velocity = (inputPosition.x - lastInputPosition.x) / Screen.width; //making velocity equal for any screen width
            CubeController.Move(velocity);
            lastInputPosition = inputPosition;
        }
        else if(touch.phase == TouchPhase.Ended && !touchIsOverUI && CubeController) //finger up
        {
            CubeController.Launch();
            CubeController = null;
            CubeSpawner.SpawnNewCube();
        }
    }


    private void MouseControls()
    {
        if (EventSystem.current.IsPointerOverGameObject()) //break the function if mouse is over UI elements
            return;

        inputPosition = Input.mousePosition;

        if (Input.GetKeyDown(movementInput)) //mouse down
        {
            lastInputPosition = Input.mousePosition;
        }
        else if (Input.GetKey(movementInput)) //mouse hold - moving the cube
        {
            velocity = (inputPosition.x - lastInputPosition.x) / Screen.width; //making velocity equal for any screen width
            CubeController.Move(velocity);
            lastInputPosition = inputPosition;
        }
        else if(Input.GetKeyUp(movementInput)) //mouse up 
        {
            CubeController.Launch();
            CubeController = null;
            CubeSpawner.SpawnNewCube();
        }
    }

}
