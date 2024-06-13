using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [field: SerializeField] public float MoveSpeed { get; private set; }
    [field: SerializeField] public DirectionalAnimationSet Idle {  get; private set; }

    private Vector2 moveInput = Vector2.zero;
    private Vector2 facingDirection = Vector2.zero;
    private Rigidbody2D rb;
    private Animator animator;
    private DirectionalAnimationSet currentAnimationSet;
    private AnimationClip currentClip;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentAnimationSet = Idle;
    }

    private void Update()
    {
        AnimationClip expectedClip = currentAnimationSet.GetFacingClip(facingDirection);

        if (currentClip == null || currentClip != expectedClip)
        {
            //Need a new animation so change it
            animator.Play(expectedClip.name);
            currentClip = expectedClip;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * MoveSpeed * Time.fixedDeltaTime);
    }


    void OnMove(InputValue value) // run by inputsystem on move action
    {
        moveInput = value.Get<Vector2>();

        if (moveInput != Vector2.zero)
        {
            facingDirection = moveInput; 
        }
    }

    void OnFire()
    {
        print("Shots fired");
    }
}
