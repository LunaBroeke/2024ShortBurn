using UnityEngine;

public class PlaceBone : MonoBehaviour
{
    private MeshRenderer renderer;

    [SerializeField] private Material Ghost;
    [SerializeField] private Material Glow;

    [SerializeField] private bool canPlace;

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
        }      
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null) 
        {
            renderer.material = Ghost;
            canPlace = true;
        }
    }
}
