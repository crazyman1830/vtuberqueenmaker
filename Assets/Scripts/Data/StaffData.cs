
using System;

[System.Serializable]
public class StaffData
{
    public string staffName;
    public string role;
    public string description;
    public UnityEngine.Sprite portraitSprite;

    public StaffData(string name, string role, string description, UnityEngine.Sprite portrait = null)
    {
        this.staffName = name;
        this.role = role;
        this.description = description;
        this.portraitSprite = portrait;
    }
}
