using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBarUI : MonoBehaviour
{
    private Image statBar;

    [SerializeField] private PlayerData playerData;

    private void Awake()
    {
        playerData = FindObjectOfType<PlayerData>();
        statBar = this.GetComponent<Image>();
        playerData.OnHpChanged += UpdateHpBar; // curHp 변동사항을 항상 반영
    }

    private void OnDestroy()
    {
        playerData.OnHpChanged -= UpdateHpBar;
    }

    private void Start()
    {
        playerData.OnHpChanged += UpdateHpBar;
    }

    public void UpdateHpBar()
    {
        // hpBar 구현 fillAmount = 1 기준으로 비율 계산하여 입력
        float hpRatio = playerData.CurHp / playerData.MaxHp;
        statBar.fillAmount = hpRatio;
    }
}
