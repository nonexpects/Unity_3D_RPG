using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletPool : MonoBehaviour
{

    int ballMaxCount = 30;
    public GameObject ballPrefab;

    List<GameObject> balls = new List<GameObject>();

    void Start()
    {
        //오브젝트 풀링
        for (int i = 0; i < ballMaxCount; i++)
        {
            GameObject ball = Instantiate(ballPrefab, transform);
            ball.SetActive(false);
            balls.Add(ball);
        }
    }

    public GameObject PickUpBullet()
    {
        foreach (GameObject item in balls)
        {
            if (!item.activeSelf)
                return item;
        }

        return null;
    }

    //public void CastMagicBall()
    //{
    //    GameObject ball = PickUpBullet();
    //
    //    ball.transform.position = transform.position + transform.up + (transform.forward * 2);
    //    //b.transform.forward = (transform.position - player.transform.position).normalized;
    //    ball.transform.eulerAngles = transform.eulerAngles + new Vector3(0, i * angle + 10f, 0);
    //
    //    ball.SetActive(true);
    //}
    
    void Update()
    {
        
    }
}
