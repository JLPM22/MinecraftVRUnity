using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusModules.Grab;

public class InputPlayerVR : MonoBehaviour
{
    public GameObject DirtPlacePrefab;
    public GameObject StonePlacePrefab;

    public GameObject RocketLauncherPrefab;
    public GameObject TNTPrefab;

    public GameObject PickaxePrefab;
    public GameObject RightHandTrackingSpace;
    public GameObject LeftHandTrackingSpace;

    private Grabbable CurrentObject;
    private bool IsRightHand;

    private void Update()
    {
        if (CurrentObject != null)
        {
            if (!CurrentObject.IsGrabbed)
                CurrentObject.transform.position = IsRightHand ? RightHandTrackingSpace.transform.position : LeftHandTrackingSpace.transform.position;
            else
                CurrentObject = null;
            return;
        }

        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            CurrentObject = GameObject.Instantiate(PickaxePrefab, RightHandTrackingSpace.transform.position, Quaternion.identity).GetComponent<Grabbable>();
            IsRightHand = true;
        }
        else if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            CurrentObject = GameObject.Instantiate(TNTPrefab, RightHandTrackingSpace.transform.position, Quaternion.identity).GetComponent<Grabbable>();
            IsRightHand = true;
        }
        else if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch))
        {
            CurrentObject = GameObject.Instantiate(RocketLauncherPrefab, RightHandTrackingSpace.transform.position, Quaternion.identity).GetComponent<Grabbable>();
            IsRightHand = true;
        }
        else if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            CurrentObject = GameObject.Instantiate(DirtPlacePrefab, RightHandTrackingSpace.transform.position, Quaternion.identity).GetComponent<Grabbable>();
            IsRightHand = false;
        }
        else if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            CurrentObject = GameObject.Instantiate(StonePlacePrefab, RightHandTrackingSpace.transform.position, Quaternion.identity).GetComponent<Grabbable>();
            IsRightHand = false;
        }
    }
}
