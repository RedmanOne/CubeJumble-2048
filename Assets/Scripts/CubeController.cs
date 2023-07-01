using System;
using UnityEngine;


[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(BoxCollider))]
public class CubeController : MonoBehaviour, IThrowingObject
{
    private int myPowerIndex;
    private int power;

    private float dragSensitivity = 7f;
    private Vector3 clampedPosition; //used to define a borders of cube`s movement
    private bool readyToLaunch = false; //used to reset cube`s transforms before launch
    private Animator animator;
    private Rigidbody rgBody;
    private BoxCollider boxCollider;


    private void Start()
    {
        ObjectsManager.Instance.AddActiveObject(this);

        clampedPosition = new Vector3(0, 0.2f, 0.2f);
    }

    // Merging
    private void OnCollisionEnter(Collision collision)
    {
        if (transform.position.z < 0.5f) //don`t merge if near start position
            return;

        if (collision.gameObject.tag == "PowerCube")
        {
            CubeController touchedOne = collision.gameObject.GetComponent<CubeController>();

            float dotProduct = Vector3.Dot(rgBody.velocity.normalized, touchedOne.rgBody.velocity.normalized);
            float collisionImpulse = rgBody.velocity.magnitude - touchedOne.rgBody.velocity.magnitude * dotProduct;

            if (rgBody.velocity.magnitude < touchedOne.rgBody.velocity.magnitude)
                return; // only 1 of 2 colliders will execute merging

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

        ObjectsManager.Instance.GetPowerByIndex(index, out newPower, out newTexture);

        myPowerIndex = index;
        GetComponent<MeshRenderer>().material.mainTexture = newTexture; //set texture of the next power
        power = newPower;
    }

    public void Move(float dragVelocity)
    {
        if (!readyToLaunch) 
        {
            PrepareToLaunch();
        }

        //Movement
        float moveDistance = dragVelocity * dragSensitivity;

        transform.Translate(moveDistance, 0, 0, Space.World);
        clampedPosition.x = Mathf.Clamp(transform.position.x, -0.65f, 0.65f); //keep object within the borders
        transform.position = clampedPosition;
    }

    public void Throw()
    {
        PrepareToLaunch();
        rgBody = GetComponent<Rigidbody>();
        rgBody.isKinematic = false;
        rgBody.AddForce(Vector3.forward * 15f, ForceMode.Impulse);
        EventBus.onObjectLaunched?.Invoke();
    }

    private void PrepareToLaunch()
    {
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<Animator>().enabled = false;
        transform.rotation = new Quaternion();
        transform.localScale = new Vector3(1, 1, 1);
        readyToLaunch = true;
    }

    private void OnDestroy()
    {
        ObjectsManager.Instance.RemoveActiveObject(this);
    }
}
