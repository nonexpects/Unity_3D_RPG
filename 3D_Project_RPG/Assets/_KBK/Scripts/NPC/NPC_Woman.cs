using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Woman : NPC_Parent
{
    QuestData data;

    protected override void Start()
    {
        data = new QuestData
        {
            state = QuestState.Ready,
            questType = QuestType.BossKill,
            count = 1,
            enemyId = 3
        };

        questId = 1;
        SetQuest(1, data, "THIS IS QUEST OF WOMAN!", "is it description? HAHAHAHAHAHAHAHAHAHAHAHHAAHAHHA", 50, 800);

        base.Start();
    }
    
}
