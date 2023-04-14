using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;
public class Level : MonoBehaviour
{
    #region  fields

    [SerializeField] private GameObject restartMenu,finishMenu;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    public List< int> items;
    public bool backClaim = false;
    public LevelData lastMoveData;
    public int prevScore;

    [SerializeField] private int _score;
    public int score
    {
        get { return _score; }
        set {
            _score = value;
            scoreText.text = _score.ToString();
        }
    }
    private int _highScore;
    public int highScore
    {
        get { return _highScore; }
        set {
            _highScore = value;
            highScoreText.text = _highScore.ToString();
        }
    }
    #endregion

    #region UnityFuncs
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
    private void Start() {
        // load 
        List<Grid> allGrids = GridManager.Instance.allGrids.Select(x=>x).ToList();
        GameData data = SaveSystem.Instance.LoadGame();
        highScore = SaveSystem.Instance.gameData.datas[SceneManager.GetActiveScene().buildIndex-1].highScore;
        prevScore = SaveSystem.Instance.gameData.datas[SceneManager.GetActiveScene().buildIndex-1].prevScore;
        score = SaveSystem.Instance.gameData.datas[SceneManager.GetActiveScene().buildIndex-1].score;
        lastMoveData = SaveSystem.Instance.gameData.lastMoveDatas[SceneManager.GetActiveScene().buildIndex-1];
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
            grid1.isFull = true;
            Grid grid2 = ItemManager.Instance.GetRandomGrid();
            Item item1 = ItemManager.CreateItem(grid1,0,2); 
            Item item2 = ItemManager.CreateItem(grid2,0,4);
            items[allGrids.IndexOf(grid1)] = item1.value;
            items[allGrids.IndexOf(grid2)] = item2.value;
        }
        backClaim = data.datas[SceneManager.GetActiveScene().buildIndex-1].backClaim;
        SaveLevel(this);

    }
    private void OnApplicationQuit() {
        SaveLevel(this);
    }

    #endregion

    public bool CheckMergeOrMove(Grid grid)
    {
        List<Grid> gridList = GridManager.GetAllDirectionGrids(grid);
        if(grid.item == null) return false;
        for (int i = 0; i < gridList.Count; i++)
        { 
            if(gridList[i].item == null) return false;
            if(gridList[i].item.value == grid.item.value)
                return false;
        }
        return true;
    }
    public void CheckFinish()
    {
        foreach (var item in GridManager.Instance.allGrids)
        {
            if(!CheckMergeOrMove(item))
            {
                return;
            }
        }
        finishMenu.SetActive(true);
    }
    
    public void SetTrueBackClaim()
    {
        backClaim = true;
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
        SaveSystem.Instance.gameData.datas[SceneManager.GetActiveScene().buildIndex-1] = new LevelData(level);
        SaveSystem.Instance.SaveGame(); 
    }
    
    public void CheckHighScore()
    {
        if(score>highScore)
        {
            highScore = score;
        }
    }
    
    #region ButtonFuncs
    public void NewGame()
    {
        CheckHighScore();
        Restart();
    }
    public void ConfirmRestartMenu(bool value)
    {
        restartMenu.SetActive(value);
    }
    public void BackBtn()
    {
        if(ItemManager.Instance.movingItems > 0) return;
        if(backClaim)
        {
            backClaim = false;
            foreach (var item in GridManager.Instance.allGrids)
            {
                if(item.item != null)
                {
                    ItemManager.DestroyItem(item.item);
                }
            }
            for (int i = 0; i < lastMoveData.items.Count; i++)
            {
                if(lastMoveData.items[i] != 0)
                {
                    ItemManager.CreateItem(GridManager.Instance.allGrids[i],0,lastMoveData.items[i]);
                }
            }
            score = prevScore;
            SaveLevel(this);
        }
    }
    public void ReturnMainMenu()
    {
        SaveLevel(this);  
        SceneManager.LoadScene(0);
    }
    public void Restart()
    {
        backClaim = false;
        foreach (var item in GridManager.Instance.allGrids)
        {
            item.item = null;
        }
        CheckHighScore();
        score = 0;
        prevScore = 0;
        SaveLevel(this);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion
}
