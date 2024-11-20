using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePuzzleBehavior : MonoBehaviour, IInteractable
{
    public GameManager gmInstance;
    public Transform cameraPositioning;
    public AnimationCurve lerpCurve;
    public Camera mainCamera;
    public bool interacting = false;

    public void Start()
    {
        gmInstance = GameManager.instance;
        mainCamera = Camera.main;
    }

    public void Interact()
    {
        Camera source = gmInstance.playerCamera.GetComponent<Camera>();
        switch (interacting)
        {
            case true:
                StartCoroutine(LerpCamera(lerpCurve, source, gmInstance.playerCamera.transform));
                SwitchPlayerControls(true);
                break;
            case false:
                source.gameObject.GetComponent<ViewBobbing>().StopAllCoroutines();
                SwitchPlayerControls(false);
                StartCoroutine(LerpCamera(lerpCurve, source, cameraPositioning));
                break;
        }
    }

    public void CeaseInteract()
    {
        
    }

    public void SwitchPlayerControls(bool setTo)
    {
        mainCamera.gameObject.GetComponent<LookScript>().enabled = setTo;
        mainCamera.gameObject.GetComponent<ViewBobbing>().enabled = setTo;
        gmInstance.playerObject.GetComponent<MovementScript>().enabled = setTo;
        gmInstance.playerObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
    }

    public IEnumerator LerpCamera(AnimationCurve aniCurve, Camera cam, Transform destination)
    {
        float t = 0;
        cam.transform.SetParent(destination, true);
        Vector3 startPos = cam.transform.localPosition;
        Vector3 endPos = Vector3.zero;
        Quaternion startRot = cam.transform.localRotation;
        Quaternion endRot = Quaternion.Euler(0, 0, 0);

        while (t < 1)
        {
            t += Time.deltaTime;
            Vector3 newCamPos = Vector3.Lerp(startPos, endPos, aniCurve.Evaluate(t));
            Quaternion newRotation = Quaternion.Lerp(startRot, endRot, aniCurve.Evaluate(t));
            cam.transform.localPosition = newCamPos;
            cam.transform.localRotation = newRotation;
            yield return null;
        }
    }
}
