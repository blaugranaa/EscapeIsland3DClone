using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    internal bool isLevelStarted;
    internal int level;
    private int _numberOfLevels;
    int textIndex;
    public TextMeshProUGUI LevelText;
    private void Start()
    {
        textIndex = PlayerPrefs.GetInt("TextIndex");

        if (LevelText ==null)
        {
            return;
        }
        LevelText.text = "LEVEL " + textIndex;
    }

    private void OnEnable()
    {
        EventManager.AddListener(GameEvent.OnLevelEnd,OnLevelEnd);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener(GameEvent.OnLevelEnd,OnLevelEnd);


    }

    void OnLevelEnd()
    {
        PoolingSystem.Instance.InstantiateAPS("finalConfetti", transform.position);
    }
    

    public void LoadNextLevel()
    {
     EventManager.Broadcast(GameEvent.OnLevelChanged);
     
    }
    
    public void LoadPreviousLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        int previousSceneIndex = (currentSceneIndex - 1 + SceneManager.sceneCountInBuildSettings) % SceneManager.sceneCountInBuildSettings;

        if (previousSceneIndex == SceneManager.sceneCountInBuildSettings - 1 && currentSceneIndex == 0)
        {
            SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
        }
        else
        {
            SceneManager.LoadScene(previousSceneIndex);
        }
    }

}