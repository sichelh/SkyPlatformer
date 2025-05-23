using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;
    Player player;

    private void Awake()
    {
        player = GameManager.Instance.player;
    }

    // 아이템 감지되면 정보 출력
    public string GetInteractPrompt()
    {
        string str = $"{data.itemName}\n{data.itemDescription}";
        return str;
    }

    // 인터랙션하면 player에게 아이템 데이터를 넘기고 destroy
    public void OnInteract()
    {
        player.itemData = data;
        player.addItem?.Invoke();
        Destroy(gameObject);
    }
}
