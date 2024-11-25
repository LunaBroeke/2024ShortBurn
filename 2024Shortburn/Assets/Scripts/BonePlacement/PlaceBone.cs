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

    private void Update()
    {

    }

    private void PutDownBone(InputAction.CallbackContext ctx)
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
            input.Player.PickUp.performed -= PutDownBone;
            renderer.material = Ghost;
            canPlace = false;
            bone = null;
        }
    }
}
