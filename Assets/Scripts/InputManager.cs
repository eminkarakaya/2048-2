using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    UP,DOWN,RIGHT,LEFT
}
public class InputManager : MonoBehaviour
{
    [SerializeField] private InputData _data;
    //swipe
    Vector2 startPos, endPos,currentPos;
    bool stopTouch = false; 
    [SerializeField] private float swipeRange,resetTime;
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            _data.TouchPosition = touch.position;
            _data.IsEnd = false;
            if (touch.phase == TouchPhase.Began)
            {
                // if (EventSystem.current.IsPointerOverGameObject())
                // {
                //     return;
                // }
                startPos = touch.position;
                _data.IsClick = true;
                
                _data.IsMove = false;
            }
            else if(touch.phase == TouchPhase.Moved)
            {
                _data.IsClick = false;
                _data.IsMove = true;
                CloseSwipe();
                StartCoroutine(ResetStartPos(touch.position));
                _data.DeltaPosition = touch.deltaPosition;
                currentPos = touch.position;
                Vector2 distance = currentPos - startPos;
                if(!stopTouch)
                {
                    if(distance.x < -swipeRange)
                    {
                        _data.SwipeLeft = true;
                        stopTouch = true;
                    }
                    else if(distance.x > swipeRange)
                    {
                        _data.SwipeRight = true;
                        stopTouch = true;
                    }
                    else if(distance.y > swipeRange)
                    {
                        _data.SwipeUp = true;
                        stopTouch = true;
                    }
                    else if(distance.y < -swipeRange)
                    {
                        _data.SwipeDown = true;
                        stopTouch = true;
                    }
                }
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                // if (EventSystem.current.IsPointerOverGameObject())
                // {
                //     return;
                // }
                _data.IsClick = false;
                _data.IsEnd = true;   
                _data.IsMove = false;
                stopTouch = false;
                endPos = touch.position;
                _data.DeltaPosition = Vector3.zero;
            }
        }
        else
        {
            _data.IsEnd = false;
            _data.IsMove = false;
            _data.IsClick = false;
            _data.DeltaPosition = Vector3.zero;
        }
    }

    public void CloseSwipe()
    {
        _data.SwipeLeft = false;
        _data.SwipeRight = false;
        _data.SwipeUp = false;
        _data.SwipeDown = false;
    }
    IEnumerator ResetStartPos(Vector2 pos)
    {
        yield return new WaitForSeconds(resetTime);
        startPos = pos;
    }
}

