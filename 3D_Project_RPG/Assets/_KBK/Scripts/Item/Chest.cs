using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    MeshFilter mf;
    Mesh chestOpenMesh;

    GameObject[] items;
    List<GameObject> itemList;

    int maxItemCount = 5;

    float itemSpawnRange = 3f;

    bool isBoxOpen;

    void Start()
    {
        mf = GetComponent<MeshFilter>();
        chestOpenMesh = Resources.Load("Model/Chest_Open") as Mesh;
        Object[] temp = Resources.LoadAll("Items");

        items = new GameObject[temp.Length];
        itemList = new List<GameObject>();

        for (int i = 0; i < temp.Length; i++)
        {
            GameObject it = temp[i] as GameObject;
            items[i] = it;
        }

    }

    public void BoxOpen()
    {
        if(!isBoxOpen)
        {
            Debug.Log(" 박스 오픈 ");
            mf.sharedMesh = chestOpenMesh;

            StartCoroutine(BoxProc());
        }
    }

    IEnumerator BoxProc()
    {
        isBoxOpen = true;

        for (int i = 0; i < maxItemCount; i++)
        {
            Vector3 iPos = (Random.insideUnitSphere * itemSpawnRange) + transform.position;
            iPos.y = 0.5f;

            GameObject it = Instantiate(items[1], iPos, Quaternion.identity, transform);
            itemList.Add(it);

        }

        Vector3 iPos2 = (Random.insideUnitSphere * itemSpawnRange) + transform.position;
        iPos2.y = 0.5f;

        GameObject it2 = Instantiate(items[0], iPos2, Quaternion.identity, transform);
        itemList.Add(it2);

        yield return new WaitForSeconds(15f);


        Destroy(this.gameObject);
    }
}
