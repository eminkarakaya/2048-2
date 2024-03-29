using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class SaveSystem : Singleton<SaveSystem>
{
    [SerializeField] private bool resetData;
    public GameData gameData;
    private void OnDisable() 
    {
        SaveGame();
        ResetData();
    }
    private void Awake() 
    {
        LoadGame();
        SaveSystem[] objs = FindObjectsOfType<SaveSystem>();

        if (objs.Length > 1)
        {
            Destroy(objs[1].gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        if(gameData == null)
        {
            gameData = new GameData();
        }
        if(gameData.datas.Count == 0)
        {
            for (int i = 0; i < 8; i++)
            {
                LevelData data = new LevelData();
                gameData.datas.Add(data);
                for (int j = 0; j < 64; j++)
                {
                    data.items.Add(0);
                }
            }
            for (int i = 0; i < 8; i++)
            {
                LevelData data = new LevelData();
                gameData.lastMoveDatas.Add(data);
                for (int j = 0; j < 64; j++)
                {
                    data.items.Add(0);
                }
            }
            SaveGame();
        }
    }
    public void SaveGame()
    {
        string data = JsonUtility.ToJson(gameData);
        PlayerPrefs.SetString("Data",data);
    }
    public GameData LoadGame()
    {
        gameData = JsonUtility.FromJson<GameData>(PlayerPrefs.GetString("Data"));
        return gameData;
    }
    [ContextMenu("Reset")]
    public void ResetData()
    {
        if(resetData) PlayerPrefs.SetString("Data",null);
    }
}
