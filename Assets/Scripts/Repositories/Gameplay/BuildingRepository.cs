#nullable enable

using System.Collections.Generic;
using ContractsInterfaces.UseCasesGameplay;
using Domain.Gameplay.Models;
using UnityEngine;

namespace Repositories.Gameplay
{
    /// <summary>
    /// ScriptableObject, представляющий собой репозиторий конфигурации для одного типа здания.
    /// </summary>
    [CreateAssetMenu(fileName = "BuildingRepository", menuName = "Repositories/Gameplay/Building Repository")]
    public class BuildingRepository : ScriptableObject, IBuildingConfig
    {
        [field: SerializeField]
        [field: Tooltip("Тип здания из доменной модели.")]
        public BuildingType BuildingType { get; private set; }

        [SerializeField] [Tooltip("Отображаемое имя для UI.")]
        private string _displayName = "";

        [SerializeField] [Tooltip("Иконка для каталога зданий в UI.")]
        private Sprite _sprite = null!;

        [SerializeField] [Tooltip("Префаб, который будет создаваться на сцене.")]
        private GameObject _prefab = null!;

        [SerializeField] [Tooltip("Список конфигураций для каждого уровня здания.")]
        private List<BuildingLevelConfig> _levels = new();

        /// <summary>
        /// Отображаемое имя для использования в UI.
        /// </summary>
        public string DisplayName => this._displayName;

        /// <summary>
        /// Иконка для UI.
        /// </summary>
        public Sprite Sprite => this._sprite;

        /// <summary>
        /// Префаб, ассоциированный со зданием.
        /// </summary>
        public GameObject Prefab => this._prefab;

        /// <summary>
        /// Список конфигураций уровней (только для чтения).
        /// </summary>
        public IReadOnlyList<BuildingLevelConfig> Levels => this._levels;

        BuildingType IBuildingConfig.BuildingType => this.BuildingType;
        Sprite IBuildingConfig.Sprite => this.Sprite;
        IReadOnlyList<IBuildingLevelData> IBuildingConfig.Levels => this._levels;
    }
}