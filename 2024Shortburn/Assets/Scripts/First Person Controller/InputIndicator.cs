using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputIndicator : MonoBehaviour
{
    GameManager gmInstance;
    [SerializeField] TextMeshProUGUI indicationText;
    [SerializeField] AnimationCurve textLerpCurve;

    Coroutine currentCoroutine;

    string pickUpText = "Press 'E' to Pick Up and Drop Objects";
    string movingText = "Left Click to Interact (Move)";
    string rotateText = "Left Click to Interact (Rotate)";
    string pedestalText = "Press 'E' to Place object on Pedestal";

    [SerializeField] int[] individualDisplayCount = new int[4];
    [SerializeField] int arrayCounter = 0;
    [SerializeField] bool[] ignoreInForLoop = new bool[4] { false, false, false, false };

    public void Start()
    { gmInstance = GameManager.instance; }

    public void Update()
    {
        if (arrayCounter < 4)
        { IndicationText(); }
        else
        { enabled = false; }
    }

    public void IndicationText()
    {
        GameObject viewing = gmInstance.playerLookingAt;
        GameObject holding = gmInstance.playerHolding;

        if (viewing != null)
        {
            if (viewing.GetComponent<IPedestal>() != null && holding != null && indicationText.text != pedestalText)
            { ClearAndStartCoroutines(pedestalText, 3); }
            if (holding == null)
            {
                if (viewing.GetComponent<IHoldable>() != null && indicationText.text != pickUpText)
                { ClearAndStartCoroutines(pickUpText, 0); }
                if (viewing.GetComponent<MovingPieceBehaviour>() != null && indicationText.text != movingText)
                { ClearAndStartCoroutines(movingText, 1); }
                if (viewing.GetComponent<RotatingPieceBehaviour>() != null && indicationText.text != rotateText)
                { ClearAndStartCoroutines(rotateText, 2); }
            }
        }
        else { indicationText.text = ""; }
    }

    public void ClearAndStartCoroutines(string inputText, int textArrayPos)
    {
        if (currentCoroutine != null)
        { StopCoroutine(currentCoroutine); }
        currentCoroutine = StartCoroutine(LerpText(inputText));
        individualDisplayCount[textArrayPos]++;
        for (int i = 0; i < individualDisplayCount.Length; i++)
        {
            if (individualDisplayCount[i] > 0 && ignoreInForLoop[i] == false)
            {
                arrayCounter++;
                ignoreInForLoop[i] = true;
            }
        }
    }

    public IEnumerator LerpText(string text)
    {
        float t = 0;
        float alpha = 0;
        Color color = indicationText.color;
        indicationText.text = text;

        while (t < 1)
        {
            t += Time.deltaTime;
            alpha = Mathf.Lerp(0, 1, textLerpCurve.Evaluate(t));
            color.a = alpha;
            indicationText.color = color;
            yield return null;
        }
    }

}
