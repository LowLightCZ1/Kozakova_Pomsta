using System;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private HandPointer pointer;
    // Read another script

    [SerializeField] private string targetTag = "Pickable";

    private GameObject handObject;
    private Array targetObjects;

    public Transform Hand;
    public bool isInhand;
    private bool hasLogged = false;

    void Start()
    {
        pointer = FindFirstObjectByType<HandPointer>();

        targetObjects = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject obj in targetObjects)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = false;
        }
    }

    void Update()
    {
        
        if (pointer && pointer.CurrentObject != null)
        {
            if (pointer.IsObjectPickable(pointer.CurrentObject))
            {
                if (!hasLogged)
                {
                    Debug.Log("Current target is pickable!");
                    hasLogged = true;
                }

            }

        }

            PickUp(Hand);
    }

    public void PickUp(Transform newParent)
    {
       if (Input.GetKeyUp(KeyCode.E))
       {
            if (pointer.CurrentObject.CompareTag(targetTag))
            {
                handObject = pointer.CurrentObject;
                handObject.transform.SetParent(newParent);
                handObject.transform.localPosition = Vector3.zero;

                handObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                Debug.Log("Nothing was picked");
            }

           Rigidbody rb = handObject.GetComponent<Rigidbody>();
           rb.isKinematic = true;
           rb.useGravity = false;
           isInhand = true;


       }

       if (Input.GetKeyUp(KeyCode.Q) && isInhand)
       {
            if (isInhand || handObject != null) 
            { 
                handObject.transform.SetParent(null);

                Rigidbody rb = handObject.GetComponent<Rigidbody>();

                rb.useGravity = true;
                rb.isKinematic = false;
            }

       }
        
    }
}