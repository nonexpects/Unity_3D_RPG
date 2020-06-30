using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int WeaponDamage = 5;
    PlayerController player;
    public GameObject hitFx;

    TrailRenderer tr;

    private void Start()
    {
        hitFx = Resources.Load("Fx/EnemyDamaged") as GameObject;
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        tr = GetComponentInChildren<TrailRenderer>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (player.isAttacking && other.gameObject.layer == LayerMask.NameToLayer("ENEMY"))
        {
            Debug.Log("hit " + other.name);
            EnemyFSM enemy = other.gameObject.GetComponent<EnemyFSM>();
            GameObject fx = Instantiate(hitFx);
            fx.transform.position = other.transform.position + new Vector3(0, 0.5f, 0);
            Destroy(fx, .5f);
            enemy.hitDamage(WeaponDamage);
        }
        else if(player.isAttacking && other.gameObject.layer == LayerMask.NameToLayer("CHEST"))
        {
            other.GetComponent<Chest>().BoxOpen();
        }
    }
}
