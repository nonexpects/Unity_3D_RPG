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
        SetQuest(questId, data, "THIS IS QUEST OF PRIEST!", "is it description? HAHAHAHAHAHAHAHAHAHAHAHHAAHAHHA", 35, 100);
        
        base.Start();
        //Debug.Log(JsonUtility.ToJson(quest));
    }

    private void Update()
    {
        
    }

}
