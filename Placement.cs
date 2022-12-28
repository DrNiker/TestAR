using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Placement : MonoBehaviour
{
    ARRaycastManager m_RaycastManager;
    Vector2 LastPosition;
    GameObject box;
    // Start is called before the first frame update
    public void GenerateBox(GameObject _box)
    {
        box = _box;
    }

    private void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if(Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    private void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        if(m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = s_Hits[0].pose;
            if (Input.touchCount > 1) 
            {
                float rot = LastPosition.x - touchPosition.x;
                box.transform.Rotate(new Vector3(0,rot,0));
                LastPosition = touchPosition;
            } else
            {
                box.transform.position = hitPose.position;
            }
        }
    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

}
