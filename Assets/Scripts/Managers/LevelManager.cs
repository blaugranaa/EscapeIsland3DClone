using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.AddListener(GameEvent.OnLevelEnd,OnLevelEnd);
        EventManager.AddListener(GameEvent.OnLevelChanged,LoadNextLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener(GameEvent.OnLevelEnd,OnLevelEnd);
        EventManager.RemoveListener(GameEvent.OnLevelChanged,LoadNextLevel);

    }

    void OnLevelEnd()
    {
        PoolingSystem.Instance.InstantiateAPS("finalConfetti", transform.position);
    }
    

    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;

        if (nextSceneIndex == 0 && currentSceneIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

}