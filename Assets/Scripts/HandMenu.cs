using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandMenu : MonoBehaviour
{
    public Transform cameraOffset;
    public Transform myIdealPositionRef;
    private Quaternion idealRotationBetweenHeadAndSeat;
    private Vector3 idealOffsetBetweenHeadAndSeat;
    private bool wasPressing = false;
    static List<XRInputSubsystem> k_CachedSubsystems = new List<XRInputSubsystem>();
    XRInputSubsystem m_InputSubsystem;

    public void SetTrackingOriginMode()
    {
        if (m_InputSubsystem == null)
        {
            SubsystemManager.GetInstances(k_CachedSubsystems);
            if (k_CachedSubsystems.Count != 0)
                m_InputSubsystem = k_CachedSubsystems[0];
        }

        Debug.Log($"Tracking Origin Mode is [{m_InputSubsystem.GetTrackingOriginMode()}]");
        if (m_InputSubsystem.TrySetTrackingOriginMode(TrackingOriginModeFlags.Floor))
            Debug.Log($"Successfully set tracking origin mode to [{m_InputSubsystem.GetTrackingOriginMode()}]");
        else
            Debug.Log("Failed to set tracking origin mode");
    }

    private void Awake()
    {
        idealRotationBetweenHeadAndSeat = GetRotationWithoutTilt(Camera.main.transform) * Quaternion.Inverse(myIdealPositionRef.rotation);
        idealOffsetBetweenHeadAndSeat = Camera.main.transform.position - myIdealPositionRef.position;
    }

    //private void Start()
    //{
        //SetTrackingOriginMode();
        //List<XRInputSubsystem> subsystems = new List<XRInputSubsystem>();
        //SubsystemManager.GetInstances<XRInputSubsystem>(subsystems);
        //Debug.Log($"Subsystems count: {subsystems.Count}");
        //for (int i = 0; i < subsystems.Count; i++)
        //{
        //    subsystems[i].TrySetTrackingOriginMode(TrackingOriginModeFlags.Floor);
        //}
    //}

    private Quaternion GetRotationWithoutTilt(Transform transform)
    {
        Vector3 originalEulers = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(0, originalEulers.y, 0);
        Quaternion rotation = transform.rotation;
        transform.localEulerAngles = originalEulers;
        return rotation;
    }

    // Update is called once per frame
    public void SetPressing(bool pressing)
    {
        if (pressing && !wasPressing)
        {
            ResetView();
        }

        wasPressing = pressing;
    }

    public void ResetView()
    {
        Debug.Log("Resetting view");
        Vector3 cameraPositionBeforeRotation = Camera.main.transform.position;
        Quaternion currentRotationBetweenHeadAndSeat = GetRotationWithoutTilt(Camera.main.transform) * Quaternion.Inverse(myIdealPositionRef.rotation);
        Quaternion rotationResetQuaternion = idealRotationBetweenHeadAndSeat * Quaternion.Inverse(currentRotationBetweenHeadAndSeat);
        cameraOffset.rotation *= rotationResetQuaternion;
        Camera.main.transform.position = cameraPositionBeforeRotation;

        Vector3 currentOffsetBetweenHeadAndSeat = Camera.main.transform.position - myIdealPositionRef.position;
        Vector3 positionResetVector = idealOffsetBetweenHeadAndSeat - currentOffsetBetweenHeadAndSeat;
        cameraOffset.position += positionResetVector;
    }
}
