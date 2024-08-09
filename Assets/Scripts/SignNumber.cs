using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ubiq.Messaging;
using UnityEngine.XR.Interaction.Toolkit;

public class SignNumber : MonoBehaviour
{
    public string plateNumber;
    public TMP_Text label;

    XRGrabInteractable interactable;
    NetworkContext context;
    Transform parent;

    [System.NonSerialized]
    public bool isHolding;
    [System.NonSerialized]
    public bool isOwner;

    // Start is called before the first frame update
    void Start()
    {
        label.text = plateNumber;

        parent = transform.parent;
        interactable = GetComponent<XRGrabInteractable>();
        interactable.firstSelectEntered.AddListener(OnPickedUp);
        interactable.lastSelectExited.AddListener(OnDropped);
        context = NetworkScene.Register(this);
        isOwner = false;

        GetComponent<Rigidbody>().isKinematic = true;
    }

    // When user pick the sign
    void OnPickedUp(SelectEnterEventArgs ev)
    {
        isOwner = true;
        isHolding = true;

        Message m = new Message();
        m.position = this.transform.localPosition;
        m.rotation = this.transform.localRotation;
        m.isHolding = isHolding;
        context.SendJson(m);
    }

    // When user drop the sign
    void OnDropped(SelectExitEventArgs ev)
    {
        transform.parent = parent;
        isOwner = false;
        isHolding = false;

        Message m = new Message();
        m.position = this.transform.localPosition;
        m.rotation = this.transform.localRotation;
        m.isHolding = isHolding;
        context.SendJson(m);

    }

    private struct Message
    {
        public Vector3 position;
        public Quaternion rotation;
        public bool isHolding;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isOwner)
        {
            Message m = new Message();
            m.position = this.transform.localPosition;
            m.rotation = this.transform.localRotation;
            m.isHolding = isHolding;
            context.SendJson(m);
        }
    }

    // Access and change plate number
    public void EditPlateNumber(int t)
    {
        plateNumber = t.ToString();
    }

    // Receive and process message from metwork scene
    // Move the sign based from user who is holding
    public void ProcessMessage(ReferenceCountedSceneGraphMessage m)
    {
        var message = m.FromJson<Message>();
        if (!isOwner)
        {
            this.transform.localPosition = message.position;
            this.transform.localRotation = message.rotation;

            if (message.isHolding != isHolding)
            {
                isHolding = message.isHolding;
            }
        }
    }
}
