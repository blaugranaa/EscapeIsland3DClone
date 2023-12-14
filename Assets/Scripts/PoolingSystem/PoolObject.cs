using UnityEngine;

namespace Systems.PoolingSystem
{
    public class PoolObject : MonoBehaviour
    {
        private void Start()
        {
            EventManager.AddListener(GameEvent.OnLevelFinish, HandleLevelFinishEvent);
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener(GameEvent.OnLevelFinish, HandleLevelFinishEvent);
        }

        private void HandleLevelFinishEvent()
        {
            EventManager.RemoveListener(GameEvent.OnLevelFinish, HandleLevelFinishEvent);
        }
    }
}

