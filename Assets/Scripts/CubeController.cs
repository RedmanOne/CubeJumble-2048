
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public int myPowerIndex { get; set; }
    public int power { get; set; }

    private float dragSensitivity = 10;
    private Vector3 clampedPosition; //used to define a borders of cube`s movement
    private bool readyToLaunch = false; //used to reset cube`s transforms before launch
    public Animator animator;
    public Rigidbody rgBody;
    public BoxCollider boxCllider;


    private void Start()
    {
        CubesManager.Instance.activeCubes.Add(this);
        dragSensitivity = GameManager.Instance.dragSensitivitySetting;
        clampedPosition = new Vector3(0, 0.2f, 0.2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (transform.position.z < 0.5f) //don`t merge if close to start position
            return;

        if (collision.gameObject.tag == "PowerCube")
        {
            CubeController touchedOne = collision.gameObject.GetComponent<CubeController>();

            Vector3 direction = touchedOne.transform.position - transform.position;
            Vector3 velocityToTouched = Vector3.Project(rgBody.velocity, direction);

            if (touchedOne.power == power && velocityToTouched.magnitude > .35f)
            {
                Debug.Log(velocityToTouched.magnitude);
                Destroy(touchedOne.gameObject);
                SetNewPower(myPowerIndex + 1);
                GameManager.Instance.ScoreUpdate(power/2);
            }
        }
    }

    public void SetNewPower(int newPowerIndex)
    {
        myPowerIndex = newPowerIndex;
        GetComponent<MeshRenderer>().material.mainTexture = CubesManager.Instance.powerCubes[newPowerIndex].powerTexture; //set texture of the next power
        power = CubesManager.Instance.powerCubes[newPowerIndex].power;
    }


    public void Move(float dragVelocity)
    {
        if (!readyToLaunch)
        {
            animator.enabled = false;
            transform.rotation = new Quaternion();
            transform.localScale = new Vector3(1,1,1);
            readyToLaunch = true;
        }

        float moveDistance = dragVelocity * dragSensitivity;

        //Movement
        transform.Translate(moveDistance, 0, 0, Space.World);
        clampedPosition.x = Mathf.Clamp(transform.position.x, -0.65f, 0.65f); //keep object within the borders
        transform.position = clampedPosition;
    }

    public void Launch()
    {
        boxCllider.enabled = true;
        rgBody.isKinematic = false;
        rgBody.AddForce(Vector3.forward * GameManager.Instance.launchForce, ForceMode.Impulse);
    }

    private void OnDestroy()
    {
        CubesManager.Instance.activeCubes.Remove(this);
    }
}
