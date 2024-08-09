using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneConfiguration : MonoBehaviour
{
    public GameObject environment;

    public OVRCameraRig ovrCameraRig;

    [Header("Scene Location")]
    public Vector3 scenePosition;
    public float sceneRotation;

    private int frontIndex;
    private int backIndex;
    private bool isSceneSet;

    private void Start()
    {
        isSceneSet = false;
    }

    private void Update()
    {
        // Count the number of anchors
        GameObject[] anchors = GameObject.FindGameObjectsWithTag("Anchor");
        if (anchors.Length == 4)
        {
            if (!isSceneSet)
            {
                SetSceneRotation(anchors, out frontIndex, out backIndex);
            } 

            Vector3 position;
            SetScenePosition(anchors, out position);
            scenePosition = position - new Vector3(0, 0.02f, 0);
            environment.transform.position = scenePosition;

            Vector3 direction = anchors[frontIndex].transform.position - anchors[backIndex].transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            environment.transform.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
            sceneRotation = rotation.eulerAngles.y;

            // Set player to be child componenet of the environment
            if (!isSceneSet && GameObject.Find("Player"))
            {
                GameObject.Find("Player").transform.parent = environment.transform;
                isSceneSet = true;
            }
        }

        // Restart the program
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch))
        {
            isSceneSet = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // Set the environment's position
    // Average the position of all anchors
    private void SetScenePosition(GameObject[] anchors, out Vector3 newPos)
    {
        newPos = Vector3.zero;
        foreach (GameObject anchor in anchors)
        {
            newPos += anchor.transform.position;
        }
        newPos = newPos / anchors.Length;
    }

    // Set the environment's rotation
    // Get the front index which is at the front of user
    // Get the back index which is at the back of user
    private void SetSceneRotation(GameObject[] anchors, out int front, out int back)
    {
        front = 0;
        back = 0;
        float smallestAngle = 180;
        float largestAngle = 0;

        for (int i = 0; i < anchors.Length; i++)
        {
            Vector3 directionToPoint = (anchors[i].transform.position - ovrCameraRig.centerEyeAnchor.position).normalized;
            float angle = Vector3.Angle(ovrCameraRig.centerEyeAnchor.forward, directionToPoint);

            if (angle < smallestAngle)
            {
                smallestAngle = angle;
                front = i;
            }
            if (angle > largestAngle)
            {
                largestAngle = angle;
                back = i;
            }
        }
    }
}
