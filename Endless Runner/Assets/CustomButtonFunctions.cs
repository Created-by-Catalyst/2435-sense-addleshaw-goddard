using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomButtonFunctions : MonoBehaviour
{
    [SerializeField] UnityEvent OnButtonPressFinish;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ButtonPressFinish()
    {
        OnButtonPressFinish.Invoke();
    }
}
