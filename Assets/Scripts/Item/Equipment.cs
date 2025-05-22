using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    private PlayerData playerData;

    private void Start()
    {
        playerData = FindObjectOfType<PlayerData>();
    }

    public void EquipStat(ItemData data)
    {
        if (data == null) return;

        playerData = FindObjectOfType<PlayerData>();

        if (data.isEquip)
        {
            for (int i = 0; i < data.equipables.Length; i++)
            {
                switch (data.equipables[i].equipStatType)
                {
                    case EquipStatType.JumpPower:
                        playerData.JumpPower += data.equipables[i].value;
                        break;
                    case EquipStatType.RunSpeed:
                        playerData.RunSpeed += data.equipables[i].value;
                        break;
                }
            }
        }

        else
        {
            for (int i = 0; i < data.equipables.Length; i++)
            {
                switch (data.equipables[i].equipStatType)
                {
                    case EquipStatType.JumpPower:
                        playerData.JumpPower -= data.equipables[i].value;
                        break;
                    case EquipStatType.RunSpeed:
                        playerData.RunSpeed -= data.equipables[i].value;
                        break;
                }
            }
        }
    }
    
}
