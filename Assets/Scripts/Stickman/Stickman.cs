using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class Stickman : MonoBehaviour
{
    private LineRenderer _lineRenderer;



    List<Vector3> pathList = new List<Vector3>();
    private bool canMove;
    float moveSpeed;

    public void ChooseCharactersForMovement(LineRenderer lineRenderer, Transform finalPos)
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

                  transform.DORotateQuaternion(rotation, 0.1f);
              }
         }).OnComplete(() =>
          {
              pathList.Remove(lastPosition);
              transform.DORotate(new Vector3(0, 90, 0), 0.1f, RotateMode.LocalAxisAdd);

          });

    }

}
