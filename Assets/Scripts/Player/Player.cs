using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerController controller;
    private PlayerData playerData;
    
    public ItemData itemData;
    public Action addItem;

    [SerializeField] private Transform dropPosition;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        playerData = GetComponent<PlayerData>();
    }
}
