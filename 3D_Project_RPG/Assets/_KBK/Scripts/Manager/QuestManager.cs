using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum QuestType
{
    Kill,
    Collect,
    BossKill,
    Talk
}

public enum QuestState
{
    Ready,
    Take,
    Clear,
    Completed
}

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public Dictionary<int, Quest> allQuest = new Dictionary<int, Quest>();

    Sprite[] questImage;

    public Transform contents;

    Sprite completeImage;
    public GameObject questLogPrefab;
    public GameObject questcompletePrefab;

    public Dictionary<int, GameObject> questLog = new Dictionary<int, GameObject>();
    
    PlayerController player;

    private void Awake()
    {
        if (instance == null) instance = this;

        //LoadQuests();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    //Quest newQuest = JsonUtility.FromJson<Quest>(Resources.Load<TextAsset>("Json Files/JsonExample").text);
    //questDictionary.Add(newQuest.id, newQuest);

    public void AddQuest(int questId, Quest quest)
    {
        allQuest.Add(questId, quest);
    }

    public void QuestActive(int id)
    {
        for (int i = 0; i < allQuest.Count; i++)
        {
            if(allQuest.ContainsKey(id))
            {
                QuestLogMaker(id);
                break;
            }
        }
    }
    
    void QuestLogMaker(int id)
    {
        QuestData data = allQuest[id].questData;
        GameObject qlog = Instantiate(questLogPrefab, contents.transform);
        switch (data.questType)
        {
            case QuestType.Kill:
                qlog.GetComponentsInChildren<Image>()[1].sprite = questImage[0];
                qlog.GetComponentsInChildren<Text>()[0].text = "Kill " + data.count + " " + MonsterName(data.enemyId);
                qlog.GetComponentsInChildren<Text>()[1].text = "( " + 0 + " / " + data.count + " )";
                break;
            case QuestType.Collect:
                qlog.GetComponentsInChildren<Image>()[1].sprite = questImage[1];
                break;
            case QuestType.BossKill:
                qlog.GetComponentsInChildren<Image>()[1].sprite = questImage[2];
                qlog.GetComponentsInChildren<Text>()[0].text = "Kill Boss";
                qlog.GetComponentsInChildren<Text>()[1].text = "( " + 0 + " / " + data.count + " )";
                break;
            case QuestType.Talk:
                qlog.GetComponentsInChildren<Image>()[1].sprite = questImage[3];
                break;
        }
        data.state = QuestState.Take;
        Debug.Log("퀘스트 로그 추가!");
        questLog.Add(id, qlog);
    }

    public void QuestCompleted(int id)
    {
        Destroy(questLog[id]);
        player.GetGold(allQuest[id].goldReward);
        player.GetExp(allQuest[id].experienceReward);
        allQuest[id].questData.state = QuestState.Completed;
        questLog.Remove(id);
    }

    string MonsterName(int id)
    {
        switch (id)
        {
            case 0:
                return "Warriors";
            case 1:
                return "Archers";
            case 2:
                return "Mages";
            case 3:
                return "Boss";
            default:
                return "Enemies";
        }
    }

    void Start()
    {
        questImage = Resources.LoadAll<Sprite>("QuestImage");
        completeImage = Resources.Load<Sprite>("Completed");
    }
    
    void Update()
    {
        foreach(int i in questLog.Keys)
        {
            if (allQuest[i].questData.state != QuestState.Take) continue;

            questLog[i].GetComponentsInChildren<Text>()[1].text = "( " + GameManager.instance.killCounter[allQuest[i].questData.enemyId] + " / " + allQuest[i].questData.count + " )";
            if (GameManager.instance.killCounter[allQuest[i].questData.enemyId] == allQuest[i].questData.count || Input.GetKeyDown(KeyCode.F1))
            {
                GameObject completeWin = Instantiate(questcompletePrefab, transform.parent);
                Destroy(completeWin, 1f);

                allQuest[i].questData.state = QuestState.Clear;
                questLog[i].GetComponentsInChildren<Image>()[1].sprite = completeImage;
                questLog[i].GetComponentsInChildren<Image>()[1].color = Color.green;
            }
        }
    }
}
