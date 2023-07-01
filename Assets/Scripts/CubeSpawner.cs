using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{

    [Range(0, 100), Tooltip("Po2 / Po4 drop chance")]
    [SerializeField] private int dropChance = 75;
    [SerializeField] private GameObject cubePrefab;


    private void Awake()
    {
        EventBus.onStartNewGame += SpawnDelay;       
        EventBus.onObjectLaunched += SpawnDelay;
    }

    private void SpawnDelay()
    {
        if(ObjectsManager.Instance.ActiveObjectsCount() < GameSettings.Instance.MaxObjectsAmount())
        {
            StartCoroutine(Delay(.25f));
        }
        else
        {
            StartCoroutine(Delay(1.25f));
        }
    }

    private IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(ObjectsManager.Instance.ActiveObjectsCount() < GameSettings.Instance.MaxObjectsAmount())
        {
            SpawnNewObject();
        }
        else
        {
            EventBus.onEndGame?.Invoke();
        }
    }

    private void SpawnNewObject()
    {

        GameObject newCube = Instantiate(cubePrefab, GameSettings.Instance.GarbageTransform());
        CubeController newCubeController = newCube.GetComponent<CubeController>();

        EventBus.onObjectSpawned?.Invoke(newCubeController);

        int randValue = Random.Range(0, 100);

        if (randValue <= dropChance)
        {
            //set to Power 2
            newCubeController.SetNewPower(0);
        }
        else
        {
            //set to Power 4
            newCubeController.SetNewPower(1);
        }
    }
}

