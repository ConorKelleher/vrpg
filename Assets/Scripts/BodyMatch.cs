using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyMatch : MonoBehaviour
{
    public Transform HeadBone;
    public Transform TorsoBone;
    public Transform RightHandBone;
    public Transform LeftHandBone;
    public Transform HeadController;
    public Transform RightHandController;
    public Transform LeftHandController;
    private Quaternion HeadBoneStartRotation;
    private Quaternion HeadControllerStartRotation;

    private void Start()
    {
        HeadControllerStartRotation = HeadController.rotation;
        HeadBoneStartRotation = HeadBone.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion diff = Quaternion.Inverse(HeadControllerStartRotation) * HeadController.rotation;
        HeadBone.rotation = diff * HeadBoneStartRotation;
        HeadBone.position = HeadController.position;

        RightHandBone.position = RightHandController.position;
        LeftHandBone.position = LeftHandController.position;

        RightHandBone.rotation = RightHandController.rotation;
        LeftHandBone.rotation = LeftHandController.rotation;
    }
}
