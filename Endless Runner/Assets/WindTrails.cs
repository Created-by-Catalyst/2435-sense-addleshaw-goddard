using UnityEngine;

public class WindTrails : MonoBehaviour
{
    [SerializeField] private GameObject windTrails;
    [SerializeField] private Transform targetToFollow;
    [SerializeField] private float followSpeed = 15;

    private bool isActive = false;
    private float trailDuration = 5f;
    private float time = 0;

    void Update()
    {
        if (targetToFollow == null) return;

        Vector3 desiredPosition = targetToFollow.position;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        if (!isActive) return;
        time -= Time.deltaTime;
        if (time <= 0)
        {
            isActive = false;
            if (windTrails != null)
                windTrails.SetActive(false);
        }
    }

    public void ToggleWindTrails(bool state)
    {
        time = trailDuration;
        isActive = state;
        if (windTrails != null)
            windTrails.SetActive(true);
    }
}
