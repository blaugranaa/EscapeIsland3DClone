using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LandController : MonoBehaviour
{
    Land firstSelected;
    Land lastSelected;
    public Land[] Lands;
    private List<Line> sameTypeLines = new();


    LineRenderer _lineRenderer;


    private void OnEnable()
    {
        EventManager.AddListener(GameEvent.OnStickmanMoved, ResetLineRenderer);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener(GameEvent.OnStickmanMoved, ResetLineRenderer);
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            Camera.main.nearClipPlane));

        const float maxDistance = 300f;
        if (Input.GetMouseButtonDown(0))
        {
            if (!Physics.Raycast(ray.origin, ray.direction, out RaycastHit hitInfo, maxDistance))
                return;
            var land = hitInfo.collider.gameObject.GetComponent<Land>();


            if (land is not null)
            {
                if (firstSelected is null)
                {
                    land.transform.DOMoveY(1, 0.3f);

                    firstSelected = land;
                }
                else if (firstSelected == land)
                {
                    land.transform.DOMoveY(0, 0.3f);

                    firstSelected = null;
                    lastSelected = null;
                }
                else
                {
                    lastSelected = land;

                    firstSelected.transform.DOMoveY(0, 0.3f).OnComplete(DrawPathBetweenLands);
                }
            }
        }
    }

    private void DrawPathBetweenLands()
    {
        Vector3[] landPathPos = new Vector3[]
        {
            new Vector3(firstSelected.transform.position.x, 0, firstSelected.transform.position.z),
            new Vector3(0, 0, firstSelected.transform.position.z),
            new Vector3(0, 0, lastSelected.transform.position.z),
            lastSelected.transform.position
        };
        _lineRenderer = PoolingSystem.Instance.InstantiateAPS("LineRenderer").GetComponent<LineRenderer>();
        _lineRenderer.SetPositions(landPathPos);

        var lines = GetFirstAvalaibleGroup();
        foreach (var line in lines)
        {
            StartCoroutine(MoveStickman(_lineRenderer, line));
        }
        
        
    }


    IEnumerator MoveStickman(LineRenderer lineRenderer, Line line)
    {
        for (int i = 0; i < 4; i++)
        {
            line.stickmans[i]
                .ChooseCharactersForMovement(lineRenderer, lastSelected.GetAvailableLine().lineCells[i], i);
            line.stickmans[i] = null;
            line.stickmanType = StickmanTypes.None;
            
            yield return new WaitForSeconds(0.2f);
        }

        line.isFull = false;


    }

    List<Line> GetFirstAvalaibleGroup()
    {
        var frontLineType = StickmanTypes.None;
        bool isFirst = true;
        for (var i = 0; i < 4; i++)
        {
            if (firstSelected.Lines[i].stickmanType == StickmanTypes.None)
                continue;
            else
            {
                if (isFirst)
                {
                    isFirst = false;
                    frontLineType = firstSelected.Lines[i].stickmanType;
                    sameTypeLines.Add(firstSelected.Lines[i]);
                    continue;
                }

                if (firstSelected.Lines[i].stickmanType == frontLineType)
                {
                    sameTypeLines.Add(firstSelected.Lines[i]);
                }
                else
                {
                    return sameTypeLines;
                    break;
                }
            }
        }

        return sameTypeLines;
    }

    public void ResetLineRenderer()
    {
        lastSelected.GetAvailableLine().isFull = true;
        lastSelected = null;
        firstSelected = null;
        
        PoolingSystem.Instance.DestroyAPS(_lineRenderer.gameObject);
        sameTypeLines.Clear();
        

    }
}