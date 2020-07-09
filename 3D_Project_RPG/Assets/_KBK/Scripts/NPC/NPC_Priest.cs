using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Priest : NPC_Parent
{
    QuestData data;
    
    protected override void Start()
    {
        data = new QuestData();
        data.state = QuestState.Ready;
        data.questType = QuestType.Kill;
        data.count = 6;
        data.enemyId = 0;
        
        questId = 0;
        SetQuest(questId, data, "QUEST OF PRIEST", "Forgive me, adventurer. I'm in need of your service. We've been under constant attack", 35, 100);


        base.Start();
        //Debug.Log(JsonUtility.ToJson(quest));
    }

    private void Update()
    {
        
    }

}
