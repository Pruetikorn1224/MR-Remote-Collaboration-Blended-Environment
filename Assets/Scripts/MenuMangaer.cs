using UnityEngine;
using UnityEngine.UI;
using Ubiq.Messaging;
using Ubiq.Logging;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class MenuMangaer : MonoBehaviour
{
    [Header("Starting UI Buttons")]
    [SerializeField] Canvas menu;
    [SerializeField] Button buttonRoom1;
    [SerializeField] Button buttonRoom2;
    [SerializeField] Button buttonSpectator;

    [Header("Room Configuration")]
    [SerializeField] Button returnButton;
    [SerializeField] Button solidRoomButton;
    [SerializeField] Button transparentRoomButton;
    [SerializeField] Button rubricOnlyButton;

    [Header("Room Environment")]
    public GameObject[] environment1;
    public GameObject[] environment2;

    [Header("Main Camera")]
    public OVRCameraRig ovrCameraRig;

    private int roomState;
    private int roomNumber;

    NetworkContext context;
    ExperimentLogEmitter events;

    public SceneConfiguration sceneConfiguration;

    private void Start()
    {
        buttonRoom1.onClick.AddListener(delegate { SetRoomNumber(1, 0); });
        buttonRoom2.onClick.AddListener(delegate { SetRoomNumber(2, 0); });
        buttonSpectator.onClick.AddListener(delegate { SpectatorRoom(); });

        solidRoomButton.onClick.AddListener(delegate { SetRoomState(0); });
        transparentRoomButton.onClick.AddListener(delegate { SetRoomState(1); });
        rubricOnlyButton.onClick.AddListener(delegate { SetRoomState(2); });

        returnButton.onClick.AddListener(ReturnToRoom);

        context = NetworkScene.Register(this);
        events = new ExperimentLogEmitter(this);

    }

    private void Update()
    {
        // When the user chooses room number, and uses Meta Quest headset
        if (ovrCameraRig.gameObject.activeSelf && roomNumber != 0)
        {
            // Send the head's position and rotation to event log
            events.Log("Number " + roomNumber.ToString(), ovrCameraRig.centerEyeAnchor.localPosition, ovrCameraRig.centerEyeAnchor.localRotation);
        }
    }

    private struct Message
    {
        public int stateOfRoom;
    }

    // Set environment regarding to room number and state
    public void SetRoomNumber(int roomN, int roomS)
    {
        CloseMenu();

        roomNumber = roomN;
        roomState = roomS;

        GameObject roomSelectionMenu = GameObject.Find("Environment/Room Selection Menu");
        roomSelectionMenu.SetActive(false);

        SetRubric(roomS);
        if (roomN == 1)
        {
            environment1[roomS].SetActive(true);
        }
        else if (roomN == 2)
        {
            environment2[roomS].SetActive(true);
        }
    }

    // Teleport spectator to configuration setting UI
    public void SpectatorRoom()
    {
        GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
        player.transform.position = new Vector3(0f, 2.5f, 0f);

        // Show merged environment
        environment1[3].SetActive(true);
        environment2[3].SetActive(true);

        Message m = new Message();
        m.stateOfRoom = roomState;
        context.SendJson(m);
    }

    // Teleport spectator to origin
    public void ReturnToRoom()
    {
        GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
        player.transform.position = new Vector3(0f, 0f, 0f);
    }

    // Remove UI screen for selcting room
    private void CloseMenu()
    {
        menu.transform.localScale = new Vector3(0f, 0f, 0f);
    }

    // Change room condition and send a message to network scene
    public void SetRoomState(int state)
    {
        roomState = state;

        Message m = new Message();
        m.stateOfRoom = roomState;
        context.SendJson(m);

        SetRubric(roomState);
        GenerateTasks();

        Debug.Log("Message sent! Room State: " + state.ToString());
        events.Log("State " + state.ToString());
    }

    // Show number signs and set transform to original point
    public void GenerateTasks()
    {
        for (int count = 3; count > 0; count--)
        {
            GameObject.Find("Environment/Sign " + count.ToString()).transform.localPosition = new Vector3((float)(count-2)/2, 1.25f, -2f);
            GameObject.Find("Environment/Sign " + count.ToString()).transform.localRotation = Quaternion.Euler(-90, 180, 0);
        }
    }

    // Show Rubik's cubes regarding to the state of rooms
    // { 5, 0, 8 }
    // { 1, 6, 9 }
    // { 2, 7, 4 }
    private void SetRubric(int setIndex)
    {
        Transform tasksTransform = GameObject.Find("Environment/Surrounding/Tasks").gameObject.transform;
        for (int i = 0; i < 10; i++)
        {
            if (setIndex == 0 && (i == 0 || i == 5 || i == 8))
            {
                tasksTransform.GetChild(i).gameObject.SetActive(true);
            }
            else if (setIndex == 1 && (i == 1 || i == 6 || i == 9))
            {
                tasksTransform.GetChild(i).gameObject.SetActive(true);
            }
            else if (setIndex == 2 && (i == 2 || i == 4 || i == 7))
            {
                tasksTransform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                tasksTransform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    // Receive and process message from metwork scene
    // Change room state according to what spectator set
    public void ProcessMessage(ReferenceCountedSceneGraphMessage m)
    {
        var Message = m.FromJson<Message>();

        if (roomNumber == 1)
        {
            environment1[roomState].SetActive(false);

            roomState = Message.stateOfRoom;
            environment1[roomState].SetActive(true);
            SetRubric(roomState);
        }
        else if (roomNumber == 2)
        {
            environment2[roomState].SetActive(false);

            roomState = Message.stateOfRoom;
            environment2[roomState].SetActive(true);
            SetRubric(roomState);
        }

        GenerateTasks();
        Debug.Log("Message received!");
    }
}
