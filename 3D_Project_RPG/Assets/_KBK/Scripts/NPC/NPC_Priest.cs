using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Priest : NPC_Parent
{
    protected override void Start()
    {
        base.Start();
        SetQuest("THIS IS QUEST OF PRIEST!", "is it description? HAHAHAHAHAHAHAHAHAHAHAHHAAHAHHA", 20, 100);
        
    }

    private void Update()
    {
        
    }

}
