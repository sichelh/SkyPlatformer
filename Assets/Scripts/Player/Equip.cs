using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip : MonoBehaviour
{
    private Equipment curEquip;
    [SerializeField] private Transform equipPosition;

    public void EquipNew(ItemData data)
    {
        UnEquip(data);
        curEquip = Instantiate(data.equipPrefab, equipPosition).GetComponent<Equipment>();
        data.isEquip = true;
        data.equipPrefab.GetComponent<Equipment>().EquipStat(data);
    }

    public void UnEquip(ItemData data)
    {
        if(curEquip != null)
        {
            Destroy(curEquip.gameObject);
            curEquip = null;
            data.isEquip = false;
            data.equipPrefab.GetComponent<Equipment>().EquipStat(data);
        }
    }
}
