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

        //Audio
        /*
        [FMODUnity.EventRef]
        public string MenuBGMEvent = "";
        FMOD.Studio.EventInstance MenuBGM;
        public GameObject _player;
        public FMOD.Studio.EventInstance MenuButtonAudio;
        private bool playerTransform;
        */

        private void Start() {
            //Audio Button
            
            /*
            MenuButtonAudio = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Button/Buttons");
            playerTransform = GetComponent<AudioTest>().ButtonPress;
            */

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
                }
                else {
                    PauseGame();
                    pauseMenuRef.SetActive(true);
                    shadow.SetActive(true);
                    _playerMovement.InputDisabled = true;
                }
            }
        }

        public void ResumeButton() {
            PlayButtonSound();
            shadow.SetActive(false);
            pauseMenuRef.SetActive(false);
            _playerMovement.InputDisabled = false;
            ResumeGame();
        }

        public void LoadCheckpointButton() {
            PlayButtonSound();
        }

    
     
        public void QuitButton() {
            PlayButtonQuit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
        private void PlayButtonQuit()
        {

            var AT = FindObjectOfType<AudioTest>();
            Debug.Log("Calling for Audio Quit Test");
            var aTposition = AT.gameObject.transform.position;
            FMODUnity.RuntimeManager.PlayOneShot("event:/THESPLIT/SFXSplit/ButtonSplit/ButtonsSplit_2", aTposition);

            /*
            AT.MenuQuitButtonPress = 0;
            AT.MenuQuitButtonPress += 1;
            */
        }
        private void PlayButtonSound() {
            var AT = FindObjectOfType<AudioTest>();
            Debug.Log("Calling for Audio Test");
            var aTposition = AT.gameObject.transform.position;
            FMODUnity.RuntimeManager.PlayOneShot("event:/THESPLIT/SFXSplit/ButtonSplit/ButtonsSplit_1", aTposition);

            /*
            AT.MenuButtonPress = 0;
            AT.MenuButtonPress += 1;

            //Debug.Log("ButtonPress True ");
            /*
            playerTransform = true;
            Debug.Log("ButtonPress " + playerTransform);
            //            buttonSound.Play();
            //FMODUnity.RuntimeManager.AttachInstanceToGameObject(MenuButtonAudio, transform, GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>());
            //FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Button/Buttons", _player.GetComponent<Transform>().position);
            //FMODUnity.RuntimeManager.AttachInstanceToGameObject(MenuButtonAudio, transform, GameObject.FindObjectOfType<player>.GetComponent<Rigidbody>()); 
            */
        }
    
        void PauseGame ()
        {
            //FMOD.Studio.EventInstance MenuBGMEvent;
            //MenuBGM = FMODUnity.RuntimeManager.CreateInstance(MenuBGMEvent);
            //MenuBGM.Start();
            Time.timeScale = 0f;
        }

        void ResumeGame ()
        {
            Time.timeScale = 1;
        }
    }
}