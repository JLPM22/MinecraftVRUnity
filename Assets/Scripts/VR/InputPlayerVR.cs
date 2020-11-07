using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayerVR : MonoBehaviour
{
    public GameObject PickaxePrefab;
    public GameObject RightHandTrackingSpace;
    public GameObject LeftHandTrackingSpace;

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            GameObject.Instantiate(PickaxePrefab, RightHandTrackingSpace.transform.position + Vector3.up, Quaternion.identity);
        }
    }
}
