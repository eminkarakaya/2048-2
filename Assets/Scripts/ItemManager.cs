using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
public class ItemManager : Singleton<ItemManager>
{
    public delegate void OnMovementFinished();
    public static OnMovementFinished onMovementFinished;
    [SerializeField] private InputData _inputData;
    [SerializeField] private List<Color> colors;
    public static readonly int [] initialValues = new int []{2,2,2,4};
    private const int INCREMENT_VALUE = 2;
    public bool inAnimation = false;
    [HideInInspector] public float duration = .01f;

    public int movingItems;
    Level level;
    LevelData lastLevelData;
    private void Start() {
        level = FindObjectOfType<Level>();
        lastLevelData = new LevelData(level);
    }
    
    private void Update() {
        // input
        if(inAnimation) return;
        if(_inputData.SwipeDown || Input.GetKeyDown(KeyCode.S))
        {
            level.prevScore = level.score;
            for (int i = 0; i < SaveSystem.Instance.gameData.datas[SceneManager.GetActiveScene().buildIndex-1].items.Count; i++)
            {
                level.lastMoveData.items[i] = SaveSystem.Instance.gameData.datas[SceneManager.GetActiveScene().buildIndex-1].items[i];
            }
            for (int i = 0; i < GridManager.Instance.down.Count; i++)
            {
                if(GridManager.Instance.down[i].item != null)
                {
                    GridManager.Instance.down[i].item.FindTarget(Direction.DOWN);
                }
            }
           
            if(movingItems > 0)
            {
                StartCoroutine(WaitMoving());

            }
            
        }
        else if(_inputData.SwipeUp || Input.GetKeyDown(KeyCode.W))
        {
            level.prevScore = level.score;
            for (int i = 0; i < SaveSystem.Instance.gameData.datas[SceneManager.GetActiveScene().buildIndex-1].items.Count; i++)
            {
                level.lastMoveData.items[i] = SaveSystem.Instance.gameData.datas[SceneManager.GetActiveScene().buildIndex-1].items[i];
            }
            for (int i = 0; i < GridManager.Instance.up.Count; i++)
            {
                if(GridManager.Instance.up[i].item != null)
                {
                    GridManager.Instance.up[i].item.FindTarget(Direction.UP);
                }
            }
            
            if(movingItems>0)
            {
                StartCoroutine(WaitMoving());

            }
        }
        else if(_inputData.SwipeLeft || Input.GetKeyDown(KeyCode.A))
        {
            level.prevScore = level.score;
            for (int i = 0; i < SaveSystem.Instance.gameData.datas[SceneManager.GetActiveScene().buildIndex-1].items.Count; i++)
            {
                level.lastMoveData.items[i] = SaveSystem.Instance.gameData.datas[SceneManager.GetActiveScene().buildIndex-1].items[i];
            }
            for (int i = 0; i < GridManager.Instance.left.Count; i++)
            {
                if(GridManager.Instance.left[i].item != null)
                {
                    GridManager.Instance.left[i].item.FindTarget(Direction.LEFT);
                }
            }
           
            if(movingItems>0)
            {
                StartCoroutine(WaitMoving());

            }
        }
        else if(_inputData.SwipeRight || Input.GetKeyDown(KeyCode.D))
        {
            for (int i = 0; i < SaveSystem.Instance.gameData.datas[SceneManager.GetActiveScene().buildIndex-1].items.Count; i++)
            {
                level.lastMoveData.items[i] = SaveSystem.Instance.gameData.datas[SceneManager.GetActiveScene().buildIndex-1].items[i];
            }
            level.prevScore = level.score;
            for (int i = 0; i < GridManager.Instance.right.Count; i++)
            {
                    
                if(GridManager.Instance.right[i].item != null)
                {
                    GridManager.Instance.right[i].item.FindTarget(Direction.RIGHT);
                }
            }
           
            if(movingItems>0)
            {
                StartCoroutine(WaitMoving());

            }
        }
    }



    public Color GetColor(int value)
    {
        int sum = 0;
        while(value != 2)
        {
            value = value/initialValues[0];
            sum ++;
        }
        return colors[sum];
    }


    
    IEnumerator WaitMoving()
    {
        inAnimation = true;
        while(movingItems>0)
        {
            yield return null;
        }
        // yield return new WaitForSeconds(duration*6);
        inAnimation = false;
        CreateItem(GetRandomGrid(),0,initialValues [Random.Range(0,initialValues.Length)]);
        onMovementFinished?.Invoke();

        level.SaveLevel(level);
        level.CheckFinish();        
    }




    public Grid GetRandomGrid()
    {
        List<Grid> grids = GridManager.Instance.allGrids.Select(x=>x).ToList();
        List<Grid> removingList = new List<Grid>();
        for (int i = 0; i < grids.Count; i++)
        {
            if(grids[i].isFull)
            {
                removingList.Add(grids[i]);
            }
        }
        for (int i = 0; i < removingList.Count; i++)
        {
            grids.Remove(removingList[i]);
        }       
        int random =  Random.Range (0,grids.Count);
        return grids[random];
    }
    



    public Item MergeItem(Item item1,Grid grid2)
    {
        int value = item1.value;
        DestroyItem(item1);
        DestroyItem(grid2.item);
        movingItems --;
        Item newItem = CreateItem(grid2,0,value*INCREMENT_VALUE);
        level.score = level.score + newItem.value;
        newItem.newItem = true;
        ItemManager.Instance.movingItems ++;
        
        return newItem;
    }

    
   
    public static Item DestroyItem(Item item)
    {
        Item item1 = item;
        item.grid.item = null;
        item.grid.isFull = false;
        ObjectPool.Instance.SetPooledObject(item.itemType,item);
        item.value = initialValues[0];
        return item;
    }


    
    public static Item CreateItem(Grid grid,int objType,int value)
    {
        Item item = ObjectPool.Instance.GetPooledObject(objType);
        // ItemManager.Instance.createdItemValue*=2;
        item.grid = grid;
        grid.item = item;
        grid.isFull = true;
        item.transform.localScale = grid.transform.localScale;
        item.value = value;
        item.transform.position = grid.transform.position;
        return item;
    }
}
