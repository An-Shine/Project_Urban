using UnityEngine;

public class MousePointer : MonoBehaviour
{
    [SerializeField] private float zPosition = 0f;
    
    private Camera mainCam;

    // GameObject 
    private GameObject currentHoverdObject = null;
    private GameObject mouseDownObject= null;

    // Flags
    private bool isMouseDown = false;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        CheckMousHover();
        CheckMouseClick();
    }

    private void CheckMousHover()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        GameObject hoverdObject = null;

        if (Physics.Raycast(ray, out RaycastHit hitInfo))
            hoverdObject = hitInfo.collider.gameObject;

        if (hoverdObject != currentHoverdObject)
        {
            if (currentHoverdObject != null)
                MouseEvents.TriggerMouseExit(currentHoverdObject);

            if (hoverdObject != null)
                MouseEvents.TriggerMouseEnter(hoverdObject);

            currentHoverdObject = hoverdObject;
        }

        if (currentHoverdObject != null)
            MouseEvents.TriggerMouseHover(currentHoverdObject);
    }

    private void CheckMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentHoverdObject != null)
                MouseEvents.TriggerMouseDown(currentHoverdObject);

            isMouseDown = true;
            mouseDownObject = currentHoverdObject;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isMouseDown && currentHoverdObject != null)
            {
                if (currentHoverdObject == mouseDownObject)
                    MouseEvents.TriggerMouseClick(currentHoverdObject);

                MouseEvents.TriggerMouseUp(currentHoverdObject);
            }

            isMouseDown = false;
            mouseDownObject = null;
        }
    }

    private void OnDisable()
    {
        currentHoverdObject = null;
        mouseDownObject = null;
        isMouseDown = false;
    }
}