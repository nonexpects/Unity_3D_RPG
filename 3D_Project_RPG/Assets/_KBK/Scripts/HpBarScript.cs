using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarScript : MonoBehaviour
{

    [SerializeField] GameObject hpBar = null;

    List<Transform> enemyList = new List<Transform>();
    List<GameObject> hpBarList = new List<GameObject>();

    Camera mainCam = null;

    void Start()
    {
        mainCam = Camera.main;
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < objects.Length; i++)
        {
            enemyList.Add(objects[i].transform);
            GameObject t_hpBar = Instantiate(hpBar, objects[i].transform.position, Quaternion.identity, transform);
            //t_hpBar.SetActive(false);
            hpBarList.Add(t_hpBar);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i] == null) continue;
            hpBarList[i].transform.position = mainCam.WorldToScreenPoint(enemyList[i].position + new Vector3(0, 1.15f, 0));
        }

    }
}
