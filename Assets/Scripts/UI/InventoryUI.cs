using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private ItemSlot[] itemSlots;
    
    private GameObject inventoryUI;
    private PlayerData playerData;
    private PlayerController playerController;
    private Player player;

    [SerializeField] private Transform itemSlotPanel;
    [SerializeField] private Transform dropPosition;

    [Header("Selected Item")]
    [SerializeField] private TextMeshProUGUI selectedItemName;
    [SerializeField] private TextMeshProUGUI selectedItemDescription;
    [SerializeField] private TextMeshProUGUI selectedItemStatName;
    [SerializeField] private TextMeshProUGUI selectedItemStatValue;
    [SerializeField] private GameObject useButton;
    [SerializeField] private GameObject dropButton;

    private ItemData selectedItemData;
    int selectedItemIndex = 0;

    private void Start()
    {
        inventoryUI = this.gameObject;
        playerData = FindObjectOfType<PlayerData>();
        playerController = FindObjectOfType<PlayerController>();
        playerController.inventory += Toggle; //player쪽에서 inventory 키 입력했는지 체크

        player = FindObjectOfType<Player>();
        player.addItem += AddItem; //player쪽에서 아이틈을 습득했는지 체크

        inventoryUI.SetActive(false);
        itemSlots = new ItemSlot[itemSlotPanel.childCount]; //itemslot 패널 아래에 있는 slot 갯수만큼 배열 받기

        for(int i = 0; i < itemSlots.Length; i++) //itemslot들 indexing
        {
            itemSlots[i] = itemSlotPanel.GetChild(i).GetComponent<ItemSlot>();
            itemSlots[i].index = i;
            itemSlots[i].inventoryUI = this;
        }

        ClearSelectedItemWindow();
        UpdateUI();
    }

    // info 내용 지워주기
    void ClearSelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        useButton.SetActive(false);
        dropButton.SetActive(false);
    }

    // UI 켜고 끄기
    void Toggle()
    {
        if (IsOpen()) // UI가 켜져있는지 확인
        {
            inventoryUI.SetActive(false);
        }
        else
        {
            inventoryUI.SetActive(true);
        }
    }

    bool IsOpen()
    {
        return inventoryUI.activeInHierarchy;
    }

    void AddItem()
    {
        ItemData data = player.itemData;

        // 여러개 가질 수 있는 item이면
        if (data.canStack) 
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null) // slot에 이미 있으면 item의 갯수 추가
            {
                slot.quantity++;
                UpdateUI();
                player.itemData = null;
                return;
            }

            // 빈 슬롯 찾기
            ItemSlot emptySlot = GetEmptySlot();

            // 빈 슬롯이 null이 아니면 인벤토리에 아이템 추가
            if (emptySlot != null)
            {
                emptySlot.item = data;
                emptySlot.quantity = 1;
                UpdateUI();
                player.itemData = null;
                return;
            }

            // 빈 슬롯이 없으면 아이템 버리기
            ThrowItem(data);

            player.itemData = data;
        }
    }

    // itemSlot이 null이 아니면 Set, null이면 Celar
    void UpdateUI()
    {
        for (int i=0; i<itemSlots.Length; i++)
        {
            if (itemSlots[i].item != null)
            {
                itemSlots[i].Set();
            }
            else
            {
                itemSlots[i].Clear();
            }
        }
    }

    // itemSlot에 stack가능한 item이 있는지 확인. 있으면 itemslot 반환, 없으면 null 반환
    ItemSlot GetItemStack(ItemData data) 
    {
        for(int i=0; i<itemSlots.Length; i++)
        {
            if (itemSlots[i].item == data && itemSlots[i].quantity < data.maxAmount)
            {
                return itemSlots[i];
            }
        }
        return null;
    }
    
    // itemSlot이 비어있는지 확인. null이면 itemSlot 반환, 없으면 null
    ItemSlot GetEmptySlot()
    {
        for(int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].item == null)
            {
                return itemSlots[i];
            }
        }
        return null;
    }

    // item 버리기
    ItemSlot ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
        return null;
    }

    // 버튼으로 선택된 itemSlot의 item 정보 띄우기
    public void SelectedItem(int index)
    {
        if (itemSlots[index].item == null) return;
        
        ClearSelectedItemWindow();

        selectedItemData = itemSlots[index].item;
        selectedItemIndex = index;

        selectedItemName.text = selectedItemData.itemName;
        selectedItemDescription.text = selectedItemData.itemDescription;

        for (int i=0; i < selectedItemData.consumables.Length; i++)
        {
            selectedItemStatName.text = selectedItemData.consumables[i].type.ToString() + "\n";
            selectedItemStatValue.text = selectedItemData.consumables[i].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItemData.type == EItemType.Consumable);
        dropButton.SetActive(true);
    }

    // 아이템 사용 시
    public void OnUseButton()
    {
        if(selectedItemData.type == EItemType.Consumable)
        {
            for(int i = 0; i < selectedItemData.consumables.Length; i++)
            {
                // 여기서 i는 ConsumableType의 enum 값
                switch (selectedItemData.consumables[i].type)
                {
                    case ConsumableType.Hp:
                        playerData.Heal(selectedItemData.consumables[i].value);
                        break;
                    case ConsumableType.JumpPower:
                        playerData.JumpBoost(selectedItemData.consumables[i].value);
                        break;
                    case ConsumableType.RunSpeed:
                        playerData.SpeedBoost(selectedItemData.consumables[i].value);
                        break;
                }
            }
            RemoveSelectedItem();
        }
    }

    // 선택한 아이템 버리기 버튼
    public void OnDropButton()
    {
        ThrowItem(selectedItemData);
        RemoveSelectedItem();
    }

    // 선택한 아이템 사용/드롭 시 제거
    void RemoveSelectedItem()
    {
        itemSlots[selectedItemIndex].quantity--;

        if (itemSlots[selectedItemIndex].quantity <= 0) // 0이하면 슬롯 비우기
        {
            selectedItemData = null;
            itemSlots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }
        UpdateUI();
    }
}
