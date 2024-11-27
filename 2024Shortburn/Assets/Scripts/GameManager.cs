using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerInputActions pInputAct;
    public PlayerState playerState;

    [Header("Player")]
    public GameObject playerObject;
    public GameObject playerLookingAt;
    public GameObject playerHolding;
    public GameObject interactingWith;
    public GameObject playerCamera;

    public GameObject camHolder;
    public GameObject itemHolder;
    public Vector3 playerLookingAtPos;
    public float playerReachDistance;
    public float playerLookDistance;
    public bool playerMoving;
    public bool playerGrounded;

    [Header("Shaders")]
    public AnimationCurve outlineLerp;
    public float outlineThickness;

    [Header("Other")]
    public bool isPaused = false;

    private void Awake()
    {
        //Singleton snippet.
        if (instance != null && instance != this)
        { Destroy(this); }
        else
        {
            instance = this;
        }

        pInputAct = new PlayerInputActions();
        ChangePlayerState(PlayerState.Player);

        pInputAct.Debug.AdvanceState.performed += advanceState;
    }

	public static void EnablePlayerInput()
    {
        instance.pInputAct.Player.Enable();
    }

    public static void DisablePlayerInput()
    {
        instance.pInputAct.Player.Disable();
    }

    public static void EnableObjectInput()
    {
        instance.pInputAct.ObjectManipulaton.Enable();
    }
    public static void DisableObjectInput()
    {
        instance.pInputAct.ObjectManipulaton.Disable();
    }

    public static void DisableAllActionMaps()
    {
        instance.pInputAct.Disable();
		instance.pInputAct.Debug.Enable();
	}

    private void advanceState(InputAction.CallbackContext ctx)
    {
        ChangePlayerState(playerState + 1);
    }

    public static void ChangePlayerState(PlayerState state)
    {
        instance.playerState = state;
        DisableAllActionMaps();
		switch (state)
		{
			case PlayerState.None:
				DisableAllActionMaps();
				Debug.Log($"FOR WHAT PURPOSE {state}");
				break;
			case PlayerState.Player:
                EnablePlayerInput();
				break;
			case PlayerState.Pulling:
                EnableObjectInput();
				break;
			case PlayerState.Placing:
                EnableObjectInput();
				break;
		}
        Debug.Log($"Changed Player State {state}");
	}
}
