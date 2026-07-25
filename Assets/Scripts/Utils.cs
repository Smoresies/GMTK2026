using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static WeightedObject<T> GetRandomWeightedObject<T>(List<WeightedObject<T>> weightedObjects)
    {
        int totalWeight = 0;
        foreach (var weightedObject in weightedObjects)
        {
            totalWeight += weightedObject.weight;
        }

        int randomValue = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;

        foreach (var weightedObject in weightedObjects)
        {
            cumulativeWeight += weightedObject.weight;
            if (randomValue < cumulativeWeight)
            {
                return weightedObject;
            }
        }

        return default; // This should never happen if the list is not empty
    }
}
