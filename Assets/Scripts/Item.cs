using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Item : MonoBehaviour
{
    [SerializeField] private Grid temp;
    [SerializeField] private Item targetItem;
    [SerializeField] private TextMeshPro _valueText;
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
                    
                    ItemManager.Instance.isMove = true;
                }
                    StartCoroutine (Move(transform,ItemManager.Instance.duration,Direction.UP));

            break;
            case Direction.DOWN:
                while(temp.bot != null && !temp.bot.isFull)
                {
                    ItemManager.Instance.isMove = true;
                    temp = temp.bot;
                    
                }
                StartCoroutine (Move(transform,ItemManager.Instance.duration,Direction.DOWN));
                
            break;
            case Direction.RIGHT:
                while(temp.right != null && !temp.right.isFull)
                {
                    temp = temp.right;
                    
                    ItemManager.Instance.isMove = true;
                }
                StartCoroutine (Move(transform,ItemManager.Instance.duration,Direction.RIGHT));
            break;
            case Direction.LEFT:
                while(temp.left != null && !temp.left.isFull)
                {
                    temp = temp.left;
                    
                    ItemManager.Instance.isMove = true;   
                }
                StartCoroutine (Move(transform,ItemManager.Instance.duration,Direction.LEFT));
            break;
        }
    }
    private IEnumerator Move(Transform current,float duration , Direction dir , System.Action action = null)
    {
        float passed = 0f;
        Vector3 initPosition = current.position; 

        
        while(GridManager.GetDirGrid(grid,dir) != null && !GridManager.GetDirGrid(grid,dir).isFull)
        {
            
            grid.item = null;
            grid.isFull = false;
            grid = GridManager.GetDirGrid(grid,dir);
            grid.isFull = true;
            grid.item = this;
            while(passed < duration)
            {
                passed += Time.deltaTime;
                var normalized = passed / duration;
                var position = Vector3.Lerp(initPosition,grid.position,normalized);
                current.position = position;
                yield return null;
            }
            passed = 0f;
            initPosition = current.position;
            action?.Invoke();
        }
    }   
}