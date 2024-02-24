using System.Collections;
using UnityEngine;

public class LightFlickering : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 5.0f)] private float _timeBetweenRoomFlickers = 5.0f;
    [SerializeField] [Range(0, 100)] private int _flickerChance = 25;
    [SerializeField] private float _flickerSpeed = 0.1f; // Used for lower bound for randomization

    private int _flickerCount = 3; // Used for lower bound for randomization 
    private Light[] _lights;
    private Coroutine _flickerLightsCoroutine;
    

    private void Awake()
    {
        _lights = GetComponentsInChildren<Light>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        if (_flickerLightsCoroutine != null)
            return;
        _flickerLightsCoroutine = StartCoroutine(FlickerLights(_lights));
    }

    private IEnumerator FlickerLights(Light[] lights)
    {
        foreach (Light light in lights)
        {
            if (!light.enabled)
                continue;
            if (Random.Range(0, 101) < _flickerChance)
                StartCoroutine(FlickerLight(light));
        }
        yield return new WaitForSeconds(_timeBetweenRoomFlickers);
        _flickerLightsCoroutine = null;
    }

    private IEnumerator FlickerLight(Light light)
    {
        for (int i = 0; i < Random.Range(_flickerCount, _flickerCount + 3) * 2; i++) // 3 - 5 flickers
        {
            light.enabled = !light.enabled;
            yield return new WaitForSeconds(Random.Range(_flickerSpeed, _flickerSpeed + 0.1f));
        }
    }
}
