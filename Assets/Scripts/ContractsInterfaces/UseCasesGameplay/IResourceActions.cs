#nullable enable

using Domain.Gameplay.Models;

namespace ContractsInterfaces.UseCasesGameplay
{
    /// <summary>
    /// Определяет контракт для всех действий, изменяющих ресурсы игрока.
    /// </summary>
    public interface IResourceActions
    {
        /// <summary>
        /// Проверяет, достаточно ли у игрока ресурсов для покрытия указанной стоимости.
        /// </summary>
        /// <param name="cost">Требуемая стоимость.</param>
        /// <returns>True, если ресурсов достаточно, иначе false.</returns>
        bool HasEnough(Cost cost);

        /// <summary>
        /// Списывает указанную стоимость с баланса игрока.
        /// Не выполняет проверку на достаточность ресурсов.
        /// </summary>
        /// <param name="cost">Стоимость для списания.</param>
        void Spend(Cost cost);

        /// <summary>
        /// Начисляет указанный доход на баланс игрока.
        /// </summary>
        /// <param name="income">Доход для начисления.</param>
        void Earn(Income income);
    }
}