
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    private string saveFilePath;

    public void Initialize()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
        Debug.Log($"Save file path: {saveFilePath}");
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerData = GameManager.Instance.CharacterManager.CurrentPlayerData,
            currentDay = GameManager.Instance.TimeManager.CurrentDay,
            currentWeek = GameManager.Instance.TimeManager.CurrentWeek,
            currentMonth = GameManager.Instance.TimeManager.CurrentMonth,
            currentYear = GameManager.Instance.TimeManager.CurrentYear
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

            GameManager.Instance.CharacterManager.SetPlayerData(saveData.playerData);
            GameManager.Instance.TimeManager.SetTime(new System.DateTime(saveData.currentYear, saveData.currentMonth, saveData.currentDay));

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
