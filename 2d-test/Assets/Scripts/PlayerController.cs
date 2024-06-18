using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    [field: SerializeField] public float MoveSpeed { get; private set; }
    [field: SerializeField] public CharacterState Idle {  get; private set; }
    [field: SerializeField] public CharacterState Walk { get; private set; }
    [field: SerializeField] public CharacterState Use { get; private set; }
    [field: SerializeField] public StateAnimationSetDictionary StateAnimations { get; private set; }

    private Vector2 moveInput = Vector2.zero;
    private Vector2 facingDirection = Vector2.zero;
    private Rigidbody2D rb;
    private Animator animator;
    private CharacterState currentState;
    private AnimationClip currentClip;

    //testing
    public Collider2D waterCollider;
    public Tilemap waterTileMap;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentState = Idle;
    }

    private void Update()
    {

        AnimationClip expectedClip = StateAnimations.GetFacingClipFromState(currentState, facingDirection);

        if (currentClip == null || currentClip != expectedClip)
        {
            //Need a new animation so change it
            animator.Play(expectedClip.name);
            currentClip = expectedClip;
        }

        //Debug.Log(waterCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    

    void FixedUpdate()
    {
        if (currentState.CanMove) //stop movement if using item, fishing rod for example
        {
            //rb.MovePosition(rb.position + moveInput * MoveSpeed * Time.fixedDeltaTime);
            //rb.AddForce(MoveSpeed * moveInput * Time.fixedDeltaTime);
            rb.velocity = MoveSpeed * moveInput;
        }
        
    }


    void OnMove(InputValue value) // run by inputsystem on move action
    {
        moveInput = value.Get<Vector2>(); //Set movement vector to be used in fixedupdate

        if (moveInput != Vector2.zero)
        {
            facingDirection = moveInput;
            currentState = Walk;  // set animation state to walking
        }
        else
        {
            currentState = Idle; // no movement so set animation state to idle
        }
    }

    void OnFire()
    {
        print("Shots fired");
        TileBase tile = FindObjectOfType<TileManager>().GetTileAtMousePosition(waterTileMap);
        Debug.Log(tile);



    }
}
