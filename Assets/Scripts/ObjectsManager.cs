using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObjectsManager : MonoBehaviour
{

    private GameSettings gameSettings;
    private SignalBus signalBus;

    [Inject]
    public void Construct(GameSettings gameSettings, SignalBus signalBus)
    {
        this.gameSettings = gameSettings;
        this.signalBus = signalBus;

        signalBus.Subscribe<EndGameSignal>(ClearTheScene);
    }


    [System.Serializable]
    private class CubeProperties
    {
        public int power;
        public Texture powerTexture;
    }
    [SerializeField, Tooltip("Set the PowerCubes types and properties")]
    private List<CubeProperties> powerObjects = new List<CubeProperties>(); //list of cube types and properties
    private List<IThrowingObject> activeObjects = new List<IThrowingObject>(); //used to destroy objects in scene on restart


    public void AddActiveObject(IThrowingObject newObj)
    {
        activeObjects.Add(newObj);

        signalBus.Fire(new ObjectsCountChangedSignal() { count = ActiveObjectsCount() });
    }
    public void RemoveActiveObject(IThrowingObject obj)
    {
        activeObjects.Remove(obj);

        signalBus.Fire(new ObjectsCountChangedSignal() { count = ActiveObjectsCount() });
    }

    public int ActiveObjectsCount()
    {
        return activeObjects.Count;
    }

    public void GetPowerByIndex(int index, out int power, out Texture powerTexture)
    {
        power = powerObjects[index].power;
        powerTexture = powerObjects[index].powerTexture;
    }


    private void ClearTheScene()//destroys all active cubes
    {
        if (activeObjects.Count == 0)
            return;

        activeObjects.Clear();

        Destroy(gameSettings.GarbageTransform().gameObject);
    }

    private void OnDestroy()
    {
        signalBus.TryUnsubscribe<EndGameSignal>(ClearTheScene);
    }

}
