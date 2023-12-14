using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LandController : MonoBehaviour
{
    Land firstSelected;
    Land lastSelected;
    private LineRenderer lineRenderer;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
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
        lineRenderer.SetPositions(landPathPos);
        lineRenderer.enabled = true;


    }
}
