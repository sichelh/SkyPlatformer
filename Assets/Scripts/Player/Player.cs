using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ItemData itemData;
    public Action addItem;

    [SerializeField] private Transform dropPosition;

    public PlayerController playerController;
    public Equip equip;

    private void Awake()
    {
        GameManager.Instance.player = this;
        playerController = GetComponent<PlayerController>();
        equip = GetComponent<Equip>();
    }
}
