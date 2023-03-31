using System;
using UnityEngine;
using RPG.Core.SavingSystem;
using System.Collections;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string DEFAULT_SAVE_FILE = "rpg_save";

        [SerializeField]
        private float fadeInTime = 0.5f;

        private IEnumerator Start()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return null;
            yield return GetComponent<JsonSavingSystem>().LoadLastScene(DEFAULT_SAVE_FILE);
            yield return fader.FadeIn(fadeInTime);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F9))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.F5))
            {
                Save();
            }
        }

        public void Save()
        {
            GetComponent<JsonSavingSystem>().Save(DEFAULT_SAVE_FILE);
        }

        public void Load()
        {
            GetComponent<JsonSavingSystem>().Load(DEFAULT_SAVE_FILE);
        }
    }
}