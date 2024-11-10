using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class StorageHolder : MonoBehaviour
{
    [SerializeField] private int storageSize;
    [SerializeField] private StorageSystem storageSystem;

    public static UnityAction<StorageSystem> OnDynamicStorageDisplayRequested;

    private void Awake()
    {
        storageSystem = new StorageSystem(storageSize);
    }
}
