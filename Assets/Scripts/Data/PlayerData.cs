
[System.Serializable]
public class PlayerData
{
    public string vtuberName;

    public int subscribers;
    public int charm;
    public int skill;
    public int stress;
    public long money;
    public int talkSkill;
    public int gameSkill;
    public int singingSkill;
    public int dancingSkill;
    public int fame;

    public PlayerData(string name)
    {
        this.vtuberName = name;
        this.subscribers = 0;
        this.charm = 10;
        this.skill = 10;
        this.stress = 0;
        this.money = 50000;
        this.talkSkill = 10;
        this.gameSkill = 10;
        this.singingSkill = 10;
        this.dancingSkill = 10;
        this.fame = 0;
    }
}
