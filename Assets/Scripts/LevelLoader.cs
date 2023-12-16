using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : Singleton<LevelLoader>
{
    public  int index;
    public int textIndex;

    private void Start()
    {
        LoadStarting();
    }

    private void OnEnable()
    {
        EventManager.AddListener(GameEvent.OnLevelChanged,Load);

    }

    private void OnDisable()
    {
        EventManager.RemoveListener(GameEvent.OnLevelChanged,Load);

      

    }


    private void Set()
    {
        PlayerPrefs.SetInt("Index", index);
        PlayerPrefs.SetInt("TextIndex", textIndex);
        PlayerPrefs.Save();
    }

    private void Get()
    {
        index = PlayerPrefs.GetInt("Index");
        textIndex = PlayerPrefs.GetInt("TextIndex");
    }

    public void Load()
    {
        Get();
        index += 1;
        textIndex += 1;

        if (index >= SceneManager.sceneCountInBuildSettings)
        {
            index = Random.Range(1, SceneManager.sceneCountInBuildSettings);
            Set();
        }
        else if (index == 0)
        {
            index = 1; 
            Set();
        }
        else
        {
            Set();
        }

        SceneManager.LoadScene(index);
    }

    public void Reload()
    {
        SceneManager.LoadScene(index);
    }

    private void LoadStarting()
    {
        Get();
        if (index == 0)
        {
            index = 1;
            textIndex = 1;
            Set();
        }
        SceneManager.LoadScene(index);

    }

}