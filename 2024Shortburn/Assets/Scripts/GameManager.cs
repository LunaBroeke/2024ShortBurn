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
    }
}
