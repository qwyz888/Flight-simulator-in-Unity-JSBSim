using Infrastructure.Data.Models.Persistent.Data;
using Infrastructure.Data.Models.Persistent.Data.Core;
using UnityEngine;

namespace Infrastructure.Data.Models.Static.Data
{
    [CreateAssetMenu(fileName = "DefaultPersistentDataConfig", menuName = "ScriptableObjects/Static/DefaultPersistentDataConfig")]
    public class PersistentDataConfig : ScriptableObject
    {
        [SerializeField] private int _coins = 100;
        [SerializeField] private SettingsData _settings;

        public PersistentData Get()
        {
            PersistentData data = new PersistentData();

            data.PlayerData.Coins.SetValue(_coins);
            data.SettingsData = new SettingsData(_settings);

            return data;
        }
    }
}