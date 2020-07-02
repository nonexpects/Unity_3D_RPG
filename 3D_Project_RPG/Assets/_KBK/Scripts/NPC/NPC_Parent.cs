using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Parent : MonoBehaviour
{
    
    //public Transform npcCamPos;

    public Camera npcCam;
    protected Camera cam;

    public GameObject windowPrefab;

    protected GameObject questButton;
    protected GameObject questWin;

    protected Button acceptButton;

    [HideInInspector]
    public Quest quest = null;
    

    protected int questId;

    //퀘스트 UI Text
    List<Text> questText;

    protected void Awake()
    {
        questButton = GameObject.Find("QuestButton");
        questWin = Instantiate(windowPrefab);
        questWin.transform.SetParent(GameObject.Find("Canvas").transform);
        questWin.SetActive(false);
        
        cam = Camera.main;

        questText = new List<Text>();
        npcCam.enabled = false;
    }

    protected virtual void Start()
    {
        if (questButton.activeSelf) questButton.SetActive(false);
        
    }
    
    protected virtual void SetQuest(int i, QuestData data, string name, string desc, int exp, int gold)
    {
        quest = new Quest(i, data, name, desc, exp, gold);

        questWin.GetComponentsInChildren<Text>(questText);
        acceptButton = questWin.GetComponentInChildren<Button>();
        acceptButton.onClick.AddListener(QuestAccept);
        questText[0].text = name;
        questText[1].text = desc;
        questText[2].text = exp.ToString();
        questText[3].text = gold.ToString();

        QuestManager.instance.AddQuest(questId, quest);
    }

    public void QuestOpen()
    {
        npcCam.enabled= true;
        questButton.SetActive(false);
        questWin.SetActive(true);
    }

    public void QuestAccept()
    {
        if(quest!= null)
        {
            questWin.SetActive(false);
            QuestManager.instance.QuestActive(questId);
            ActivatorOff();
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
        {
            switch (QuestManager.instance.allQuest[questId].questData.state)
            {
                case QuestState.Ready:
                    questButton.GetComponent<Button>().onClick.RemoveAllListeners();
                    questButton.GetComponent<Button>().onClick.AddListener(QuestOpen);
                    questButton.SetActive(true);
                    break;
                case QuestState.Clear:
                    questButton.GetComponent<Button>().onClick.RemoveAllListeners();
                    questButton.GetComponent<Button>().onClick.AddListener(QuestReward);
                    questButton.SetActive(true);
                    break;
            }

        }
    }

    public void QuestReward()
    {
        QuestManager.instance.QuestCompleted(questId);

        ActivatorOff();
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
        {
            ActivatorOff();
        }
    }
    
    protected void ActivatorOff()
    {
        questButton.SetActive(false);
        questWin.SetActive(false);
        npcCam.enabled = false;
    }


}
