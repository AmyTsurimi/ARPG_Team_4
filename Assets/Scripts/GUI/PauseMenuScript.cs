﻿using Units.Player;
using UnityEngine;
//using AudioScripts.AudioTest;

namespace GUI {
// TODO PortalButton sound effect

    public class PauseMenuScript : MonoBehaviour
    {
        public GameObject pauseMenuRef;
        private readonly string menuInputString = "Menu";
        public GameObject shadow;
        private PlayerMovement _playerMovement;

        private void Start() {
            _playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
            if (_playerMovement == null)
                Debug.LogWarning("ClickToMove missing", this);
        }

        void Update() {

            if (Input.GetButtonDown(menuInputString)) {
                bool theMenuIsActive = pauseMenuRef.activeSelf;
                if (theMenuIsActive) {
                    pauseMenuRef.SetActive(false);
                    shadow.SetActive(false);
                    _playerMovement.InputDisabled = false;
                    ResumeGame();
                    SetMenuSoundActive(false);
                }
                else {
                    PauseGame();
                    pauseMenuRef.SetActive(true);
                    shadow.SetActive(true);
                    _playerMovement.InputDisabled = true;
                    SetMenuSoundActive(true);
                }
            }
        }

        public void ResumeButton() {
            PlayButtonSound(true);
            shadow.SetActive(false);
            pauseMenuRef.SetActive(false);
            _playerMovement.InputDisabled = false;
            ResumeGame();
            SetMenuSoundActive(false);
        }

        public void LoadCheckpointButton() {
            PlayButtonSound(true);
        }

    
     
        public void QuitButton() {
            PlayButtonQuit(true);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
        private void PlayButtonQuit(bool setButton_1_SFX)
        {
            var AT = FindObjectOfType<Audio_SFX_Controller>();
            AT.Button_1_SFXSet(setButton_1_SFX);
            //Debug.Log("Calling for Audio Quit Test");
            //var aTposition = AT.gameObject.transform.position;
            //FMODUnity.RuntimeManager.PlayOneShot("event:/THESPLIT/SFXSplit/ButtonSplit/ButtonsSplit_2", aTposition);

        }
        private void PlayButtonSound(bool setButton_2_SFX) {
            var AT = FindObjectOfType<Audio_SFX_Controller>();
            AT.Button_2_SFXSet(setButton_2_SFX);
            //Debug.Log("Calling for Audio Test");
            //var aTposition = AT.gameObject.transform.position;
            //FMODUnity.RuntimeManager.PlayOneShot("event:/THESPLIT/SFXSplit/ButtonSplit/ButtonsSplit_1", aTposition);
        }
    
        void PauseGame ()
        {
            
            //var AT = FindObjectOfType<AudioTest>();
            //AT.MenuBGMSoundOn += 1;
            Time.timeScale = 0f;
        }

        void ResumeGame ()
        {

            //var AT = FindObjectOfType<AudioTest>();
            //AT.MenuBGMSoundOn -= 1;
            Time.timeScale = 1;
        }

        private void SetMenuSoundActive(bool menuActive)
        {
            var audioTest = FindObjectOfType<Audio_BGM_Controller>();
            audioTest.MenuActive(menuActive);
            //Debug.Log("Trigger menu sound: " + menuActive);
        }

        /*
        //Audio
        [FMODUnity.EventRef]
        public string MenuBGMEvent = "";
        FMOD.Studio.EventInstance MenuBGM;
        public GameObject _player;
        public FMOD.Studio.EventInstance MenuButtonAudio;
        private bool playerTransform;
        
        //Audio Button
        MenuButtonAudio = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Button/Buttons");
            playerTransform = GetComponent<AudioTest>().ButtonPress;
            
            AT.MenuQuitButtonPress = 0;
            AT.MenuQuitButtonPress += 1;    

            AT.MenuBGMSoundOn += 1;
            FMODUnity.RuntimeManager.
            FMOD.Studio.EventInstance MenuBGMEvent;
            MenuBGM = FMODUnity.RuntimeManager.CreateInstance(MenuBGMEvent);
            MenuBGM.Start();
            
            AT.MenuButtonPress = 0;
            AT.MenuButtonPress += 1;

            Debug.Log("ButtonPress True ");
            
            playerTransform = true;
            Debug.Log("ButtonPress " + playerTransform);
            buttonSound.Play();
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(MenuButtonAudio, transform, GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>());
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Button/Buttons", _player.GetComponent<Transform>().position);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(MenuButtonAudio, transform, GameObject.FindObjectOfType<player>.GetComponent<Rigidbody>()); 
        */
    }
}