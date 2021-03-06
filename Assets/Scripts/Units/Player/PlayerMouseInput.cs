﻿using StateMachine;
using UnityEngine;
using UnityEngine.AI;

//TODO check that knockback works, check that disable input from menus etc works.
namespace Units.Player {
    [RequireComponent(typeof(PlayerMeleeAttack))]
    [RequireComponent(typeof(PlayerRangedAttack))]
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerMouseInput : MonoBehaviour {
        [SerializeField] private LayerMask walkableLayers;
        [SerializeField] private LayerMask enemyLayers;
        [SerializeField] private Texture2D defaultCursorTexture;
        [SerializeField] private Texture2D meleeCursorTexture;
        [SerializeField] private Texture2D rangedCursorTexture;
        public HealthScriptableObject healthScriptableObject;
        private GameObject _target;
        private UnityEngine.Camera _mainCamera;

        private PlayerMeleeAttack _playerMeleeAttack;
        private PlayerMovement _playerMovement;
        private PlayerRangedAttack _playerRangedAttack;
        private FSMWorkWithAnimation _FSMWorkWithAnimation;

        private bool _inputDisabled;

        private void DisableInput() {
            _inputDisabled = true;
        }
        
        public void EnableInput() {
            _inputDisabled = false;
            GetComponent<NavMeshAgent>().enabled = true;
        }

        private void Start() {
            _mainCamera = UnityEngine.Camera.main;
            _playerMeleeAttack = GetComponent<PlayerMeleeAttack>();
            _playerMovement = GetComponent<PlayerMovement>();
            _playerRangedAttack = GetComponent<PlayerRangedAttack>();
            _FSMWorkWithAnimation = GetComponent<FSMWorkWithAnimation>();
            healthScriptableObject.OnDeath += DisableInput;
            SetDefaultCursor();
        }

        private void OnDestroy() {
            healthScriptableObject.OnDeath -= DisableInput;
        }

        private void Update() {

            if (_inputDisabled)
                return;

            bool LMBClickedThisTurn = Input.GetMouseButtonDown(0);
            bool LMBHeld = Input.GetMouseButton(0);
            bool RMBClickedThisTurn = Input.GetMouseButtonDown(1);
            bool RMBHeld = Input.GetMouseButton(1);

            SetMouseCursor();

            if (LMBClickedThisTurn || RMBClickedThisTurn) {
                HandleNewAction(LMBClickedThisTurn, RMBClickedThisTurn);
                return;
            }
            
            if (LMBHeld || RMBHeld) {
                HandleButtonHeld(LMBHeld, RMBHeld);
            }
        }

        private void SetMouseCursor() {
            Ray myRay = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (IsMouseCursorOnEnemy(myRay, out hitInfo)) {
                if (_playerMeleeAttack.WithinAttackRange(hitInfo.collider.gameObject.transform.position) &&
                    _FSMWorkWithAnimation.CrowbarIsReady)
                    SetMeleeCursor();
                else if (_FSMWorkWithAnimation.GunIsReady)
                    SetRangedCursor();
            }
            else {
                SetDefaultCursor();
            }
        }

        private void SetRangedCursor() {
            Cursor.SetCursor(rangedCursorTexture, Vector2.zero, CursorMode.Auto);
        }

        private void SetMeleeCursor() {
            Cursor.SetCursor(meleeCursorTexture, Vector2.zero, CursorMode.Auto);
        }

        private void HandleNewAction(bool LMBClickedThisTurn, bool RMBClickedThisTurn) {
            _playerMovement.ResetPath();
            _target = null;
            
            Ray myRay = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            
            if (IsMouseCursorOnEnemy(myRay, out hitInfo)) {
                _target = hitInfo.collider.gameObject;
                if (LMBClickedThisTurn) {
                    TryMeleeAttack();
                }
                else if (RMBClickedThisTurn) {
                    if (_playerRangedAttack.AttackIsReady && _FSMWorkWithAnimation.GunIsReady)
                        StartRangedAttack();
                }
            }
        }

        private void WalkToMousePoint() {
            Ray myRay = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(myRay, out hitInfo, 1000, walkableLayers)) {
                _playerMovement.SetDestination(hitInfo.point);
            }
        }

        private void HandleButtonHeld(bool LMBHeld, bool RMBheld) {
            _playerMovement.ResetPath();
            if (RMBheld)
            {
                if (!_FSMWorkWithAnimation.GunIsReady) return;
                HandleRangedAttackCharging();

            }
            else if (LMBHeld) {
                TryMeleeAttack();
            }
        }

        private void HandleRangedAttackCharging() {
            if (!RangeChargeBroken())
            {
                transform.LookAt(_target.transform.position);
                if (_playerRangedAttack.AttackIsReady && _playerRangedAttack.BuildUpIsDone) {
                    _playerRangedAttack.FireProjectile(_target.transform.position);
                }
            }
        }
        
        private bool RangeChargeBroken() {
            return _target == null || !_playerRangedAttack.TargetWithinAttackRange(_target.transform.position);
        }

        private void SetDefaultCursor() {
            Cursor.SetCursor(defaultCursorTexture, Vector2.zero, CursorMode.Auto);
        }

        private bool IsMouseCursorOnEnemy(Ray myRay, out RaycastHit hitInfo) {
            return Physics.Raycast(myRay, out hitInfo, 1000, enemyLayers);
        }
        
        private void StartRangedAttack() {
            if (!_FSMWorkWithAnimation.GunIsReady) return;
            _playerRangedAttack.StartRangedAttack();
        }

        private void TryMeleeAttack() {
            if (_target != null && _playerMeleeAttack.WithinAttackRange(_target.transform.position) &&
                _FSMWorkWithAnimation.CrowbarIsReady) {
                _playerMeleeAttack.TryAttack(_target);
            }
            else {
                WalkToMousePoint();
            }
        }
    }
}