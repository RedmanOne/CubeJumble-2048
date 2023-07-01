using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBoundaries : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PowerCube")
        {
            Destroy(other.gameObject);
        }
    }
}
