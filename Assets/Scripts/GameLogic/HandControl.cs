using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandControl : MonoBehaviour
{
    private InputDevice device;
    private ObjectGrab objectGrab;
    private SpawnDice spawnDice;
    private HandMenu handMenu;

    private void Awake()
    {
        objectGrab = GetComponent<ObjectGrab>();
        spawnDice = GetComponent<SpawnDice>();
        handMenu = GetComponent<HandMenu>();
    }

    void OnEnable()
    {
        List<InputDevice> allDevices = new List<InputDevice>();
        InputDevices.GetDevices(allDevices);
        foreach (InputDevice device in allDevices)
            InputDevices_deviceConnected(device);

        InputDevices.deviceConnected += InputDevices_deviceConnected;
        InputDevices.deviceDisconnected += InputDevices_deviceDisconnected;
    }

    private void OnDisable()
    {
        InputDevices.deviceConnected -= InputDevices_deviceConnected;
        InputDevices.deviceDisconnected -= InputDevices_deviceDisconnected;
    }

    private InputDevice GetCorrectHand()
    {
        return gameObject.name == "RightController" ? InputDevices.GetDeviceAtXRNode(XRNode.RightHand) : InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
    }

    private void InputDevices_deviceConnected(InputDevice device)
    {
        InputDevice correctHand = GetCorrectHand();

        if (correctHand == device)
        {
            this.device = device;
        }
    }

    private void InputDevices_deviceDisconnected(InputDevice device)
    {
        InputDevice correctHand = GetCorrectHand();

        if (correctHand == device)
        {
            // todo, clear this reference despite being non-nullable
            //this.device = null;
        }
    }

    void Update()
    {
        bool gripping;
        device.TryGetFeatureValue(CommonUsages.gripButton, out gripping);
        objectGrab.SetGripping(gripping);

        bool pressingPrimary;
        device.TryGetFeatureValue(CommonUsages.primaryButton, out pressingPrimary);
        spawnDice.SetPressing(pressingPrimary);

        bool pressingSecondary;
        device.TryGetFeatureValue(CommonUsages.secondaryButton, out pressingSecondary);
        handMenu.SetPressing(pressingSecondary);
    }
}
