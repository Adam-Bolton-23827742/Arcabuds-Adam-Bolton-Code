using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class AssignControllers : MonoBehaviour
{
    public List<PlayerInput> PlayerInputs;
    
    void Awake()
    {
        foreach (PlayerInput input in PlayerInputs)
        {
            input.neverAutoSwitchControlSchemes = true;
        }

        if (PlayerConfigurationManager.PlayerConfigurationList == null)
        {
            PlayerConfigurationManager.PlayerConfigurationList = new List<int>();
            print("List size: " + PlayerConfigurationManager.PlayerConfigurationList.Count);
            PlayerConfigurationManager.PlayerConfigurationList.Add(0);
            print("List size: " + PlayerConfigurationManager.PlayerConfigurationList.Count);
            Assign(0);
        }

        else if (PlayerConfigurationManager.PlayerConfigurationList.Count > 0)
        {
            Assign(0);

            if (PlayerConfigurationManager.PlayerConfigurationList.Count == 2)
            {
                PlayerInputs[1].GetComponent<PlayerMovement>().enabled = true;
                PlayerInputs[1].GetComponent<CharacterSwitch>().enabled = true;
                PlayerInputs[1].GetComponent<NavMeshAgent>().enabled = false;
                PlayerInputs[1].GetComponent<CompanionState>().enabled = false;
                PlayerInputs[1].enabled = true;
                Assign(1);
            }
        }
    }

    private void Assign(int i)
    {
        if (InputSystem.devices[PlayerConfigurationManager.PlayerConfigurationList[i]] is Keyboard)
        {
            PlayerInputs[i].enabled = true;
            PlayerInputs[i].SwitchCurrentControlScheme("Keyboard&Mouse");
            InputUser.PerformPairingWithDevice(Keyboard.current, PlayerInputs[i].user);
            InputUser.PerformPairingWithDevice(Mouse.current, PlayerInputs[i].user);
        }

        else if (InputSystem.devices[PlayerConfigurationManager.PlayerConfigurationList[i]] is Gamepad)
        {
            PlayerInputs[i].enabled = true;
            PlayerInputs[i].SwitchCurrentControlScheme(InputSystem.devices[PlayerConfigurationManager.PlayerConfigurationList[i]]);
            InputUser.PerformPairingWithDevice(InputSystem.devices[PlayerConfigurationManager.PlayerConfigurationList[i]], PlayerInputs[i].user);
        }
    }
}
