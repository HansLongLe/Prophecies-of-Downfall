using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroKnight : MonoBehaviour
{

    [HideInInspector] public bool canRoll = true;
    [HideInInspector] public bool dealtDamage;
    public float rollCooldown = 5.0f;

    [SerializeField] private float speed = 4.0f;
    [SerializeField] private float jumpForce = 7.5f;
    [SerializeField] private float rollForce = 6.0f;
    [SerializeField] private GameObject slideDust;

    private Animator animator;
    private Rigidbody2D body2d;
    private HeroAttackArea heroAttackArea;
    private BoxCollider2D attackAreaCollider;
    private SensorHeroKnight groundSensor;
    private SensorHeroKnight wallSensorR1;
    private SensorHeroKnight wallSensorR2;
    private SensorHeroKnight wallSensorL1;
    private SensorHeroKnight wallSensorL2;

    private bool isWallSliding;
    private bool grounded;
    private bool rolling;
    private float facingDirection = 1;
    private int currentAttack;
    private float timeSinceAttack;
    private float delayToIdle;
    private const float rollDuration = 8.0f / 14.0f;
    private float rollCurrentTime;
    private bool blocking;
    private Vector2 inputMovement;
    private InputActionPhase moving;
    
    private static readonly int AnimState = Animator.StringToHash("AnimState");
    private static readonly int Grounded = Animator.StringToHash("Grounded");
    private static readonly int AirSpeedY = Animator.StringToHash("AirSpeedY");
    private static readonly int WallSlide = Animator.StringToHash("WallSlide");
    private static readonly int Block = Animator.StringToHash("Block");
    private static readonly int IdleBlock = Animator.StringToHash("IdleBlock");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Roll = Animator.StringToHash("Roll");


    // Use this for initialization
    private void Start()
    {
        animator = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();
        heroAttackArea = transform.Find("AttackArea").GetComponent<HeroAttackArea>();
        attackAreaCollider = transform.Find("AttackArea").GetComponent<BoxCollider2D>();
        groundSensor = transform.Find("GroundSensor").GetComponent<SensorHeroKnight>();
        wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<SensorHeroKnight>();
        wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<SensorHeroKnight>();
        wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<SensorHeroKnight>();
        wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<SensorHeroKnight>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //attack combo timer
        timeSinceAttack += Time.deltaTime;
        
        //Check if character just landed on the ground
        if (!grounded && groundSensor.State())
        {
            grounded = true;
            animator.SetBool(Grounded, grounded);
        }

        //Check if character just started falling
        if (grounded && !groundSensor.State())
        {
            grounded = false;
            animator.SetBool(Grounded, grounded);
        }

        //Set AirSpeed in animator
        animator.SetFloat(AirSpeedY, body2d.velocity.y);

        // -- Handle Animations --
        //Wall Slide
        isWallSliding = (wallSensorR1.State() && wallSensorR2.State()) ||
                          (wallSensorL1.State() && wallSensorL2.State());
        animator.SetBool(WallSlide, isWallSliding);
        if (isWallSliding)
        {
            body2d.drag = 8;
        }
        else
        {
            body2d.drag = 1;
        }

            // Move
        if (moving == InputActionPhase.Performed && !rolling && !blocking)
        {
            body2d.velocity = new Vector2(inputMovement.x * speed, body2d.velocity.y);
            // Reset timer
            delayToIdle = 0.05f;
            animator.SetInteger(AnimState, 1);
        }
        else
        {
            // Prevents flickering transitions to idle
            delayToIdle -= Time.deltaTime;
            if(delayToIdle < 0)
                animator.SetInteger(AnimState, 0);
        }
        
        //roll
        // Disable rolling if timer extends duration
        if (rollCurrentTime > rollDuration)
            rolling = false;

        if (rollCurrentTime > rollDuration + rollCooldown)
            canRoll = true;
        //roll timer
        if (!canRoll)
            rollCurrentTime += Time.deltaTime;
        else
        {
            rollCurrentTime = 0;
        }
    }

    public void OnAttack(InputAction.CallbackContext value)
    {
        if (!value.started || !(timeSinceAttack > 0.25f) || rolling) return;
        currentAttack++;

        // Loop back to one after third attack
        if (currentAttack > 3)
            currentAttack = 1;

        // Reset Attack combo if time since last attack is too large
        if (timeSinceAttack > 1.0f)
            currentAttack = 1;

        // Call one of three attack animations "Attack1", "Attack2", "Attack3"
        animator.SetTrigger("Attack" + currentAttack);

        // Reset timer
        timeSinceAttack = 0.0f;

        if (!heroAttackArea.enteredArea) return;
        var enemy = heroAttackArea.enemy.GetComponent<EnemyMovement>();
        dealtDamage = true;
        enemy.DamageTaken();
    }

    public void OnBlock(InputAction.CallbackContext value)
    {
        if (value.started && !rolling)
        {
            animator.SetTrigger(Block);
            animator.SetBool(IdleBlock, true);
            blocking = true;
        }
        else if (value.canceled)
        {
            animator.SetBool(IdleBlock, false);
            blocking = false;
        }
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (!grounded || rolling) return;
        animator.SetTrigger(Jump);
        grounded = false;
        animator.SetBool(Grounded, grounded);
        body2d.velocity = new Vector2(body2d.velocity.x, jumpForce);
        groundSensor.Disable(0.2f);
    }

    public void OnRoll(InputAction.CallbackContext value)
    {
        if (!canRoll || isWallSliding) return;
        rolling = true;
        canRoll = false;
        animator.SetTrigger(Roll);
        body2d.velocity = new Vector2(facingDirection * rollForce, body2d.velocity.y);
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        inputMovement = value.ReadValue<Vector2>();
        moving = value.phase;
        var offsetX = Math.Abs(attackAreaCollider.offset.x);

        switch (inputMovement.x)
        {
            // Swap direction of sprite depending on walk direction
            case > 0:
                GetComponent<SpriteRenderer>().flipX = false;
                attackAreaCollider.offset =  new Vector2(offsetX, attackAreaCollider.offset.y);
                facingDirection = inputMovement.x;
                break;
            case < 0:
                GetComponent<SpriteRenderer>().flipX = true;
                attackAreaCollider.offset =  new Vector2(-offsetX, attackAreaCollider.offset.y);
                facingDirection = inputMovement.x;
                break;
        }
    }

// Animation Events
    // Called in slide animation.
    private void AE_SlideDust()
    {
        var spawnPosition = Math.Abs(facingDirection - 1) == 0 ? wallSensorR2.transform.position : wallSensorL2.transform.position;

        if (slideDust == null) return;
        // Set correct arrow spawn position
        var dust = Instantiate(slideDust, spawnPosition, gameObject.transform.localRotation);
        // Turn arrow in correct direction
        dust.transform.localScale = new Vector3(facingDirection, 1, 1);
    }
}
