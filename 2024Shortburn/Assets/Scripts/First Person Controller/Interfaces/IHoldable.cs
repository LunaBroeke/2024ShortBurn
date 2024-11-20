using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHoldable
{
    public void PickUp();
    public void SetDown(Vector3 setDownPosition);
}
