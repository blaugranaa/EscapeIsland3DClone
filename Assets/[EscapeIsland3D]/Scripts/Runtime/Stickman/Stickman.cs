using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Stickman : MonoBehaviour
{
    public StickmanTypes _stickmanType;
    private LineRenderer _lineRenderer;
    private Animator _animator;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    List<Vector3> pathList = new List<Vector3>();


    public void ChooseCharactersForMovement(LineRenderer lineRenderer, Transform finalPos, int order, Line targetLine)
    {
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            pathList.Add(lineRenderer.GetPosition(i));
        }
        var lastPosition = finalPos.position;

        pathList.Add(lastPosition);

        transform.DOPath(pathList.ToArray(), 2f, PathType.Linear).OnWaypointChange(index =>
        {
            if (index < pathList.Count - 1)
            {
                Vector3 direction = (pathList[index + 1] - pathList[index]).normalized;

                Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
                RunAnimation(true);
                transform.DORotateQuaternion(rotation, 0.1f);
            }
        }).OnComplete(() =>
        {
            pathList.Remove(lastPosition);
            transform.SetParent(finalPos);

            transform.DOLocalRotate(new Vector3(0, 0, 0), 0.1f);
            var finalLine = finalPos.GetComponentInParent<Line>();
            finalLine.stickmanType = _stickmanType;
            finalLine.stickmans[order] = this;
            pathList.Clear();
            RunAnimation(false);

            

            if (order == 3)
            {
                EventManager.Broadcast(GameEvent.OnStickmanMoved);
                targetLine.isFull = true;

            }
        });
    }

    private void RunAnimation(bool isMoving)
    {
        _animator.SetBool("isMoving", isMoving);
    }
}