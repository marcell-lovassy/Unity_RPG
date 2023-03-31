using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool alreadyTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player") && !alreadyTriggered)
            {
                GetComponent<PlayableDirector>().Play();
                alreadyTriggered = true;
            }
        }
    }
}
