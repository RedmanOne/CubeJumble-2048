using System;
using System.Collections;
using UnityEngine;

// for such simple projects i decided this static action-holder class could be suitable to use
static class EventBus
{
    // General events
    public static Action onStartNewGame;
    public static Action onEndGame;

    public static Action<IThrowingObject> onObjectSpawned;
    public static Action onObjectLaunched;
    public static Action<int> onObjectMerge;
    public static Action<int> onObjectsCountChanged;
}
