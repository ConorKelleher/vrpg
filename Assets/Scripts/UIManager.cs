using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public InputField usernameField;

    private void Awake()
    {
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Debug.Log("Instance already exists, destroying object!");
                Destroy(this);
            }
        }
    }

    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        usernameField.interactable = false;
        Client.instance.ConnectToServer();
    }

    //public void TryRecenter()
    //{
    //    List<InputDevice> devices = new List<InputDevice>();
    //    InputDevices.GetDevices(devices);
    //    Debug.Log($"num devices: {devices.Count}");
    //    foreach (InputDevice device in devices)
    //    {
    //        device.subsystem.TryRecenter();
    //    }
    //}
}
