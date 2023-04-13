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
    public bool backClaim = false;
    public LevelData lastMoveData;
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
        ItemManager.onMovementFinished += SetTrueBackClaim;
        for (int i = 0; i < GridManager.Instance.allGrids.Count; i++)
        {
            items.Add(0);
        }
    }
    
    private void OnDisable() {
        
        ItemManager.onMovementFinished -= SetTrueBackClaim;
    }
    public void SetTrueBackClaim()
    {
        backClaim = true;
    }
    private void Start() {
        List<Grid> allGrids = GridManager.Instance.allGrids.Select(x=>x).ToList();
        GameData data = SaveSystem.Instance.LoadGame();
        lastMoveData = SaveSystem.Instance.gameData.lastMoveDatas[SceneManager.GetActiveScene().buildIndex-1];
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
            Grid grid1 = ItemManager.Instance.GetRandomGrid();
            Grid grid2 = ItemManager.Instance.GetRandomGrid();
            Item item1 = ItemManager.CreateItem(grid1,0,ItemManager.INITIAL_VALUE); 
            Item item2 = ItemManager.CreateItem(grid2,0,ItemManager.INITIAL_VALUE);
            items[allGrids.IndexOf(grid1)] = item1.value;
            items[allGrids.IndexOf(grid2)] = item2.value;
        }
        backClaim = data.datas[SceneManager.GetActiveScene().buildIndex-1].backClaim;
        SaveLevel(this);
    }
    
    public void ReturnMainMenu()
    {
        SaveLevel(this);  
        SceneManager.LoadScene(0);
    }
    public void BackBtn()
    {
        // eger back hakkı varsa
        if(backClaim)
        {
        // back hakkı olmucak 
            backClaim = false;
        // butun ıtemlerı sılcez 
            foreach (var item in GridManager.Instance.allGrids)
            {
                if(item.item != null)
                {
                    ItemManager.DestroyItem(item.item);
                }
            }
            // prev ıtemlerı yuklucez
            for (int i = 0; i < lastMoveData.items.Count; i++)
            {
                if(lastMoveData.items[i] != 0)
                {
                    ItemManager.CreateItem(GridManager.Instance.allGrids[i],0,lastMoveData.items[i]);
                }
            }
            SaveLevel(this);
        // save game yapcaz
        }
    }
    public void Restart()
    {
        backClaim = false;
        foreach (var item in GridManager.Instance.allGrids)
        {
            item.item = null;
        }
        // for (int i = 0; i < items.Count; i++)
        // {
        //     items[i] = 0;
        //     lastMoveData.items[i] = 0;
        // }
        SaveLevel(this);
        Debug.Log("reset");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ConfirmRestartMenu(bool value)
    {
        restartMenu.SetActive(value);
    }
    public void SaveLevel(Level level)
    {
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
        Debug.Log("Saved");
        SaveSystem.Instance.gameData.datas[SceneManager.GetActiveScene().buildIndex-1] = new LevelData(level);
        // SaveSystem.Instance.gameData.datas[SceneManager.GetActiveScene().buildIndex-1].backClaim = backClaim;
        
        SaveSystem.Instance.SaveGame(); 
    }
    private void OnApplicationQuit() {
        SaveLevel(this);
    }
}
