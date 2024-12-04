using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
   [SerializeField] private int currentBones;
    [SerializeField] private int totalBones;

    public void Count() 
    { 
        currentBones++;
        if (currentBones == totalBones)
        {
            Debug.Log("Skeleton complete!!!");
        }
    }
}
