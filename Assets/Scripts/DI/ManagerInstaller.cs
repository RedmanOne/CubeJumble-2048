using UnityEngine;
using Zenject;

public class ManagerInstaller : MonoInstaller
{
    [SerializeField] private ObjectsManager objectsManager;
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private UIManager uiManager;

    public override void InstallBindings()
    {
        BindGameSettings();
        BindObjectsManager();
        BindUIManager();
    }

    private void BindUIManager()
    {
        Container.Bind<UIManager>()
            .FromInstance(uiManager)
            .AsSingle()
            .NonLazy();
    }

    private void BindObjectsManager()
    {
        Container.Bind<ObjectsManager>()
            .FromInstance(objectsManager)
            .AsSingle()
            .NonLazy();
    }

    private void BindGameSettings()
    {
        Container.Bind<GameSettings>()
            .FromInstance(gameSettings)
            .AsSingle()
            .NonLazy();
    }
}