using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    //initialize singleton
    public static GameSettings Instance { get; set; }
    private void Awake()
    {
        Application.targetFrameRate = 60;
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

    private void Start()
    {
        StartNewGame();
    }

    private Transform _runtimeGarbage; //used to hold runtime-spawned objects

    public Transform GarbageTransform()
    {
        return _runtimeGarbage;
    }

    [SerializeField, Tooltip("Objects amount limit for GameOver")]
    private int _maxObjectsAmount;

    public int MaxObjectsAmount()
    {
        return _maxObjectsAmount;
    }

    public void SetMaxObjectsAmount(int amount)
    {
        _maxObjectsAmount = amount;
    }

    public void StartNewGame()
    {
        if (_runtimeGarbage != null)
        {
            EventBus.onEndGame?.Invoke();
            return;
        }
        _runtimeGarbage = Instantiate(new GameObject("RuntimeGarbage"), Vector3.zero, Quaternion.identity).transform;
        EventBus.onStartNewGame?.Invoke();
    }


}
