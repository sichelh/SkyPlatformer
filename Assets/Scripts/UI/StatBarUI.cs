using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class StatBarUI : MonoBehaviour
{
    private Image statBarImage;

    [SerializeField] private StatType statType;

    private void Awake()
    {
        statBarImage = GetComponent<Image>();
        PlayerDataManager.Instance.Subscribe(statType, UpdateBar);
    }

    private void OnDestroy()
    {
        PlayerDataManager.Instance.Unsubscribe(statType, UpdateBar);
    }

    private void Start()
    {
        UpdateBar();
    }


    public void UpdateBar()
    {
        statBarImage.fillAmount = PlayerDataManager.Instance.GetNormalized(statType);
    }
}
