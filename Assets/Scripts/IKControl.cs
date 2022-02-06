using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]

// Adapted from https://docs.unity3d.com/Manual/InverseKinematics.html
public class IKControl : MonoBehaviour
{
    protected Animator animator;

    public Transform rightHandObj = null;
    public Transform leftHandObj = null;
    public Transform headObj = null;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void SetWeights()
    {
        animator.SetLookAtWeight(1);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
    }

    //a callback for calculating IK
    void OnAnimatorIK()
    {
        SetWeights();
        animator.SetLookAtPosition(headObj.position + headObj.forward);

        // Set the right hand target position and rotation
        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);

        // Set the right hand target position and rotation
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
    }
}