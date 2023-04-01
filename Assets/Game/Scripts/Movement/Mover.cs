using RPG.Core;
using RPG.Core.Interfaces;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core.SavingSystem;
using Newtonsoft.Json.Linq;
using System;

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
            return JToken.FromObject(new MovementData(transform.position, transform.eulerAngles));
        }

        public void RestoreFromJToken(JToken state)
        {
            MovementData data = state.ToObject<MovementData>();
            agent = GetComponent<NavMeshAgent>();
            agent.enabled = false;
            transform.position = data.Location.ToVector3();
            transform.eulerAngles = data.Rotation.ToVector3();
            agent.enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }

    [Serializable]
    struct MovementData
    {
        public JToken Location { get; set; }
        public JToken Rotation { get; set; }

        public MovementData(Vector3 l, Vector3 r)
        {
            Location = l.ToToken();
            Rotation = r.ToToken();
        }
    }
}
