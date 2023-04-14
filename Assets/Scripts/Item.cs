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
        // StopAllCoroutines();
    }
    
    
    public void FindTarget(Direction dir)
    {
        if(ItemManager.Instance.inAnimation)
        {
            return;
        }
        temp = this.grid;
        while(GridManager.GetDirGrid(temp,dir) != null && !GridManager.GetDirGrid(temp,dir).isFull)
        {
            temp = GridManager.GetDirGrid(temp,dir);
        }
            StartCoroutine (Move(transform,ItemManager.Instance.duration,dir));
    }
    
    private IEnumerator Move(Transform current,float duration , Direction dir , System.Action action = null)
    {
        float passed = 0f;
        Vector3 initPosition = current.position; 
        newItem = false;
        bool isMove = false;
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
                    if(!isMove)
                    {
                        ItemManager.Instance.movingItems++;
                        ItemManager.Instance.WaitW();
                    }
                    ItemManager.Instance.MergeItem(this,GridManager.GetDirGrid(grid,dir));
                }
                else
                {
                    break;
                }
            }
            else
            {
                isMove = true;
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
                ItemManager.Instance.movingItems --;
            }
            
        }
    }   
}