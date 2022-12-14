using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace RGYB
{
    [System.Serializable]
    public enum SFXType
    {
        // Card
        Card_Create,//
        Card_MouseOver,
        Card_Select,//
        Card_Open,//
        // GameBoard
        GameBoard_ClickAnywhere,//
        GameBoard_Scroll,//
        GameBoard_UnScroll,
        // Sequence
        Sequece_AlertPopUpOpen,//
        Sequece_AlertPopUpClose,//
        Sequece_Select,//
        Sequece_CannotSelect,//
        Sequece_TimeOver,//
        Sequece_Timer,//
        Sequece_TurnSign,//
        Sequece_Victory,//
        // ETC
        Emotion//
    }

    public enum MenuType
    {
        EnterWaitRoom,
        Logo, //
        MatchFound,
        MatchStart,//
        MatchUnable,//
        MenuSelect,//
        Purchase,
        SelectButton,//
        SelectToggle//
    }

    [System.Serializable]
    public struct SFXClip
    {
        public SFXType Type;
        public AudioClip Clip;
    }

    [System.Serializable]
    public struct MenuClip
    {
        public MenuType Type;
        public AudioClip Clip;
    }

    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;
        [SerializeField] private AudioSource BGMSpeaker;
        [SerializeField] private AudioSource SFXSpeaker;
        [SerializeField] private AudioMixer masterMixer;
        [SerializeField] private Slider bgmAudioSlider;
        [SerializeField] private Toggle bgmAudioToggle;
        [SerializeField] private Slider sfxAudioSlider;
        [SerializeField] private Toggle sfxAudioToggle;
        private int bgmIndex;

        [SerializeField] private AudioClip bgmAudioClip;
        [SerializeField] private List<SFXClip> sfxAudioList;
        [SerializeField] private List<MenuClip> menuAudioList;
        private Dictionary<SFXType, AudioClip> sfxAudioDictionary = new Dictionary<SFXType, AudioClip>();
        private Dictionary<MenuType, AudioClip> menuAudioDictionary = new Dictionary<MenuType, AudioClip>();


        void Awake()
        {
            if (Instance == null) { Instance = this; }
            else { Destroy(gameObject); }
            BGMSpeaker.loop = true;
            SFXSpeaker.loop = false;

            foreach (SFXClip sfx in sfxAudioList)
            {
                sfxAudioDictionary[sfx.Type] = sfx.Clip;
            }

            foreach (MenuClip menu in menuAudioList)
            {
                menuAudioDictionary[menu.Type] = menu.Clip;
            }

            PlayBGM();
        }

        public void PlayBGM()
        {
            BGMSpeaker.clip = bgmAudioClip;
            BGMSpeaker.Play();
        }

        public bool IsSFXPlaying()
        {
            return SFXSpeaker.isPlaying;
        }

        public void PlaySFX(SFXType type, bool isLoop = false)
        {
            SFXSpeaker.loop = isLoop;
            SFXSpeaker.PlayOneShot(sfxAudioDictionary[type]);
        }

        public void PlayMenu(MenuType type, bool isLoop = false)
        {
            if (MenuManager.Instance.IsResetPhase) return;
            SFXSpeaker.loop = isLoop;
            SFXSpeaker.PlayOneShot(menuAudioDictionary[type]);
        }

        public void StopAllAudio()
        {
            BGMSpeaker.Stop();
            SFXSpeaker.Stop();
        }

        public void StopSFX()
        {
            SFXSpeaker.Stop();
        }

        public void StopMenu()
        {
            SFXSpeaker.Stop();
        }

        public void BGMAudioControl()
        {
            float sound = bgmAudioSlider.value;

            if (sound <= -30f)
            {
                masterMixer.SetFloat("BGM", -80);
                bgmAudioToggle.isOn = false;
                BGMSpeaker.mute = true;
            }
            else
            {
                masterMixer.SetFloat("BGM", sound);
                bgmAudioToggle.isOn = true;
                BGMSpeaker.mute = false;
            }
        }

        public void SFXAudioControl()
        {
            float sound = sfxAudioSlider.value;

            if (sound <= -30f)
            {
                masterMixer.SetFloat("SFX", -80);
                sfxAudioToggle.isOn = false;
                SFXSpeaker.mute = true;
            }
            else
            {
                masterMixer.SetFloat("SFX", sound);
                sfxAudioToggle.isOn = true;
                SFXSpeaker.mute = false;
            }
        }

        public void ToggleBGM()
        {
            BGMSpeaker.mute = !BGMSpeaker.mute;
        }

        public void ToggleSFX()
        {
            SFXSpeaker.mute = !SFXSpeaker.mute;
        }
    }
}