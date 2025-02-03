using UnityEngine;
using UnityEngine.UI;

namespace Tcp4
{
    public class SoundManager : MonoBehaviour
    {
        public AudioSource musicSource;
        public AudioSource[] sfxSources;
        public Toggle toggleMusic;
        public Toggle toggleSFX;

        private void Start()
        {
            bool isMusicOn = PlayerPrefs.GetInt("Music", 1) == 1;
            bool isSFXOn = PlayerPrefs.GetInt("SFX", 1) == 1;

            toggleMusic.isOn = !isMusicOn;
            toggleSFX.isOn = !isSFXOn;

            ToggleMusic(toggleMusic.isOn);
            ToggleSFX(toggleSFX.isOn);

            toggleMusic.onValueChanged.AddListener(ToggleMusic);
            toggleSFX.onValueChanged.AddListener(ToggleSFX);
        }

        public void ToggleMusic(bool isOn)
        {
            musicSource.mute = isOn;
            PlayerPrefs.SetInt("Music", isOn ? 0 : 1);
        }

        public void ToggleSFX(bool isOn)
        {
            foreach (AudioSource sfx in sfxSources)
            {
                sfx.mute = isOn;
            }
            PlayerPrefs.SetInt("SFX", isOn ? 0 : 1);
        }
    }
}