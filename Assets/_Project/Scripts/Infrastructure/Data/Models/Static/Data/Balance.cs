using UnityEngine;

namespace Infrastructure.Data.Models.Static.Data
{
    [CreateAssetMenu(fileName = "GameBalance", menuName = "ScriptableObjects/Static/GameBalance")]
    public class Balance : ScriptableObject
    {
        [SerializeField] private PersistentDataConfig _defaultData;

        public PersistentDataConfig DefaultData => _defaultData;
    }
}