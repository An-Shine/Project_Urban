using UnityEngine;

public class MousePointer : MonoBehaviour
{
    [SerializeField] private float zPosition = 0.0f;
    
    private Camera mainCam;

    // GameObject 
    private GameObject currentHoveredObject = null;
    private GameObject mouseDownObject = null;

    // Flags
    private bool isMouseDown = false;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        CheckMouseHover();
        CheckMouseClick();
    }

    private void CheckMouseHover()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        GameObject hoveredObject = null;

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
            hoveredObject = hitInfo.collider.gameObject;

        if (hoveredObject != currentHoveredObject)
        {
            if (currentHoveredObject != null)
                MouseEvents.TriggerMouseExit(currentHoveredObject);

            if (hoveredObject != null)
                MouseEvents.TriggerMouseEnter(hoveredObject);

            currentHoveredObject = hoveredObject;
        }

        if (currentHoveredObject != null)
            MouseEvents.TriggerMouseHover(currentHoveredObject);
    }

    private void CheckMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentHoveredObject != null)
                MouseEvents.TriggerMouseDown(currentHoveredObject);

            isMouseDown = true;
            mouseDownObject = currentHoveredObject;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isMouseDown && currentHoveredObject != null)
            {
                if (currentHoveredObject == mouseDownObject)
                    MouseEvents.TriggerMouseClick(currentHoveredObject);

                MouseEvents.TriggerMouseUp(currentHoveredObject);
            }

            isMouseDown = false;
            mouseDownObject = null;
        }
    }

    private void OnDisable()
    {
        currentHoveredObject = null;
        mouseDownObject = null;
        isMouseDown = false;
    }
}