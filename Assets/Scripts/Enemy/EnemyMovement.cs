using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float knockbackStrength;
    private Rigidbody2D body2d;
    private Transform target;
    private bool enteredAreaVision;
    private bool enteredAreaAttack;
    private Animator animator;
    private EnemyVisionArea enemyVisionArea;
    private EnemyAttackArea enemyAttackArea;
    private BoxCollider2D enemyAttackAreaCollider;
    private float direction;
    private const float attackDuration = 0.7f;
    private const float damageTakenDuration = 0.25f;
    private bool isAttacking;
    private float attackCooldown;
    private float damageTakenCooldown;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer spriteRenderer1;
    private bool damageTaken;
    
    private static readonly int AnimState = Animator.StringToHash("AnimState");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Hurt = Animator.StringToHash("Hurt");

    // Start is called before the first frame update
    private void Start()
    {
        spriteRenderer1 = GetComponent<SpriteRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        body2d = GetComponent<Rigidbody2D>();
        body2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyVisionArea = transform.Find("VisionArea").GetComponent<EnemyVisionArea>();
        enemyAttackArea = transform.Find("AttackArea").GetComponent<EnemyAttackArea>();;
        enemyAttackAreaCollider = enemyAttackArea.GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        enteredAreaVision = enemyVisionArea.enteredArea;
        enteredAreaAttack = enemyAttackArea.enteredArea;
        
        AttackTimer();
        
        var targetPosition = target.position;
        var currentPosition = transform.position;
        var faceDirection = currentPosition.x - targetPosition.x;
        SwitchDirection(faceDirection);
        if(!damageTaken)
        {
            animator.SetBool(Hurt, false);
            damageTakenCooldown = 0;
            switch (enteredAreaVision)
            {
                case true when target:
                    if (!enteredAreaAttack && !isAttacking)
                    {
                        Move(currentPosition, targetPosition);
                    }
                    else
                    {
                        animator.SetBool(Attack, true);
                        isAttacking = true;
                    }

                    break;
                case false:
                    animator.SetInteger(AnimState, 0);
                    break;
            }
        }
        else
        {
            animator.SetBool(Hurt, true);
            var transform1 = transform;
            var position = transform1.position;
            position = new Vector2(position.x + direction * knockbackStrength*Time.deltaTime, position.y + knockbackStrength*Time.deltaTime);
            transform1.position = position;
            
            DamageTakenTimer();
        }
    }

    private void Move(Vector2 currentPosition, Vector2 targetPosition)
    {
        currentPosition = Vector2.MoveTowards(currentPosition, new Vector2(targetPosition.x, currentPosition.y), speed * Time.deltaTime);
        animator.SetInteger(AnimState, 1);
        attackCooldown = 0;
        animator.SetBool(Attack, false);
        transform.position = currentPosition;
    }

    private void SwitchDirection(float faceDirection)
    {
        var offsetX = Math.Abs(enemyAttackAreaCollider.offset.x);
        switch (faceDirection)
        {
            case > 0:
                direction = 1;
                spriteRenderer.flipX = false;
                enemyAttackAreaCollider.offset = new Vector2(-offsetX, enemyAttackAreaCollider.offset.y);
                break;
            case < 0:
                direction = -1;
                spriteRenderer1.flipX = true;
                enemyAttackAreaCollider.offset = new Vector2(offsetX, enemyAttackAreaCollider.offset.y);
                break;
        }
    }

    private void AttackTimer()
    {
        if (!isAttacking) return;
        attackCooldown += Time.deltaTime;
        if (attackCooldown >= attackDuration)
        {
            isAttacking = false;
        }
    }

    private void DamageTakenTimer()
    {
        damageTakenCooldown += Time.deltaTime;
        if (damageTakenCooldown >= damageTakenDuration)
        {
            damageTaken = false;
        }
    }

    public void DamageTaken()
    {
        damageTaken = true;
    }
}
