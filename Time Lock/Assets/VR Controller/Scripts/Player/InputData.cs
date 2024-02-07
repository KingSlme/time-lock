using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InputData : MonoBehaviour
{
    private InputDevice _leftController;
    private InputDevice _rightController;
    private InputDevice _HMD;

    public InputDevice LeftController => _leftController;
    public InputDevice RightController => _rightController;
    public InputDevice HMD => _HMD;

    private void Update()
    {
        if (!_leftController.isValid || !_rightController.isValid || !_HMD.isValid)
            InitializeInputDevices();
    }

    private void InitializeInputDevices()
    {
        if (!_leftController.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, ref _leftController);
        if (!_rightController.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, ref _rightController);
        if (!_HMD.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.HeadMounted, ref _HMD);
    }

    private void InitializeInputDevice(InputDeviceCharacteristics inputCharacteristics, ref InputDevice inputDevice)
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(inputCharacteristics, devices);

        if (devices.Count > 0)
            inputDevice = devices[0];
    }
}
