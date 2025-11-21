using UnityEngine;

class GameManager : Singleton<GameManager>
{
    private Card selectedCard;
    private Vector3 originPos;

    private void Start()
    {
        MouseEvents.OnMouseDown.AddListener((gameObject) =>
        {
            if (gameObject.CompareTag("Card"))
            {
                selectedCard = gameObject.GetComponent<Card>();
                originPos = gameObject.transform.position;
            }
        });

        MouseEvents.OnMouseUp.AddListener((gameObject) =>
        {
            if (selectedCard != null && gameObject.CompareTag("Card"))
            {  
                selectedCard.transform.position = originPos;
                originPos = Vector3.zero;
                selectedCard = null;
            }
        });

        MouseEvents.OnMouseEnter.AddListener((gameObject) =>
        {
            if (selectedCard != null && gameObject.CompareTag("Enemy"))
            {
                gameObject.transform.localScale *= 1.25f;
            }
        });

        MouseEvents.OnMouseExit.AddListener((gameObject) =>
        {
            if (selectedCard != null && gameObject.CompareTag("Enemy"))
            {
                gameObject.transform.localScale /= 1.25f;
            }
        });

        MouseEvents.OnMouseUp.AddListener((gameObject) =>
        {
            if (selectedCard != null && gameObject.CompareTag("Enemy"))
            {
                gameObject.transform.localScale /= 1.25f;
            }
        });
    }

    private void Update()
    {
        if (selectedCard != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = -1;

            selectedCard.transform.position = mousePos;
        }
    }
}