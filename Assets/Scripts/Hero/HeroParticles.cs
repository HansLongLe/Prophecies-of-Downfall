using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroParticles : MonoBehaviour
{

    [SerializeField]
    private GameObject hero;
    private float particleDuration;
    private bool cooldownStarted;
    private SpriteRenderer sprite;
    private Animator rollAnimator;
    private float rollCountdown;
    private float particleCountdown;
    private bool particleStarted = false;
    private float rollCooldown;
    private HeroKnight heroKnight;


    // Start is called before the first frame update
    void Start()
    {
        heroKnight = hero.GetComponent<HeroKnight>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        rollCooldown = hero.GetComponent<HeroKnight>().rollCooldown;
        rollAnimator = GetComponent<Animator>();
        AnimationClip[] animationClips = GetComponent<Animator>().runtimeAnimatorController.animationClips;
        foreach (var animation in animationClips)
        {
            if (animation.name == "RollReady")
            {
                particleDuration = animation.length + 0.015f;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(hero)
        {
            var canRoll = heroKnight.canRoll;
            transform.position = new Vector2(hero.transform.position.x, hero.transform.position.y + 0.3f);
            if(canRoll)
            {
                cooldownStarted = true;
                rollCountdown = 0f;
            }
            if(cooldownStarted)
            {
                rollCountdown += Time.deltaTime;
            }
            if(rollCountdown >= rollCooldown)
            {
                rollCountdown = 0f;
                particleCountdown = 0f;
                cooldownStarted= false;
                rollAnimator.Play("RollReady", -1, 0f);
                particleStarted = true;
                sprite.enabled = true;
            }
            if(particleStarted)
            {
                particleCountdown += Time.deltaTime;
            }
            if(particleCountdown >= particleDuration)
            {
                particleStarted = false;
                sprite.enabled = false;
            }
        }
    }
}
