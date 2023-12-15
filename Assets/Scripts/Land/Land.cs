using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Land : MonoBehaviour
{
    internal Line[] Lines;

    private void Awake()
    {
        Lines = GetComponentsInChildren<Line>();
    }

    public Line GetAvailableLine()
    {
        Line availableLine = null;

        for (int i = Lines.Length - 1; i >= 0; i--)
        {
            if (!Lines[i].isFull)
            {
                availableLine = Lines[i];
                break;
            }
        }

        return availableLine;
    }
}
