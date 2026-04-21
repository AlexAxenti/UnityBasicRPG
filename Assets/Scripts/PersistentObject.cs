using System.Collections.Generic;
using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    [SerializeField] private string persistenceKey;

    private static readonly HashSet<string> activeKeys = new();

    private void Awake()
    {
        string key = string.IsNullOrWhiteSpace(persistenceKey) ? gameObject.name : persistenceKey;

        if (activeKeys.Contains(key))
        {
            Destroy(gameObject);
            return;
        }

        activeKeys.Add(key);
        DontDestroyOnLoad(gameObject);
    }
}