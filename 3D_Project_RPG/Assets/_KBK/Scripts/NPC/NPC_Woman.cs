using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Woman : NPC_Parent
{
    protected override void Start()
    {
        base.Start();
        SetQuest("THIS IS QUEST OF WOMAN!", "is it description? HAHAHAHAHAHAHAHAHAHAHAHHAAHAHHA", 50, 800);

    }
    
}
