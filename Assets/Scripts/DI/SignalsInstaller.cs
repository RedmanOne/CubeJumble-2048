using UnityEngine;
using Zenject;

public class SignalsInstaller : MonoInstaller
{

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        GameplayManagementSignals();
        ObjectsManagementSignals();
    }

    private void GameplayManagementSignals()
    {
        Container.DeclareSignal<StartNewGameSignal>()
            .OptionalSubscriberWithWarning();

        Container.DeclareSignal<EndGameSignal>()
            .OptionalSubscriberWithWarning();
    }

    private void ObjectsManagementSignals()
    {
        Container.DeclareSignal<CanSpawnNewSignal>()
            .OptionalSubscriberWithWarning();

        Container.DeclareSignal<ObjectSpawnedSignal>()
            .RequireSubscriber();

        Container.DeclareSignal<ObjectMergeSignal>()
            .OptionalSubscriberWithWarning();

        Container.DeclareSignal<ObjectsCountChangedSignal>()
            .OptionalSubscriber();
    }
}

// Signals
public class StartNewGameSignal { }
public class EndGameSignal { }

public class CanSpawnNewSignal { }
public class ObjectSpawnedSignal { public IThrowingObject throwingObject;}
public class ObjectMergeSignal { public int power;}
public class ObjectsCountChangedSignal { public int count;}