using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour, IDamageableEnemy
{
    [SerializeField] private float speed;
    private Rigidbody2D body2d;
    private bool enteredAreaVision;
    private bool enteredAreaAttack;
    private Animator animator;
    private EnemyVisionArea enemyVisionArea;
    private EnemyAttackArea enemyAttackArea;
    private EnemyHealth healthGameObject;
    private BoxCollider2D enemyAttackBoxCollider;
    private SpriteRenderer spriteRenderer;
    private Coroutine followingCoroutine;
    private Coroutine attackingCoroutine;
    private GameObject sacredTree;
    private Coroutine movingToTreeCoroutine;
    
    private IDamageable currentAttackTarget;
    private const int damage = 5;
    private const int attackCd = 1;
    private bool died = false;
    private readonly List<IDamageable> attackColliderList = new List<IDamageable>();
    private readonly List<IDamageable> movingColliderList = new List<IDamageable>();

    public delegate void EnemyMovementWithGameObject(GameObject gameObject);

    public delegate void EnemyMovementWithoutArgs();

    public static event EnemyMovementWithGameObject DiedEvent;
    public static event EnemyMovementWithoutArgs StartMoving;
    public static event EnemyMovementWithoutArgs StopMoving;
    public static event EnemyMovementWithoutArgs StartAttack;
    public static event EnemyMovementWithoutArgs GettingHit;
    public static event EnemyMovementWithoutArgs Dying;
    

    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Hurt = Animator.StringToHash("Hurt");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Death = Animator.StringToHash("Death");

    // Start is called before the first frame update
    private void Start()
    {
        enemyVisionArea = transform.Find("VisionArea").GameObject().GetComponent<EnemyVisionArea>();
        enemyVisionArea.MoveTowards += MoveTowards;
        enemyVisionArea.StopFollowing += StopFollowing;
        enemyAttackArea = transform.Find("AttackArea").GameObject().GetComponent<EnemyAttackArea>();
        enemyAttackBoxCollider = enemyAttackArea.GetComponent<BoxCollider2D>();
        enemyAttackArea.AttackTarget += AttackTarget;
        enemyAttackArea.StopAttacking += StopAttacking;
        healthGameObject = transform.Find("HpLine").GameObject().GetComponent<EnemyHealth>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        body2d = GetComponent<Rigidbody2D>();
        body2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        animator = GetComponent<Animator>();
        
        sacredTree = GameObject.Find("Yggdrasil tree");
        movingToTreeCoroutine = StartCoroutine(MovingToTree());
    }

    private IEnumerator MovingToTree()
    {
        StartMoving?.Invoke();
        //small delay
        while (true)
        {
            var steps = Time.deltaTime * speed;
            var position = transform.position;
            position = Vector2.MoveTowards(position, new Vector2(sacredTree.transform.position.x, position.y), steps);
            transform.position = position;
            animator.SetBool(Walk, true);
            spriteRenderer.flipX = false;
            yield return null;
        }
    }
    

    private void MoveTowards(IDamageable component)
    {
        if(died || attackColliderList.Count > 0 || movingToTreeCoroutine == null) return;
        movingColliderList.Add(component);
        if(movingColliderList.Count > 1) return;
        StopCoroutine(movingToTreeCoroutine);
        followingCoroutine = StartCoroutine(StartFollowing(component));
        
    }

    private IEnumerator StartFollowing(IDamageable component)
    {
        StartMoving?.Invoke();
        var targetGameObject = component.GetGameObject();
        
        //small delay
        var timer = 0f;
        const float timerDelay = 0.5f;
        animator.SetBool(Walk, false);
        ChangeDirection(targetGameObject);
        while (timer < timerDelay)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        animator.SetBool(Walk, true);
        while (true)
        {
            var steps = Time.deltaTime * speed;
            var currentPosition = transform.position;
            var targetPosition = targetGameObject.transform.position;
            ChangeDirection(targetGameObject);
            currentPosition = Vector2.MoveTowards(currentPosition, new Vector2(targetPosition.x, currentPosition.y), steps);
            animator.SetBool(Walk, true);
            transform.position = currentPosition;
            yield return null;
        }
    }

    private void ChangeDirection(GameObject targetGameObject)
    {
        var currentPosition = transform.position;
        var targetPosition = targetGameObject.transform.position;
        var faceDirection =  currentPosition.x - targetPosition.x;
        switch (faceDirection)
        {
            case > 0:
            {
                spriteRenderer.flipX = false;
                var offset = enemyAttackBoxCollider.offset;
                offset =
                    new Vector2(Math.Abs(offset.x)*-1, offset.y);
                enemyAttackBoxCollider.offset = offset;
                break;
            }
            case < 0:
            {
                spriteRenderer.flipX = true;
                var offset = enemyAttackBoxCollider.offset;
                offset =
                    new Vector2(Math.Abs(offset.x*-1), offset.y);
                enemyAttackBoxCollider.offset = offset;
                break;
            }
        }
    }

    private void StopFollowing(IDamageable component)
    {
        if(died || followingCoroutine == null || attackColliderList.Count > 0) return;
        StopMoving?.Invoke();
        StopCoroutine(followingCoroutine);
        if (movingColliderList.Count > 1)
        {
            followingCoroutine = StartCoroutine(StartFollowing(movingColliderList[1]));
        }
        else
        {
            movingToTreeCoroutine = StartCoroutine(MovingToTree());
        }
        movingColliderList.Remove(component);
    }

    private void AttackTarget(IDamageable component)
    {
        if(died || attackColliderList.Count > 0 || followingCoroutine == null) return;
        StopMoving?.Invoke();
        animator.SetBool(Walk, false);
        StopCoroutine(followingCoroutine);
        attackingCoroutine = StartCoroutine(StartAttacking(component));
        currentAttackTarget = component;
        animator.SetTrigger(Attack);
        attackColliderList.Add(component);
    }

    private IEnumerator StartAttacking(IDamageable component)
    {
        var attackTimer = 0f;
        while (true)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCd)
            {
                currentAttackTarget = component;
                animator.SetTrigger(Attack);
                attackTimer = 0f;
            }
            yield return null;
        }
    }

    private void StopAttacking(IDamageable component)
    {
        if(died || attackingCoroutine == null) return;
        StopMoving?.Invoke();
        attackColliderList.Remove(component);
        movingColliderList.Remove(component);
        MoveTowards(currentAttackTarget);
        StopCoroutine(attackingCoroutine);
    }
    
    public void TakeDamage(int value)
    {
        if(died) return;
        StopMoving?.Invoke();
        GettingHit?.Invoke();
        animator.SetTrigger(Hurt);
        if (!(healthGameObject.TakeDamage(value) <= 0)) return;
        died = true;
        animator.SetTrigger(Death);
        Dying?.Invoke();
        StartCoroutine(CheckDeathAnimationFinished());
    }

    private IEnumerator CheckDeathAnimationFinished()
    {
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            yield return null;
        }

        while (!(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >=
                 animator.GetCurrentAnimatorStateInfo(0).length))
        {
            yield return null;
        }
        DiedEvent?.Invoke(gameObject);
        Destroy(gameObject);
    }
    
    //Animation event
    private void DealDamageIfTargetWithinRange()
    {
        StartAttack?.Invoke();
        if (attackColliderList.Count>0)
        {
            currentAttackTarget.TakeDamage(damage);
        }
    }
}
