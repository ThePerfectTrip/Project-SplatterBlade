
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour {

    const string MOVE_STATE_NAME = "move_state";
    const string ACTION_STATE_NAME = "action_state";
    // Move States
    const int MOVE_IDLE    = 00;
    const int MOVE_RUNNING = 01;
    const int MOVE_AIRBORNE = 02;
    // Action States
    const int ACTION_NONE = 0;
    const int ACTION_WIDE_SHOOT = 1;
    const int ACTION_WIDE_KICK = 2;

    const int SAWBLADE_LAYER = 9;

    // Enums for clarity
    enum MovementType
    {
        Run,
        Jump,
        Grapple
    }
    enum PlayerStatus
    {
        Idle = 0,
        Running = 1,
        Airborne = 2,
        Damaged = 4
    }
    enum PlayerWeapon
    {
        None,
        Kick,
        Sawblade
    }
    enum AttackType
    {
        Narrow,
        Wide
    }
    enum AttackStatus
    {
        Set,
        Fire
    }
    enum ShredderMove
    {
        None,
        Comet,
        Meteor
    }

    // Forces for sprite movement
    [Header("Basic Stats")]
    public float health = 100f;
    public float luster = 100f;
    public float runVelocity = 128f;
    public float groundedJumpVelocity = 128f;
    public float midairJumpVelocity = 36f;

    [Header("Advanced Stats")]
    [Header("Jump")]
    public float groundedJumpDuration = 0.5f;
    public float midairJumpDuration = 0.2f;
    public float jumpCount = 1;
    public float jumpGravity = 0;
    [Header("Grapple")]
    public float grappleDelay = 0.1f;
    public float grappleCooldown = 1f;
    public float grappleVelocity = 450f;
    public GameObject grapplingHookProjectile;
    [Header("Kick Attacks")]
    public float kickDelay = 0.4f;
    public float kickDuration = 0.2f;
    public float kickCooldown = 0.1f;
    public GameObject narrowKickProjectile;
    public GameObject wideKickProjectile;
    [Header("Sawblade Attacks")]
    public float shotDelay = 0.1f;
    public float shotCooldown = 0.1f;
    public GameObject narrowShotProjectile;
    public GameObject wideShotProjectile;
    public int currentSawblades;
    [Header("Shredder Attacks")]
    public GameObject shredderCometAttack;
    public GameObject shredderMeteorAttack;

    [Header("Low-level Stats")]
    public LayerMask whatIsGround;
    public Transform meleeFiringPoint;
    public Transform rangedFiringPoint;
    public Transform groundCheck;
    public float groundRadius = 2.0f;

    [Header("Camera Stats")]
    public Camera playerPerspective;
    public float fieldOfView = 5f;
    public bool followHorizontalMovement;
    public bool followVerticalMovement;

    [Header("Audio Stats")]
    public AudioClip sawShot;
    AudioSource source;

    // Clarity Variables
    Animator animator;
    Rigidbody2D bodyPhysics;

    // Tracker Variables
    [HideInInspector]
    public bool facingRight = true;
    [HideInInspector]
    public bool hasSawblade
    {
        get
        {
            return currentSawblades > 0;
        }
    }
    float defaultGravityScale;
    bool hasGravityChanged;
    GameObject grappleObject;

    // Input Variables
    string horizontalAxisName = "Horizontal";
    string verticalAxisName = "Vertical";
    string jumpButtonName = "Jump";
    string narrowButtonName = "Narrow";
    string wideButtonName = "Wide";
    string grappleButtonName = "Grapple";

    // Input Tracking & Smoothing Variables
    float horizontalDeadzone = 0.25f;
    float verticalDeadzone = 0.15f;
    float horizontalAxis;
    float verticalAxis;
    bool isJumpInputTapped;
    bool isJumpInputHeld;
    bool isNarrowInputTapped;
    bool isNarrowInputReleased;
    bool isWideInputTapped;
    bool isWideInputReleased;
    bool isGrappleInputTapped;
    bool isGrappleInputReleased;


    // Status Variables
    bool isGrounded;
    bool isMoving;
    float currentRunVelocity;
    float decelerationSpeed;

    bool isJumping;
    bool isJumpStopping;
    float jumpVelocity;
    float jumpCancelVelocity;
    float jumpCompleteVelocity;
    float jumpDuration;
    float jumpDurationLeft;
    float jumpCountLeft;

    bool isAttacking;
    float attackDelayLeft;
    float attackDurationLeft;
    PlayerStatus playerStatus;
    PlayerWeapon attackWeapon;
    AttackType attackType;
    AttackStatus attackStatus;

    bool isGrappling;
    Vector2 currentGrappleVelocity;
    ShredderMove currentShredderMove;
    Vector2 currentShredderVelocity;


    // Perspective Variables
    Vector3 perspectivePosition;
    
    void Start ()
    {
        bodyPhysics = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();

        jumpCancelVelocity = 100f;
        jumpCompleteVelocity = 210f;
        attackDelayLeft = 0;
        
        perspectivePosition = playerPerspective.transform.position;
        perspectivePosition.z = fieldOfView <= 0? playerPerspective.transform.position.z : -fieldOfView;
    }

    void Update()
    {
        PerspectiveUpdate();
        InputUpdate();
        StatusUpdate();
        AnimationUpdate();
    }

    void FixedUpdate()
    {
        // Determines sprite's proximity to ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

        // Moves sprite based on received input
        if((attackDurationLeft > 0))
        {
            Move(0, MovementType.Run);
        }
        else if (isGrappling)
        {
            Move(currentGrappleVelocity, MovementType.Grapple);
        }
        else if (horizontalAxis != 0 || isGrounded)
        {
            Move(horizontalAxis * currentRunVelocity, MovementType.Run);
        }

        if (verticalAxis > 0)
        {
            //Move(verticalAxis * runVelocity, MovementType.Jump);
        }
        /*else if (bodyPhysics.velocity.x != 0)
        {
            /*decelerationSpeed = Vector2.right * (runVelocity * 3) * Time.deltaTime;
            decelerationSpeed =
                Mathf.Abs(bodyPhysics.velocity.x) > decelerationSpeed.x ?
                    bodyPhysics.velocity.x > 0?
                        -decelerationSpeed
                    :
                        decelerationSpeed
                :
                    Vector2.right * -bodyPhysics.velocity.x
            ;

            bodyPhysics.velocity += decelerationSpeed;
        }*/

        // Screen wrapper
        /*Vector3 viewport = Camera.main.WorldToViewportPoint(transform.position);
        if(viewport.x < 0)
        {
            viewport.x = 1;
        }
        else if(viewport.x > 1)
        {
            viewport.x = 0;
        }
        transform.position = Camera.main.ViewportToWorldPoint(viewport);*/
    }

    void PerspectiveUpdate()
    {
        perspectivePosition.x = followHorizontalMovement ? transform.position.x : perspectivePosition.x;
        perspectivePosition.y = followVerticalMovement ? transform.position.y : perspectivePosition.y;
        playerPerspective.transform.position = perspectivePosition;
    }

    void InputUpdate()
    {
        horizontalAxis = Input.GetAxisRaw(horizontalAxisName);
        verticalAxis = Input.GetAxisRaw(verticalAxisName);

        // Thumbstick Deadzone & Snapping
        horizontalAxis =
            horizontalAxis > horizontalDeadzone?
                1
            :
                horizontalAxis < -horizontalDeadzone?
                    -1
                :
                    0
        ;
        verticalAxis =
            verticalAxis > verticalDeadzone?
                1
            :
                0
        ;

        isJumpInputTapped = Input.GetButtonDown(jumpButtonName);
        isJumpInputHeld = Input.GetButton(jumpButtonName);
        isNarrowInputTapped = Input.GetButtonDown(narrowButtonName);
        isNarrowInputReleased = Input.GetButtonUp(narrowButtonName);
        isWideInputTapped = Input.GetButtonDown(wideButtonName);
        isWideInputReleased = Input.GetButtonUp(wideButtonName);
        isGrappleInputTapped = Input.GetButtonDown(grappleButtonName);
        isGrappleInputReleased = Input.GetButtonUp(grappleButtonName);
    }

    void StatusUpdate()
    {
        // Clarity Variables
        bool canJump = jumpCountLeft > 0 && !(attackDurationLeft > 0) && !isGrappling;
        bool hasJumped = !(jumpCountLeft == jumpCount);

        // Moving
        isMoving = bodyPhysics.velocity.x != 0;
        currentRunVelocity =
            !isGrounded && Mathf.Abs(bodyPhysics.velocity.x) > runVelocity?
                Mathf.Abs(bodyPhysics.velocity.x)
            :
                runVelocity
        ;

        // Jumping
        jumpCountLeft =
            isGrounded?
                jumpCount
            :
                !hasJumped?
                    jumpCount - 1
                :
                    jumpCountLeft
        ;
        isJumping = jumpDurationLeft > 0;
        jumpDurationLeft =
            isJumping?
                    jumpDurationLeft -= Time.deltaTime
                :
                    0
        ;
        isJumpStopping = isJumping && (!(jumpDurationLeft > 0) || !isJumpInputHeld);
        if (isJumpInputTapped && canJump)
        {
            jumpVelocity =
                isGrounded?
                    groundedJumpVelocity
                :
                    midairJumpVelocity
            ;
            jumpDuration =
                 isGrounded ?
                     groundedJumpDuration
                 :
                     midairJumpDuration
             ;
            ChangeGravity(jumpGravity);
            jumpDurationLeft = jumpDuration;
            jumpCountLeft -= 1;
        }
        if (isJumping)
        {
            Move(jumpVelocity / jumpDuration, MovementType.Jump);
        }
        if (isJumpStopping)
        {
            RevertGravity();
            float endingLength =
                isJumpInputHeld?
                    jumpCompleteVelocity
                :
                    jumpCancelVelocity
            ;
            Move(endingLength, MovementType.Jump);
            jumpDurationLeft = 0;
        }

        // Attacking
        attackDurationLeft -=
            isAttacking?
                !(attackDelayLeft > 0)?
                    Time.deltaTime
                :
                    0
            :
                attackDurationLeft
        ;
        attackDelayLeft -=
            isAttacking?
                Time.deltaTime
            :
                attackDelayLeft
        ;
        if ((isNarrowInputTapped || isWideInputTapped) && !isAttacking)
        {
            attackWeapon =
                hasSawblade?
                    PlayerWeapon.Sawblade
                    :
                    PlayerWeapon.Kick
            ;
            attackType =
                isNarrowInputTapped?
                    AttackType.Narrow
                :
                    AttackType.Wide
            ;
            attackDelayLeft =
                hasSawblade?
                    shotDelay
                :
                    kickDelay
            ;
            attackDurationLeft =
                !hasSawblade?
                    kickDuration
                :
                    attackDurationLeft
            ;
        }
        isAttacking = isAttacking || attackDelayLeft > 0 || attackDurationLeft > 0;
        attackStatus =
            attackDelayLeft <= 0?
                AttackStatus.Fire
            :
                AttackStatus.Set
        ;
        if (isAttacking && attackStatus == AttackStatus.Fire)
        {
            if (attackWeapon == PlayerWeapon.Sawblade)
            {
                Attack(attackWeapon, attackType);
            }
            else if (attackDurationLeft == kickDuration)
            {
                Debug.Log(attackWeapon);
                Attack(attackWeapon, attackType);
            }
        }
        if(!(attackDelayLeft > 0 || attackDurationLeft > 0))
        {
            isAttacking = false;
            attackWeapon = PlayerWeapon.None;
            attackStatus = AttackStatus.Set;
        }


        // Grapple
        isGrappling = currentGrappleVelocity != Vector2.zero && grappleObject != null;
        if (isGrappleInputTapped && grappleObject != null && !isGrappling)
        {
            Grapple(grappleObject);
        }
        else if (grappleObject == null)
        {
            currentGrappleVelocity = Vector2.zero;
        }

        // Shredder Moves

    }

    void AnimationUpdate()
    {
        // Determines whether sprite should be flipped to be directionally correct 
        if (horizontalAxis > 0 && !facingRight && attackStatus != AttackStatus.Fire)
        {
            Flip();
        }
        else if (horizontalAxis < 0 && facingRight && attackStatus != AttackStatus.Fire)
        {
            Flip();
        }

        // Set movementType accordingly
        playerStatus =
            !isGrounded?
                PlayerStatus.Airborne
            : 
                isMoving?
                    PlayerStatus.Running
                :
                    PlayerStatus.Idle
        ;

        switch (playerStatus)
        {
            case PlayerStatus.Airborne:
                animator.SetInteger(MOVE_STATE_NAME, MOVE_AIRBORNE);
                break;
            case PlayerStatus.Running:
                animator.SetInteger(MOVE_STATE_NAME, MOVE_RUNNING);
                break;
            case PlayerStatus.Idle:
                animator.SetInteger(MOVE_STATE_NAME, MOVE_IDLE);
                break;
        }

        switch (attackWeapon)
        {
            case PlayerWeapon.Sawblade:
                animator.SetInteger(ACTION_STATE_NAME, ACTION_WIDE_SHOOT);
                break;
            case PlayerWeapon.Kick:
                animator.SetInteger(ACTION_STATE_NAME, ACTION_WIDE_KICK);
                break;
            case PlayerWeapon.None:
                animator.SetInteger(ACTION_STATE_NAME, ACTION_NONE);
                break;
        }
    }

    void Move(float speed, MovementType direction)
    {
        switch(direction)
        {
            case MovementType.Run:
                Move(speed * Vector2.right, MovementType.Run);
                break;
            case MovementType.Jump:
                Move(speed * Vector2.up, MovementType.Jump);
                break;
            case MovementType.Grapple:
                // TODO: Remove later.
                Debug.Log("Don't do this.");
                break;
        }
    }

    void Move(Vector2 speed, MovementType direction)
    {
        switch (direction)
        {
            case MovementType.Run:
                bodyPhysics.velocity = new Vector2(speed.x, bodyPhysics.velocity.y);
                break;
            case MovementType.Jump:
                bodyPhysics.velocity = new Vector2(bodyPhysics.velocity.x, speed.y);
                break;
            case MovementType.Grapple:
                bodyPhysics.velocity = speed;
                break;
        }
    }

    void Attack(PlayerWeapon weapon, AttackType type)
    {
        GameObject kick;
        switch (weapon)
        {
            case PlayerWeapon.Kick:
                switch (type)
                {
                    case AttackType.Narrow:
                        kick = Instantiate(narrowKickProjectile, gameObject.transform);
                        kick.GetComponent<MeleeAttack>().Initialize(this, attackDurationLeft);
                        break;
                    case AttackType.Wide:
                        kick = Instantiate(wideKickProjectile, gameObject.transform);
                        kick.GetComponent<MeleeAttack>().Initialize(this, attackDurationLeft);
                        break;
                }
                break;
            case PlayerWeapon.Sawblade:
                switch (type)
                {
                    case AttackType.Narrow:
                        grappleObject = Instantiate(narrowShotProjectile, rangedFiringPoint.transform.position, rangedFiringPoint.transform.rotation);
                        grappleObject.GetComponent<SawbladeMovement>().Initialize(this);
                        break;
                    case AttackType.Wide:
                        grappleObject = Instantiate(wideShotProjectile, rangedFiringPoint.transform.position, rangedFiringPoint.transform.rotation);
                        grappleObject.GetComponent<SawbladeMovement>().Initialize(this);
                        break;
                }
                source.PlayOneShot(sawShot);
                currentSawblades -= 1;
                break;
        }
    }

    void Grapple(GameObject _grappleObject)
    {
        Vector2 targetPosition = _grappleObject.transform.position - transform.position;
        float angle = Mathf.Atan2(targetPosition.y, targetPosition.x);

        currentGrappleVelocity = new Vector2(grappleVelocity * Mathf.Cos(angle), grappleVelocity * Mathf.Sin(angle));
        if(_grappleObject.GetComponent<SawbladeMovement>() != null)
        {
            _grappleObject.GetComponent<SawbladeMovement>().launchSpeed = 0;
        }
    }

    void PerformShredderMove(ShredderMove moveType)
    {
        switch (currentShredderMove)
        {
            case ShredderMove.Comet:
                Instantiate(shredderCometAttack, transform);
                break;
            case ShredderMove.Meteor:
                Instantiate(shredderMeteorAttack, transform);
                break;
        }
    }

    void ChangeGravity(float newGravity)
    {
        defaultGravityScale =
            !hasGravityChanged?
                bodyPhysics.gravityScale
            :
                defaultGravityScale
        ;
        bodyPhysics.gravityScale = newGravity;
        hasGravityChanged = true;
    }

    void RevertGravity()
    {
        bodyPhysics.gravityScale = defaultGravityScale;
        hasGravityChanged = false;
    }

    // Horizontally flips the sprite, forces and all
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void GiveSawblade()
    {
        currentSawblades += 1;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        SawbladeMovement sawblade = col.GetComponent<SawbladeMovement>();
        if (sawblade != null)
        {
            if (isGrappling)
            {
                currentShredderMove =
                    col.gameObject.tag == "Narrow"?
                        ShredderMove.Comet
                    :
                        col.gameObject.tag == "Wide"?
                            ShredderMove.Meteor
                        :
                            ShredderMove.None
                ;

                if(col.gameObject.tag == "Narrow")
                {
                    ChangeGravity(jumpGravity);
                }

                currentShredderVelocity = currentGrappleVelocity;
            }

            if(!sawblade.justFired)
            {
                Destroy(col.gameObject);
                GiveSawblade();
            }
        }
    }
    
    void OnTriggerExit2D(Collider2D col)
    {
        SawbladeMovement sawblade = col.GetComponent<SawbladeMovement>();
        if (sawblade.justFired)
        {
            sawblade.justFired = false;
        }
    }
}
