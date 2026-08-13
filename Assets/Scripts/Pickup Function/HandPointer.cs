using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandPointer : MonoBehaviour
{
    public bool IsPickable = false;
    private GameObject currentObject;

    public float mouseSensitivity = 100f;
    public Transform handPointer;
    public TextMeshProUGUI PickupText;
    public Camera PlayerCam;
    public LayerMask Pickable;

    private Vector3 camPos;
    private float camRot;
    public bool IsObjectPickable(GameObject obj)
    {
        return obj.CompareTag("Pickable");
    }
    public GameObject CurrentObject => currentObject;

    void Start()
    {
        Debug.Log(PlayerCam.transform.position);
    }

    void Update()
    {
        RotatePointer();
        PointerLine();
        CamPosition();
    }

    public void RotatePointer()
    {
        // Represantion of rotation of the pointer
        Quaternion pointerRotation = handPointer.transform.rotation;

        Vector3 euler = pointerRotation.eulerAngles; //Returning angle representaion in degrees
        euler.x = PlayerCam.transform.rotation.eulerAngles.x;
        handPointer.transform.rotation = Quaternion.Euler(euler);
    }

    public void CamPosition()
    {
        camPos = PlayerCam.transform.position;
        handPointer.transform.position = camPos;
    }

    public void PointerLine()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, 1.5f, Pickable))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
            IsPickable = true;
            currentObject = hit.collider.gameObject;
            PickupText.text = "Press E to Pick";
            

        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 1.5f,Color.red);
            currentObject = null;
            IsPickable = false;
            PickupText.text = "";
        }
    }
    public void ResetRotation(Quaternion targetRotation)
    {

        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        handPointer.rotation = targetRotation;

    }
}
