using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator anim;
    PlayerMovement pm;
    Rigidbody2D rb;

    int groundID;
    int speedID;
    int crouchID;
    int hangingID;
    int fallID;
    void Start()
    {
        anim = GetComponent<Animator>();
        pm = GetComponentInParent<PlayerMovement>();
        rb = GetComponentInParent<Rigidbody2D>();

        groundID = Animator.StringToHash("isOnGround");
        speedID = Animator.StringToHash("speed");
        crouchID = Animator.StringToHash("isCrouching");
        hangingID = Animator.StringToHash("isHanging");
        fallID = Animator.StringToHash("verticalVelocity");
    }

    void Update()
    {
        anim.SetFloat(speedID, Mathf.Abs(pm.xVelocity));
        anim.SetFloat(fallID, rb.velocity.y);
        anim.SetBool(groundID, pm.isOnGround);
        anim.SetBool(crouchID, pm.isCrouch);
        anim.SetBool(hangingID, pm.isHanging);
    }

    public void StepAudio()
    {
        AudioManager.PlayerFootstepAudio();
    }

    public void CrouchStepAudio()
    {
        AudioManager.PlayerCrouchFootstepAudio();
    }
}
