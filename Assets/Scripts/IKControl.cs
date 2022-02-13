using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]

// Adapted from https://docs.unity3d.com/Manual/InverseKinematics.html
public class IKControl : MonoBehaviour
{
    protected Animator animator;

    public Transform rightHandTarget = null;
    public Transform leftHandTarget = null;
    public Transform headTarget = null;
    public Transform headBone = null;
    public Transform spineBone = null;
    private Vector3 startSpineUp;
    private Quaternion startRotationBetween;
    private Quaternion spineStartRotation;
    private Quaternion spineLocalRotationIKGoal;
    private float headToChestOffset;

    void Start()
    {
        animator = GetComponent<Animator>();
        spineStartRotation = spineBone.rotation;
        headToChestOffset = Vector3.Distance(headBone.position, spineBone.position);
        Vector3 torsoLine = spineBone.position - headBone.position;
        startSpineUp = spineBone.up;
        startRotationBetween = Quaternion.FromToRotation(startSpineUp, torsoLine);
    }

    void SetWeights()
    {
        animator.SetLookAtWeight(1);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
    }

    private void LateUpdate()
    {
        headBone.position = headTarget.position;

        Vector3 torsoLine = spineBone.position - headBone.position;
        Quaternion newRotationBetween = Quaternion.FromToRotation(startSpineUp, torsoLine);
        Quaternion rotationBefore = spineBone.rotation;
        spineBone.rotation = (newRotationBetween * Quaternion.Inverse(startRotationBetween)) * spineStartRotation;
        spineLocalRotationIKGoal = spineBone.localRotation;
        spineBone.rotation = rotationBefore;

        // Headbone will have moved due to above rotation, so need to RE-set the position to match goal
        spineBone.position = headTarget.position + torsoLine.normalized * headToChestOffset;
        headBone.position = headTarget.position;
        headBone.eulerAngles = headTarget.eulerAngles;

        // May need to find + restore all descenents of the spine bone, but unsure how necessary/difficult that'll be. Will see if the auto IK can handle that
    }

    //a callback for calculating IK
    void OnAnimatorIK()
    {
        SetWeights();

        animator.SetBoneLocalRotation(HumanBodyBones.Spine, spineLocalRotationIKGoal);

        animator.SetLookAtPosition(headTarget.position + headTarget.forward);

        // Set the right hand target position and rotation
        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);

        // Set the right hand target position and rotation
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
    }
}
