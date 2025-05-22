using UnityEngine;
using UnityEngine.UI;

public enum StatType
{
    Hp,
    Stamina
}

public class StatBarUI : MonoBehaviour
{
    private Image statBar;

    [SerializeField] private PlayerData playerData;
    [SerializeField] private StatType statType;

    private void Awake()
    {
        playerData = FindObjectOfType<PlayerData>();
        statBar = this.GetComponent<Image>();
        if (statType == StatType.Hp)
            playerData.OnHpChanged += UpdateBar;
        else if (statType == StatType.Stamina)
            playerData.OnStaminaChanged += UpdateBar;
    }

    private void OnDestroy()
    {
        if (statType == StatType.Hp)
            playerData.OnHpChanged -= UpdateBar;
        else if (statType == StatType.Stamina)
            playerData.OnStaminaChanged -= UpdateBar;
    }


    public void UpdateBar()
    {
        // statBar 구현 fillAmount = 1 기준으로 비율 계산하여 입력
        if (statType == StatType.Hp)
            statBar.fillAmount = playerData.CurHp / playerData.MaxHp;
        else if (statType == StatType.Stamina)
            statBar.fillAmount = playerData.CurStamina / playerData.MaxStamina;
    }
}
