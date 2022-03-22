using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class HandTrackingManager : MonoBehaviour
{
    public RawImage webcamOutput;
    public TMP_Dropdown webcamDropdown;
    public List<string> webcamDevices;

    WebCamTexture webcamChosen;
    WebCamDevice[] devices;

    void Awake()
    {
        devices = WebCamTexture.devices;
        webcamDropdown.options.Clear();

        for (int i = 0; i < devices.Length; i++)
        {
            webcamDevices.Add(devices[i].name);
            webcamDropdown.options.Add(new TMP_Dropdown.OptionData() { text = devices[i].name });
        }
    }

    void Start()
    {
        if (webcamChosen == null)
            webcamChosen = new WebCamTexture();

        webcamOutput.texture = webcamChosen;

        if (!webcamChosen.isPlaying)
            webcamChosen.Play();

        webcamDropdown.onValueChanged.AddListener(delegate {
            UpdatingWebcam(webcamDropdown);
        });
    }

    void UpdatingWebcam(TMP_Dropdown change)
    {
        webcamChosen.Stop();
        webcamChosen.deviceName = webcamDropdown.options[webcamDropdown.value].text;
        webcamChosen.Play();
    }
}
