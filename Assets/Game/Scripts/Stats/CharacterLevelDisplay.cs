using RPG.Stats;
using TMPro;
using UnityEngine;

public class CharacterLevelDisplay : MonoBehaviour
{
    [SerializeField]
    TMP_Text levelValueText;

    private BaseStats baseStats;
    private void Awake()
    {
        baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
    }

    private void OnEnable()
    {
        levelValueText.text = baseStats.GetCurrentLevel().ToString();
        baseStats.LevelChanged += UpdateLevelDisplay;
    }

    private void UpdateLevelDisplay()
    {
        levelValueText.text = baseStats.GetCurrentLevel().ToString();
    }

    private void OnDisable()
    {
        baseStats.LevelChanged -= UpdateLevelDisplay;
    }
}
