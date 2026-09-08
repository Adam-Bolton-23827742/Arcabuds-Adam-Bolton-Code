using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Rumble : MonoBehaviour
{
    [SerializeField] private float RumbleTime;
    [HideInInspector] public bool CallFromRumbleScript;
    [HideInInspector] public Gamepad PlayerGamepad;
    [SerializeField] private Toggle RumbleToggle;

    private void Update()
    {
        if (CallFromRumbleScript && RumbleToggle.isOn)
        {
            StartCoroutine(VibrateController(PlayerGamepad));
        }
    }

    public IEnumerator VibrateController(Gamepad Gamepad)
    {
        if (RumbleToggle.isOn)
        {
            CallFromRumbleScript = false;
            Gamepad.SetMotorSpeeds(SetUpOptions.RumbleIntensity, SetUpOptions.RumbleIntensity);
            yield return new WaitForSeconds(RumbleTime);
            Gamepad.SetMotorSpeeds(0.0f, 0.0f);
            PlayerGamepad = null;
        }
    }
}
