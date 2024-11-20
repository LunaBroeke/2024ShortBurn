using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovingPieceBehaviour : MonoBehaviour, IInteractable
{   
    [SerializeField, Tooltip("0.5 speed is recommended.")] private float speed = 5f;
    PlayerInputActions pInputAct;
    InputAction movement;
    GameManager gmInstance;
    OutlineBehavior outline;
    [SerializeField] Collider pieceCollider;
    [SerializeField] Rigidbody pieceRB;
    [SerializeField] Transform playerPositioner;

    float playerPosZOffset;

    //moves pillar in desired direction and stops movement in undesired direction
    private void Start()
    {
        gmInstance = GameManager.instance;
        pieceCollider = GetComponent<Collider>();
        pieceRB = GetComponent<Rigidbody>();
        outline = GetComponent<OutlineBehavior>();

        InstantiatePositioningElement();
        InstantiateActions();
    }

    private void Update()
    {
        if (gmInstance.interactingWith == gameObject)
        {
            gmInstance.playerObject.transform.position = playerPositioner.position;
            MoveObject();
        }
    }

    #region Input System Management
    public void InstantiateActions()
    {
        pInputAct = gmInstance.pInputAct;
        movement = pInputAct.ObjectManipulaton.Move;
        EnableActions();
    }

    public void EnableActions()
    { movement.Enable(); }

    public void DisableActions()
    { movement.Disable(); }
    #endregion Input System Management

    public void InstantiatePositioningElement()
    {
        playerPosZOffset = pieceCollider.bounds.extents.z + 1.5f;
        Vector3 posElementPos = new(transform.position.x, transform.position.y, transform.position.z - playerPosZOffset);

        //Instantiates the positioning element.
        playerPositioner = new GameObject("PlayerPositioningElement").transform;
        playerPositioner.tag = "Positioner";
        playerPositioner.SetParent(transform);
        playerPositioner.transform.position = posElementPos;

        //Ensures no ground clipping takes place with the Player.
        float pSize = gmInstance.playerObject.GetComponent<Collider>().bounds.extents.y;
        Ray groundCheck = new(playerPositioner.position, Vector3.down);
        Physics.Raycast(groundCheck, out RaycastHit groundHit);

        if (groundHit.collider != null)
        {
            if (groundHit.distance < pSize)
            {
                Vector3 groundPos = groundHit.point;
                Vector3 adjustedPos = new(groundPos.x, groundPos.y + pSize, groundPos.z);
                playerPositioner.position = adjustedPos;
            }
        }
        else
        {
            Debug.LogError(gameObject.name + " is below any reachable surface; " +
                gameObject.name + "'s current position is " + gameObject.transform.position);
        }
    }

    public void Interact()
    {
        gmInstance.interactingWith = gameObject;
        outline.ChangeOutlineColor(outline.interactingColor);
        StartCoroutine(LerpToPositioner());

        pieceRB.constraints = RigidbodyConstraints.None;
        pieceRB.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

        gmInstance.playerObject.GetComponent<MovementScript>().enabled = false;
        EnableActions();
    }

    public void CeaseInteract()
    {
        gmInstance.interactingWith = null;
        gmInstance.playerObject.transform.parent = null;
        outline.ChangeOutlineColor(outline.outlineColor);
        StopAllCoroutines();

        pieceRB.constraints = RigidbodyConstraints.FreezeAll;
        
        gmInstance.playerObject.GetComponent<MovementScript>().enabled = true;
        DisableActions();
        Debug.Log("Ceasing Interaction");
    }

    public void MoveObject()
    {
        Vector3 rawMove = movement.ReadValue<Vector3>() * Time.deltaTime * (speed * 100);

        if (rawMove.x != 0 && rawMove.z != 0)
        { rawMove /= 2; }

        Vector3 movementVector = gmInstance.playerObject.transform.right * rawMove.x + gmInstance.playerObject.transform.forward * rawMove.z;
        movementVector.y = pieceRB.linearVelocity.y;
        pieceRB.linearVelocity = movementVector;
    }

    public IEnumerator LerpToPositioner()
    {
        float t = 0;
        Vector3 StartPos = gmInstance.playerObject.transform.position;
        Vector3 EndPos = playerPositioner.position;

        while (t < 1)
        {
            t += Time.deltaTime;
            gmInstance.playerObject.transform.position = Vector3.Lerp(StartPos, EndPos, t);
            yield return null;
        }
    }
}
