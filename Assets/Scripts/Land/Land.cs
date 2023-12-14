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
}
