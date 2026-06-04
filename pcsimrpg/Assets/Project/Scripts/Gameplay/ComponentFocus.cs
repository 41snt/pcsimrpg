using UnityEngine;
using UnityEngine.EventSystems;

public class ComponentFocus : MonoBehaviour, IPointerClickHandler
{
    public PCPanelCamera cameraSystem;

    public void OnPointerClick(PointerEventData eventData)
    {
        RectTransform target = GetComponent<RectTransform>();

        if (cameraSystem != null)
        {
            cameraSystem.FocusOn(target);
        }
    }
}