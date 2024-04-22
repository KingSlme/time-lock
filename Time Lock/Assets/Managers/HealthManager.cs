public class HealthManager : Singleton<HealthManager>
{
    private float _maxHealth = 5;
    private float _currentHealth;

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
