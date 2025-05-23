using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    private PlayerDataManager playerDataManager;

    public void EquipStat(ItemData data)
    {
        if (data == null) return;

        if (playerDataManager == null)
            playerDataManager = PlayerDataManager.Instance;

        if (data.isEquip)
        {
            for (int i = 0; i < data.equipables.Length; i++)
            {
                switch (data.equipables[i].equipStatType)
                {
                    case EquipStatType.JumpPower:
                        playerDataManager.AddStat(StatType.JumpPower, data.equipables[i].value);
                        break;
                    case EquipStatType.RunSpeed:
                        playerDataManager.AddStat(StatType.RunSpeed, data.equipables[i].value);
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
                        playerDataManager.SubStat(StatType.JumpPower, data.equipables[i].value);
                        break;
                    case EquipStatType.RunSpeed:
                        playerDataManager.SubStat(StatType.RunSpeed, data.equipables[i].value);
                        break;
                }
            }
        }
    }
    
}
