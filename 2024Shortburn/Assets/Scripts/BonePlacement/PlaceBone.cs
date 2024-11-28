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
        renderer.material = Ghost;
    }


    private void PutDownBone(InputAction.CallbackContext ctx)
    {
        if (CanPlace(bone))
        {
            Debug.Log("Placing bone...");

            Rigidbody rb = bone.GetComponent<Rigidbody>();
            rb.isKinematic = true; // Disable physics

            bone.transform.position = transform.position; // Match position
            bone.transform.rotation = transform.rotation; // Match rotation

            Debug.Log($"Bone placed at position: {bone.transform.position}, rotation: {bone.transform.rotation.eulerAngles}");

            Destroy(gameObject); // Remove the PlaceBone object
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
        if (other.gameObject == bone)
        {
            renderer.material = Glow;          
            Rigidbody rb = bone.GetComponent<Rigidbody>();
            input.Player.PickUp.performed += PutDownBone;                       
        }      
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == bone) 
        {
            input.Player.PickUp.performed -= PutDownBone;
            renderer.material = Ghost;
            canPlace = false;           
        }
    }
}
