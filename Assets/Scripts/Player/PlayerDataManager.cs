using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDataManager : Singleton<PlayerDataManager>
{
    [SerializeField] private float AddStaminaPerTime = 5.0f;
    private Dictionary<StatType, StatValue> statDict = new();
    private Dictionary<StatType, Action> onStatChanged = new();
    public event Action OnTakeDamage;

    protected override void Awake()
    {
        base.Awake();

        SetInitStats();
    }

    private void Update()
    {
        AddStat(StatType.Stamina, AddStaminaPerTime * Time.deltaTime);
    }

    public void SetInitStats()
    {
        foreach (var stat in SetPlayerData.StatValues)
        {
            statDict[stat.statType] = new StatValue
            {
                statType = stat.statType,
                maxValue = stat.maxValue,
                curValue = stat.curValue
            };
        }
    }

    public float GetCurValue(StatType statType)
    {
        return statDict[statType].curValue;
    }

    public void SetCurMaxStat(StatType type)
    {
        statDict[type].curValue = statDict[type].maxValue;
    }

    public void AddStat(StatType type, float amount)
    {
        statDict[type].curValue = Mathf.Clamp(statDict[type].curValue + amount, 0, statDict[type].maxValue);
        TriggerStatChanged(type);
    }

    public void SubStat(StatType type, float amount)
    {
        statDict[type].curValue = Mathf.Clamp(statDict[type].curValue - amount, 0, statDict[type].maxValue);
        TriggerStatChanged(type);
    }

    public bool UseStamina(float value)
    {
        if (statDict[StatType.Stamina].curValue <= value)
        {
            return false;
        }

        SubStat(StatType.Stamina, value);
        return true;
    }

    public void TakeDamage(float damage)
    {
        SubStat(StatType.Hp, damage);
        OnTakeDamage?.Invoke();

        if (statDict[StatType.Hp].curValue <= 0)
        {
            statDict[StatType.Hp].curValue = 0;
            Death();
        }
    }

    // 사망 시 처리
    void Death()
    {
        GameManager.Instance.ReplayeGame();
        SetInitStats();
    }

    public void BoostStat(StatType type, float boostAmount)
    {
        StartCoroutine(StatBoostRoutine(type, boostAmount));
    }

    // 10초동안만 효과 지속
    private IEnumerator StatBoostRoutine(StatType type, float amount)
    {
        AddStat(type, amount);

        yield return new WaitForSeconds(10f);

        SubStat(type, amount);
    }

    public void Subscribe(StatType type, Action callback)
    {
        // StatType 키가 아직 추가되지 않았다면 빈 델리게이트 등록.
        if (!onStatChanged.ContainsKey(type))
            onStatChanged[type] = () => { };
        onStatChanged[type] += callback;
    }

    public void Unsubscribe(StatType type, Action callback)
    {
        if (onStatChanged.ContainsKey(type))
            onStatChanged[type] -= callback;
    }

    private void TriggerStatChanged(StatType type)
    {
        if (onStatChanged.TryGetValue(type, out var callback))
            callback?.Invoke();
    }

    public float GetNormalized(StatType type)
    {
        return statDict[type].maxValue == 0 ? 0 : statDict[type].curValue / statDict[type].maxValue;
    }
}
