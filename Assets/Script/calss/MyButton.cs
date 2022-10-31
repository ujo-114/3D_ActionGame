using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton
{
    public bool IsPressing = false;
    public bool OnPressed = false;
    public bool OnReleased = false;
    public bool IsExtending = false;
    public bool IsDalying = false;

    private float  delayingDuration = 0.2f;

    private bool curState = false;
    private bool lastState = false;
    private MyTimer extTimer = new MyTimer();
    private MyTimer delayTimer = new MyTimer();
    public void Tick(bool input)
    {
        extTimer.Tick();
        delayTimer.Tick();
        curState = input;
        IsPressing = curState;

        OnPressed = false;
        OnReleased = false;
        IsDalying = false;
        if (curState != lastState)
        {
            if (curState == true)
            {
                OnPressed = true;
                StartTimer(delayTimer, delayingDuration);
                //StartTimer(dalyingTimer, 0.5f);
            }
            else
            {
                OnReleased = true;
                //StartTimer(extTimer, 0.5f);
            }
        }

        if (delayTimer.state == MyTimer.STATE.RUN)
        {
            IsDalying = true;
        }
        lastState = curState;
        IsExtending = (extTimer.state == MyTimer.STATE.RUN);
        IsDalying = (delayTimer.state == MyTimer.STATE.RUN);
    }
    private void StartTimer(MyTimer timer,float duration)
    {
        timer.duration = duration;
        timer.Go();
    }




}
