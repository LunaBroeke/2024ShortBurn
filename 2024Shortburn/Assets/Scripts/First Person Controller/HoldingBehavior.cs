using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoldingBehavior : MonoBehaviour
{
    [SerializeField] GameObject itemHolderPrefab;
    [SerializeField] GameObject itemHolder;
    GameManager gmInstance;

    //Input System
    PlayerInputActions pInputAct;
    InputAction pickUp;
    InputAction armUse;

    Ray reach;

    [SerializeField] float maxReachDistance;
    [SerializeField] float currentReachDistance;
    [SerializeField] float armSensitivity = 1;

    bool playerSees;
    bool playerHolds = false;
    public static event Action<GameObject> PickingUp;
    public static void TriggerPickUp(GameObject obj)
    {
        PickingUp?.Invoke(obj);
    }

    public void Start()
    {
        gmInstance = GameManager.instance;
        gmInstance.playerHolding = null;
        maxReachDistance = gmInstance.playerReachDistance;

        ///Only functions if there is a corresponding Resources folder
        //itemHolderPrefab = Resources.Load<GameObject>("Prefabs/ItemHolder");

        InstantiateItemHolder();

        InstantiateActions();
    }
    public void InstantiateActions()
    {
        pInputAct = gmInstance.pInputAct;
        pickUp = pInputAct.Player.PickUp;
        armUse = pInputAct.Player.ArmManipulation;
        EnableActions();
    }

    public void EnableActions()
    {
        pickUp.Enable();
        pickUp.performed += PickUpConditions;

        armUse.Enable();
        armUse.performed += ChangeDistance;
    }

    public void OnDisable()
    {
        pickUp.Disable();
        armUse.Disable();
    }

    public void PickUpConditions(InputAction.CallbackContext obj)
    {
        if (gmInstance.playerLookingAt != null)
        {
            if (gmInstance.playerLookingAt.GetComponent<IHoldable>() != null)
            {
                IHoldable target = gmInstance.playerLookingAt.GetComponent<IHoldable>();
                //If the player is looking at an object, and is holding an object, the two objects will get swapped.
                if (gmInstance.playerHolding != null)
                {
                    if (gmInstance.playerLookingAt != gmInstance.playerHolding)
                    {
                        SetDownCurrentObject();
                        PickUpObservedObject();
                    }
                    else { SetDownCurrentObject(); }
                }
                else if (gmInstance.playerHolding == null) { PickUpObservedObject(); }
                //Else if the player is *not* holding an object, but is looking at an object, the player will pick up the object they are looking at.
            }
            else if (gmInstance.playerLookingAt.GetComponent<IPedestal>() != null)
            {
                if (gmInstance.playerHolding != null)
                { gmInstance.playerLookingAt.GetComponent<IPedestal>().Place(); }
            }
            else if (gmInstance.playerHolding != null) { SetDownCurrentObject(); }
        }
        else if (gmInstance.playerHolding != null)
        { { SetDownCurrentObject(); } }
    }

    public void SetDownCurrentObject()
    {
        IHoldable holdable = gmInstance.playerHolding.GetComponent<IHoldable>();
        holdable.SetDown(gmInstance.playerLookingAtPos);
    }

    public void PickUpObservedObject()
    {
        gmInstance.playerLookingAt.GetComponent<IHoldable>().PickUp();

        DrawItemRangeRay();
        Physics.Raycast(reach, out RaycastHit reachHit);
        currentReachDistance = reachHit.distance;

        ChangeDistance(0);
    }

    public void InstantiateItemHolder()
    {
        GameObject camHolder = null;
        GameObject[] allPositioners = GameObject.FindGameObjectsWithTag("Positioner");
        foreach (var gO in allPositioners)
        {
            Camera cam = gO.GetComponentInChildren<Camera>();
            if (cam != null && cam == Camera.main)
            { camHolder = gO; }
        }

        if (camHolder != null)
        {
            gmInstance.camHolder = camHolder;
            itemHolder = Instantiate(itemHolderPrefab, camHolder.transform);
            gmInstance.itemHolder = itemHolder;
        }
        else
        { Debug.LogError("Could not find Camera or Camera Parent object."); }
    }

    public void DrawItemRangeRay()
    {
        Ray fromCamHolder = new(gmInstance.camHolder.transform.position, gmInstance.camHolder.transform.forward);
        Vector3 fCHMax = fromCamHolder.GetPoint(maxReachDistance);
        Debug.DrawLine(fromCamHolder.origin, fCHMax);

        Vector3 reachMax = gmInstance.playerCamera.GetComponent<Camera>().ViewportPointToRay(new(0.5f, 0.5f, maxReachDistance)).GetPoint(maxReachDistance);
        Vector3 reachDir = reachMax - itemHolder.transform.position;

        reach = new(itemHolder.transform.position, reachDir);
    }

    public void ChangeDistance(InputAction.CallbackContext obj) 
    { ChangeDistance((armUse.ReadValue<float>() / 1200) * armSensitivity); }

    /// <summary>
    /// Changes the Distance between the Player and whatever the player is holding.
    /// </summary>
    /// <param name="distanceIncrement"> Positive values increase distance, negative values decrease distance. </param>
    public void ChangeDistance(float distanceIncrement)
    {
        if (gmInstance.playerHolding != null)
        {
            DrawItemRangeRay();

            currentReachDistance += distanceIncrement;
            currentReachDistance = Mathf.Clamp(currentReachDistance, 0, maxReachDistance);

            Vector3 holderLocalPos = itemHolder.transform.InverseTransformPoint(reach.GetPoint(currentReachDistance));
            gmInstance.playerHolding.transform.localPosition = holderLocalPos;
        }
    }
}
