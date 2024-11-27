using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaceBone : MonoBehaviour
{
    private MeshRenderer renderer;

    [SerializeField] private Material Ghost;
    [SerializeField] private Material Glow;

    [SerializeField] private bool canPlace;

    [SerializeField] private GameObject bone;
    PlayerInputActions input;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        canPlace = false;
        input = GameManager.instance.pInputAct;
    }


    private void PutDownBone(InputAction.CallbackContext ctx)
    {
        if (CanPlace(bone))
        {
            Debug.Log("Placing");
            Rigidbody rb = bone.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            bone.transform.position = transform.position;
			bone.transform.rotation = transform.rotation;
            Destroy(gameObject);
        }
    }
    private bool CanPlace(GameObject bone)
    {
        float d = StaticCalculators.Distance(bone.transform.position, transform.position);
        Debug.Log(d);
        if (d < 5)
            return true; else return false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            renderer.material = Glow;
            bone = other.gameObject;
            Rigidbody rb = bone.GetComponent<Rigidbody>();
            input.Player.PickUp.performed += PutDownBone;                       
        }      
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null) 
        {
            input.Player.PickUp.performed -= PutDownBone;
            renderer.material = Ghost;
            canPlace = false;
            bone = null;
        }
    }
}
