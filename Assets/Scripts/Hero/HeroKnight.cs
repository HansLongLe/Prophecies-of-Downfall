using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroKnight : MonoBehaviour, IDamageable
{
    [HideInInspector] public bool dealtDamage;
    public float rollCooldown = 5.0f;

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float rollForce;
    [SerializeField] private GameObject slideDust;

    private Animator animator;
    private Rigidbody2D body2d;
    private SensorHeroKnight groundSensor;
    private SensorHeroKnight wallSensorR1;
    private SensorHeroKnight wallSensorR2;
    private SensorHeroKnight wallSensorL1;
    private SensorHeroKnight wallSensorL2;
    
    public delegate void HeroKnightFunctionWithoutArgs();
    public static event HeroKnightFunctionWithoutArgs Rolled;
    public static event HeroKnightFunctionWithoutArgs Attacked;

    public delegate void HeroKnightFunctionWithInt(int number);
    public static event HeroKnightFunctionWithInt TurnedAround;
    public static event HeroKnightFunctionWithInt DamageTaken;


    private bool isWallSliding;
    private bool grounded = true;
    private bool rolling;
    private bool canRoll = true;
    private float rollCurrentTime;
    private float facingDirection = 1;
    private int currentAttack;
    private float timeSinceAttack;
    private float rollDuration;
    private bool blocking;
    private Vector2 inputMovement;
    private bool isDead = false;

    private static readonly int AnimState = Animator.StringToHash("AnimState");
    private static readonly int Grounded = Animator.StringToHash("Grounded");
    private static readonly int AirSpeedY = Animator.StringToHash("AirSpeedY");
    private static readonly int WallSlide = Animator.StringToHash("WallSlide");
    private static readonly int Block = Animator.StringToHash("Block");
    private static readonly int IdleBlock = Animator.StringToHash("IdleBlock");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Roll = Animator.StringToHash("Roll");
    private static readonly int Hurt = Animator.StringToHash("Hurt");
    private static readonly int Death = Animator.StringToHash("Death");


    // Use this for initialization
    private void Start()
    {
        PlayerHealth.ZeroHealth += PlayerDied;
        animator = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();
        groundSensor = transform.Find("GroundSensor").GetComponent<SensorHeroKnight>();
        wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<SensorHeroKnight>();
        wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<SensorHeroKnight>();
        wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<SensorHeroKnight>();
        wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<SensorHeroKnight>();
        animator.SetBool(Grounded, grounded);
        foreach (var animationClip in animator.runtimeAnimatorController.animationClips)
        {
            if (animationClip.name == "HeroKnight_Roll")
            {
                rollDuration = animationClip.length;
            }
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //Attack combo timer
        timeSinceAttack += Time.deltaTime;
        
        //Set AirSpeed in animator
        animator.SetFloat(AirSpeedY, body2d.velocity.y);
        
    }

    public void OnAttack(InputAction.CallbackContext value)
    {
        if (!value.started || !(timeSinceAttack > 0.25f) || rolling || isDead) return;
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
        
    }

    public void OnBlock(InputAction.CallbackContext value)
    {
        if(isDead) return;
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
        if (!grounded || rolling || isDead) return;
        animator.SetTrigger(Jump);
        grounded = false;
        animator.SetBool(Grounded, grounded);
        body2d.velocity = new Vector2(body2d.velocity.x, jumpForce);
        groundSensor.Disable(0.2f);
        StartCoroutine(InAir());
    }

    private IEnumerator InAir()
    {
        while (!grounded)
        {
            //Check if character just landed on the ground
            if (groundSensor.State())
            {
                grounded = true;
                animator.SetBool(Grounded, grounded);
            }

            //Check if character just started falling
            if (!groundSensor.State())
            {
                grounded = false;
                animator.SetBool(Grounded, grounded);
            }
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
            yield return new WaitForFixedUpdate();
        }
    }

    public void OnRoll(InputAction.CallbackContext value)
    {
        if (!canRoll || isWallSliding || isDead) return; 
        rollCurrentTime = 0;
        animator.SetTrigger(Roll);
        Rolled?.Invoke();
        StartCoroutine(IsRolling());
    }

    private IEnumerator IsRolling()
    {
        while (rollCurrentTime < rollDuration + rollCooldown)
        {
            while (rollCurrentTime < rollDuration)
            {
                rolling = true;
                canRoll = false;
                rollCurrentTime += Time.deltaTime;
                body2d.velocity = new Vector2(facingDirection * rollForce, body2d.velocity.y);
                yield return new WaitForFixedUpdate();
            }
            rolling = false;
            rollCurrentTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        canRoll = true;
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        if(isDead) return;
        inputMovement = value.ReadValue<Vector2>();
        var moving = value.phase;

        switch (inputMovement.x)
        {
            // Swap direction of sprite depending on walk direction
            case > 0:
                GetComponent<SpriteRenderer>().flipX = false;
                TurnedAround?.Invoke(1);
                facingDirection = inputMovement.x;
                break;
            case < 0:
                GetComponent<SpriteRenderer>().flipX = true;
                TurnedAround?.Invoke(-1);
                facingDirection = inputMovement.x;
                break;
        }
        animator.SetInteger(AnimState, 1);
        StartCoroutine(Move(moving));
    }

    private IEnumerator Move(InputActionPhase moving)
    {
        while (moving == InputActionPhase.Performed && !blocking)
        {
            body2d.velocity = new Vector2(inputMovement.x * speed, body2d.velocity.y);
            yield return new WaitForFixedUpdate();
        }
        animator.SetInteger(AnimState, 0);
    }

    private void PlayerDied()
    {
        animator.SetTrigger(Death);
        isDead = true;
    }

    public void TakeDamage(int amount)
    {
        if(isDead || blocking || rolling) return;
        animator.SetTrigger(Hurt);
        DamageTaken?.Invoke(amount);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
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

    private void AttackAnimation()
    {
        Attacked?.Invoke();
    }
}
