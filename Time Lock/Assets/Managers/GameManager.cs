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

    protected override void Awake()
    {
        base.Awake();
        PopulateSceneIndices();
        LogManager.Instance.Log("Game instance started");
    }

    private void Update()
    {
        if (HealthManager.Instance.GetCurrentHealth() <= 0.0f)
            RestartGame();
    }

    private void OnApplicationQuit()
    {
        LogManager.Instance.Log("Game instance closed\n");
    }

    // private void GoToRandomScene()
    // {   
    //     int randomListIndex = Random.Range(0, _sceneIndices.Count);
        
    //     if (_sceneIndices.Count > 0)
    //     {
    //         int randomSceneIndex = _sceneIndices[randomListIndex]; 
    //         if (randomSceneIndex != _lastSceneIndex)
    //         {
    //             _sceneIndices.RemoveAt(randomListIndex);
    //             LogManager.Instance.Log($"Entering scene {randomSceneIndex}");
    //             SceneManager.LoadScene(randomSceneIndex);
    //             _lastSceneIndex = randomSceneIndex;
    //         }
    //         else
    //         {
    //             GoToRandomScene();
    //         }
    //     }
    //     else
    //     {
    //         PopulateSceneIndices();
    //         GoToRandomScene();
    //     }
    // }

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
                if (_sceneIndices.Count == 1)
                {
                    PopulateSceneIndices(); // Repopulate if only one scene left
                }
                else
                {
                    GoToRandomScene(); // Recursive call
                }
            }
        }
        else
        {
            PopulateSceneIndices(); // Repopulate if _sceneIndices is empty
            GoToRandomScene(); // Recursive call
        }
    }


    private void PopulateSceneIndices()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        int lastSceneIndex = sceneCount - 1 - 1; // Temp -1 to not include Win scene
        _sceneIndices = new List<int>(Enumerable.Range(1, lastSceneIndex)); // Don't include 'Start' scene index
    }

    public void RestartGame()
    {
        LogManager.Instance.Log("Restarting game");
        HealthManager.Instance.ResetHealth();
        // GoToRandomScene();
        // temp restart
        SceneManager.LoadScene("1");
    }

    public void WinGame()
    {
        LogManager.Instance.Log("Player has won");
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
    }
}
