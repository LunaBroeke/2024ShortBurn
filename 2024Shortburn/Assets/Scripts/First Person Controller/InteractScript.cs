using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractScript : MonoBehaviour
{
    //Input System
    PlayerInputActions pInputAct;
    InputAction interact;
    InputAction pickUp;
    private GameManager gmInstance;

    private void Start()
    {
        gmInstance = GameManager.instance;
        InstantiateActions();
    }

    public void Update()
    {
        if (!interact.enabled) { EnableActions(); }
    }

    public void InstantiateActions()
    {
        pInputAct = gmInstance.pInputAct;
        interact = pInputAct.Player.Interact;
        EnableActions();
    }

    public void EnableActions()
    {
        interact.Enable();
        interact.performed += Interact;
    }

    public void OnDisable()
    {
        interact.Disable();
    }

    public void Interact(InputAction.CallbackContext obj)
    {
        if (gmInstance.playerLookingAt != null)
        {
            if (gmInstance.playerLookingAt.GetComponent<IInteractable>() != null)
            {
                IInteractable interactable = gmInstance.playerLookingAt.GetComponent<IInteractable>();

                if (gmInstance.interactingWith != null)
                { interactable.CeaseInteract(); }
                else { interactable.Interact(); }
            }
        }
        else if (gmInstance.interactingWith != null)
        {
            gmInstance.interactingWith.GetComponent<IInteractable>().CeaseInteract();
        }
    }
}
