using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerInputActions pInputAct;

    [Header("Player")]
    public GameObject playerObject;
    public GameObject playerLookingAt;
    public GameObject playerHolding;
    public GameObject interactingWith;
    public GameObject playerCamera;

    public GameObject camHolder;
    public GameObject itemHolder;
    public Vector3 playerLookingAtPos;
    public float playerReachDistance;
    public float playerLookDistance;
    public bool playerMoving;
    public bool playerGrounded;

    [Header("Shaders")]
    public AnimationCurve outlineLerp;
    public float outlineThickness;

    [Header("Lighting")]
    public List<ColoredLight> lights = new();

    [Header("Other")]
    public bool isPaused = false;

    private void Awake()
    {
        //Singleton snippet.
        if (instance != null && instance != this)
        { Destroy(this); }
        else
        {
            instance = this;
        }

        pInputAct = new PlayerInputActions();
        
        // Finds each light
        foreach (ColoredLight gameObject in FindObjectsOfType<ColoredLight>())
        {
            lights.Add(gameObject);
            foreach (ColoredLight light in lights)
            {
                light.gameObject.SetActive(false);
            }
        }
    }

    private void Start()
    {
        Enable();
    }

    private async Task Enable()
    {
        await Task.Delay(1000);
        foreach (ColoredLight light in lights)
        {
            light.gameObject.SetActive(true);// enables every light
        }

    }

    private void Update()
    {

        for (int i = 0; i < FindObjectsOfType<Light>().Length; i++)
        {
            FindObjectsOfType<Light>()[i].shadows = LightShadows.Hard; // sets everything to hard shadows because it doesnt do that automatically for some reason even though we set the lights to.
        }
    }

    /// <summary>
    /// Stupid light system needs to refresh itself every reload
    /// </summary>
    /// <returns></returns>
    private async Task Refresh()
    {
        lights.Clear();
        foreach (ColoredLight gameObject in FindObjectsOfType<ColoredLight>()) // gets each light
        {
            lights.Add(gameObject);
        }
        foreach (ColoredLight light in lights) // sets each light inactive
        {
            light.gameObject.SetActive(false);
        }
        await Task.Delay(1000);
        for (int i = 0; i < lights.Count; i++)
        {
            ColoredLight light = lights[i];
            if (light != null)
            {
                light.gameObject.SetActive(true); // set each light active
                Debug.Log($"Refreshed Light {light.gameObject.name}");
            }
            else
            {
               Destroy(light.gameObject);// please work
                return;
            }
        }
       
    }
}
