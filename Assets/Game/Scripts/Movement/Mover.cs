using RPG.Core;
using RPG.Core.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, ICharacterAction
    {
        const string ANIM_SPEED_PARAM = "ForwardSpeed";

        private NavMeshAgent agent;
        private Animator animator;
        private ActionScheduler actionScheduler;

        void Start()
        {
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            actionScheduler.StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            agent.destination = destination;
            agent.isStopped = false;
        }

        public void StopAction()
        {
            agent.isStopped = true;
        }

        public void DisableAgent()
        {
            agent.enabled = false;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            animator.SetFloat(ANIM_SPEED_PARAM, speed);
        }
    }
}
