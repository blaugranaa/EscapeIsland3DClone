using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour
{
    internal Line[] Lines;
    private List<Line> avalaibleLines = new List<Line>();
    internal bool isCompleted;
    private LandController _landController;

    private void Awake()
    {
        Lines = GetComponentsInChildren<Line>();
        _landController = GetComponentInParent<LandController>();
    }

    private void OnEnable()
    {
        EventManager.AddListener(GameEvent.OnStickmanMoved, CheckLinesCompleted);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener(GameEvent.OnStickmanMoved, CheckLinesCompleted);
    }

    void CheckLinesCompleted()
    {
        if (isCompleted == true)
             return;
        var frontLineType = StickmanTypes.None;
        bool isFirst = true;
        int sameLineCount = 0;
        for (var i = 0; i < 4; i++)
        {
            if (Lines[i].stickmanType == StickmanTypes.None)
                break;

            if (isFirst)
            {
                isFirst = false;
                frontLineType = Lines[i].stickmanType;
                sameLineCount++;
            }
            else if (Lines[i].stickmanType == frontLineType)
            {
                sameLineCount++;
            }
        }
        
        if (sameLineCount==4)
        {
            isCompleted = true;
            PoolingSystem.Instance.InstantiateAPS("confetti", transform.position);
            _landController.CheckLevelComplete();


        }
    }

    public List<Line> GetAvailableLines()
    {
        avalaibleLines.Clear();

        for (int i = Lines.Length - 1; i >= 0; i--)
        {
            if (!Lines[i].isFull)
            {
                avalaibleLines.Add(Lines[i]);
            }
        }

        return avalaibleLines;
    }
}