using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorHandler : MonoBehaviour
{

    [SerializeField] private Slider progressBar;


    public BodySourceView bodySourceView;

    private RectTransform targetRectTransform;


    private void Start()
    {
        targetRectTransform = GetComponent<RectTransform>();
    }

    private Button currentOverlappingButton = null;
    private float overlapStartTime = -1f;
    private bool buttonAlreadyTriggered = false;
    private float targetFillTime = 1.5f;

    void Update()
    {
        Button overlappingButton = GetOverlappingButton();



        if (overlappingButton != currentOverlappingButton)
        {
            // Overlap has changed or stopped — reset
            currentOverlappingButton = overlappingButton;
            overlapStartTime = (currentOverlappingButton != null) ? Time.time : -1f;
            buttonAlreadyTriggered = false;


            progressBar.value = 0;
        }

        if (currentOverlappingButton != null)
        {

            progressBar.value = (Time.time - overlapStartTime) / targetFillTime;


            if (Time.time - overlapStartTime >= targetFillTime)
            {
                // Still overlapping after 2 seconds — trigger the button
                currentOverlappingButton.onClick.Invoke();
                currentOverlappingButton.GetComponent<CustomButtonFunctions>().ButtonPressFinish();
                Debug.Log($"Triggered button: {currentOverlappingButton.name}");
                buttonAlreadyTriggered = true;

                overlapStartTime = (currentOverlappingButton != null) ? Time.time : -1f;

            }
        }
    }

    Button GetOverlappingButton()
    {
        Rect targetRect = GetWorldRect(targetRectTransform);
        Button[] allButtons = FindObjectsOfType<Button>();

        foreach (Button button in allButtons)
        {
            RectTransform btnRect = button.GetComponent<RectTransform>();
            Rect buttonRect = GetWorldRect(btnRect);

            if (targetRect.Overlaps(buttonRect))
            {
                return button;
            }
        }

        return null;
    }


    Rect GetWorldRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        Vector3 bottomLeft = corners[0];
        Vector3 topRight = corners[2];

        return new Rect(bottomLeft, topRight - bottomLeft);
    }
}
