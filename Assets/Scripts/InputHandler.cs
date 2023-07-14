using UnityEngine.EventSystems;
using UnityEngine;
using Zenject;

public class InputHandler : MonoBehaviour
{

    private IThrowingObject throwingObject; //current controller
    private SignalBus signalBus;
    private Vector3 inputPosition, lastInputPosition;
    private float velocity;
    private bool runningOnMobile = false;


    [Inject]
    public void Construct(SignalBus signalBus)
    {
        this.signalBus = signalBus;
        signalBus.Subscribe<ObjectSpawnedSignal>(x => SetNewThrowingObject(x.throwingObject));
    }

    private void Awake()
    {
        if(SystemInfo.deviceType == DeviceType.Handheld)
        {
            runningOnMobile = true;
        }
        else
        {
            runningOnMobile = false;
        }
    }

    private void Update()
    {
        if (throwingObject == null)
            return;

        if (runningOnMobile)
        {
            TouchControls();
        }
        else
        {
            MouseControls();
        }
    }

    private void SetNewThrowingObject(IThrowingObject obj)
    {
        throwingObject = obj;
    }


    private void TouchControls()
    {
        if (Input.touchCount <= 0)
            return;   

        Touch touch = Input.GetTouch(0);
        inputPosition = touch.position;

        if (touch.phase == TouchPhase.Began)
        {
            throwingObject.Move(0);
            lastInputPosition = touch.position;
        }
        else if (touch.phase == TouchPhase.Moved)
        {

            velocity = (inputPosition.x - lastInputPosition.x) / Screen.width;
            throwingObject.Move(velocity);
            lastInputPosition = inputPosition;
        }
        else if(touch.phase == TouchPhase.Ended)
        {

            throwingObject.Throw();
            throwingObject = null;
        }
    }


    private void MouseControls()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        inputPosition = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            throwingObject.Move(0);
            lastInputPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {

            velocity = (inputPosition.x - lastInputPosition.x) / Screen.width;
            throwingObject.Move(velocity);
            lastInputPosition = inputPosition;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            throwingObject.Throw();
            throwingObject = null;
        }
    }

    private void OnDestroy()
    {
        signalBus.TryUnsubscribe<ObjectSpawnedSignal>(x => SetNewThrowingObject(x.throwingObject));
    }

}
