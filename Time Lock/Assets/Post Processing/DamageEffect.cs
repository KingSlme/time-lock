using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DamageEffect : MonoBehaviour
{
    private Volume _volume;
    private Vignette _vignette;

    private void Awake()
    {
        _volume = GetComponent<Volume>();
    }

    private void Start()
    {
        _volume.profile.TryGet(out _vignette);
    }

    private void Update()
    {
        if (HealthManager.Instance.GetMissingHealthPercent() > 0.0f)
            SetVignette(GetVignetteValue());
    }

    private float GetVignetteValue()
    {
        return (HealthManager.Instance.GetMissingHealthPercent() / 20.0f) * 0.1f + 0.1f;
    }

    private void SetVignette(float intensity)
    {
        _vignette.color.value = Color.red;
        _vignette.intensity.value = intensity;
    }
}
