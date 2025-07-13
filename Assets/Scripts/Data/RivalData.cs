
using System;
using System.Collections.Generic;

[System.Serializable]
public class RivalData
{
    public string rivalName;
    public int subscribers;
    public int fame;
    public string description;
    public UnityEngine.Sprite portraitSprite;

    public RivalData(string name, int subscribers, int fame, string description, UnityEngine.Sprite portrait = null)
    {
        this.rivalName = name;
        this.subscribers = subscribers;
        this.fame = fame;
        this.description = description;
        this.portraitSprite = portrait;
    }
}
