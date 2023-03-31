using RPG.SceneManagement;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public enum PortalIdentifier
    {
        A, B, C, D, E, F, G, H, I,
    }

    [SerializeField]
    private PortalIdentifier Id;
    [SerializeField]
    private PortalIdentifier OtherPortalId;

    [SerializeField]
    int sceneToLoad = -1;
    [SerializeField]
    private Transform spawnPoint;

    [Header("Transition Settings")]
    [SerializeField]
    private float fadeOutTime = 1f; 
    [SerializeField]
    private float fadeInTime = 1f;
    [SerializeField]
    private float transitionWaitingTime = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SceneTransition());
        }
    }

    private IEnumerator SceneTransition()
    {
        if(sceneToLoad < 0)
        {
            Debug.LogWarning("Scene to load not configured!");
            yield break;
        }

        Fader fader = FindObjectOfType<Fader>();
        yield return fader.FadeOut(fadeOutTime);

        //save current level
        SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
        savingWrapper.Save();

        DontDestroyOnLoad(gameObject);
        yield return SceneManager.LoadSceneAsync(sceneToLoad);

        //load current level
        savingWrapper.Load();

        Portal otherPortal = GetOtherPortal();
        if(otherPortal != null)
        {
            UpdatePlayer(otherPortal);
        }

        yield return new WaitForSeconds(transitionWaitingTime);
        yield return fader.FadeIn(fadeInTime);

        savingWrapper.Save();
        Destroy(gameObject);
    }

    private void UpdatePlayer(Portal otherPortal)
    {
        GameObject player = GameObject.FindWithTag("Player");
        NavMeshAgent agent = player.GetComponent<NavMeshAgent>();
        agent.Warp(otherPortal.spawnPoint.position);
        player.transform.rotation = otherPortal.spawnPoint.rotation;
    }

    private Portal GetOtherPortal()
    {
        Portal other = FindObjectsOfType<Portal>().FirstOrDefault(p => p != this && p.Id == OtherPortalId );
        return other;
    }
}
