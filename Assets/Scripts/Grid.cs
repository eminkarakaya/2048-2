using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Grid : MonoBehaviour
{
    [SerializeField] private Item _item;
    public Item item
    {
        get => _item;
        set
        {
            _item = value;
            if(_item == null)
            {
                isFull = false;
            }
            else
            {
                isFull = true;
            }
        }
    }
    public Vector2Int index;
    public Vector2 position;
    public bool roadBlocked;
    public bool isDisabledGrid;
    public bool isFull;
    public Grid top,bot,right,left;
    void Start()
    {
        position = this.transform.position;
        SetNeighbor();
    }

    bool CheckLeft(int i)
    {
        if(this.index.x -1 == GridManager.Instance.allGrids[i].index.x) return true;
        return false;
    }
    bool CheckRight(int i)
    {
        if(this.index.x +1 == GridManager.Instance.allGrids[i].index.x) return true;
        return false;
    }
    bool CheckBottom(int i)
    {
        if(this.index.y +1 == GridManager.Instance.allGrids[i].index.y) return true;
        return false;
    }
    bool CheckTop(int i)
    {
        if(this.index.y -1 == GridManager.Instance.allGrids[i].index.y) return true;
        return false;
    }
    [ContextMenu("SetNeighbor")]
    public void SetNeighbor()
    {
        for (int i = 0; i < GridManager.Instance.allGrids.Count; i++)
        {
            if(this.index.y == GridManager.Instance.allGrids[i].index.y)
            {
                if(CheckRight(i))
                {
                    right = GridManager.Instance.allGrids[i];
                }
                else if(CheckLeft(i))
                {
                    left = GridManager.Instance.allGrids[i];
                }
            }
            else if(this.index.x == GridManager.Instance.allGrids[i].index.x)
            {
                if(CheckBottom(i))
                {
                    bot = GridManager.Instance.allGrids[i];
                }
                else if(CheckTop(i))
                {  
                    top = GridManager.Instance.allGrids[i];
                }
            }
        }
    }
}
