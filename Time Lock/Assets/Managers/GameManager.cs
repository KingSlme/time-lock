using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

/// <summary>
/// Ensure Start scene has build index of '0'
/// </summary>
public class GameManager : Singleton<GameManager>
{
    private List<int> _sceneIndices;
    private int _lastSceneIndex; 

    private void Start()
    {
        LogManager.Instance.Log("Game instance started");
        PopulateSceneIndices();
        GoToRandomScene();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GoToRandomScene();
        }
    }

    private void OnApplicationQuit()
    {
        LogManager.Instance.Log("Game instance closed\n");
    }

    private void GoToRandomScene()
    {   
        int randomListIndex = Random.Range(0, _sceneIndices.Count);
        
        if (_sceneIndices.Count > 0)
        {
            int randomSceneIndex = _sceneIndices[randomListIndex]; 
            if (randomSceneIndex != _lastSceneIndex)
            {
                _sceneIndices.RemoveAt(randomListIndex);
                LogManager.Instance.Log($"Entering scene {randomSceneIndex}");
                SceneManager.LoadScene(randomSceneIndex);
                _lastSceneIndex = randomSceneIndex;
            }
            else
            {
                PopulateSceneIndices();
                GoToRandomScene();
            }
        }
        else
        {
            PopulateSceneIndices();
            GoToRandomScene();
        }
    }

    private void PopulateSceneIndices()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        int lastSceneIndex = sceneCount - 1;
        _sceneIndices = new List<int>(Enumerable.Range(1, lastSceneIndex)); // Don't include 'Start' scene index
    }
}
