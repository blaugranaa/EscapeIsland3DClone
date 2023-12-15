using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Land : MonoBehaviour
{
    internal Line[] Lines;
    private List<Line> avalaibleLines = new List<Line>();

    private void Awake()
    {
        Lines = GetComponentsInChildren<Line>();
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
