using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiController : MonoBehaviour
{
    private void Start()
    {
        var main = GetComponent<ParticleSystem>().main;
        main.stopAction = ParticleSystemStopAction.Callback;

    }
    private void OnParticleSystemStopped()
    {
        EventManager.Broadcast(GameEvent.OnLevelChanged);
    }
}