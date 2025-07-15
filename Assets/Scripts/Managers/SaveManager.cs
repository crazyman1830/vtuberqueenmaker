
using UnityEngine;
using System.IO;

public class SaveManager : ManagerBase
{
    private string saveFilePath;

    public override void ManagedInitialize()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
        GameEvents.OnSaveGame += SaveGame;
        Debug.Log($"Save file path: {saveFilePath}");
    }

    void OnDestroy()
    {
        GameEvents.OnSaveGame -= SaveGame;
    }

    public void SaveGame()
    {
        var timeManager = GameManager.Instance.GetManager<TimeManager>();
        var characterManager = GameManager.Instance.GetManager<CharacterManager>();

        SaveData saveData = new SaveData
        {
            playerData = characterManager.CurrentPlayerData,
            currentDay = timeManager.CurrentDay,
            currentWeek = timeManager.CurrentWeek,
            currentMonth = timeManager.CurrentMonth,
            currentYear = timeManager.CurrentYear
        };

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game saved successfully.");
    }

    public bool LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            var characterManager = GameManager.Instance.GetManager<CharacterManager>();
            var timeManager = GameManager.Instance.GetManager<TimeManager>();

            characterManager.SetPlayerData(saveData.playerData);
            timeManager.SetTime(new System.DateTime(saveData.currentYear, saveData.currentMonth, saveData.currentDay));

            Debug.Log("Game loaded successfully.");
            return true;
        }
        else
        {
            Debug.LogWarning("Save file not found.");
            return false;
        }
    }
}
