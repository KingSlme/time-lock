using System.Collections.Generic;
using UnityEngine;

public class LightRandomizer : MonoBehaviour
{
    private List<Light> _lights = new List<Light>();
    [SerializeField] private int _minActiveLights;
    [SerializeField] [Range(0, 100)] private int _inactiveLightChance = 50;

    private void Awake()
    {
        _lights.AddRange(GetComponentsInChildren<Light>());
    }

    private void Start()
    {   
        // Remove lights to prevent them from being considered for randomization
        RemoveRandomLights(_lights, _minActiveLights);
        DisableRandomLights(_lights, _inactiveLightChance);
    }

    private void RemoveRandomLights(List<Light> lights, int numToRemove)
    {
        numToRemove = Mathf.Clamp(numToRemove, 0, _lights.Count);
        for (int i = 0; i < numToRemove; i++)
        {
            int index = Random.Range(0, lights.Count);
            lights.RemoveAt(index);
        }
    }

    private void DisableRandomLights(List<Light> lights, int chanceToDisable)
    {
        foreach (Light light in lights)
            if (Random.Range(0, 101) < chanceToDisable)
                light.enabled = false;
    }
}
