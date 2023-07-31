using UnityEngine;

public class DrawRaycast : MonoBehaviour
{
    public Transform pointA; // Start point of the line
    public Transform pointB; // End point of the line
    private LineRenderer lineRenderer;

    void Start()
    {
        // Add the LineRenderer component if it's not already attached to the game object
        if (!gameObject.GetComponent<LineRenderer>())
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        else
        {
            lineRenderer = gameObject.GetComponent<LineRenderer>();
        }

        // Set the width of the line
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        // Set the number of points to 2
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        // Check if both points are set
        if (pointA != null && pointB != null)
        {
            RaycastHit hit;
            // Create a ray from point A to point B
            Ray ray = new Ray(pointA.position, pointB.position - pointA.position);

            // Set the line's points
            lineRenderer.SetPosition(0, pointA.position);
            lineRenderer.SetPosition(1, pointB.position);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Hit object: " + hit.transform.name);
            }
        }
    }
}