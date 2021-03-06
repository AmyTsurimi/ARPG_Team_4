﻿using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.AI;

namespace Units.Player {
    public class PlayerHealth : MonoBehaviour, IDamagable {
        public HealthScriptableObject healthScriptableObject;
        public UnityEvent GetDamaged;
        [Tooltip("the duration the player is invulnerable from damage on revival")]
        [SerializeField] private float _invulnerabilityDuration = 5f;
        private bool _invulnerable = false; 
        LayerMask _deadPlayerLayer;
        LayerMask _alivePlayerLayer;

        private void Start() {
            _deadPlayerLayer = 0;
            _alivePlayerLayer = 9;
            if (healthScriptableObject.CurrentHealth == 0) {
                healthScriptableObject.CurrentHealth = healthScriptableObject.MaxHealth;
            }
        }

        private void FixedUpdate() {
            if (healthScriptableObject.CurrentHealth > 0 && gameObject.layer != _alivePlayerLayer) {
                gameObject.layer = _alivePlayerLayer;
            } else if (healthScriptableObject.CurrentHealth == 0) {
                gameObject.layer = _deadPlayerLayer;
            }
        }

        public void TakeDamage(int damage) {
            
            if (_invulnerable || healthScriptableObject.CurrentHealth == 0)
                return;
            
            healthScriptableObject.CurrentHealth -= damage;
            SendMessage("ShowBloodVFX", SendMessageOptions.DontRequireReceiver);
            GetDamaged.Invoke();
            StartCoroutine(DamageFeedback());
        }
        
        IEnumerator DamageFeedback() {
            //Audio Player Hit
            PlayHurtAudio(true);

            var playerMesh = GetComponent<MeshRenderer>();
            playerMesh.enabled = false;
            yield return new WaitForSeconds(0.2f);
            playerMesh.enabled = true;
            yield return null;
        }
        private void PlayHurtAudio(bool setPlayerHitSFX)
        {
            var AT = FindObjectOfType<Audio_Character_Controller>();
            AT.PlayerHitSFXSet(setPlayerHitSFX);
        }
        
        public void GainHealth(int healValue) {
            healthScriptableObject.CurrentHealth += healValue;
            SendMessage("ShowHealthVFX", SendMessageOptions.DontRequireReceiver);
        }

        public void TriggerInvulnerability() {
            _invulnerable = true;
            StartCoroutine(InvulnerabilityTimer());
        }

        private IEnumerator InvulnerabilityTimer() {
            yield return new WaitForSeconds(_invulnerabilityDuration);
            _invulnerable = false;
        }
    }
}
