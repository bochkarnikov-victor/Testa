#nullable enable

using System.Collections.Generic;
using UnityEngine;

namespace Presentation.Gameplay.Views
{
    /// <summary>
    /// Отвечает за визуализацию "призрачного" здания в режиме строительства.
    /// </summary>
    public class BuildingGhostView : MonoBehaviour
    {
        private GameObject? _ghostInstance;
        private readonly List<Material> _ghostMaterials = new();

        private void Awake()
        {
            this.Hide();
        }

        /// <inheritdoc/>
        public void Show(GameObject buildingPrefab)
        {
            if (this._ghostInstance != null)
            {
                Destroy(this._ghostInstance);
            }

            this._ghostInstance = Instantiate(buildingPrefab, this.transform);
            this.CacheAndReplaceMaterials(this._ghostInstance);

            this.gameObject.SetActive(true);
        }

        /// <inheritdoc/>
        public void Hide()
        {
            if (this._ghostInstance != null)
            {
                Destroy(this._ghostInstance);
                this._ghostInstance = null;
            }

            // Очищаем старые материалы
            foreach (Material? material in this._ghostMaterials)
            {
                Destroy(material);
            }

            this._ghostMaterials.Clear();

            this.gameObject.SetActive(false);
        }

        /// <inheritdoc/>
        public void SetPosition(Vector3 worldPosition)
        {
            this.transform.position = worldPosition;
        }

        /// <summary>
        /// Устанавливает цвет для всех материалов "призрачного" здания.
        /// </summary>
        public void SetColor(Color color)
        {
            foreach (Material? material in this._ghostMaterials)
            {
                material.color = color;
            }
        }

        private void CacheAndReplaceMaterials(GameObject instance)
        {
            if (instance == null)
            {
                return;
            }

            Renderer[] renderers = instance.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                // Создаем новые экземпляры материалов, чтобы не изменять ассеты
                Material[] newMaterials = new Material[renderer.sharedMaterials.Length];
                for (int i = 0; i < renderer.sharedMaterials.Length; i++)
                {
                    Material newMaterial = new(renderer.sharedMaterials[i]);
                    this._ghostMaterials.Add(newMaterial);
                    newMaterials[i] = newMaterial;
                }

                renderer.materials = newMaterials;
            }
        }
    }
}