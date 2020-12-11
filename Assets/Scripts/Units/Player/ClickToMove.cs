﻿using System;
using UnityEngine;
using UnityEngine.AI;


namespace Units.Player {
    [RequireComponent(typeof(Rigidbody))]
    public class ClickToMove : MonoBehaviour{

        public LayerMask whatCanBeClickedOn;
        private NavMeshAgent myAgent;
        private UnityEngine.Camera mainCamera;
        public HealthScriptableObject healthScriptableObject;
        private Rigidbody _rigidbody;
        
        private bool _inputDisabled;
        private bool _knockbackActive = false;

        // Audio Walking Test
        [SerializeField] [FMODUnity.EventRef] private string FootstepEventPath;
        [SerializeField] private float _raycastDistance = 0.5f;
        private RaycastHit raycastHit;



        /*
        FMOD.Studio.EventInstance FootstepEvent;
        //FMOD.Studio.EventInstance.setParameterByName(string name, float value, bool ignoreseekspeed = false);
        public string inputSound;
        bool playersMoving;
        public float walkingSpeed;
        //FMOD.Studio.par 
        */
        public bool InputDisabled {
            set {
                _inputDisabled = value;
                myAgent.ResetPath();
            }
        }
        
        void Start() {
            myAgent = GetComponent<NavMeshAgent>();
            mainCamera = UnityEngine.Camera.main;

            healthScriptableObject.OnDeath += DisableInput;
            
            if (mainCamera == null) {
                throw new Exception("Main camera is null: Camera needs MainCamera tag.");
            }

            _rigidbody = GetComponent<Rigidbody>();

            // Audio Walking Test
            // InvokeRepeating("PlayerIsWalking", 0, walkingSpeed);
        }

        void Update() {
            Debug.DrawRay(transform.position, Vector3.down * _raycastDistance, Color.green);

            if (_knockbackActive) {
                if (_rigidbody.velocity.magnitude < 2f)
                    DisableKnockback();
                return;
            }

            if (_inputDisabled) {
                myAgent.SetDestination(transform.position);
                return;
            }
            if (Input.GetMouseButton(0)) {
                Ray myRay = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;

                // Audio Walking Test

                //playersMoving = true;
                //PlayerIsWalking();

                if (Physics.Raycast(myRay, out hitInfo, 1000, whatCanBeClickedOn)){
                    myAgent.SetDestination(hitInfo.point);

                    // Audio Walking Test

                    //playersMoving = false;
                    //Debug.Log("Player Walking Audio Ends");
                }
                //Debug.Log("hitInfo: " + hitInfo.collider.name);
            }
        }
        // Audio Walking Test
        void FloorCheck()
        {
            Physics.Raycast(transform.position, Vector3.down, out raycastHit, _raycastDistance);
            //if(raycastHit.collider)
                

        }
        void PlayerMoveSound()
        {
            FMOD.Studio.EventInstance Footstep = FMODUnity.RuntimeManager.CreateInstance(FootstepEventPath);
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(Footstep, transform, GetComponent<Rigidbody>());
        }

        /*
        void PlayerIsWalking()
        {
            if (playersMoving == true)
            {
                FMODUnity.RuntimeManager.PlayOneShot(inputSound);
                Debug.Log("Player Walking Audio Plays");
            }
        }
        */
        public void DisableInput() {
            _inputDisabled = true;
        }

        public void Knockback(Vector3 velocity) {
            _knockbackActive = true;
            _rigidbody.isKinematic = false;
            myAgent.updatePosition = false;
            _rigidbody.velocity = velocity;
        }

        private void DisableKnockback() {
            _knockbackActive = false;
            _rigidbody.isKinematic = true;
            myAgent.nextPosition = _rigidbody.position;
            myAgent.ResetPath();
            myAgent.updatePosition = true;
        }

        public void ResetPath() {
            myAgent.ResetPath();
        }
    }
}

