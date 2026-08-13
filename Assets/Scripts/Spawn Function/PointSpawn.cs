using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.SceneView;

public class PointSpawn : MonoBehaviour
{
    private BodyPointer pointer;
    public FadeImage image;

    public Transform StartPoint;
    public Transform EndPoint;
    private Vector3 pointPos1;
    private Vector3 pointPos2;
    private BoxCollider hitBox;

    public GameObject Player;

    void Start()
    {
        pointPos1 = StartPoint.position;
        pointPos2 = EndPoint.position;
        hitBox = GetComponent<BoxCollider>();

        if (Player != null)
        {
            pointer = Player.GetComponentInChildren<BodyPointer>();
            if (pointer == null)
            {
                Debug.LogError("Skript BodyPointer nebyl nalezen v žádném z potomků objektu Character!");
            }
        }
        else
        {
            Debug.Log("Objekt 'Character' nebyl nakezen");
        }

        gameObject.tag = "OutHouse";

        Debug.Log("House Position" + pointPos1);
        Debug.Log("Outside Position" + pointPos2);
    }

    void Update()
    {
        PointSapwn();
    }
    
    public void PointSapwn()
    {
        if (Input.GetKeyUp(KeyCode.E) && pointer != null && pointer.IsHome == true)
        {
            StartCoroutine(image.FadeOutIn(() =>
            {
                CharacterController cc = Player.GetComponent<CharacterController>();

                if (cc != null) cc.enabled = false; // Dočasně vypnout fyziku controlleru

                if (!gameObject.CompareTag("InHouse"))
                {
                    Player.transform.position = pointPos1;
                    Player.transform.rotation = StartPoint.rotation;
                    gameObject.tag = "InHouse";
                }
                else
                {
                    SceneSpawn.TargetSpawn = "Level2Point";
                    SceneManager.LoadScene("Level2");
                    gameObject.tag = "OutHouse";
                    if (Player.transform.position != pointPos2)
                    {
                        Player.transform.position = pointPos2;
                    }
                }



                pointer.ResetRotation(StartPoint.rotation);

                if (cc != null) cc.enabled = true;

                pointer.IsHome = false;

                Debug.Log("Teleport dokončen. Pozice: " + Player.transform.position);
            }));

        }
    }
}



