using UnityEngine;
using UnityEngine.InputSystem;

public class BonePulling : MonoBehaviour
{
    Bone selectedBone;
    PlayerInput recieveInput;
    private Vector2 moveDirection;
    //bool isPulling(PlayerState);
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
    private void Update()
    {
        selectedBone.transform.Translate(moveDirection.x * Time.deltaTime * 5f, 0f, moveDirection.y * Time.deltaTime * 5f);
    }
}
