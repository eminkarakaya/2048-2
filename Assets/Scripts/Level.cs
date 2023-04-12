using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;
public class Level : MonoBehaviour
{
    [SerializeField] private GameObject restartMenu;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    public List< int> items;
    
    LevelData lastMoveData;
    private int _score;
    public int score
    {
        get { return _score; }
        set {
            _score = value;
            scoreText.text = _score.ToString();
        }
    }
    private void OnEnable() {
        for (int i = 0; i < GridManager.Instance.allGrids.Count; i++)
        {
            items.Add(0);
        }
    }
    private void Start() {
        List<Grid> allGrids = GridManager.Instance.allGrids.Select(x=>x).ToList();
        GameData data = SaveSystem.Instance.LoadGame();
        lastMoveData = SaveSystem.Instance.gameData.datas[SceneManager.GetActiveScene().buildIndex-1] = new LevelData(this);
        // levelData = data.datas[SceneManager.GetActiveScene().buildIndex-1];
        for (int i = 0; i < data.datas[SceneManager.GetActiveScene().buildIndex-1].items.Count; i++)
        {
            if(data.datas[SceneManager.GetActiveScene().buildIndex-1].items[i] != 0)
            {
                items[i] = data.datas[SceneManager.GetActiveScene().buildIndex-1].items[i];
                ItemManager.CreateItem(allGrids[i],0,items[i]);
            }
        }
        if(FindObjectsOfType<Item>().Length == 0)
        {
            ItemManager.CreateItem(ItemManager.Instance.GetRandomGrid(),0,ItemManager.INITIAL_VALUE); 
            ItemManager.CreateItem(ItemManager.Instance.GetRandomGrid(),0,ItemManager.INITIAL_VALUE); 
        }
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.A))
        {
            List<Grid> allGrids = GridManager.Instance.allGrids.Select(x=>x).ToList();
            GameData data = SaveSystem.Instance.LoadGame();
            for (int i = 0; i < data.datas[SceneManager.GetActiveScene().buildIndex-1].items.Count; i++)
            {
                if(data.datas[SceneManager.GetActiveScene().buildIndex-1].items[i] != 0)
                {
                    ItemManager.CreateItem(allGrids[i],0,(int)data.datas[SceneManager.GetActiveScene().buildIndex-1].items[i]);
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
            for (int i = 0; i < GridManager.Instance.allGrids.Count; i++)
            {
                if(!object.ReferenceEquals(GridManager.Instance.allGrids[i].item,null))
                {
                    items[i] = GridManager.Instance.allGrids[i].item.value;
                }
            }
            SaveSystem.Instance.gameData.datas[SceneManager.GetActiveScene().buildIndex-1] = new LevelData(this);
            SaveSystem.Instance.SaveGame();           
        }
    }
    public void ReturnMainMenu()
    {
        SaveLevel(this);  
        SceneManager.LoadScene(0);
    }
    public void BackBtn()
    {
        
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ConfirmRestartMenu(bool value)
    {
        restartMenu.SetActive(value);
    }
    public void SaveLevel(Level level)
    {
        lastMoveData = SaveSystem.Instance.gameData.datas[SceneManager.GetActiveScene().buildIndex-1] = new LevelData(level);
        items.Clear();
        for (int i = 0; i < GridManager.Instance.allGrids.Count; i++)
        {
            items.Add(0);
        }
        for (int i = 0; i < GridManager.Instance.allGrids.Count; i++)
        {
            if(!object.ReferenceEquals(GridManager.Instance.allGrids[i].item,null))
            {
                items[i] = GridManager.Instance.allGrids[i].item.value;
            }
        }
        SaveSystem.Instance.gameData.datas[SceneManager.GetActiveScene().buildIndex-1] = new LevelData(level);
        SaveSystem.Instance.SaveGame(); 
    }
    private void OnApplicationQuit() {
        SaveLevel(this);
    }
}
