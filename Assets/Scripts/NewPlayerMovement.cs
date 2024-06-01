using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.LightAnchor;

public class NewPlayerMovement : MonoBehaviour
{
    public float walkSpeed = 10f;
    public float maxFallSpeed = -20f;
    public float dashForce = 100f;
    public float jumpForce = 3f;
    public int canMove = 0;
    public bool canDash;
    public int dashCharges = 1;
    public int jumpCharges = 1;
    public int maxDashCharges = 1;
    public int maxJumpCharges = 2;
    public float dashCooldown = 1f;
    public float dashCooldownBegin;
    public float dashTime = 0.5f;
    public float smoothTime;
    public float dashSmoothTime;
    public bool isGrounded;
    public bool isDash;
    public bool isDashCooldown;
    public float inputH;
    public int characterDirection = 1;
    public BoxCollider2D groundCheck;
    Rigidbody2D rb;
    [SerializeField]
    PlayerBattleSystem pbs;
    Vector2 diffPos;
    Vector2 smoothedVelocity;
    float smoothedVelocityX;
    public LayerMask groundMask;
    public LayerMask enemyMask;
    public Animator animator;
    public GameObject jumpParticle;
    public GameObject dashParticle;
    ParticleSystem jumpParticles;
    ParticleSystem dashParticles;

    private void Start()
    {
        jumpParticles = jumpParticle.GetComponent<ParticleSystem>();
        dashParticles = dashParticle.GetComponent<ParticleSystem>();
        animator = GetComponent<Animator>();
        pbs = GetComponent<PlayerBattleSystem>();
        rb = GetComponent<Rigidbody2D>();
    }



    private void FixedUpdate()
    {
        if (groundCheck.IsTouchingLayers(groundMask)) { isGrounded = true; }
        else { isGrounded = false; }

        if (isGrounded == true) 
        {
            //dashCharges = maxDashCharges;
            jumpCharges = maxJumpCharges; 
        }

        inputH = Input.GetAxis("Horizontal");

        if (inputH != 0)
        {
            if (inputH < 0) {  characterDirection = -1; }
            else { characterDirection = 1; }
        }
        transform.localScale = new Vector3(characterDirection * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        animator.SetBool("Grounded", isGrounded);

        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && isGrounded == true) { animator.SetTrigger("Run"); }
        else if (isGrounded == true) { animator.SetTrigger("Idle"); }
        else if (isGrounded == false && rb.velocity.y >= 0) { animator.SetTrigger("Jump"); animator.ResetTrigger("Fall"); }
        else if (isGrounded == false && rb.velocity.y < 0) { animator.SetTrigger("Fall"); animator.ResetTrigger("Jump"); }
        

        var direction = inputH * walkSpeed;
        smoothedVelocityX = Mathf.Lerp(rb.velocity.x, direction, smoothTime);
        smoothedVelocity = new Vector2(smoothedVelocityX, rb.velocity.y);
        if (rb.velocity.y <= maxFallSpeed) smoothedVelocity.y = maxFallSpeed;

        if (canMove == 1 && isDash == false)
        {
            rb.velocity = smoothedVelocity;
        }

        if (dashCharges < maxDashCharges) 
        {
            if (isDashCooldown == false)
            {
                dashCooldownBegin = Time.time;
            }
            isDashCooldown = true;
            if (dashCooldownBegin + dashCooldown <= Time.time && isDashCooldown == true) { dashCharges++; isDashCooldown = false; }
        }

        if (dashCharges <= 0 || isDash == true) { canDash = false; }
        else { canDash = true; }
        if ((Input.GetKeyDown(KeyCode.LeftShift)) && canDash == true) { Dash(); }
        if ((Input.GetKeyDown(KeyCode.Space)) && jumpCharges > 0) { Jump(); }
    }

    private void Jump()
    {
        if (Mathf.Abs(rb.velocity.y) > 0.1) { jumpParticles.Play(); }
        rb.velocity = new Vector2(smoothedVelocityX, jumpForce);
        jumpCharges--;
    }

    private void Dash()
    {
        diffPos = new Vector2(characterDirection, 0.0000001f);
        dashParticles.Play();
        pbs.attackDash(characterDirection, pbs.attackPoint.transform);
        rb.velocity = diffPos.normalized * dashForce;
        dashCharges--;
        Invoke("gravityEnable", dashTime + dashSmoothTime);
        rb.gravityScale = 0;
        isDash = true;
    }

    private void gravityEnable()
    {
        rb.velocity = smoothedVelocity;
        isDash = false;
        rb.gravityScale = 1;
    }
}
