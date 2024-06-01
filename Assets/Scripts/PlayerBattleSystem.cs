using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBattleSystem : MonoBehaviour
{
    public GameObject attackPoint;
    public Vector3 attackPointDeltaPos = new Vector3(10f, 0.5f, 0f);
    public float attackRange = 0.5f;
    public Vector2 attackDashRange = new Vector2(20f, 1f);
    public LayerMask enemyLayer;
    public int attackDamage = 10;
    public int dashDamage = 50;
    public float critChance = 0.2f;
    public float critMultiplier = 2f;
    public float attackDelayTime = 0.05f;
    public float attackDashDelayTime = 0.5f;
    public float attackCooldownTime = 0.3f;
    public float attackCooldownStartTime = 0f;
    Collider2D[] dashHitEnemies;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && (attackCooldownStartTime + attackCooldownTime) <= Time.time) 
        {
            animator.Play("attackPlayer");
            Invoke("Attack", attackDelayTime);
            attackCooldownStartTime = Time.time;
        }
    }

    public void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies) 
        {
            Debug.Log(enemy.gameObject.name);
            enemy.GetComponent<HealthSystem>().takeDamage(attackDamage);
        }
    }

    public void attackDash(int dashDirection, Transform attackPointOld)
    {
        animator.Play("dashPlayer");
        Vector2 currentAttackDashRange = new Vector2(attackDashRange.x, attackDashRange.y);
        dashHitEnemies = Physics2D.OverlapBoxAll(attackPointOld.position + attackPointDeltaPos * dashDirection, currentAttackDashRange, 0, enemyLayer);
        Invoke("attackDashDelayed", attackDashDelayTime);
    }

    void attackDashDelayed()
    {
        foreach (Collider2D enemy in dashHitEnemies)
        {
            Debug.Log(enemy.gameObject.name);
            enemy.GetComponent<HealthSystem>().takeDamage(dashDamage);
        }
    }


}
