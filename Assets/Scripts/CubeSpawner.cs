using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{

    [Range(0, 100), Tooltip("Po2 / Po4 drop chance")]
    public int dropChance = 75;
    public GameObject cubePrefab;

    private InputHandler inputHandler;

    private void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        inputHandler.CubeSpawner = this;
    }

    public void SpawnNewCube()
    {
        if (GameManager.Instance.cubesAmountLeft < 0)
            return;


        GameObject newCube = Instantiate(cubePrefab);
        CubeController newCubeController = newCube.GetComponent<CubeController>();

        inputHandler.CubeController = newCubeController;

        int randValue = Random.Range(0, 100);

        if (randValue <= dropChance) //then set to Po2
        {
            newCubeController.SetNewPower(0); //set new power by index in powerCubes list
        }
        else //set to Po4
        {
            newCubeController.SetNewPower(1); //set new power by index
        }
    }

}

