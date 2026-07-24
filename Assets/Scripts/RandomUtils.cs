using System;
using System.Collections.Generic;
using UnityEngine;

public static class RandomUtils
{
    private static int _seed = -1;
    private static bool _isSeedInit = false;
    private static System.Random _seededRandomizer = new();
    private static System.Random _unseededRandomizer = new();
    public static void ShuffleList<T>(this IList<T> list, bool seeded = true)
    {
        InitSeed();
        Debug.Log("Shuffle List with seed: " + _seed);
        for (int i = list.Count - 1; i > 0; i--)
        {
            // Pick a random index from 0 to i
            int randomIndex = getRandomizer(seeded).Next(0, list.Count);
            // Swap elements
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    /// <summary>
    /// Inits the seed with an optional given value or a random value.
    /// </summary>
    /// <param name="seed">Optional seed to set. Used for debugging.</param>
    private static void InitSeed(int seed = -1)
    {
        // if seed is not set
        if (!_isSeedInit)
        {
            if (seed == -1)
            {
                _seed = (int)System.DateTime.Now.Ticks;
            } else
            {
                _seed = seed;
            }
            Debug.Log("Seed set to: " + _seed);
        }
    }

    private static System.Random getRandomizer(bool seeded)
    {
        return seeded ? _seededRandomizer : _unseededRandomizer;
    }
}
