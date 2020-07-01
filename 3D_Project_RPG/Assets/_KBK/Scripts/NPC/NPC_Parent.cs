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
    [HideInInspector]
    public PlayerController player;

    //퀘스트 UI Text
    List<Text> questText;

    protected void Awake()
    {
        questButton = GameObject.Find("QuestButton");
        questWin = Instantiate(windowPrefab);
        questWin.transform.parent = GameObject.Find("Canvas").transform;
        questWin.SetActive(false);
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        cam = Camera.main;

        questText = new List<Text>();
        npcCam.enabled = false;
    }

    protected virtual void Start()
    {
        if (questButton.activeSelf) questButton.SetActive(false);
        
    }
    
    protected virtual void SetQuest(string name, string desc, int exp, int gold)
    {
        quest = new Quest(name, desc, exp, gold);

        questWin.GetComponentsInChildren<Text>(questText);
        acceptButton = questWin.GetComponentInChildren<Button>();
        acceptButton.onClick.AddListener(QuestAccept);
        questText[0].text = name;
        questText[1].text = desc;
        questText[2].text = exp.ToString();
        questText[3].text = gold.ToString();
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
            quest.isActive = true;

            ActivatorOff();
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("PLAYER"))
        {
            questButton.GetComponent<Button>().onClick.RemoveAllListeners();
            questButton.GetComponent<Button>().onClick.AddListener(QuestOpen);
            questButton.SetActive(true);
        }
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
