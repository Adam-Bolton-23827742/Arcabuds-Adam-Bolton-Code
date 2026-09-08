using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AddCharacter : MonoBehaviour
{
    [SerializeField] private AssignControllers AssignControllers;
    [SerializeField] private RespawnHandler RespawnHandler;
    [SerializeField] private DialogueActivator[] AllDialogueActivators;
    [SerializeField] private GameObject Pebbles, Splashy;
    [SerializeField] private SoundHandler SoundHandler;
    [SerializeField] private Rumble[] RumbleHandlers;

    public void Add(GameObject Character, Vector3 SpawnPosition, int CharacterIndex, string ProfileName, string MovesName, string Attack1, string Attack2)
    {
        GameObject NewCharacter = Instantiate(Character, SpawnPosition, Quaternion.identity);
        NewCharacter.name = Character.name;
        NewCharacter.GetComponentInChildren<Footstep>().SoundHandler = SoundHandler;
        AssignControllers.PlayerInputs.Add(NewCharacter.GetComponent<PlayerInput>());
        RespawnHandler.Player3 = NewCharacter;
        RespawnHandler.Stats3 = RespawnHandler.Player3.GetComponent<PlayerStats>();
        RespawnHandler.GroundCheck3 = RespawnHandler.Player3.GetComponent<GroundCheck>();
        RespawnHandler.SpawnPoint3 = NewCharacter.transform;

        foreach (DialogueActivator Activator in AllDialogueActivators)
        {
            Activator.Zappy = NewCharacter.GetComponent<PlayerInput>();
        }

        CharacterSwitch CSwitch = NewCharacter.GetComponent<CharacterSwitch>();
        CSwitch.SwitchableCharacters[0] = Pebbles;
        CSwitch.SwitchableCharacters[1] = Splashy;
        CSwitch.MovesUI = GameObject.Find("HUD Canvas").transform.Find("In-Game HUD").transform.Find(MovesName);
        CSwitch.Attack1UI = CSwitch.MovesUI.transform.Find(Attack1);
        CSwitch.Attack2UI = CSwitch.MovesUI.transform.Find(Attack2);
        Pebbles.GetComponent<CharacterSwitch>().SwitchableCharacters[CharacterIndex] = NewCharacter;
        Splashy.GetComponent<CharacterSwitch>().SwitchableCharacters[CharacterIndex] = NewCharacter;
        NewCharacter.GetComponent<PlayerMovement>().SoundHandler = SoundHandler;
        NewCharacter.GetComponent<CompanionState>().PlayerToFollow = Pebbles.transform.Find("Comp1");
        PlayerStats Stats = NewCharacter.GetComponent<PlayerStats>();
        CSwitch.PlayerStats = Stats;
        GameObject.Find("HUD Canvas").transform.Find("In-Game HUD").transform.Find(ProfileName).gameObject.SetActive(true);
        Stats.HeartImages[0] = GameObject.Find("HUD Canvas").transform.Find("In-Game HUD").transform.Find(ProfileName).transform.Find("Stats").transform.Find("HealthBackground").transform.Find("HealthFills").transform.Find("Fill1").GetComponent<Image>();
        Stats.HeartImages[1] = GameObject.Find("HUD Canvas").transform.Find("In-Game HUD").transform.Find(ProfileName).transform.Find("Stats").transform.Find("HealthBackground").transform.Find("HealthFills").transform.Find("Fill2").GetComponent<Image>();
        Stats.HeartImages[2] = GameObject.Find("HUD Canvas").transform.Find("In-Game HUD").transform.Find(ProfileName).transform.Find("Stats").transform.Find("HealthBackground").transform.Find("HealthFills").transform.Find("Fill3").GetComponent<Image>();
        Stats.StarImage = GameObject.Find("HUD Canvas").transform.Find("In-Game HUD").transform.Find(ProfileName).transform.Find("Stats").transform.Find("Points").transform.Find("Star").GetComponent<Image>();
        Stats.Rumble = RumbleHandlers[CharacterIndex];
        Stats.RespawnHandler = RespawnHandler;
        Stats.SoundHandler = SoundHandler;
        Stats.CharacterPortrait = GameObject.Find("HUD Canvas").transform.Find("In-Game HUD").transform.Find(ProfileName).transform.Find("Character Image").GetComponent<Image>();
        Stats.CollectableCountText = GameObject.Find("HUD Canvas").transform.Find("In-Game HUD").transform.Find(ProfileName).transform.Find("Stats").transform.Find("Points").transform.Find("Point Text").GetComponent<TextMeshProUGUI>();
        Stats.Profile = GameObject.Find("HUD Canvas").transform.Find("In-Game HUD").transform.Find(ProfileName).transform;
        Stats.Points = GameObject.Find("HUD Canvas").transform.Find("In-Game HUD").transform.Find(ProfileName).transform.Find("Stats").transform.Find("Points");
        Stats.HeartsBackground = GameObject.Find("HUD Canvas").transform.Find("In-Game HUD").transform.Find(ProfileName).transform.Find("Stats").transform.Find("HealthBackground");
        NewCharacter.GetComponent<NavmeshLinkJump>().SoundHandler = SoundHandler;
        NewCharacter.GetComponent<GroundCheck>().SoundHandler = SoundHandler;
        PauseMenu Pause = NewCharacter.GetComponent<PauseMenu>();
        PauseMenu PebblesPause = Pebbles.GetComponent<PauseMenu>();
        Pause.ES = PebblesPause.ES;
        Pause.Pause = PebblesPause.Pause;
        Pause.Tint = PebblesPause.Tint;
        Pause.ResumeButton = PebblesPause.ResumeButton;
        Pause.Option = PebblesPause.Option;
        Pause.TopOptionButton = PebblesPause.TopOptionButton;
        Pause.DialogueBox = PebblesPause.DialogueBox;
        CinemachineTargetGroup FollowGroup = GameObject.Find("FollowGroup").GetComponent<CinemachineTargetGroup>();
        FollowGroup.AddMember(Character.transform, 20.0f, 20.0f);
    }
}
