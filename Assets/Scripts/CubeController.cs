using System;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(BoxCollider))]
public class CubeController : MonoBehaviour, IThrowingObject
{
    private int myPowerIndex;
    private int power;
    private float dragSensitivity = 7f;
    private Vector3 clampedPosition; //used to define a borders of cube`s movement
    private bool readyToLaunch = false; //used to reset cube`s transforms before launch

    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rgBody;
    [SerializeField] private BoxCollider boxCollider;
    private ObjectsManager ObjectsManager;

    [Inject]
    public void Construct(ObjectsManager objectsManager)
    {
        ObjectsManager = objectsManager;
        ObjectsManager.AddActiveObject(this);

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

            if (touchedOne.power == power && collisionImpulse > .1f)
            {
                Destroy(touchedOne.gameObject);
                EventBus.onObjectMerge?.Invoke(power);
                SetNewPower(myPowerIndex + 1);
            }
        }
    }

    public void SetNewPower(int index)
    {
        int newPower;
        Texture newTexture;

        ObjectsManager.GetPowerByIndex(index, out newPower, out newTexture);

        myPowerIndex = index;
        GetComponent<MeshRenderer>().material.mainTexture = newTexture;
        power = newPower;
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
        EventBus.onObjectLaunched?.Invoke();
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
        ObjectsManager.RemoveActiveObject(this);
    }
}
