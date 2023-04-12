using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ItemManager : Singleton<ItemManager>
{
    [SerializeField] private InputData _inputData;
    [SerializeField] private static List<Item> _allItems = new List<Item>();
    [SerializeField] private List<Color> colors;
    public const int INITIAL_VALUE = 2;
    private const int INCREMENT_VALUE = 2;
    public bool inAnimation = false;
    public float duration;
    public bool isMove;
    Level level;
    LevelData lastLevelData;
    private void Start() {
        level = FindObjectOfType<Level>();
        lastLevelData = new LevelData(level);
    }
    
    private void Update() {
         if(_inputData.SwipeDown)
        {
            for (int i = 0; i < GridManager.Instance.down.Count; i++)
            {
                if(GridManager.Instance.down[i].item != null)
                {
                    GridManager.Instance.down[i].item.FindTarget(Direction.DOWN);
                    
                }
            }
            if(isMove)
            {
                StartCoroutine(WaitAnimation());
                isMove = false;
            }
            
        }
        else if(_inputData.SwipeUp)
        {
            for (int i = 0; i < GridManager.Instance.up.Count; i++)
            {
                if(GridManager.Instance.up[i].item != null)
                {
                    GridManager.Instance.up[i].item.FindTarget(Direction.UP);
                }
            }
            if(isMove)
            {
                StartCoroutine(WaitAnimation());
                isMove = false;
            }
        }
        else if(_inputData.SwipeLeft)
        {
            for (int i = 0; i < GridManager.Instance.left.Count; i++)
            {
                if(GridManager.Instance.left[i].item != null)
                {
                    GridManager.Instance.left[i].item.FindTarget(Direction.LEFT);
                }
            }
            if(isMove)
            {
                StartCoroutine(WaitAnimation());
                isMove = false;
            }
        }
        else if(_inputData.SwipeRight)
        {
            for (int i = 0; i < GridManager.Instance.right.Count; i++)
            {
                if(GridManager.Instance.right[i].item != null)
                {
                    GridManager.Instance.right[i].item.FindTarget(Direction.RIGHT);
                    
                }
            }
            if(isMove)
            {
                StartCoroutine(WaitAnimation());
                isMove = false;
            }
        }
        
    }
    public Color GetColor(int value)
    {
        int sum = 0;
        while(value != 2)
        {
            value = value/INITIAL_VALUE;
            sum ++;
        }
        return colors[sum];
    }
    IEnumerator WaitAnimation()
    {
        inAnimation = true;
        yield return new WaitForSeconds(duration*4);
        inAnimation = false;

        CreateItem(GetRandomGrid(),0,2);
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
    
    public static Item MergeItem(Item item1,Grid grid2)
    {
        int value = item1.value;
        DestroyItem(item1);
        DestroyItem(grid2.item);
        Item newItem = CreateItem(grid2,0,value*INCREMENT_VALUE);
        newItem.newItem = true;
        return newItem;
    }
   
    public static Item DestroyItem(Item item)
    {
        Item item1 = item;
        _allItems.Remove(item);
        item.grid.item = null;
        item.grid.isFull = false;
        ObjectPool.Instance.SetPooledObject(item.itemType,item);
        item.value = INITIAL_VALUE;
        return item;
    }
    
    public static Item CreateItem(Grid grid,int objType,int value)
    {
        Item item = ObjectPool.Instance.GetPooledObject(objType);

        item.grid = grid;
        grid.item = item;
        grid.isFull = true;
        
        item.value = value;
        _allItems.Add(item);
        item.transform.position = grid.transform.position;
        return item;
    }
}
