using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerConfigurationManager : MonoBehaviour
{
    private int PlayerID = 1, ControllerID;
    [HideInInspector] public static List<int> PlayerConfigurationList;
    [SerializeField] private Button LoadSceneButton;

    private void Start()
    {
        PlayerConfigurationList = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerConfigurationList == null || PlayerConfigurationList.Count < 2)
        {
            for (int i = 0; i < InputSystem.devices.Count; i++)
            {
                JoinPlayer(i);
            }
        }
    }

    private void JoinPlayer(int ID)
    {
        if (InputSystem.devices[ID] is Gamepad)
        {
            Gamepad Gamepad = (Gamepad)InputSystem.devices[ID];

            if (Gamepad.startButton.isPressed && !PlayerConfigurationList.Contains(ID))
            {
                ControllerID = ID;
                PlayerConfigurationList.Add(ControllerID);
                LoadSceneButton.interactable = true;
                print("Device " + Gamepad + " is assigned to player " + PlayerID);
                print("ControllerID: " + ControllerID);
                PlayerID++;
            }
        }

        else if (InputSystem.devices[ID] is Keyboard)
        {
            Keyboard Keyboard = (Keyboard)InputSystem.devices[ID];

            if (Keyboard.enterKey.isPressed && !PlayerConfigurationList.Contains(ID))
            {
                ControllerID = ID;
                PlayerConfigurationList.Add(ControllerID);
                LoadSceneButton.interactable = true;
                print("Device " + Keyboard + " is assigned to player " + PlayerID);
                print("ControllerID: " + ControllerID);
                PlayerID++;
            }
        }
    }
}
