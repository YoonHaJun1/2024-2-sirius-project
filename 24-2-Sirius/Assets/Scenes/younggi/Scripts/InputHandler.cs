using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    [SerializeField] InputField inputField;
    public int moneytaken;

    public void ValidateInput()
    {
        string input = inputField.text;
        int.TryParse(input, out moneytaken);
        Debug.Log(input);
        Debug.Log(moneytaken);
    }

}
