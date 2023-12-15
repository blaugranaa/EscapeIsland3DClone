using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LandController : MonoBehaviour
{
    Land firstSelected;
    Land lastSelected;
    public Land[] Lands;

    LineRenderer _lineRenderer;



    private void OnEnable()
    {
        EventManager.AddListener(GameEvent.OnStickmanMoved,ResetLineRenderer);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener(GameEvent.OnStickmanMoved,ResetLineRenderer);
    }
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

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


        StartCoroutine(MoveStickman(_lineRenderer));

    }


    IEnumerator MoveStickman(LineRenderer lineRenderer)
    {

        for (int i = 0; i < firstSelected.Lines[firstSelected.Lines.Length - 1].stickmans.Length; i++)
        {

            firstSelected.Lines[firstSelected.Lines.Length - 1].stickmans[i].ChooseCharactersForMovement(lineRenderer, lastSelected.GetAvailableLine().lineCells[i], i);
            firstSelected.Lines[firstSelected.Lines.Length - 1].stickmans[i] = null;
            yield return new WaitForSeconds(0.2f);

        }

    }

    public void ResetLineRenderer( )
    {
        lastSelected = null;
        firstSelected = null;
        PoolingSystem.Instance.DestroyAPS(_lineRenderer.gameObject);
    }

}
