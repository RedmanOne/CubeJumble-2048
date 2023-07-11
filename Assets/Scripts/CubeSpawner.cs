using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CubeSpawner : MonoBehaviour
{

    [Range(0, 100), Tooltip("Po2 / Po4 drop chance")]
    [SerializeField] private int dropChance = 75;
    [SerializeField] private GameObject cubePrefab;

    private DiContainer diContainer;
    private GameSettings gameSettings;
    private ObjectsManager objectsManager;


    [Inject]
    public void Construct(DiContainer diContainer, GameSettings gameSettings, ObjectsManager objectsManager)
    {
        this.diContainer = diContainer;
        this.gameSettings = gameSettings;
        this.objectsManager = objectsManager;
    }

    private void Awake()
    {
        EventBus.onStartNewGame += SpawnDelay;       
        EventBus.onObjectLaunched += SpawnDelay;
    }

    private void SpawnDelay()
    {
        if(objectsManager.ActiveObjectsCount() < gameSettings.MaxObjectsAmount())
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
        if(objectsManager.ActiveObjectsCount() < gameSettings.MaxObjectsAmount())
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
        var newCube = diContainer.InstantiatePrefab(cubePrefab, gameSettings.GarbageTransform());
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

