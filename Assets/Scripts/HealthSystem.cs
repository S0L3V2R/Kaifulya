using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int currentHp = 100;
    public int maxHp = 100;
    private void Update()
    {
        if (currentHp > maxHp) currentHp = maxHp;
        if (currentHp <= 0) Death(gameObject);
    }
    public void takeDamage(int damage)
    {
        currentHp -= damage;
    }

    public void takeHealth(int heal)
    {
        currentHp += heal;
    }

    void Death(GameObject creature)
    {
        if (creature.name != "Player") { Destroy(creature); }
        else { Destroy(creature); }
    }
}
