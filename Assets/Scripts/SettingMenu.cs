using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingMenu : MonoBehaviour
{

    [SerializeField] private AudioMixer auMixer;
    [SerializeField] private TextMeshProUGUI tmpVolumeValue;
    [SerializeField] private TMP_Dropdown dropdownGraphic;
    [SerializeField] private TextMeshProUGUI auValTMP;
    [SerializeField] private Slider sliderVolume;
    [SerializeField] private TMP_Dropdown dropdownResolution;
    [SerializeField] private Toggle toggleFullScreen;
    [SerializeField] private AudioClip audioClipBGM;
    [SerializeField] private AudioSource audioSourceBGM;
    [SerializeField] private float bgmPlayTime = 3;
    private float bmgPlayTimemer;


    // Start is called before the first frame update
    private void Start()
    {
        this.InitFullScreenToggle();
        this.InitAudioMixerSlider();
        this.InitGraphicDropdown();
        this.InitResolutionDropdown();
    }

    // Update is called once per frame
    private void Update()
    {
        if(audioSourceBGM.isPlaying)
        {
            bmgPlayTimemer -= Time.deltaTime;
            if(bmgPlayTimemer <= 0)
            {
                audioSourceBGM.Stop();
            }
        }
    }

    private void InitFullScreenToggle()
    {
        this.toggleFullScreen.SetIsOnWithoutNotify(Screen.fullScreen);
    }

    private void InitAudioMixerSlider()
    {
        float currentVol = 0;
        this.auMixer.GetFloat("mainVolume", out currentVol);
        this.sliderVolume.SetValueWithoutNotify(currentVol);
        this.auValTMP.SetText(currentVol.ToString("0.00"));
    }

    private void InitGraphicDropdown()
    {
        this.dropdownGraphic.ClearOptions();
        List<TMP_Dropdown.OptionData> optionsList = new List<TMP_Dropdown.OptionData>();
        foreach(var name in QualitySettings.names)
            optionsList.Add(new TMP_Dropdown.OptionData() { text = name });
        this.dropdownGraphic.AddOptions(optionsList);
        this.dropdownGraphic.SetValueWithoutNotify(QualitySettings.GetQualityLevel());
    }

    private static string StringifyResolution(Resolution res)
    {
        return $"{res.width} x {res.height}";
    }

    private static bool isCurrentResolution(TMP_Dropdown.OptionData res) {
        return StringifyResolution(Screen.currentResolution) == res.text;
    }

    private void InitResolutionDropdown()
    {
        this.dropdownResolution.ClearOptions();
        List<TMP_Dropdown.OptionData> optionsList = new List<TMP_Dropdown.OptionData>();
        foreach(var res in Screen.resolutions)
            optionsList.Add(new TMP_Dropdown.OptionData() { text = StringifyResolution(res) });
        this.dropdownResolution.AddOptions(optionsList);
        this.dropdownResolution.SetValueWithoutNotify(optionsList.FindIndex(0, optionsList.Count, isCurrentResolution));
    }

    public void SetVolume(float volume)
    {
        this.auMixer.SetFloat("mainVolume", volume);
        this.tmpVolumeValue.SetText(volume.ToString("0.00"));
        if(audioSourceBGM.isPlaying) return;
        audioSourceBGM.Play();
        bmgPlayTimemer = bgmPlayTime;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution selectedResolution = Screen.resolutions[resolutionIndex];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
    }
}
