using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int WeaponDamage = 2;
    PlayerAttack player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerAttack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player.isAttacking && other.gameObject.layer == LayerMask.NameToLayer("ENEMY"))
        {
            Debug.Log("hit " + other.name);
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            enemy.hitDamage(WeaponDamage);
        }
    }
}
