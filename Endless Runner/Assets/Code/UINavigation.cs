using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UINavigation : MonoBehaviour
{

    List<GameObject> uiElementList;

    public EventSystem eventSystem;

    int currentElementIndex = 0;

    public void SelectUIElement(GameObject selectable)
    {
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(selectable);
    }

    public void IterateForwards()
    {
        if (this.enabled)
        {
            //SelectUIElement(uiElementList);
        }
    }

    public void IterateBackwards()
    {
        if (this.enabled)
        {

        }
    }


    public void Press()
    {
        //eventSystem.
    }

}
