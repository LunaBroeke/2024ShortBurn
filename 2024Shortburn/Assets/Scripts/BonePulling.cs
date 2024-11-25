using UnityEngine;
using UnityEngine.InputSystem;

public class BonePulling : MonoBehaviour
{
    GameManager gameManager;
    GameObject selectedBone;
    PlayerInput recieveInput;
    private Vector2 moveDirection;
    //bool isPulling(PlayerState);
    private void Start()
    {
        gameManager = GameManager.instance;
        HoldingBehavior.PickingUp += SelectBone;
    }
    private void HandleOnPull(InputAction.CallbackContext ctx)
    {
        moveDirection = ctx.ReadValue<Vector2>();
    }
    public void Initialize(PlayerInput recieveInput)
    {
        this.recieveInput = recieveInput;
        recieveInput.actions["Move"].performed += HandleOnPull;
        recieveInput.actions["Move"].started += HandleOnPull;
        recieveInput.actions["Move"].canceled += HandleOnPull;
    }
    private void SelectBone(GameObject obj)
    {
        if (obj.CompareTag("Bone"))
        {
            selectedBone = obj;
        }
    }
    private void Update()
    {

        if (selectedBone != null)
        {
            selectedBone.transform.Translate(moveDirection.x * Time.deltaTime * 5f, 0f, moveDirection.y * Time.deltaTime * 5f);
        }
    }
}
