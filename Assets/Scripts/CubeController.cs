using UnityEngine;
using Zenject;

[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(BoxCollider))]
public class CubeController : MonoBehaviour, IThrowingObject
{
    private int myPowerIndex;
    private int myPower;
    private float dragSensitivity = 7f;
    private Vector3 clampedPosition; //used to define a borders of cube`s movement
    private bool readyToLaunch = false; //used to reset cube`s transforms before launch

    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rgBody;
    [SerializeField] private BoxCollider boxCollider;

    private ObjectsManager objectsManager;
    private SignalBus signalBus;

    [Inject]
    public void Construct(ObjectsManager objectsManager, SignalBus signalBus)
    {
        this.objectsManager = objectsManager;
        this.signalBus = signalBus;
    }

    private void Start()
    {
        objectsManager.AddActiveObject(this);
        clampedPosition = new Vector3(0, 0.2f, 0.2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (transform.position.z < 0.5f)
            return;

        if (collision.gameObject.tag == "PowerCube")
        {
            CubeController touchedOne = collision.gameObject.GetComponent<CubeController>();

            float dotProduct = Vector3.Dot(rgBody.velocity.normalized, touchedOne.rgBody.velocity.normalized);
            float collisionImpulse = rgBody.velocity.magnitude - touchedOne.rgBody.velocity.magnitude * dotProduct;

            if (rgBody.velocity.magnitude < touchedOne.rgBody.velocity.magnitude)
                return;

            if (touchedOne.myPower == myPower && collisionImpulse > .1f)
            {
                Destroy(touchedOne.gameObject);
                signalBus.Fire(new ObjectMergeSignal() { power = myPower});
                SetNewPower(myPowerIndex + 1);
            }
        }
    }

    public void SetNewPower(int index)
    {
        int newPower;
        Texture newTexture;

        objectsManager.GetPowerByIndex(index, out newPower, out newTexture);

        myPowerIndex = index;
        GetComponent<MeshRenderer>().material.mainTexture = newTexture;
        myPower = newPower;
    }

    public void Move(float dragVelocity)
    {
        if (!readyToLaunch) 
        {
            PrepareToLaunch();
        }

        float moveDistance = dragVelocity * dragSensitivity;

        transform.Translate(moveDistance, 0, 0, Space.World);
        clampedPosition.x = Mathf.Clamp(transform.position.x, -0.65f, 0.65f);
        transform.position = clampedPosition;
    }

    public void Throw()
    {
        PrepareToLaunch();
        rgBody.isKinematic = false;
        rgBody.AddForce(Vector3.forward * 15f, ForceMode.Impulse);
        signalBus.Fire<CanSpawnNewSignal>();
    }

    private void PrepareToLaunch()
    {
        boxCollider.enabled = true;
        animator.enabled = false;
        transform.rotation = new Quaternion();
        transform.localScale = new Vector3(1, 1, 1);
        readyToLaunch = true;
    }

    private void OnDestroy()
    {
        objectsManager.RemoveActiveObject(this);
    }
}
