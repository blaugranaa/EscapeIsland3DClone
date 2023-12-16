using System;
using System.Collections.Generic;


public enum GameEvent
{
    OnGameStart,
    OnNextLevel,
    OnReloadLevel,
    OnLevelFinish,
    OnMoneyCollect,
    OnLevelDataChange,
    LevelFail,
    OnPlayerDataChange,
    OnStickmanMoved,
    OnLevelEnd,
    OnLevelChanged
}

public static class EventManager
{
    private static Dictionary<GameEvent, Action> _events = new();
    private static Dictionary<GameEvent, Action<int>> _rankevents = new();

    public static void AddListener(GameEvent gameEvent, Action action)
    {
        if (!_events.ContainsKey(gameEvent))
            _events[gameEvent] = action;
        else
            _events[gameEvent] += action;
    }

    public static void RemoveListener(GameEvent gameEvent, Action action)
    {
        if (_events.ContainsKey(gameEvent))
        {
            _events[gameEvent] -= action;
            if (_events[gameEvent] == null)
                _events.Remove(gameEvent);
        }
    }

    public static void AddListenerRank(GameEvent gameEvent, Action<int> action)
    {
        if (!_rankevents.ContainsKey(gameEvent))
            _rankevents[gameEvent] = action;
        else
            _rankevents[gameEvent] += action;
    }

    public static void RemoveListenerRank(GameEvent gameEvent, Action<int> action)
    {
        if (_rankevents.ContainsKey(gameEvent))
        {
            _rankevents[gameEvent] -= action;
            if (_rankevents[gameEvent] == null)
                _rankevents.Remove(gameEvent);
        }
    }

    public static void Broadcast(GameEvent gameEvent)
    {
        if (!_events.TryGetValue(gameEvent, out var @event)) return;
        if (@event != null)
            @event.Invoke();
    }

    public static void BroadcastRank(GameEvent gameEvent, int playerRank)
    {
        if (!_events.TryGetValue(gameEvent, out var @event)) return;
        if (@event != null)
            @event.Invoke();
    }
}