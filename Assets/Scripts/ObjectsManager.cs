using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsManager : MonoBehaviour
{
    //initialize singleton
    public static ObjectsManager Instance { get; set; }

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

        EventBus.onEndGame += ClearTheScene;
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

        EventBus.onObjectsCountChanged?.Invoke(ActiveObjectsCount());
    }
    public void RemoveActiveObject(IThrowingObject obj)
    {
        activeObjects.Remove(obj);

        EventBus.onObjectsCountChanged?.Invoke(ActiveObjectsCount());
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

        Destroy(GameSettings.Instance.GarbageTransform().gameObject);
    }

}
