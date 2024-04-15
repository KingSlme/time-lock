using TMPro;

public class HealthManager : Singleton<HealthManager>
{
    private int _maxHealth = 10;
    private int _currentHealth;

    private TextMeshProUGUI _healthText;

    protected override void Awake()
    {
        base.Awake();
        _healthText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        ResetHealth();
    }

    private void Update()
    {
        _healthText.text = $"{_currentHealth}/{_maxHealth}";
        if (_currentHealth <= 0)
        {
            GameManager.Instance.RestartGame();
        }
    }

    public void Damage(int amount)
    {
        _currentHealth -= amount;
    }

    public void ResetHealth()
    {
        _currentHealth = _maxHealth;
    }
}
