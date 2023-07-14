using System.Collections;
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
    private SignalBus signalBus;


    [Inject]
    public void Construct(DiContainer diContainer, GameSettings gameSettings, ObjectsManager objectsManager, SignalBus signalBus)
    {
        this.diContainer = diContainer;
        this.gameSettings = gameSettings;
        this.objectsManager = objectsManager;
        this.signalBus = signalBus;

        signalBus.Subscribe<StartNewGameSignal>(SpawnWithDelay);
        signalBus.Subscribe<CanSpawnNewSignal>(SpawnWithDelay);
    }

    private void SpawnWithDelay()
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
            signalBus.Fire<EndGameSignal>();
        }
    }

    private void SpawnNewObject()
    {
        var newCube = diContainer.InstantiatePrefab(cubePrefab, gameSettings.GarbageTransform());
        CubeController newCubeController = newCube.GetComponent<CubeController>();

        signalBus.Fire(new ObjectSpawnedSignal() { throwingObject = newCubeController } );

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

    private void OnDestroy()
    {
        signalBus.TryUnsubscribe<StartNewGameSignal>(SpawnWithDelay);
        signalBus.TryUnsubscribe<CanSpawnNewSignal>(SpawnWithDelay);
    }
}

