using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GridManager : Singleton<GridManager>
{
    [SerializeField] private Vector2 _startPos;
    public Vector2Int scale;
    [SerializeField] private Transform _parent;
    public AnimationCurve animationCurve;
    [SerializeField] private Grid _gridPrefab;
    public List<Grid> left,right,up,down;
    public List<Grid> allGrids = new List<Grid>();
    
    private void Awake() {
        CreateGrids();
        FindSortedGrids();
        foreach (var item in allGrids)
        {
            item.SetNeighbor();
        }
        
    }
    void Start()
    {
    }
    [ContextMenu("FindSortedGrids")]
    private void FindSortedGrids()
    {
        for (int i = 0; i < scale.x; i++)
        {
            for (int j = 0; j < allGrids.Count; j++)
            {
                if(allGrids[j].index.x == i)
                {
                    left.Add(allGrids[j]);
                }
            }
        }
        for (int i = scale.x-1; i >= 0; i--)
        {
            for (int j = 0; j < allGrids.Count; j++)
            {
                if(allGrids[j].index.x == i)
                {
                    right.Add(allGrids[j]);
                }
            }
        }
        for (int i = 0; i < scale.y; i++)
        {
            for (int j = 0; j < allGrids.Count; j++)
            {
                if(allGrids[j].index.y == i)
                {
                    up.Add(allGrids[j]);
                }
            }
        }
        for (int i = scale.y-1; i >= 0; i--)
        {
            for (int j = 0; j < allGrids.Count; j++)
            {
                if(allGrids[j].index.y == i)
                {
                    down.Add(allGrids[j]);
                }
            }
        }
    }
    private void CreateGrid(Vector2Int index,Vector2 pos)
    {
        var obj = Instantiate(_gridPrefab,pos,Quaternion.identity,_parent);
        obj.index = index;
        obj.name = "Grid "+index; 
        allGrids.Add(obj);
    }
    [ContextMenu("CreateGrids")]
    private void CreateGrids()
    {
        for (int y = 0; y < scale.y; y++)
        {            
            for (int x = 0; x < scale.x; x++)
            {
                CreateGrid(new Vector2Int(x,y),new Vector2(_startPos.x + x *.9f,_startPos.y - y*.9f));
            }
        }
    }

    public static Grid GetDirGrid(Grid grid, Direction dir)
    {
        switch (dir)
        {
            case Direction.UP:
                return grid.top;
            case Direction.DOWN:
                return grid.bot;
            case Direction.RIGHT:
                return grid.right;
            case Direction.LEFT:
                return grid.left;
        }
        return null;
    }
}
