public class HealthManager : Singleton<HealthManager>
{
    private float _maxHealth = 5.0f;
    private float _currentHealth = 5.0f;

    private void Start()
    {
        ResetHealth();
    }

    public void Damage(float amount)
    {
        _currentHealth -= amount;
    }

    public float GetMaxHealth()
    {
        return _maxHealth;
    }

    public float GetCurrentHealth()
    {
        return _currentHealth;
    }

    public float GetMissingHealthPercent()
    {
        return 100 - (_currentHealth / _maxHealth * 100);
    }

    public void ResetHealth()
    {
        _currentHealth = _maxHealth;
    }
}
