using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.LightAnchor;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 10f;
    public float maxFallSpeed = -20f;
    public float dashForce = 100f;
    public float jumpForce = 3f;
    public float gravity = 9.82f * -2;
    public float dashTime = 0.5f;
    public int canMove = 0;
    public int dashCharges = 1;
    public int maxDashCharges = 1;
    public float groundDistance = 2f;
    public float maxMultiple = 5;
    public float minMultiple = -3.5f;
    public float smoothTime;
    public float dashSmoothTime;
    public bool canDash;
    public bool isGrounded;
    public bool isDash;
    Rigidbody2D rb;
    Vector2 dashDirection;
    Vector2 mousePos;
    Vector2 selfPos;
    Vector2 diffPos;
    Vector2 velocity = Vector2.zero;
    Vector2 smoothedVelocity;
    float smoothedVelocityX;
    public LayerMask groundMask;
    public LayerMask enemyMask;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gravity /= walkSpeed;
    }


    
    private void Update()
    {
        if (rb.velocity.y < -0.1f) Debug.Log(rb.velocity);
        if (rb.velocity.y == 0) { isGrounded = true; }
        else { isGrounded = false; } 
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selfPos = new Vector2(transform.position.x, transform.position.y);
        diffPos = (mousePos - selfPos);
        diffPos = diffPos.normalized;

        var inputH = Input.GetAxis("Horizontal");
        var direction = inputH * walkSpeed;
        smoothedVelocityX = Mathf.Lerp(rb.velocity.x, direction, smoothTime);
        smoothedVelocity = new Vector2(smoothedVelocityX, rb.velocity.y);
        if (rb.velocity.y <= maxFallSpeed) smoothedVelocity.y = maxFallSpeed;
        if (canMove == 1 && isDash == false)
        {
            rb.velocity = smoothedVelocity;
        }

        if (diffPos[0] >= maxMultiple) { diffPos[0] = maxMultiple; }
        else if (diffPos[0] <= minMultiple) { diffPos[0] = minMultiple; }
        if (diffPos[1] >= maxMultiple) { diffPos[1] = maxMultiple; }
        else if (diffPos[1] <= minMultiple) { diffPos[1] = minMultiple; }

        if (dashCharges <= 0 || isDash == true) { canDash = false; }
        else { canDash = true; }
        if (isGrounded) { dashCharges = maxDashCharges; }
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Space)) && canDash) { Dash(); }

        
    }

    //private void Jump()
    //{
    //    pass;
    //}

    private void Dash()
    {
        //Debug.Log("kaif");
        //rb.AddForce(diffPos * dashForce, ForceMode2D.Impulse);
        diffPos = new Vector2(diffPos[0], diffPos[1]/2);
        rb.velocity = diffPos * dashForce;
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
