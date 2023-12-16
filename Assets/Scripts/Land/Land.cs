using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Land : MonoBehaviour
{
    internal Line[] Lines;
    private List<Line> avalaibleLines = new List<Line>();
    internal bool isCompleted;

    private void Awake()
    {
        Lines = GetComponentsInChildren<Line>();
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
            Debug.Log($"COMPLETED {name}");
            isCompleted = true;

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