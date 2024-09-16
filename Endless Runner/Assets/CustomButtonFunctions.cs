using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomButtonFunctions : MonoBehaviour
{
    [SerializeField] UnityEvent OnButtonPressFinish;


    public void ButtonPressFinish()
    {
        OnButtonPressFinish.Invoke();
    }
}
