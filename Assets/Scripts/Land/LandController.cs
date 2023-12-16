using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class LandController : MonoBehaviour
{
    private Land _firstSelected;
    private Land _lastSelected;
    private Land[] _lands;
    private List<Line> _sameTypeLines = new();
    private LineRenderer _lineRenderer;
    private bool _canClick = true;
    public int landNumToComplete;

    private void Awake()
    {
        _lands = GetComponentsInChildren<Land>();
    }

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
        if (_canClick is not true)
            return;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            Camera.main.nearClipPlane));

        const float maxDistance = 300f;
        if (Input.GetMouseButtonDown(0))
        {
            if (!Physics.Raycast(ray.origin, ray.direction, out RaycastHit hitInfo, maxDistance))
                return;
            var land = hitInfo.collider.gameObject.GetComponent<Land>();


            if (land is not null && land.isCompleted is not true)
            {
                if (_firstSelected is null)
                {
                    land.transform.DOMoveY(1, 0.3f);

                    _firstSelected = land;
                }
                else if (_firstSelected == land)
                {
                    land.transform.DOMoveY(0, 0.3f);

                    _firstSelected = null;
                    _lastSelected = null;
                }
                else
                {
                    _canClick = false;
                    _lastSelected = land;
                    
                    _firstSelected.transform.DOMoveY(0, 0.3f).OnComplete(()=>StartCoroutine(MoveStickmanLinesCo()));
                }
            }
        }
    }

    private void DrawPathBetweenLands()
    {
        Vector3[] landPathPos = new Vector3[]
        {
            new Vector3(_firstSelected.transform.position.x, 0, _firstSelected.transform.position.z),
            new Vector3(0, 0, _firstSelected.transform.position.z),
            new Vector3(0, 0, _lastSelected.transform.position.z),
            _lastSelected.transform.position
        };
        _lineRenderer = PoolingSystem.Instance.InstantiateAPS("LineRenderer").GetComponent<LineRenderer>();
        _lineRenderer.SetPositions(landPathPos);
        
    }

    IEnumerator MoveStickmanLinesCo()
    {
        var lines = GetFirstAvalaibleGroup();
        var targetLines = _lastSelected.GetAvailableLines();
        if (lines.Count > targetLines.Count)
        {
            ResetLineRenderer();
            yield break;
        }
        DrawPathBetweenLands();
        if (targetLines.Count<=3)
        {
            if (_lastSelected.Lines[targetLines.Count].stickmanType == lines[0].stickmanType)
            {
                for (int i = lines.Count - 1; i >= 0; i--)
                {
                    StartCoroutine(MoveStickman(_lineRenderer, lines[(lines.Count - 1) - i], targetLines[i]));

                    yield return new WaitForSeconds(0.9f);
                }
            }
            else
            {
                ResetLineRenderer();
            }
           
        }
        else
        {
            for (int i = lines.Count - 1; i >= 0; i--)
            {
                StartCoroutine(MoveStickman(_lineRenderer, lines[(lines.Count - 1) - i], targetLines[i]));

                yield return new WaitForSeconds(0.9f);
            }
        }
    }


 IEnumerator MoveStickman(LineRenderer lineRenderer, Line line, Line _targetLine)
{
    for (int i = 0; i < 4; i++)
    {
        if (line.stickmans[i] != null)
        {
            line.stickmans[i]
                .ChooseCharactersForMovement(lineRenderer, _targetLine.lineCells[i], i, _targetLine);
            line.stickmans[i] = null;
            line.stickmanType = StickmanTypes.None;
        }

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
            if (_firstSelected.Lines[i].stickmanType == StickmanTypes.None)
                continue;

            if (isFirst)
            {
                isFirst = false;
                frontLineType = _firstSelected.Lines[i].stickmanType;
                _sameTypeLines.Add(_firstSelected.Lines[i]);
                continue;
            }

            if (_firstSelected.Lines[i].stickmanType == frontLineType)
            {
                _sameTypeLines.Add(_firstSelected.Lines[i]);
            }
            else
            {
                return _sameTypeLines;
            }
        }

        return _sameTypeLines;
    }

    public void ResetLineRenderer()
    {
        _lastSelected = null;
        _firstSelected = null;
        PoolingSystem.Instance.DestroyAPS(_lineRenderer.gameObject);
        _sameTypeLines.Clear();
        _canClick = true;
        
    }

    public void CheckLevelComplete()
    {
        landNumToComplete--;
        if (landNumToComplete <=0)
        {
            EventManager.Broadcast(GameEvent.OnLevelEnd);
        }
    }
}