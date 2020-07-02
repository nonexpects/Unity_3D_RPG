public class QuestData
{
    public QuestState state;
    public QuestType questType;
    public int enemyId;
    public int count;
}


public class Quest
{
    public int id;

    public QuestData questData;

    public string title;
    public string description;
    public int experienceReward;
    public int goldReward;

    public Quest(int d, QuestData data, string name, string desc, int exp, int gold)
    {
        id = d;
        questData = data;
        title = name;
        description = desc;
        experienceReward = exp;
        goldReward = gold;
    }
}
