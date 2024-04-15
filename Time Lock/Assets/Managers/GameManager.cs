using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private void Start()
    {
        LogManager.Instance.Log("Game instance started");
    }

    private void OnApplicationQuit()
    {
        LogManager.Instance.Log("Game instance closed\n");
    }
}
