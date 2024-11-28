using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementScript : MonoBehaviour
{
    [SerializeField] Rigidbody playerRB;
    [SerializeField] GameManager gmInstance;

    //Input System
    PlayerInputActions pInputAct;
    InputAction movement;
    InputAction jump;

    [SerializeField] float movementSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float maxGroundDist;
    [SerializeField] bool moving = false;

    public void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody>();
        gmInstance = GameManager.instance;
        InstantiateActions();
    }

    public void InstantiateActions() 
    {
        pInputAct = gmInstance.pInputAct;
        movement = pInputAct.Player.Move;
        jump = pInputAct.Player.Jump;
		jump.performed += Jump;
		//EnableActions();
    }

    public void EnableActions()
    {
        movement.Enable();

        jump.Enable();

    }

    public void DisableActions() 
    { 
        movement.Disable();
        jump.Disable();
    }

	private void OnDisable()
	{
		DisableActions();
	}

	public void Update()
    {
        //if (!movement.enabled) { EnableActions(); }
        if (!gmInstance.isPaused) { Movement(); }

        gmInstance.playerMoving = moving;
        gmInstance.playerGrounded = isGrounded();
    }

#region Basic Movement
    public void Movement()
    {
        Vector3 rawMove = movement.ReadValue<Vector3>() * (movementSpeed * 10);

        if (rawMove != Vector3.zero) { moving = true; }
        else { moving = false; }

        if (rawMove.x != 0 && rawMove.z != 0)
        { rawMove /= 2; }
        if (!isGrounded())
        { rawMove /= 1.5f; }

        Vector3 movementVector = transform.right * rawMove.x + transform.forward * rawMove.z;
        movementVector.y = playerRB.linearVelocity.y;
        playerRB.linearVelocity = movementVector;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded())
        {
            Vector3 jumpVector = new(0, jumpForce, 0);
            playerRB.AddForce(jumpVector, ForceMode.VelocityChange);
        }
    }
#endregion Basic Movement

    public bool isGrounded()
    {
        Ray detectGround = new Ray(transform.position, Vector3.down);
        Physics.Raycast(detectGround, out RaycastHit detectHit, maxGroundDist);

        //Debug.DrawLine(detectGround.origin, detectHit.point, Color.red);
        Vector3 endOfRay = new Vector3(detectGround.origin.x, detectGround.origin.y - maxGroundDist, detectGround.origin.z);

        Debug.DrawLine(detectGround.origin, endOfRay);

        if (detectHit.collider != null) { return true; }
        return false;
    }
}
