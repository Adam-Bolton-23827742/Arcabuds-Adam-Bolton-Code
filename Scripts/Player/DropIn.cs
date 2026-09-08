using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class DropIn : MonoBehaviour
{
    private AssignControllers AssignControllers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AssignControllers = GetComponent<AssignControllers>();
    }

    private void Update()
    {
        if (PlayerConfigurationManager.PlayerConfigurationList.Count < 2)
        {
            for (int i = 0; i < InputSystem.devices.Count; i++)
            {
                if (InputSystem.devices[i] is Gamepad && !PlayerConfigurationManager.PlayerConfigurationList.Contains(i))
                {
                    Gamepad Gamepad = (Gamepad)InputSystem.devices[i];

                    if (Gamepad.startButton.wasPressedThisFrame)
                    {
                        foreach (PlayerInput PlayerInput in AssignControllers.PlayerInputs)
                        {
                            if (!PlayerInput.enabled)
                            {
                                DropPlayerIn(PlayerInput.GetComponent<PlayerMovement>(), PlayerInput.GetComponent<CharacterSwitch>(), PlayerInput.GetComponent<NavMeshAgent>(), PlayerInput.GetComponent<CompanionState>(), PlayerInput, i);
                                return;
                            }
                        }
                    }
                }

                else if (InputSystem.devices[i] is Keyboard && !PlayerConfigurationManager.PlayerConfigurationList.Contains(i))
                {
                    Keyboard Keyboard = (Keyboard)InputSystem.devices[i];

                    if (Keyboard.enterKey.wasPressedThisFrame)
                    {
                        foreach (PlayerInput PlayerInput in AssignControllers.PlayerInputs)
                        {
                            if (!PlayerInput.enabled)
                            {
                                DropPlayerIn(PlayerInput.GetComponent<PlayerMovement>(), PlayerInput.GetComponent<CharacterSwitch>(), PlayerInput.GetComponent<NavMeshAgent>(), PlayerInput.GetComponent<CompanionState>(), PlayerInput, i);
                                return;
                            }
                        }
                    }
                }
            }
        }
    }

    private void DropPlayerIn(PlayerMovement PlayerMovement, CharacterSwitch CharacterSwitch, NavMeshAgent NavMeshAgent, CompanionState CompanionState, PlayerInput PlayerInput, int Index)
    {
        PlayerMovement.enabled = true;
        CharacterSwitch.enabled = true;
        NavMeshAgent.enabled = false;
        CompanionState.enabled = false;
        PlayerInput.enabled = true;
        PlayerInput.GetComponent<Scale>().NextPlayer = PlayerInput.transform;
        PlayerInput.GetComponent<Scale>().CanChangeScale = true;

        PlayerConfigurationManager.PlayerConfigurationList.Add(Index);
        print(Index);
        PlayerInput.GetComponent<DropOut>().ControllerIndex = Index;

        if (InputSystem.devices[Index] is Gamepad)
        {
            PlayerInput.SwitchCurrentControlScheme("Gamepad", InputSystem.devices[Index]);
        }
        else if (InputSystem.devices[Index] is Keyboard)
        {
            PlayerInput.SwitchCurrentControlScheme("Keyboard&Mouse", Keyboard.current, Mouse.current);
        }

        InputUser.PerformPairingWithDevice(InputSystem.devices[Index], PlayerInput.user);
        print(InputSystem.devices[Index]);

        if (InputSystem.devices[Index] is Keyboard)
        {
            InputUser.PerformPairingWithDevice(Mouse.current, PlayerInput.user);
        }

        CharacterSwitch.NextCharacterIndex = CharacterSwitch.CharacterIndex;
        CharacterSwitch.SwapUI(true);
        print(PlayerInput.gameObject.name + " has dropped in");
    }
}
