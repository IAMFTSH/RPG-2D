using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    //TODO 需要把IsPause运用到所有的Update和lastUpdate中，否则还是会运行，暂停后还是会跳跃
    public static bool IsPause;

    private float timeScaleTmp;

    public AudioMixer audioMixer;
    private Slider mainVolumeSlider;

    private GameObject UI;
    // Start is called before the first frame update
    private void Awake()
    {
        UI = transform.Find("UI").gameObject;
        mainVolumeSlider=UI.transform.Find("MainVolumeSlider").gameObject.GetComponent<Slider>();
    }
    public void Continue()
    {
        UI.SetActive(false);
        Time.timeScale = timeScaleTmp;
    }
    public void Exit()
    {
        Application.Quit();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPause)
            {
                Time.timeScale = timeScaleTmp;
                UI.SetActive(false);
            }
            else
            {
                timeScaleTmp = Time.timeScale;
                Time.timeScale = 0;
                UI.SetActive(true);
            }
            IsPause = !IsPause;
        }
    }
    public void setMainVolume(){
        audioMixer.SetFloat("MainVolume",mainVolumeSlider.value);
    }
}
