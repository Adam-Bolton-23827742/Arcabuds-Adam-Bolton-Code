using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class CharacterSwitch : MonoBehaviour
{
    public List<GameObject> SwitchableCharacters;
    private int IndexesChecked;
    [HideInInspector] public int NextCharacterIndex, CharacterIndex;
    private CharacterSwitch CharacterSwitchComponent;
    private PlayerInput PlayerInput;
    private PlayerMovement PlayerMovement;
    private CompanionState CompanionState;
    private DropOut DropOut;
    [HideInInspector] public PlayerStats PlayerStats;
    private Scale Scale;
    private Rigidbody Rigidbody;
    public Transform MovesUI, Attack1UI, Attack2UI;
    [HideInInspector] public bool CanSwitch = true;

    void OnEnable()
    {
        CharacterSwitchComponent = GetComponent<CharacterSwitch>();
        CharacterIndex = SwitchableCharacters.IndexOf(gameObject);
        PlayerInput = GetComponent<PlayerInput>();
        PlayerMovement = GetComponent<PlayerMovement>();
        CompanionState = GetComponent<CompanionState>();
        DropOut = GetComponent<DropOut>();
        PlayerStats = GetComponent<PlayerStats>();
        Scale = GetComponent<Scale>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void OnNext()
    {
        if (Time.timeScale == 1.0f && CanSwitch)
        {
            if (FindNext() && SwitchableCharacters[NextCharacterIndex] != null && SwitchableCharacters[NextCharacterIndex].activeSelf)
            {
                IndexesChecked = 0;
                StartCoroutine(SwitchCharacter(true));
            }
        }
    }

    private void OnPrevious()
    {
        if (Time.timeScale == 1.0f && CanSwitch)
        {
            if (FindPrevious() && SwitchableCharacters[NextCharacterIndex] != null && SwitchableCharacters[NextCharacterIndex].activeSelf)
            {
                IndexesChecked = 0;
                StartCoroutine(SwitchCharacter(false));
            }
        }
    }

    private IEnumerator SwitchCharacter(bool NextEQUALSTrueOrPreviousEQUALSFalse)
    {
        foreach (GameObject Character in SwitchableCharacters)
        {
            if (Character != null)
            {
                Character.GetComponent<CharacterSwitch>().CanSwitch = false;
            }
        }

        Rigidbody.linearVelocity = new Vector3(0.0f, Rigidbody.linearVelocity.y, 0.0f);
        Scale.NextPlayer = SwitchableCharacters[NextCharacterIndex].transform;
        Scale.CurrentPlayer = transform;
        Scale.CanChangeScale = true;
        SwitchableCharacters[NextCharacterIndex].GetComponent<PlayerMovement>().enabled = true;
        SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().enabled = true;
        SwitchableCharacters[NextCharacterIndex].GetComponent<NavMeshAgent>().enabled = false;
        SwitchableCharacters[NextCharacterIndex].GetComponent<CompanionState>().enabled = false;
        SwitchableCharacters[NextCharacterIndex].GetComponent<DropOut>().enabled = true;
        SwitchableCharacters[NextCharacterIndex].GetComponent<DropOut>().ControllerIndex = DropOut.ControllerIndex;
        PlayerStats NextPlayerStats = SwitchableCharacters[NextCharacterIndex].GetComponent<PlayerStats>();
        int PlayerNumberCache = SwitchableCharacters[NextCharacterIndex].GetComponent<PlayerStats>().PlayerNumber;
        SwitchableCharacters[NextCharacterIndex].GetComponent<PlayerStats>().PlayerNumber = PlayerStats.PlayerNumber;
        InputDevice CurrentDevice = PlayerInput.devices[0];
        PlayerInput.enabled = false;
        PlayerMovement.enabled = false;
        CharacterSwitchComponent.enabled = false;
        CompanionState.PlayerToFollow = SwitchableCharacters[NextCharacterIndex].transform;
        CompanionState.enabled = true;
        DropOut.enabled = false;
        PlayerStats.PlayerNumber = PlayerNumberCache;
        SwitchableCharacters[NextCharacterIndex].GetComponent<PlayerInput>().enabled = true;

        SwapUI(NextEQUALSTrueOrPreviousEQUALSFalse);

        if (CurrentDevice is Keyboard)
        {
            SwitchableCharacters[NextCharacterIndex].GetComponent<PlayerInput>().SwitchCurrentControlScheme("Keyboard&Mouse");
        }

        else if (CurrentDevice is Gamepad)
        {
            SwitchableCharacters[NextCharacterIndex].GetComponent<PlayerInput>().SwitchCurrentControlScheme(CurrentDevice);
        }

        InputUser.PerformPairingWithDevice(CurrentDevice, SwitchableCharacters[NextCharacterIndex].GetComponent<PlayerInput>().user);
        InputUser.PerformPairingWithDevice(Mouse.current, SwitchableCharacters[NextCharacterIndex].GetComponent<PlayerInput>().user);

        yield return new WaitForSeconds(0.2f);

        foreach (GameObject Character in SwitchableCharacters)
        {
            if (Character != null)
            {
                Character.GetComponent<CharacterSwitch>().CanSwitch = true;
            }
        }
    }

    private void MoveUI(Transform Position1, Transform Position2)
    {
        Vector3 TempPosition = Position1.position;
        Position1.position = Position2.position;
        Position2.position = TempPosition;
    }

    public void SwapUI(bool NextEQUALSTrueOrPreviousEQUALSFalse)
    {
        if (PlayerConfigurationManager.PlayerConfigurationList.Count < 2)
        {
            MovesUI.localScale = new Vector3(-MovesUI.localScale.x, MovesUI.localScale.y, MovesUI.localScale.z);
            SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.localScale = new Vector3(-SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.localScale.x, SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.localScale.y, SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.localScale.z);
            Attack1UI.localScale = new Vector3(-Attack1UI.localScale.x, Attack1UI.localScale.y, Attack1UI.localScale.z);
            Attack2UI.localScale = new Vector3(-Attack2UI.localScale.x, Attack2UI.localScale.y, Attack2UI.localScale.z);
            MovesUI.GetChild(2).localScale = new Vector3(-MovesUI.GetChild(2).localScale.x, MovesUI.GetChild(2).localScale.y, MovesUI.GetChild(2).localScale.z);
            MovesUI.GetChild(3).localScale = new Vector3(-MovesUI.GetChild(3).localScale.x, MovesUI.GetChild(3).localScale.y, MovesUI.GetChild(3).localScale.z);
            SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.GetChild(2).localScale = new Vector3(-SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.GetChild(2).localScale.x, SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.GetChild(2).localScale.y, SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.GetChild(2).localScale.z);
            SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.GetChild(3).localScale = new Vector3(-SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.GetChild(3).localScale.x, SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.GetChild(3).localScale.y, SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.GetChild(3).localScale.z);
            SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().Attack1UI.localScale = new Vector3(-SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().Attack1UI.localScale.x, SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().Attack1UI.localScale.y, SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().Attack1UI.localScale.z);
            SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().Attack2UI.localScale = new Vector3(-SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().Attack2UI.localScale.x, SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().Attack2UI.localScale.y, SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().Attack2UI.localScale.z);
            MoveUI(MovesUI, SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI);
            MoveUI(PlayerStats.CharacterPortrait.transform, SwitchableCharacters[NextCharacterIndex].GetComponent<PlayerStats>().CharacterPortrait.transform);
            MoveUI(PlayerStats.HeartsBackground.transform, SwitchableCharacters[NextCharacterIndex].GetComponent<PlayerStats>().HeartsBackground.transform);
            MoveUI(PlayerStats.CollectableCountText.transform, SwitchableCharacters[NextCharacterIndex].GetComponent<PlayerStats>().CollectableCountText.transform);

            if (NextEQUALSTrueOrPreviousEQUALSFalse)
            {
                if (NextCharacterIndex + 1 == SwitchableCharacters.Count)
                {
                    MovesUI.gameObject.SetActive(false);
                    MovesUI.SetAsFirstSibling();
                    PlayerStats.CollectableCountText.gameObject.SetActive(false);
                    PlayerStats.CollectableCountText.transform.SetAsFirstSibling();
                    SwitchableCharacters[0].GetComponent<CharacterSwitch>().MovesUI.SetAsLastSibling();
                    SwitchableCharacters[0].GetComponent<CharacterSwitch>().MovesUI.gameObject.SetActive(true);
                    SwitchableCharacters[0].GetComponent<CharacterSwitch>().PlayerStats.CharacterPortrait.transform.parent.SetAsLastSibling();
                    SwitchableCharacters[0].GetComponent<PlayerStats>().HeartsBackground.transform.SetAsLastSibling();
                    SwitchableCharacters[0].GetComponent<PlayerStats>().CollectableCountText.transform.SetAsLastSibling();
                    SwitchableCharacters[0].GetComponent<PlayerStats>().CollectableCountText.gameObject.SetActive(true);
                }

                else if (SwitchableCharacters[NextCharacterIndex + 1] != null)
                {
                    MovesUI.gameObject.SetActive(false);
                    MovesUI.SetAsFirstSibling();
                    PlayerStats.CollectableCountText.gameObject.SetActive(false);
                    PlayerStats.CollectableCountText.transform.SetAsFirstSibling();
                    SwitchableCharacters[NextCharacterIndex + 1].GetComponent<CharacterSwitch>().MovesUI.SetAsLastSibling();
                    SwitchableCharacters[NextCharacterIndex + 1].GetComponent<CharacterSwitch>().MovesUI.gameObject.SetActive(true);
                    SwitchableCharacters[NextCharacterIndex + 1].GetComponent<CharacterSwitch>().PlayerStats.CharacterPortrait.transform.parent.SetAsLastSibling();
                    SwitchableCharacters[NextCharacterIndex + 1].GetComponent<PlayerStats>().HeartsBackground.transform.SetAsLastSibling();
                    SwitchableCharacters[NextCharacterIndex + 1].GetComponent<PlayerStats>().CollectableCountText.transform.SetAsLastSibling();
                    SwitchableCharacters[NextCharacterIndex + 1].GetComponent<PlayerStats>().CollectableCountText.gameObject.SetActive(true);
                }
            }

            else
            {
                if (CharacterIndex == SwitchableCharacters.Count - 1)
                {
                    SwitchableCharacters[0].GetComponent<CharacterSwitch>().MovesUI.gameObject.SetActive(false);
                    SwitchableCharacters[0].GetComponent<PlayerStats>().CollectableCountText.gameObject.SetActive(false);
                    SwitchableCharacters[0].GetComponent<CharacterSwitch>().MovesUI.SetAsFirstSibling();
                    SwitchableCharacters[0].GetComponent<PlayerStats>().CollectableCountText.transform.SetAsFirstSibling();
                }

                else if (SwitchableCharacters[CharacterIndex + 1] != null)
                {
                    SwitchableCharacters[CharacterIndex + 1].GetComponent<CharacterSwitch>().MovesUI.gameObject.SetActive(false);
                    SwitchableCharacters[CharacterIndex + 1].GetComponent<PlayerStats>().CollectableCountText.gameObject.SetActive(false);
                    SwitchableCharacters[CharacterIndex + 1].GetComponent<CharacterSwitch>().MovesUI.SetAsFirstSibling();
                    SwitchableCharacters[CharacterIndex + 1].GetComponent<PlayerStats>().CollectableCountText.transform.SetAsFirstSibling();
                }

                SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.SetAsLastSibling();
                SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.gameObject.SetActive(true);
                SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().PlayerStats.CharacterPortrait.transform.parent.SetAsLastSibling();
                SwitchableCharacters[NextCharacterIndex].GetComponent<PlayerStats>().HeartsBackground.transform.SetAsLastSibling();
                SwitchableCharacters[NextCharacterIndex].GetComponent<PlayerStats>().CollectableCountText.transform.SetAsLastSibling();
                SwitchableCharacters[NextCharacterIndex].GetComponent<PlayerStats>().CollectableCountText.gameObject.SetActive(true);

                MovesUI.SetAsLastSibling();
                MovesUI.gameObject.SetActive(true);
                PlayerStats.CharacterPortrait.transform.parent.SetAsLastSibling();
                PlayerStats.HeartsBackground.transform.SetAsLastSibling();
                PlayerStats.CollectableCountText.transform.SetAsLastSibling();
                PlayerStats.CollectableCountText.gameObject.SetActive(true);
            }
        }

        else
        {
            if (MovesUI.localPosition.x > 0.0f && SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.localPosition.x < 0.0f
                || MovesUI.localPosition.x < 0.0f && SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.localPosition.x > 0.0f)
            {
                MovesUI.localScale = new Vector3(-MovesUI.localScale.x, MovesUI.localScale.y, MovesUI.localScale.z);
                SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.localScale = new Vector3(-SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.localScale.x, SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.localScale.y, SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.localScale.z);
                Attack1UI.localScale = new Vector3(-Attack1UI.localScale.x, Attack1UI.localScale.y, Attack1UI.localScale.z);
                Attack2UI.localScale = new Vector3(-Attack2UI.localScale.x, Attack2UI.localScale.y, Attack2UI.localScale.z);
                MovesUI.GetChild(2).localScale = new Vector3(-MovesUI.GetChild(2).localScale.x, MovesUI.GetChild(2).localScale.y, MovesUI.GetChild(2).localScale.z);
                MovesUI.GetChild(3).localScale = new Vector3(-MovesUI.GetChild(3).localScale.x, MovesUI.GetChild(3).localScale.y, MovesUI.GetChild(3).localScale.z);
                SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.GetChild(2).localScale = new Vector3(-SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.GetChild(2).localScale.x, SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.GetChild(2).localScale.y, SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.GetChild(2).localScale.z);
                SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.GetChild(3).localScale = new Vector3(-SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.GetChild(3).localScale.x, SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.GetChild(3).localScale.y, SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.GetChild(3).localScale.z);
                SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().Attack1UI.localScale = new Vector3(-SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().Attack1UI.localScale.x, SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().Attack1UI.localScale.y, SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().Attack1UI.localScale.z);
                SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().Attack2UI.localScale = new Vector3(-SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().Attack2UI.localScale.x, SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().Attack2UI.localScale.y, SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().Attack2UI.localScale.z);
                MoveUI(MovesUI, SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI);
                MoveUI(PlayerStats.CharacterPortrait.transform, SwitchableCharacters[NextCharacterIndex].GetComponent<PlayerStats>().CharacterPortrait.transform);
                MoveUI(PlayerStats.HeartsBackground.transform, SwitchableCharacters[NextCharacterIndex].GetComponent<PlayerStats>().HeartsBackground.transform);
                MoveUI(PlayerStats.CollectableCountText.transform, SwitchableCharacters[NextCharacterIndex].GetComponent<PlayerStats>().CollectableCountText.transform);
            }

            MovesUI.gameObject.SetActive(false);
            MovesUI.SetAsFirstSibling();
            SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.SetAsLastSibling();
            SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().MovesUI.gameObject.SetActive(true);
            PlayerStats.CharacterPortrait.transform.parent.SetAsFirstSibling();
            SwitchableCharacters[NextCharacterIndex].GetComponent<CharacterSwitch>().PlayerStats.CharacterPortrait.transform.parent.SetAsLastSibling();
            PlayerStats.HeartsBackground.transform.SetAsFirstSibling();
            SwitchableCharacters[NextCharacterIndex].GetComponent<PlayerStats>().HeartsBackground.transform.SetAsLastSibling();
            PlayerStats.CollectableCountText.transform.SetAsFirstSibling();
            SwitchableCharacters[NextCharacterIndex].GetComponent<PlayerStats>().CollectableCountText.transform.SetAsLastSibling();
            PlayerStats.CollectableCountText.gameObject.SetActive(false);
            SwitchableCharacters[NextCharacterIndex].GetComponent<PlayerStats>().CollectableCountText.gameObject.SetActive(true);
        }
    }

    private bool FindNext()
    {
        for (int i = CharacterIndex; i < SwitchableCharacters.Count; i++)
        {
            if (SwitchableCharacters[i] != null && SwitchableCharacters[i].GetComponent<CompanionState>().enabled)
            {
                NextCharacterIndex = i;
                return true;
            }

            if (i == SwitchableCharacters.Count - 1)
            {
                i = -1;
            }

            IndexesChecked++;

            if (IndexesChecked == SwitchableCharacters.Count)
            {
                IndexesChecked = 0;
                return false;
            }
        }

        return false;
    }

    public bool FindPrevious()
    {
        for (int i = CharacterIndex; i > -1; i--)
        {
            if (SwitchableCharacters[i] != null && SwitchableCharacters[i].GetComponent<CompanionState>().enabled)
            {
                NextCharacterIndex = i;
                return true;
            }

            if (i == 0)
            {
                i = SwitchableCharacters.Count;
            }

            IndexesChecked++;

            if (IndexesChecked == SwitchableCharacters.Count)
            {
                IndexesChecked = 0;
                return false;
            }
        }

        return false;
    }
}
