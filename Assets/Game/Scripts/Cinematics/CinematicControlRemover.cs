using RPG.Controlls;
using RPG.Core;
using System;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        private GameObject player;
        private PlayableDirector director;

        private void Start()
        {
            director = GetComponent<PlayableDirector>();
            player = GameObject.FindWithTag("Player");
            director.played += DisableControl;
            director.stopped += EnableControl;
        }

        private void DisableControl(PlayableDirector pd)
        {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        private void EnableControl(PlayableDirector pd)
        {
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}
