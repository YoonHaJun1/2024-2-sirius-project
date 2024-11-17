using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    [SerializeField] GameObject targetObject;

    public void ToggleSetState()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(!targetObject.activeSelf);
        }

    }

    private void OnDisable()
    {
        targetObject.SetActive(false);
    }
}
