using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script controll player mouvement using an animator and character controller
/// </summary>
public class CharacterLocomotion : MonoBehaviour
{

    private CharacterController cc;
    private ActiveWeapon activeWeapon;
    private CharacterAiming characterAiming;

    [SerializeField]
    private Animator rigController;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float gravity;
    [SerializeField]
    public float stepDown;
    [SerializeField]
    private float airControl;
    [SerializeField]
    private float jumpDamp;
    [SerializeField]
    private float groundSpeed;
    [SerializeField]
    private float pushPower;

    private Animator animator;
    private Vector2 input;
    private int inputXId;
    private int inputYId;

    private Vector3 velocity;
    private bool isJumping;

    private readonly int isSprintingParam = Animator.StringToHash("isSprinting");

    private Vector3 rootMotion;
    private static readonly int IsJumpingId = Animator.StringToHash("isJumping");
    private static readonly int Roll1 = Animator.StringToHash("roll");

    public static bool isRolling;

    void Start()
    {
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        activeWeapon = GetComponent<ActiveWeapon>();
        characterAiming = GetComponent<CharacterAiming>();

        inputXId = Animator.StringToHash("InputX");
        inputYId = Animator.StringToHash("InputY");
    }

    void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        animator.SetFloat(inputXId, input.x);
        animator.SetFloat(inputYId, input.y);

        UpdateIsSprinting();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            isRolling = true;
            Roll();
        }

        if (rigController.GetCurrentAnimatorStateInfo(3).IsName("weapon_rifle_rolling") || 
            rigController.GetCurrentAnimatorStateInfo(3).IsName("weapon_pistol_rolling"))
        {
            isRolling = true;
        }
        else
        {
            isRolling = false;
        }
    }

    private void Roll()
    {
        animator.SetTrigger(Roll1);
        rigController.SetTrigger(Roll1);
    }

    /// <summary>
    /// Check if the player is sprinting
    /// </summary>
    /// <returns> return true if the player is sprinting</returns>
    private bool IsSprinting()
    {
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        bool isFiring = activeWeapon.IsFiring();
        bool isChangingWeapon = activeWeapon.isChangingWeapon;
        bool isAiming = characterAiming.isAiming;
        return isSprinting && !isFiring && !isChangingWeapon && !isAiming;
    }

    /// <summary>
    /// Update the isSpriting value in the 2 animators
    /// </summary>
    private void UpdateIsSprinting()
    {
        bool isSprinting = IsSprinting();
        animator.SetBool(isSprintingParam, isSprinting);
        rigController.SetBool(isSprintingParam, isSprinting);
    }

    /// <summary>
    /// Get the values of rootMotion from animator
    /// Used to move the player with character controller
    /// </summary>
    private void OnAnimatorMove()
    {
        rootMotion += animator.deltaPosition;
    }

    private void FixedUpdate()
    {
        if (isJumping)
        {
            UpdateInAir();
        }
        else
        {
            UpdateOnGround();
        }
    }

    /// <summary>
    /// Update player movement
    /// </summary>
    private void UpdateOnGround()
    {
        var stepForwardAmount = rootMotion * groundSpeed;
        var stepDownAmount = Vector3.down * stepDown;

        cc.Move(stepForwardAmount + stepDownAmount);
        rootMotion = Vector3.zero;
        if (!cc.isGrounded)
        {
            SetInAir(0);
        }
    }

    /// <summary>
    /// Update player jumping animation
    /// </summary>
    private void UpdateInAir()
    {
        velocity.y -= gravity * Time.fixedDeltaTime;
        Vector3 displacement = velocity * Time.fixedDeltaTime;
        displacement += CalculateAirControl();
        cc.Move(displacement);
        isJumping = !cc.isGrounded;
        rootMotion = Vector3.zero;
        animator.SetBool(IsJumpingId, isJumping);
    }

    /// <summary>
    /// Controll player in air will jumping
    /// </summary>
    /// <returns>Vector3 of deplacement</returns>
    Vector3 CalculateAirControl()
    {
        return ((transform.forward * input.y) + (transform.right * input.x)) * (airControl / 100);
    }

    private void Jump()
    {
        if (!isJumping)
        {
            float jumpVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
            SetInAir(jumpVelocity);
        }
    }

    private void SetInAir(float jumpVelocity)
    {
        isJumping = true;
        velocity = animator.velocity * (jumpDamp * groundSpeed);
        velocity.y = jumpVelocity;
        animator.SetBool(IsJumpingId, true);
    }

    // Push objects when colliding
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
            return;

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3f)
            return;

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * pushPower;
    }

}
