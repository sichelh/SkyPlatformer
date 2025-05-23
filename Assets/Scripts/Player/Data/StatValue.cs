using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Hp,
    Stamina,
    RunSpeed,
    JumpPower
}

[System.Serializable]
public class StatValue
{
    public StatType statType;
    public float maxValue;
    public float curValue;
}

public static class SetPlayerData
{
    public static readonly List<StatValue> StatValues = new List<StatValue>
    {
        new StatValue { statType = StatType.Hp, maxValue = 100, curValue = 100 },
        new StatValue { statType = StatType.Stamina, maxValue = 100, curValue = 50 },
        new StatValue { statType = StatType.RunSpeed, maxValue = 30, curValue = 5 },
        new StatValue { statType = StatType.JumpPower, maxValue = 300, curValue = 100 }
    };
}
