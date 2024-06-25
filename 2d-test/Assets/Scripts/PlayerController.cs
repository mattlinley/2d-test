using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] public float MoveSpeed { get; private set; }
    [field: SerializeField] public CharacterState Idle {  get; private set; }
    [field: SerializeField] public CharacterState Walk { get; private set; }
    [field: SerializeField] public CharacterState Use { get; private set; }
    [field: SerializeField] public StateAnimationSetDictionary StateAnimations { get; private set; }

    private Vector2 moveInput = Vector2.zero;
    public Vector2 facingDirection = Vector2.zero;
    public Rigidbody2D rb;
    private Animator animator;
    private CharacterState currentState;
    private AnimationClip currentClip;
    private UIManager uiManager;

    //testing
    public Collider2D waterCollider;
    public Tilemap waterTileMap;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        uiManager = FindObjectOfType<UIManager>();
    }
    private void Start()
    {
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

        if (uiManager.IsPaused && animator.enabled)
        {
            animator.enabled = false;
        }
        if (!uiManager.IsPaused && !animator.enabled)
        {
            animator.enabled = true;
        }

        //Debug.Log(waterCollider.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    

    void FixedUpdate()
    {
        if (currentState.CanMove) //stop movement if using item, fishing rod for example
        {
            rb.velocity = MoveSpeed * moveInput;
        }
        
    }


    void OnMove(InputValue value) // run by inputsystem on move action
    {
        if (!uiManager.UIOpen)
        {
            moveInput = value.Get<Vector2>(); //Set movement vector to be used in fixedupdate
        }
        

        if (moveInput != Vector2.zero)
        {
            facingDirection = moveInput;
            currentState = Walk;  // set animation state to walking
        }
        else
        {
            currentState = Idle; // no movement so set animation state to idle

            //Round position to nearest pixel
            transform.position = GetPixelPosition();
        }
    }

    /// <summary>
    /// Round position to nearest pixel (16ppu)
    /// </summary>
    /// <returns>rounded position</returns>
    private Vector3 GetPixelPosition()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Round(16 * position.x) / 16;
        position.y = Mathf.Round(16 * position.y) / 16;
        return position;
    }

    void OnFire()
    {

        TileBase tile = FindObjectOfType<TileManager>().GetTileAtMousePosition(waterTileMap);
        Debug.Log(tile);



    }
}
