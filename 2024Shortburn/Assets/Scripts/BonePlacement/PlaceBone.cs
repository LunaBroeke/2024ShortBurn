using System.Runtime.CompilerServices;
using UnityEngine;

public class PlaceBone : MonoBehaviour
{
    private MeshRenderer renderer;

    [SerializeField] private Material Ghost;
    [SerializeField] private Material Glow;

    [SerializeField] private bool canPlace;

    [SerializeField] private GameObject bone;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        canPlace = false;
    }

    private void Update()
    {
        PutDownBone();
    }

    private void PutDownBone()
    {
        if (canPlace)
        {
            Debug.Log("Placing");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            renderer.material = Glow;
            canPlace = true;
            bone = other.gameObject;
            bone.transform.position = this.transform.position;
            bone.transform.rotation = this.transform.rotation;
            Rigidbody rb = bone.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            HoldableItem drop = bone.GetComponent<HoldableItem>(); 
            drop.SetDown(this.transform.position);
            Destroy(this.gameObject);
        }      
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null) 
        {
            renderer.material = Ghost;
            canPlace = false;
            bone = null;
        }
    }
}
