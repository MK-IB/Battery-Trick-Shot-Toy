using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonUIEventsManager : MonoBehaviour
{
    public static CommonUIEventsManager instance;
    public event Action LevelStartEvent;
    public event Action LevelCompleteEvent;
    public event Action ToyChangedEvent;
    
    private void Awake()
    {
        instance = this;
    }

    public void StartLevelStartEvent()
    {
        if (LevelStartEvent != null)
            LevelStartEvent();
    }
    public void StartLevelCompleteEvent()
    {
        if (LevelCompleteEvent != null)
            LevelCompleteEvent();
    }
    public void StartToyChangedEvent()
    {
        if (ToyChangedEvent != null)
        {
            ToyChangedEvent();
        }
    }
}
