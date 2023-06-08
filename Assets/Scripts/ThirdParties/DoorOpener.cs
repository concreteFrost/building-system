using UnityEngine;

public class DoorOpener : MonoBehaviour, IInteractable
{
    public Transform objectToOpen;
    public float openAngle = 90f;
    public float openSpeed = 2.5f;

    private bool isOpen = false;
    private Quaternion initialRotation;
    private Quaternion targetRotation;

    private void Start()
    {
        initialRotation = objectToOpen.localRotation;
        targetRotation = initialRotation;
    }

    private void Update()
    {


        if (objectToOpen.localRotation != targetRotation)
        {
            objectToOpen.localRotation = Quaternion.Lerp(objectToOpen.localRotation, targetRotation, openSpeed * Time.deltaTime);
        }
    }

    public void Interact(GameObject obj=null)
    {
        if (!isOpen)
            Open();
        else
            Close();
    }

    private void Open()
    {
        isOpen = true;
        targetRotation = initialRotation * Quaternion.Euler(0f, openAngle, 0f);
    }

    private void Close()
    {
        isOpen = false;
        targetRotation = initialRotation;
    }
}
