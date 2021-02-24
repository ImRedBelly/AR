using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Experimental.XR;

using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using System;

public class ARTapToPlace : MonoBehaviour
{
    public GameObject objectToPlace;

    public GameObject placementIndicator;

    //private ARSessionOrigin arOrigin;
    private Pose placementPose;
    private bool PlacementPoseIsValid = false;

    ARRaycastManager aRRaycastManager;
    void Start()
    {
        //arOrigin = FindObjectOfType<ARSessionOrigin>();
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();


        if(PlacementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }
    }

    private void PlaceObject()
    {
        Instantiate(objectToPlace, placementPose.position, placementPose.rotation);
    }


    private void UpdatePlacementIndicator()
    {
        if (PlacementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        PlacementPoseIsValid = hits.Count > 0;
        if (PlacementPoseIsValid)
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;

            placementPose.rotation = Quaternion.LookRotation(cameraBearing);

        }
    }
}
