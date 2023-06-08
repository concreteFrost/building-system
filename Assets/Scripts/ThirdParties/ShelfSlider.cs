using UnityEngine;

public class ShelfSlider : MonoBehaviour, IInteractable
{
    private Transform shelf;
    private float openDistance = 0.2f;
    private float openSpeed = 20f;

    private bool isOpen = false;
    private Vector3 initialPosition;
    private Vector3 targetPosition;

    private void Start()
    {
        shelf = transform.parent;
        initialPosition = shelf.localPosition;
        targetPosition = initialPosition;
    }

    private void Update()
    {
        if (shelf.localPosition != targetPosition)
        {
            shelf.localPosition = Vector3.Lerp(shelf.localPosition, targetPosition, openSpeed * Time.deltaTime);
        }
    }

    public void Interact(GameObject obj = null)
    {
        if (!isOpen)
            Open();
        else
            Close();
    }

    private void Open()
    {
        isOpen = true;
        targetPosition = initialPosition + (-shelf.forward * openDistance);
    }

    private void Close()
    {
        isOpen = false;
        targetPosition = initialPosition;
    }
}
