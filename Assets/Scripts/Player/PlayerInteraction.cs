using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float checkRate = 0.5f;
    private float lastCheckTime;
    [SerializeField] private float maxDistance = 3f;
    [SerializeField] private LayerMask layerMask;

    public GameObject curInteractGO;
    private IInteractable curInteractable;

    [SerializeField] private TextMeshProUGUI promptText;
    private Camera playerCamera;


    private void Start()
    {
        playerCamera = Camera.main;
    }

    private void Update()
    {
        if(Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width /2, Screen.height /2));
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, maxDistance, layerMask))
            {
                if(hit.collider.gameObject != curInteractGO)
                {
                    curInteractGO = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else
            {
                SetNull();
            }
        }
    }

    void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    void SetNull()
    {
        curInteractGO = null;
        curInteractable = null;
        promptText.gameObject.SetActive(false);
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            SetNull();
        }
    }
}
