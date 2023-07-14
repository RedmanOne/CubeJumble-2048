using UnityEngine;
using Zenject;

public class GameSettings : MonoBehaviour
{

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private SignalBus signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        this.signalBus = signalBus;
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
            signalBus.Fire<EndGameSignal>();
            return;
        }
        _runtimeGarbage = Instantiate(new GameObject("RuntimeGarbage"), Vector3.zero, Quaternion.identity).transform;
        signalBus.Fire<StartNewGameSignal>();
    }
}
