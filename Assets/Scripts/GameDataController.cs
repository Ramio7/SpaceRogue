using Abstracts;
using UnityEngine;

public class GameDataController : BaseController
{
    private const string RecordKey = "RecordCompletedLevels";

    public int CompletedLevels { get; private set; } = 0;
    public int RecordCompletedLevels { get; private set; } = 0;
    public float PlayerCurrentHealth { get; private set; } = 0;
    public float PlayerCurrentShield { get; private set; } = 0;

    public GameDataController()
    {
        RecordCompletedLevels = PlayerPrefs.GetInt(RecordKey);
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
        PlayerCurrentHealth = health;
    }

    public void SetPlayerCurrentShield(float shield)
    {
        PlayerCurrentShield = shield;
    }

    public void ResetPlayerHealthAndShield()
    {
        PlayerCurrentHealth = 0;
        PlayerCurrentShield = 0;
    }
}
