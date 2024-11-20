using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarPedestalBehaviour : MonoBehaviour, IPedestal
{
    GameManager gmInstance;
    [SerializeField] Transform attachTo; 
    [SerializeField] Vector3 relPosition;
    [SerializeField] bool attachToEqualsSelf;

    public void Start()
    {
        if (attachToEqualsSelf) { attachTo = gameObject.transform; }
        gmInstance = GameManager.instance;
    }

    public void Place() 
    {
        if (gmInstance == null)
        { gmInstance = GameManager.instance; }

        Transform pHoldTransform = gmInstance.playerHolding.transform;
        float yOffset = gmInstance.playerHolding.GetComponent<Collider>().bounds.extents.y / 2;
        relPosition = new Vector3(0, yOffset, 0);

        Rigidbody heldRB = pHoldTransform.GetComponent<Rigidbody>();

        pHoldTransform.gameObject.layer = pHoldTransform.GetComponent<HoldableItem>().defaultLayer;
        heldRB.useGravity = false;
        heldRB.constraints = RigidbodyConstraints.FreezeAll;
        pHoldTransform.SetParent(attachTo);
        pHoldTransform.localPosition = relPosition;

        gmInstance.playerHolding = null;
    }
}
