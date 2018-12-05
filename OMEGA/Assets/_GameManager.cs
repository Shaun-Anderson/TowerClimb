using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _GameManager
{
    private static bool noMove;
	// Use this for initialization

	public static bool NoMove
    {
        get { return noMove; }
        set { noMove = value; }
	}
}

[System.Serializable]
public class Mission
{
    public string title;
    public MissionType missionType;
    public float killsRequired;
    public float coinsRequired;
    public string WorldRequired;
    public float precentComplete;
    public int Reward;
}

public enum MissionType
{
    Kill,
    World,
    Coins
}