﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int WeaponDamage = 2;
    PlayerController player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player.isAttacking && other.gameObject.layer == LayerMask.NameToLayer("ENEMY"))
        {
            Debug.Log("hit " + other.name);
            EnemyFSM enemy = other.gameObject.GetComponent<EnemyFSM>();
            enemy.hitDamage(WeaponDamage);
        }
    }
}