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

}
