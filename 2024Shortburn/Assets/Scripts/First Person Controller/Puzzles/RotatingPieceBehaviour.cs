using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotatingPieceBehaviour : MonoBehaviour, IInteractable
{
    PlayerInputActions pInputAct;
    InputAction rotate;
    GameManager gmInstance;
    OutlineBehavior outline;
    [SerializeField] Collider pieceCollider;
    [SerializeField] Transform playerPositioner;

    public float emmitterRotY = 0;

    float playerPosZOffset;

    public void Start()
    {
        gmInstance = GameManager.instance;
        pieceCollider = GetComponent<Collider>();
        outline = GetComponent<OutlineBehavior>();

        InstantiatePositioningElement();
        InstantiateActions();
    }

#region Input System Management
    public void InstantiateActions()
    {
        pInputAct = gmInstance.pInputAct;
        rotate = pInputAct.ObjectManipulaton.Rotate;
        EnableActions();
    }

    public void EnableActions()
    {
        rotate.Enable();
    }

    public void DisableActions()
    {
        rotate.Disable();
    }
    #endregion Input System Management

    public void Update()
    {
        if (gmInstance.interactingWith == gameObject && !gmInstance.isPaused)
        {
            //gmInstance.playerObject.transform.position = playerPositioner.position;
            RotateEmmitter();
        }
    }

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

    public void CorrectPositioningElement()
    {
        Ray findPlayer = new(transform.position, gmInstance.playerObject.transform.position - transform.position);
        playerPositioner.position = findPlayer.GetPoint(playerPosZOffset);
    }

    public void Interact()
    {
        CorrectPositioningElement();
        emmitterRotY = transform.localRotation.eulerAngles.y;
        gmInstance.interactingWith = gameObject;
        outline.ChangeOutlineColor(outline.interactingColor);

        StartCoroutine(LerpToPositioner());

        gmInstance.playerObject.GetComponent<MovementScript>().enabled = false;
        EnableActions();
    }

    public void CeaseInteract()
    {
        gmInstance.interactingWith = null;
        gmInstance.playerObject.GetComponent<MovementScript>().enabled = true;
        outline.ChangeOutlineColor(outline.outlineColor);
        DisableActions();
        Debug.Log("Ceasing Interaction");
    }

    public void RotateEmmitter()
    {
        float rawMove = rotate.ReadValue<float>() * Time.deltaTime * 50;

        emmitterRotY += rawMove;

        transform.rotation = Quaternion.Euler(0, emmitterRotY, 0);
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
