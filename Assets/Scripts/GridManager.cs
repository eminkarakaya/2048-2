using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    [SerializeField] private Vector2 _startPos;
    [SerializeField] private Vector2Int _scale;
    [SerializeField] private Transform _parent;
    public AnimationCurve animationCurve;
    [SerializeField] private Grid _gridPrefab;
    public List<Grid> left,right,up,down;
    public List<Grid> allGrids;
    private void Awake() {
    }
    void Start()
    {
        foreach (var item in allGrids)
        {
            item.SetNeighbor();
        }
    }
    [ContextMenu("FindSortedGrids")]
    private void FindSortedGrids()
    {
        for (int i = 0; i < _scale.x; i++)
        {
            for (int j = 0; j < allGrids.Count; j++)
            {
                if(allGrids[j].index.x == i)
                {
                    left.Add(allGrids[j]);
                }
            }
        }
        for (int i = _scale.x-1; i >= 0; i--)
        {
            for (int j = 0; j < allGrids.Count; j++)
            {
                if(allGrids[j].index.x == i)
                {
                    right.Add(allGrids[j]);
                }
            }
        }
        for (int i = 0; i < _scale.y; i++)
        {
            for (int j = 0; j < allGrids.Count; j++)
            {
                if(allGrids[j].index.y == i)
                {
                    up.Add(allGrids[j]);
                }
            }
        }
        for (int i = _scale.y-1; i >= 0; i--)
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
        for (int y = 0; y < _scale.y; y++)
        {            
            for (int x = 0; x < _scale.x; x++)
            {
                CreateGrid(new Vector2Int(x,y),new Vector2(_startPos.x + x *.9f,_startPos.y - y*.9f));
            }
        }
    }
    [ContextMenu("ClearGrids")]
    private void Clear()
    {
        int count = allGrids.Count;
        for (int i = 0; i < _parent.childCount; i++)
        {
            allGrids.Remove(_parent.GetChild(i).GetComponent<Grid>());
        }
    }
}
