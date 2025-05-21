using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;

    private Button button;
    private Image icon;
    private TextMeshProUGUI quantityText;
    private Outline outline;

    public InventoryUI inventoryUI;

    public int index;
    public int quantity;

    private void Awake()
    {
        button = GetComponent<Button>();
        icon = transform.GetChild(0).GetComponent<Image>();
        quantityText = GetComponentInChildren<TextMeshProUGUI>();
        outline = GetComponent<Outline>();
    }

    // 아이템 슬롯 추가
    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;
    }

    // 아이템 슬롯 제거
    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }

    // 아이템 슬롯 클릭했을 때 아이템 선택
    public void OnClickButton()
    {
        inventoryUI.SelectedItem(index);
    }
    
}
