public class Quest
{
    public bool isActive;

    public string title;
    public string description;
    public int experienceReward;
    public int goldReward;

    public Quest(string name, string desc, int exp, int gold)
    {
        title = name;
        description = desc;
        experienceReward = exp;
        goldReward = gold;
    }
}
