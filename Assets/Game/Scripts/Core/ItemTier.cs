using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public enum Tier
    {
        Common,
        Rare,
        Epic,
        Legendary
    }

    [CreateAssetMenu(fileName = "ItemTier", menuName = "RPG/Weapons/Make new Tier", order = 0)]
    public class ItemTier : ScriptableObject
    {
        [SerializeField]
        public Tier Tier;
        [SerializeField]
        public Color Color;
    }
}
