using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Item : MonoBehaviour
{
    [SerializeField] private Grid temp;
    [SerializeField] private Item targetItem;
    [SerializeField] private TextMeshPro _valueText;
    public bool newItem;
    public int itemType;
    [SerializeField] private int _value;
    private SpriteRenderer _renderer;
    public int value
    {
        get { return _value; }
        set {
            _value = value;
            _renderer.color = ItemManager.Instance.GetColor(_value);
            _valueText.text = _value.ToString();
        }
    }
    
    public Grid grid;


    private void Awake() {
        _renderer = GetComponent<SpriteRenderer>();
        temp = this.grid;
    }
    private void OnDisable() {
        StopAllCoroutines();
    }
    public void FindTarget(Direction dir)
    {
        if(ItemManager.Instance.inAnimation)
        {
            return;
        }
        temp = this.grid;
        switch (dir)
        {
            case Direction.UP:
                while(temp.top != null && !temp.top.isFull)
                {
                    temp = temp.top;
                }
                    StartCoroutine (Move(transform,ItemManager.Instance.duration,Direction.UP));

            break;
            case Direction.DOWN:
                while(temp.bot != null && !temp.bot.isFull)
                {
                    temp = temp.bot;
                }
                StartCoroutine (Move(transform,ItemManager.Instance.duration,Direction.DOWN));
                
            break;
            case Direction.RIGHT:
                while(temp.right != null && !temp.right.isFull)
                {
                    temp = temp.right;
                }
                StartCoroutine (Move(transform,ItemManager.Instance.duration,Direction.RIGHT));
            break;
            case Direction.LEFT:
                
                while(temp.left != null && !temp.left.isFull)
                {
                    temp = temp.left; 
                }
                StartCoroutine (Move(transform,ItemManager.Instance.duration,Direction.LEFT));
            break;
        }
    }


    private IEnumerator Move(Transform current,float duration , Direction dir , System.Action action = null)
    {
        float passed = 0f;
        Vector3 initPosition = current.position; 
        newItem = false;
        
        while(GridManager.GetDirGrid(grid,dir) != null)
        {
            if(GridManager.GetDirGrid(grid,dir).isFull)
            {
                if(GridManager.GetDirGrid(grid,dir).item.newItem)
                {
                    break;
                }
                if(GridManager.GetDirGrid(grid,dir).item.value == value)
                {
                    ItemManager.Instance.MergeItem(this,GridManager.GetDirGrid(grid,dir));
                }
                else
                {
                    break;
                }
            }
            else
            {
                ItemManager.Instance.movingItems ++;
                grid.item = null;
                grid.isFull = false;
                grid = GridManager.GetDirGrid(grid,dir);
                grid.isFull = true;
                grid.item = this;
            
                while(passed < duration)
                {
                    passed += Time.deltaTime;
                    var normalized = passed / duration;
                    var position = Vector3.Lerp(initPosition,grid.transform.position,normalized);
                    current.position = position;
                    yield return null;
                }
                // check merge
                passed = 0f;
                initPosition = current.position;
                action?.Invoke();
            }
        }
    }   
}