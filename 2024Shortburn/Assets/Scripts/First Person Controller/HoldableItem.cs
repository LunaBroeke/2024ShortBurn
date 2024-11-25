using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableItem : MonoBehaviour, IHoldable
{
    GameManager gmInstance;
    OutlineBehavior outline;
    public LayerMask defaultLayer;
    public Vector3 customRotation;
    Rigidbody rb;

    public void Start()
    {
        gmInstance = GameManager.instance;
        outline = GetComponent<OutlineBehavior>();
        defaultLayer = gameObject.layer;
        rb = GetComponent<Rigidbody>();
    }
    public void Update()
    {
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    public void PickUp()
    {
        gameObject.transform.SetParent(gmInstance.itemHolder.transform);
        gameObject.transform.localRotation = Quaternion.Euler(customRotation);
        gmInstance.playerHolding = gameObject;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        outline.ChangeOutlineColor(outline.interactingColor);
        HoldingBehavior.TriggerPickUp(gameObject);
    }

    public void SetDown(Vector3 setDownPos)
    {
        gmInstance.itemHolder.transform.DetachChildren();
        gmInstance.playerHolding = null;
        gameObject.layer = defaultLayer;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        outline.ChangeOutlineColor(outline.outlineColor);
        HoldingBehavior.TriggerSettingDown(gameObject);
    }
}
