using RPG.Core.Interfaces;
using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        ICharacterAction currentAction = null;

        public void StartAction(ICharacterAction action)
        {
            if (currentAction == action) return;

            if(currentAction != null)
            {
                currentAction.StopAction();
                print($"Cancelling action {currentAction}");
            }
            print($"Starting action {action}");
            currentAction = action;
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}
