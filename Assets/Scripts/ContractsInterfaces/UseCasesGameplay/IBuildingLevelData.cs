using Domain.Gameplay.Models;

namespace ContractsInterfaces.UseCasesGameplay
{
    /// <summary>Контракт, описывающий данные для одного уровня здания.</summary>
    public interface IBuildingLevelData
    {
        Cost Cost { get; }
        Income Income { get; }
    }
}