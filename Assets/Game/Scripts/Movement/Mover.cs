using RPG.Core;
using RPG.Core.Interfaces;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core.SavingSystem;
using Newtonsoft.Json.Linq;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, ICharacterAction, IJsonSaveable
    {
        const string ANIM_SPEED_PARAM = "ForwardSpeed";

        [SerializeField]
        private float movementSpeed;

        private NavMeshAgent agent;
        private Animator animator;
        private ActionScheduler actionScheduler;

        private void Awake()
        {
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
        }

        void Start()
        {
            agent.speed = movementSpeed;
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

        public void EnableAgent()
        {
            agent.enabled = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            animator.SetFloat(ANIM_SPEED_PARAM, speed);
        }

        public void SetAgentSpeed(float speed)
        {
            agent.speed = speed;
        }

        public void ResetSpeed()
        {
            agent.speed = movementSpeed;
        }

        public JToken CaptureAsJToken()
        {
            return transform.position.ToToken();
        }

        public void RestoreFromJToken(JToken state)
        {
            agent = GetComponent<NavMeshAgent>();
            agent.enabled = false;
            transform.position = state.ToVector3();
            agent.enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}
