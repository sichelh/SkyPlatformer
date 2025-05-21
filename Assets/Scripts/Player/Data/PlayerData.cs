using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void Heal(int heal)
    {
        if (CurHp >= MaxHp)
        {
            Debug.Log("이미 최대체력입니다!");
            return;
        }

        CurHp += heal;
    }

    public void TakeDamage(float damage)
    {
        CurHp -= damage;
        OnTakeDamage?.Invoke();

        if (CurHp <= 0)
        {
            CurHp = 0;
            Death();
        }
    }

    public void JumpBoost(int value)
    {
        StartCoroutine(JumpBoostRoutine(value));
    }

    // 10초동안만 효과 지속
    IEnumerator JumpBoostRoutine(int boostJumpPower)
    {
        JumpPower += boostJumpPower;
        yield return new WaitForSeconds(10f);
        JumpPower -= boostJumpPower;
    }
    
    public void SpeedBoost(int value)
    {
        StartCoroutine(SpeedBoostRoutine(value));
    }

    // 10초동안만 효과 지속
    IEnumerator SpeedBoostRoutine(int boostSpeedPower)
    {
        runSpeed += boostSpeedPower;
        yield return new WaitForSeconds(10f);
        runSpeed -= boostSpeedPower;
    }

    // 사망 시 게임 다시 시작
    public void Death()
    {
        SceneManager.LoadScene("MainScene");
    }
}
