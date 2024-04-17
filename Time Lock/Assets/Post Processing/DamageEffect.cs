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
        if (HealthManager.Instance.GetCurrentHealth() < 5)
            SetVignette(GetMissingHealthPercent());
    }

    private float GetMissingHealthPercent()
    {
        // float currentHealth = HealthManager.Instance.GetCurrentHealth();
        // float maxHealth = HealthManager.Instance.GetMaxHealth();
    
        // float missingHealthPercent = (maxHealth - currentHealth) / maxHealth * 100.0f;
        // return missingHealthPercent;
        if (HealthManager.Instance.GetCurrentHealth() == 4.0f)
            return 0.1f;
        if (HealthManager.Instance.GetCurrentHealth() == 3.0f)
            return 0.2f;
        if (HealthManager.Instance.GetCurrentHealth() == 2.0f)
            return 0.3f;
        else
            return 0.4f;
    }

    private void SetVignette(float intensity)
    {
        _vignette.color.value = Color.red;
        _vignette.intensity.value = intensity;
    }
}
