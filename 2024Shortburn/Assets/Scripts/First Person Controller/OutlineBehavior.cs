using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineBehavior : MonoBehaviour
{
    [SerializeField] GameObject outlineObjectPrefab;
    GameObject outlineObject = null;
    GameManager gmInstance;
    Coroutine currentCoroutine;
    Material outlineMat;
    Material outlineMaterialInstance;
    public Color outlineColor;
    public Color interactingColor;
    float maxOutlineThick;
    float minOutlineThick;

    void Start()
    {
        gmInstance = GameManager.instance;

        ///Only functions in tandem with an existing Resources Folder
        //outlineObjectPrefab = (GameObject)Resources.Load("Prefabs/Outline");
        
        maxOutlineThick = gmInstance.outlineThickness;
        minOutlineThick = 1;
        outlineMat = outlineObjectPrefab.GetComponent<MeshRenderer>().sharedMaterial;
    }


    void Update()
    {
        if (outlineObject != null)
        {
            if (gmInstance.playerLookingAt == gameObject && outlineObject.transform.localScale.x == minOutlineThick)
            { OutlineFunction(true); }
            else if (gmInstance.playerLookingAt != gameObject && outlineObject.transform.localScale.x == maxOutlineThick)
            { OutlineFunction(false); }
        }
        else if (outlineObject == null)
        { if (gmInstance.playerLookingAt == gameObject) { InstantiateOutlineObject(); } }
    }

    /// <summary>
    /// Instantiates the outline of this object upon the first viewing.
    /// </summary>
    public void InstantiateOutlineObject()
    {
        outlineObject = Instantiate(outlineObjectPrefab, transform.position, transform.rotation, transform);

        outlineMaterialInstance = new Material(outlineMat);
        ChangeOutlineColor(outlineColor);

        //Compensation for several materials on one object
        if (GetComponent<MeshRenderer>().materials.Length > 1)
        {
            Material[] materials = new Material[GetComponent<MeshRenderer>().materials.Length];
            for (int i = 0; i < materials.Length; i++) 
            { materials[i] = outlineMaterialInstance; }

            outlineObject.GetComponent<MeshRenderer>().materials = materials;
        }
        else
        { outlineObject.GetComponent<MeshRenderer>().material = outlineMaterialInstance; }

        outlineObject.GetComponent<MeshFilter>().mesh = GetComponent<MeshFilter>().mesh;
        currentCoroutine = StartCoroutine(SetOutline(true));
    }

    public void ChangeOutlineColor(Color targetColor)
    { outlineMaterialInstance.SetColor("_OutlineColor", targetColor); }

    public void OutlineFunction(bool onOrOff)
    {
        if (currentCoroutine != null) { StopCoroutine(currentCoroutine); }
        currentCoroutine = StartCoroutine(SetOutline(onOrOff));
    }

    /// <summary>
    /// Turns on or off the outline of this object depending on the bool onOrOff.
    /// </summary>
    /// <param name="onOrOff"> true = on, false = off.</param>
    /// <param name=""></param>
    /// <returns></returns>
    public IEnumerator SetOutline(bool onOrOff)
    {
        Keyframe lastKF = gmInstance.outlineLerp.keys[gmInstance.outlineLerp.length - 1];
        Debug.Log(lastKF.time);

        float t = 0;
        while (t < lastKF.time)
        {
            t += Time.deltaTime;
            float scaleLerp;

            if (onOrOff) { scaleLerp = Mathf.Lerp(minOutlineThick, maxOutlineThick, gmInstance.outlineLerp.Evaluate(t)); }
            else { scaleLerp = Mathf.Lerp(maxOutlineThick, minOutlineThick, gmInstance.outlineLerp.Evaluate(t)); }
            outlineObject.transform.localScale = new Vector3(scaleLerp, scaleLerp, scaleLerp);
            
            yield return null;
        }
    }
}
