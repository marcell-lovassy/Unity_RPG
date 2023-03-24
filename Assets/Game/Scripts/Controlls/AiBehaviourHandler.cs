using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AiBehaviourHandler : MonoBehaviour
{
    List<AiBehaviour> behaviours;

    AiBehaviour currentBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        behaviours = GetComponents<AiBehaviour>().OrderBy(b => b.BehaviourPriority).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBehaviour != null && !currentBehaviour.CanDoBehaviour())
        {
            currentBehaviour.StopBehaviour();
            currentBehaviour = null;
        }
        
        var availableBehaviour = behaviours.FirstOrDefault(b => b.CanDoBehaviour());
        if (availableBehaviour != null && currentBehaviour != availableBehaviour)
        {
            Debug.LogWarning($"stopping: {currentBehaviour}");
            currentBehaviour?.StopBehaviour();
            currentBehaviour = availableBehaviour;
        }
        currentBehaviour?.StartBehaviour();
    }
}
