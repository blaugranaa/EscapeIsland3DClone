using UnityEngine;

namespace Systems.PoolingSystem
{
    public class PoolObject : MonoBehaviour
    {
        private void Start()
        {
            EventManager.AddListener(GameEvent.OnLevelChanged, HandleLevelFinishEvent);
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener(GameEvent.OnLevelChanged, HandleLevelFinishEvent);
        }

        private void HandleLevelFinishEvent()
        {
            EventManager.RemoveListener(GameEvent.OnLevelChanged, HandleLevelFinishEvent);
        }
    }
}

