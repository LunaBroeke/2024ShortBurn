using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewBobbing : MonoBehaviour
{
    public Camera mainCamera;
    public GameManager gmInstance;
    public AnimationCurve viewBobbing;

    public Coroutine bobbingCoroutine;
    public BobbingStatus bState;

    public Transform cameraPositioning;
    public Vector3 defaultCamPos;
    public Vector2 xyCamBobbingOffset;

    [SerializeField] Vector2 bobPosL;
    [SerializeField] Vector2 bobPosR;

    public float bobTime;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        gmInstance = GameManager.instance;
        defaultCamPos = transform.localPosition;

        bobPosL = new Vector2(defaultCamPos.x - xyCamBobbingOffset.x, defaultCamPos.y + xyCamBobbingOffset.y);
        bobPosR = new Vector2(defaultCamPos.x + xyCamBobbingOffset.x, defaultCamPos.y + xyCamBobbingOffset.y);
    }

    private void Update()
    {
        if (gmInstance.playerMoving)
        {
            if (bobbingCoroutine == null)
            { bobbingCoroutine = StartCoroutine(DoViewBobbing(viewBobbing)); }
        }
    }

    public IEnumerator DoViewBobbing(AnimationCurve bobbingCurve)
    {
        float aniDuration = bobbingCurve.keys[bobbingCurve.keys.Length - 1].time;
        Debug.Log(aniDuration);

        bobTime = 0;

        bState = BobbingStatus.Starting;

        float currentCamPosX;
        float currentCamPosY;

        float startX = defaultCamPos.x;
        float endX = defaultCamPos.x;

        while (gmInstance.playerMoving)
        {
            bobTime += Time.deltaTime;

            if (bobTime > aniDuration)
            { bState = BobbingStatus.Running; }

            switch (bState)
            {
                case BobbingStatus.Starting:
                    startX = mainCamera.transform.localPosition.x;
                    endX = bobPosR.x;
                    break;
                case BobbingStatus.Running:
                    endX = bobPosR.x;
                    startX = bobPosL.x;
                    break;
            }

            currentCamPosX = Mathf.Lerp(startX, endX, bobbingCurve.Evaluate(bobTime));
            currentCamPosY = Mathf.Lerp(defaultCamPos.y, bobPosR.y, bobbingCurve.Evaluate(bobTime * 2));
            Vector3 newCamPos = new(currentCamPosX, currentCamPosY, defaultCamPos.z);

            mainCamera.transform.localPosition = newCamPos;
            yield return null;
        }

        bState = BobbingStatus.Ending;
        Vector3 storedCamPos = mainCamera.transform.localPosition;
        float returnTime = 0;
        while (returnTime < aniDuration)
        {
            returnTime += Time.deltaTime;
            Vector3 newCamPos = Vector3.Lerp(storedCamPos, defaultCamPos, bobbingCurve.Evaluate(returnTime));
            mainCamera.transform.localPosition = newCamPos;
            yield return null;
        }

        bobbingCoroutine = null;
    }
}
