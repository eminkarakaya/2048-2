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
                while(temp.top != null )
                {
                    if(temp.top.isFull)
                    {
                        if(temp.top.item.value == value)
                        {
                            Debug.Log(temp.top + " " + temp.top.item ,temp.top);
                            temp = temp.top;
                            Item temp2 = temp.item;
                            ItemManager.Instance.isMove = true;
                            Item newItem = ItemManager.MergeItem(this,temp);
                            targetItem = temp2;
                            
                            StartCoroutine (Move(transform,temp,ItemManager.Instance.duration,()=>ItemManager.MergeAnimation(this,temp,newItem)));
                            return;
                        }
                        else
                        {
                            ItemManager.Instance.isMove = true;
                            break;
                        }
                    }
                    else
                    {
                        temp = temp.top;
                        ItemManager.Instance.isMove = true;
                    }
                }
                
                StartCoroutine (Move(transform,temp,ItemManager.Instance.duration));
                
            break;
            case Direction.DOWN:
                while(temp.bot != null)
                {
                    if(temp.bot.isFull)
                    {
                        if(temp.bot.item.value == value)
                        {
                            Debug.Log(temp.bot + " " + temp.bot.item,temp.bot);
                            
                            temp = temp.bot;
                            Item temp2 = temp.item;
                            targetItem = temp2;
                            ItemManager.Instance.isMove = true;
                            Item newItem = ItemManager.MergeItem(this,temp);
                            StartCoroutine (Move(transform,temp,ItemManager.Instance.duration,()=>ItemManager.MergeAnimation(this,temp,newItem)));
                            return;
                        }
                        else
                        {
                            ItemManager.Instance.isMove = true;
                            break;
                        }
                    }
                    else
                    {
                        ItemManager.Instance.isMove = true;
                        temp = temp.bot;
                    }
                }
                
                StartCoroutine (Move(transform,temp,ItemManager.Instance.duration));
            break;
            case Direction.RIGHT:
                while(temp.right != null)
                {
                    if(temp.right.isFull)
                    {
                        if(temp.right.item.value == value)
                        {
                            Debug.Log(temp.right + " " + temp.right.item,temp.right);
                            temp = temp.right;
                            Item temp2 = temp.item;
                            targetItem = temp2;
                            ItemManager.Instance.isMove = true;
                            Item newItem = ItemManager.MergeItem(this,temp);
                            StartCoroutine (Move(transform,temp,ItemManager.Instance.duration,()=>ItemManager.MergeAnimation(this,temp,newItem)));
                            return;
                        }
                        else
                        {
                            ItemManager.Instance.isMove = true;
                            break;
                        }
                    }
                    else
                    {
                        temp = temp.right;
                        ItemManager.Instance.isMove = true;
                    }
                }
                
                StartCoroutine (Move(transform,temp,ItemManager.Instance.duration));
            break;
            case Direction.LEFT:
                while(temp.left != null)
                {
                    if(temp.left.isFull)
                    {
                        if(temp.left.item.value == value)
                        {
                            Debug.Log(temp.left + " " + temp.left.item,temp.left);
                            temp = temp.left;
                            Item temp2 = temp.item;
                            targetItem = temp2;
                            ItemManager.Instance.isMove = true;
                            Item newItem = ItemManager.MergeItem(this,temp);
                            // 2 obje de sılınıcek ve anımasyon sonunda yok olcak

                            StartCoroutine (Move(transform,temp,ItemManager.Instance.duration,()=>ItemManager.MergeAnimation(this,temp,newItem)));
                            return;
                        }
                        else
                        {
                            ItemManager.Instance.isMove = true;
                            break;
                        }
                    }
                    else
                    {
                        temp = temp.left;
                        ItemManager.Instance.isMove = true;
                    }
                }
                StartCoroutine (Move(transform,temp,ItemManager.Instance.duration));
            break;
        }
    }
    private IEnumerator Move(Transform current,Grid target,float duration , System.Action action = null)
    {
        float passed = 0f;
        Vector3 initPosition = current.position; 

        grid.item = null;
        grid.isFull = false;
        grid = target;
        grid.isFull = true;
        target.item = this;

        while(passed < duration)
        {
            passed += Time.deltaTime;
            var normalized = passed / duration;
            var position = Vector3.Lerp(initPosition,target.position,normalized);
            current.position = position;
            yield return null; 
        }
        action?.Invoke();
    }   
}