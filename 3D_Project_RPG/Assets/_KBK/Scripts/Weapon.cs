using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            EnemyFSM enemy = other.gameObject.GetComponent<EnemyFSM>();
            
            enemy.GetComponentInChildren<SkinnedMeshRenderer>().SetPropertyBlock(GameManager.instance.mpb);
            enemy.hitDamage(WeaponDamage + Random.Range(0, 5));

            GameObject fx = Instantiate(hitFx);
            fx.transform.position = other.transform.position + new Vector3(0, 0.5f, 0);
            Destroy(fx, .5f);            
        }
        else if(player.isAttacking && other.gameObject.layer == LayerMask.NameToLayer("BOSS"))
        {
            BossController boss = other.gameObject.GetComponent<BossController>();
            
            boss.GetDamage(WeaponDamage + Random.Range(0, 5));
        }
        else if(player.isAttacking && other.gameObject.layer == LayerMask.NameToLayer("CHEST"))
        {
            other.GetComponent<Chest>().BoxOpen();
        }
        else if(player.isAttacking && other.gameObject.layer == LayerMask.NameToLayer("BOSSCHEST"))
        {
            other.GetComponent<BossChest>().BoxOpen();
        }
    }
}
