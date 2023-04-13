using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class LevelData
{
    public List< int>  items = new List<int>();
    public int highScore;
    public bool backClaim = false;
    public LevelData()
    {
        //
    }
    public LevelData(Level level)
    {
        backClaim = level.backClaim;
        items = level.items;
    }

}
[System.Serializable]
public class GameData
{
    public List< LevelData> datas = new List<LevelData>();
    public List<LevelData> lastMoveDatas = new List<LevelData>();
    public GameData()
    {
        for (int i = 0; i < datas.Count; i++)
        {
            datas[i] = new LevelData();
        }
    }
  
}