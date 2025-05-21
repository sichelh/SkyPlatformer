using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EItemType
{
    Consumable,
    Equipable
}

public enum ConsumableType
{
    Hp,
    Stamina,
    RunSpeed,
    JumpPower,
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public int value;
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string itemName;
    public string itemDescription;
    public EItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Equipable")]
    public GameObject equipPrefab;
}
