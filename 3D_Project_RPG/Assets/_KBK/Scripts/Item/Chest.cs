using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    MeshFilter mf;
    Mesh chestOpenMesh;

    GameObject[] items;
    List<GameObject> itemList;
    GameObject[] fx;
    public GameObject glowFx;

    int maxItemCount = 5;

    float itemSpawnRange = 3f;

    bool isBoxOpen;

    void Start()
    {
        mf = GetComponent<MeshFilter>();
        chestOpenMesh = Resources.Load("Model/Chest_Open") as Mesh;
        Object[] temp = Resources.LoadAll("Items");
        fx = Resources.LoadAll<GameObject>("Fx/Chest");

        items = new GameObject[temp.Length];
        itemList = new List<GameObject>();

        for (int i = 0; i < temp.Length; i++)
        {
            GameObject it = temp[i] as GameObject;
            items[i] = it;
        }

        GameObject fxAppear = Instantiate(fx[0]);
        fxAppear.transform.position = transform.position;
        Destroy(fxAppear, 2f);
    }

    public void BoxOpen()
    {
        if(!isBoxOpen)
        {
            mf.sharedMesh = chestOpenMesh;
            glowFx.SetActive(false);
            StartCoroutine(BoxProc());
        }
    }

    IEnumerator BoxProc()
    {
        isBoxOpen = true;

        GameObject fxOpen = Instantiate(fx[1]);
        fxOpen.transform.position = transform.position;
        Destroy(fxOpen, 1.5f);

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
