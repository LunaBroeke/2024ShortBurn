using System.Runtime.CompilerServices;
using UnityEngine;

public class PlaceBone : MonoBehaviour
{
    private MeshRenderer renderer;

    [SerializeField] private Material Ghost;
    [SerializeField] private Material Glow;

    [SerializeField] private bool canPlace;

    [SerializeField] private GameObject bone; // for debugging, dont apply anywhere.

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
        if (CanPlace(bone))
        {
            Debug.Log("Placing");
			bone.transform.position = transform.position;
			bone.transform.rotation = transform.rotation;
		}
    }
    private bool CanPlace(GameObject bone)
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            renderer.material = Glow;
            canPlace = true;
            GameObject bone = other.gameObject;

            Rigidbody rb = bone.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            HoldableItem drop = bone.GetComponent<HoldableItem>(); 
            drop.SetDown(transform.position);
            //Destroy(gameObject);
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
