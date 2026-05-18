using UnityEngine;

public class PCPanelCamera : MonoBehaviour
{
    public RectTransform content;

    public float dragSpeed = 1f;
    public float zoomSpeed = 5f;
    public float minZoom = 0.5f;
    public float maxZoom = 2f;

    private Vector3 lastMousePos;
    private float zoom = 1f;

    void Update()
    {
        // DRAG
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            content.anchoredPosition += new Vector2(delta.x, delta.y) * dragSpeed;
            lastMousePos = Input.mousePosition;
        }

        // ZOOM
        float scroll = Input.mouseScrollDelta.y;

        if (scroll != 0)
        {
            zoom += scroll * Time.deltaTime * zoomSpeed;
            zoom = Mathf.Clamp(zoom, minZoom, maxZoom);

            content.localScale = Vector3.one * zoom;
        }
    }

    public void FocusOn(RectTransform target)
    {
        Vector2 targetPos = (Vector2)content.InverseTransformPoint(target.position);
        content.anchoredPosition = -targetPos;
    }
}