﻿using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

// PATROLLING THREAT 
namespace Units.EnemyAI {
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    public class WaypointMovement : MonoBehaviour {
        public Transform[] targetLocationArray;
        private NavMeshAgent _myAgent;
        private int _index = 0;
        private bool _movingForwards = true;
        [Tooltip("If not looping the movement reverses on the end node")]
        public PatrolBehaviour patrolBehaviour;
        public enum PatrolBehaviour { Loop, BackAndForth, Random, Stationary } 
        //TODO add stationary state for clarity

        void Start() {
            _myAgent = GetComponent<NavMeshAgent>();
            _myAgent.SetDestination(targetLocationArray[_index].position);
            GetComponent<Rigidbody>().isKinematic = true;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.transform.position == targetLocationArray[_index].position) {
                GoToNextPosition();
            }
        }

        private void GoToNextPosition() {
            switch (patrolBehaviour) {
                case PatrolBehaviour.Loop:
                    _index++;
                    _index %= (targetLocationArray.Length);
                    break;
                case PatrolBehaviour.BackAndForth:
                    _index = _movingForwards ? _index + 1 : _index - 1;
                    //at the end
                    if (_index == targetLocationArray.Length - 1) {
                        _movingForwards = false;
                    }
                    else if (_index == 0) {
                        _movingForwards = true;
                    }
                    break;
                case PatrolBehaviour.Random:
                    int randomIndex = _index;
                    while (randomIndex == _index) {
                        randomIndex = Random.Range(0, targetLocationArray.Length);
                    }
                    _index = randomIndex;
                    break;                
                case PatrolBehaviour.Stationary:
                    _index = 0;
                    break;
            }
            SetDestination();
        }

        public void SetDestination() {
            var nextPosition = targetLocationArray[_index].position;
            _myAgent.SetDestination(nextPosition);
        }
    }
}
