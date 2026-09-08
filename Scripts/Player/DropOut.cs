using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class DropOut : MonoBehaviour
{
    private PlayerMovement PlayerMovement;
    private CharacterSwitch CharacterSwitch;
    private NavMeshAgent NavMeshAgent;
    private CompanionState CompanionState;
    private PlayerInput PlayerInput;
    public int ControllerIndex;
    private Scale Scale;

    private void Start()
    {
        PlayerMovement = GetComponent<PlayerMovement>();
        CharacterSwitch = GetComponent<CharacterSwitch>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        CompanionState = GetComponent<CompanionState>();
        PlayerInput = GetComponent<PlayerInput>();
        Scale = GetComponent<Scale>();
    }

    private void Update()
    {
        if (PlayerInput.devices.Count == 0 && PlayerInput.enabled)
        {
            DropOutPlayer();
        }
    }

    public void DropOutPlayer()
    {
        if (PlayerConfigurationManager.PlayerConfigurationList.Count > 1)
        {
            print(name + " has dropped out");
            PlayerMovement.enabled = false;
            CharacterSwitch.enabled = false;
            NavMeshAgent.enabled = true;
            CompanionState.enabled = true;
            PlayerInput.enabled = false;
            Scale.NextPlayer = null;
            Scale.CurrentPlayer = transform;
            Scale.CanChangeScale = true;

            if (ControllerIndex > 0)
            {
                ControllerIndex--;
            }

            PlayerConfigurationManager.PlayerConfigurationList.RemoveAt(ControllerIndex);
        }
    }
}
