using UnityEngine;

public class LevelBoundaries : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IThrowingObject throwingObject))
        {
            Destroy(other.gameObject);
        }
    }
}
