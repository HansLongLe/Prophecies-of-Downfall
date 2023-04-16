using System.Collections;
using UnityEngine;


public class HeroParticles : MonoBehaviour
{

    [SerializeField]
    private GameObject hero;
    private float particleDuration;
    private SpriteRenderer sprite;
    private Animator rollAnimator;
    private float rollCountdown = 0.0f;
    private float particleCountdown;
    private float rollCooldown;


    // Start is called before the first frame update
    private void Start()
    {
        HeroKnight.Rolled += RollCooldownParticle;
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        rollCooldown = hero.GetComponent<HeroKnight>().rollCooldown;
        rollAnimator = GetComponent<Animator>();
        var animationClips = GetComponent<Animator>().runtimeAnimatorController.animationClips;
        foreach (var animationClip in animationClips)
        {
            if (animationClip.name == "RollReady")
            {
                particleDuration = animationClip.length + 0.015f;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //track player position
        var position = hero.transform.position;
        transform.position = new Vector2(position.x, position.y + 0.3f);
    }

    private void RollCooldownParticle()
    {
        StartCoroutine(RollParticleTimer());
    }

    private IEnumerator CoolDownTimer()
    {
        while (rollCountdown < rollCooldown)
        {
            rollCountdown += Time.deltaTime;
            yield return null;
        }
        if (rollCountdown >= rollCooldown)
        {
            rollCountdown = 0f;
            rollAnimator.Play("RollReady", -1, 0f);
            sprite.enabled = true;
        }
    }
    private IEnumerator RollParticleTimer()
    {
        yield return StartCoroutine(CoolDownTimer());
        while (particleCountdown < particleDuration)
        {
            particleCountdown += Time.deltaTime;
            yield return null;
        }
        if (particleCountdown >= particleDuration)
        {
            sprite.enabled = false;
            particleCountdown = 0f;
        }
    }
    
}
