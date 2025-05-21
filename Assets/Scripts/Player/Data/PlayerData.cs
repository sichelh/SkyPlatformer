using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float maxHp;
    [SerializeField] private float curHp;
    
    public float RunSpeed { get { return runSpeed; } set { runSpeed = value; } }
    public float JumpPower { get { return jumpPower; } set { jumpPower = value; } }
    public float MaxHp { get { return maxHp; } set { maxHp=value; } }
    public float CurHp 
    { 
        get { return curHp; } 
        set 
        { 
            curHp = Mathf.Clamp(value, 0, maxHp);
            OnHpChanged?.Invoke();
        } 
    }

    public event Action OnTakeDamage;
    public event Action OnHpChanged;

    private void Start()
    {
        curHp = maxHp;
    }

    public void TakeDamage(int damage)
    {
        CurHp -= damage;
        OnTakeDamage?.Invoke();

        if (CurHp <= 0)
        {
            curHp = 0;
            Death();
        }
    }

    public void Death()
    {
        Debug.Log("사망");
    }
}
