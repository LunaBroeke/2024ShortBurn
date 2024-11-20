using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum BobbingStatus
{
    Starting,
    Running,
    Ending
}

public class LookScript : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject camHolder;
    public GameObject playerObject;
    public GameManager gmInstance;

    public float camSensitivity;
    public float rotationX = 0;
    public float rotationY = 0;
    public float maxReachDistance;

    PlayerInputActions pInputAct;
    InputAction look;

    void Start()
    {
        gmInstance = GameManager.instance;
        mainCamera = GetComponent<Camera>();
        gmInstance.playerCamera = mainCamera.gameObject;
        playerObject = gmInstance.playerObject;
        maxReachDistance = gmInstance.playerLookDistance;
        InstantiateActions();
    }

    private void Update()
    {
        if (!look.enabled) { EnableActions(); }
        if (!gmInstance.isPaused) { Rotation(); }

        gmInstance.playerLookingAt = LookingAtGameObject();
    }

#region Input System Management

    public void InstantiateActions()
    {
        pInputAct = gmInstance.pInputAct;
        look = pInputAct.Player.Look;
        EnableActions();
    }

    public void EnableActions()
    {
        look.Enable();
    }

    public void OnDisable()
    {
        look.Disable();
    }
#endregion Input System Management

    public void Rotation()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector2 lookVector = (camSensitivity * 10) * Time.fixedDeltaTime * look.ReadValue<Vector2>();

        rotationY += lookVector.x;
        rotationX -= lookVector.y;
        rotationX = Mathf.Clamp(rotationX, -85, 85);

        playerObject.transform.rotation = Quaternion.Euler(0, rotationY, 0);
        camHolder.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }

    public GameObject LookingAtGameObject()
    {
        Ray sight = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Physics.Raycast(sight, out RaycastHit sightHit, maxReachDistance);
        gmInstance.playerLookingAtPos = sight.GetPoint(maxReachDistance);

        if (sightHit.collider != null)
        { return sightHit.collider.gameObject; }
        else { return null; }
    }

    public void OnDrawGizmos()
    {
        Ray sight = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Physics.Raycast(sight, out RaycastHit sightHit, maxReachDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(sight.origin, sightHit.point);
    }
}
