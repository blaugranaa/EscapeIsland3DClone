using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Land : MonoBehaviour
{
    GameObject LastSelected;


    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

        const float maxDistance = 300f;
        if (Input.GetMouseButtonDown(0))
        {
            if (!Physics.Raycast(ray.origin, ray.direction, out RaycastHit hitInfo, maxDistance))
                return;
            else
            {
                if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("land"))
                {
                    if (LastSelected != null)
                    {
                        Debug.Log("eeee");
                    }

                    LastSelected = hitInfo.collider.gameObject;
                    LastSelected.transform.DOShakePosition(1,1,1);



                }

            }
        }
    }
}
