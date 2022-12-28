using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public enum GestureType
{
    None,
    Tap,
    DoubleTap,
    SwipeUp,
    SwipeDown,
    SwipeLeft,
    SwipeRight,
    PinchIn,
    PinchOut
}

public class Gesture
{
    public Gesture()
    {
        SetStandardValues();
    }

    public void SetStandardValues()
    {
        startPos = new Vector2(0, 0);
        endPos = new Vector2(0, 0);
        delta = new Vector2(0, 0);
        distance = 0;
        currentGesture = GestureType.None;
        touchLists.Clear();
    }

    [SerializeField] private float tapTime;
    private List<Touch> touchLists = new List<Touch>();
    private Vector2 startPos;
    private Vector2 endPos;
    private Vector2 delta;
    private float distance;
    private GestureType currentGesture;

    #region Setters

    public void SetStartPos(Vector2 newStartPos)
    {
        startPos = newStartPos;
    }

    public void SetEndPos(Vector2 newEndPos)
    {
        endPos = newEndPos;
    }

    public void SetDelta(Vector2 newDelta)
    {
        delta = newDelta;
    }

    public void SetDistance(float newDistance)
    {
        distance = newDistance;
    }

    public void SetCurrentGesture(GestureType newCurrentGesture)
    {
        currentGesture = newCurrentGesture;
    }

    #endregion
    
    #region Getters

    public float GetTapTime()
    {
        return tapTime;
    }
    
    public Vector2 GetStartPos()
    {
        return startPos;
    }
    
    public Vector2 GetEndPos()
    {
        return endPos;
    }
    
    public Vector2 GetDelta()
    {
        return delta;
    }
    
    public float GetDistance()
    {
        return distance;
    }
    
    public GestureType GetCurrentGesture()
    {
        return currentGesture;
    }
    
    public List<Touch> GetTouchList()
    {
        return touchLists;
    }
    
    public Touch GetTouchListElement(int index)
    {
        return touchLists[index];
    }

    #endregion
}

public class GestureDetector : Singleton<GestureDetector>
{
    #region Variables & Properties

    private Gesture gesture= new Gesture();
    
    #endregion
    
    #region MonoBehaviour
    private void Update()
    {
        VerifyInput();
    }
    
    #endregion

    #region Methods

    private void AssignTouchList()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            gesture.GetTouchList().Add(Input.GetTouch(i));
        }
    }
    
    private void VerifyInput()
    {

        AssignTouchList();
        
        switch (Input.touchCount)
        {
            case 0:

                ZeroTouch();
                break;

            case 1:

                OneTouch();
                break;
            case 2:

                gesture.startPos = touch.position;
                
                else if (touch.phase == TouchPhase.Moved)
                {
                    gesture.endPos = touch.position;
                    gesture.delta = gesture.endPos - gesture.startPos;
                    gesture.distance = gesture.delta.magnitude;

                    if (gesture.distance > 50f)
                    {
                        // Swipe
                        if (Mathf.Abs(gesture.delta.x) > Mathf.Abs(gesture.delta.y))
                        {
                            // Horizontal swipe
                            if (gesture.delta.x > 0)
                            {
                                gesture.currentGesture = GestureType.SwipeRight;
                            }
                            else
                            {
                                gesture.currentGesture = GestureType.SwipeLeft;
                            }
                        }
                        else
                        {
                            // Vertical swipe
                            if (gesture.delta.y > 0)
                            {
                                gesture.currentGesture = GestureType.SwipeUp;
                            }
                            else
                            {
                                gesture.currentGesture = GestureType.SwipeDown;
                            }
                        }
                    }
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    if (gesture.distance < 50f)
                    {
                        // Tap
                        gesture.currentGesture = GestureType.Tap;
                    }
                }

                break;
        }
    }

    private void ZeroTouch()
    {
        gesture.SetStandardValues();
    }
    
    private void OneTouch()
    {
        switch (gesture.GetTouchListElement(0).phase)
        {
            case TouchPhase.Began:

                gesture.SetStartPos(gesture.GetTouchListElement(0).position);
                gesture.SetEndPos(gesture.GetTouchListElement(0).position);
                StartCoroutine(VerifyTimeTap());
                        
                break;
            case TouchPhase.Moved:

                break;
            case TouchPhase.Ended:
                
                StopCoroutine(VerifyTimeTap());
                break;
        }
    }

    #region Courutines

    private IEnumerator VerifyTimeTap()
    {
        Touch tapped = gesture.GetTouchListElement(0);
        yield return new WaitForSeconds(gesture.GetTapTime());
        if (tapped == gesture.GetTouchListElement(0))
        {
            
        }
    }

    #endregion
   

    #endregion
}
