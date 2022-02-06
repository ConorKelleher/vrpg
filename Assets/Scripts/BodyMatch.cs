using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyMatch : MonoBehaviour
{
    public Transform HeadTarget;
    public Transform TorsoBone;
    public Transform RightHandTarget;
    public Transform LeftHandTarget;
    public Transform HeadController;
    public Transform RightHandController;
    public Transform LeftHandController;
    private Quaternion HeadTargetStartRotation;
    private Quaternion HeadControllerStartRotation;

    private void Start()
    {
        HeadControllerStartRotation = HeadController.rotation;
        HeadTargetStartRotation = HeadTarget.rotation;
        HeadTarget.parent = null;
        LeftHandTarget.parent = null;
        RightHandTarget.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion diff = Quaternion.Inverse(HeadControllerStartRotation) * HeadController.rotation;
        HeadTarget.rotation = diff * HeadTargetStartRotation;
        HeadTarget.position = HeadController.position;

        RightHandTarget.position = RightHandController.position;
        LeftHandTarget.position = LeftHandController.position;

        RightHandTarget.rotation = RightHandController.rotation;
        LeftHandTarget.rotation = LeftHandController.rotation;
    }
}
