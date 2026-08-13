using TMPro;
using UnityEngine;

public class BodyPointer : MonoBehaviour
{

    public float mouseSensitivity = 100f;
    public Transform bodyPoint;
    public bool IsHome = false;
    float xRotation = 0;
    public LayerMask Door;

    public TextMeshProUGUI doorText;

    Quaternion initialBodyRotation;
    void Start()
    {
        initialBodyRotation = bodyPoint.rotation;
        ResetRotation(bodyPoint.rotation);

    }

    void Update()
    {
        RotatePointer();
        PointerLine();
    }

    public void RotatePointer()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        bodyPoint.Rotate(Vector3.up * mouseX);
    }

    public void PointerLine()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1f, Door))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.green);
            doorText.text = "Leave House ?";
            IsHome = true;

        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 1f, Color.red);
            doorText.text = "";
            IsHome = false;
        }
    }

    public void ResetRotation(Quaternion targetRotation)
    {
        xRotation = 0f;

        transform.localRotation = Quaternion.identity;
        bodyPoint.rotation = initialBodyRotation;

    }
}
