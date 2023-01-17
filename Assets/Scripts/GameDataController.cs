using Abstracts;
using Scriptables;
using Scriptables.Health;
using UnityEngine;
using Utilities.ResourceManagement;

public class GameDataController : BaseController
{
    private const string RecordKey = "RecordCompletedLevels";

    private readonly HealthInfo _playerHealthInfo;
    private readonly ShieldInfo _playerShieldInfo;
    private readonly float _playerDefaultStartingHealth;
    private readonly float _playerDefaultStartingShield;
    
    private readonly ResourcePath _playerConfigPath = new(Constants.Configs.Player.PlayerConfig);

    public int CompletedLevels { get; private set; } = 0;
    public int RecordCompletedLevels { get; private set; } = 0;

    public HealthInfo PlayerHealthInfo => _playerHealthInfo;
    public ShieldInfo PlayerShieldInfo => _playerShieldInfo;

    public GameDataController()
    {
        RecordCompletedLevels = PlayerPrefs.GetInt(RecordKey);
        _playerHealthInfo = new(ResourceLoader.LoadObject<PlayerConfig>(_playerConfigPath).HealthConfig);
        _playerDefaultStartingHealth = _playerHealthInfo.StartingHealth;
        _playerShieldInfo = new(ResourceLoader.LoadObject<PlayerConfig>(_playerConfigPath).ShieldConfig);
        _playerDefaultStartingShield = _playerShieldInfo.StartingShield;
    }

    protected override void OnDispose()
    {
        ResetCompletedLevels();
        PlayerPrefs.SetInt(RecordKey, RecordCompletedLevels);
    }

    public void ResetCompletedLevels()
    {
        CompletedLevels = 0;
    }

    public void AddCompletedLevels()
    {
        CompletedLevels++;
    }

    public void UpdateRecord()
    {
        if (RecordCompletedLevels < CompletedLevels)
        {
            RecordCompletedLevels = CompletedLevels;
        }
    }

    public void ResetRecord()
    {
        RecordCompletedLevels = 0;
    }

    public void SetPlayerCurrentHealth(float health)
    {
        _playerHealthInfo.SetStartingHealth(health);
    }

    public void SetPlayerCurrentShield(float shield)
    {
        _playerShieldInfo.SetShieldAmount(shield);
    }

    public void ResetPlayerHealthAndShield()
    {
        _playerHealthInfo.SetStartingHealth(_playerDefaultStartingHealth);
        _playerShieldInfo.SetShieldAmount(_playerDefaultStartingShield);
    }
}
