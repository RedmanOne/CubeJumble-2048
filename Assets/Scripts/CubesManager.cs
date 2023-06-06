using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesManager : MonoBehaviour
{
    //initialize singleton
    public static CubesManager Instance { get; set; }

    private void Awake()
    {
        //check if there is another instance
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [System.Serializable]
    public struct CubeProperties
    {
        public int power;
        public Texture powerTexture;
    }
    [Tooltip("Set the PowerCubes types and properties")]
    public List<CubeProperties> powerCubes = new List<CubeProperties>(); //list of cube types and properties
    [HideInInspector]
    public List<CubeController> activeCubes = new List<CubeController>(); //used to destroy cubes in scene on restart


    public void ClearTheScene()//destroys all active cubes
    {
        if (activeCubes.Count == 0)
            return;

        for(int i = 0; i < activeCubes.Count; i++)
        {
            GameObject cube = activeCubes[i].gameObject;
            Destroy(cube);
        }
    }

}
