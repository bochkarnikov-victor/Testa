#nullable enable

using R3;
using System.Numerics;

namespace ContractsInterfaces.Infrastructure
{
    /// <summary>
    /// Интерфейс для сервиса, предоставляющего доступ к событиям ввода.
    /// </summary>
    public interface IInputService
    {
        // Общие действия
        /// <summary>Событие, вызываемое при нажатии кнопки удаления здания.</summary>
        ReadOnlyReactiveProperty<Unit> OnDeleteBuildingPressed { get; }

        /// <summary>Событие, вызываемое при нажатии кнопки размещения/основного действия.</summary>
        ReadOnlyReactiveProperty<Unit> OnPlace { get; }

        /// <summary>Событие, вызываемое при нажатии кнопки вращения.</summary>
        ReadOnlyReactiveProperty<Unit> OnRotate { get; }

        /// <summary>Событие, вызываемое при нажатии кнопки отмены/выхода.</summary>
        ReadOnlyReactiveProperty<Unit> OnCancel { get; }

        /// <summary>Событие, анонсирующее выбор типа здания по горячей клавише (1, 2, 3).</summary>
        ReadOnlyReactiveProperty<int> OnSelectBuildingRequested { get; }

        /// <summary>Текущая позиция курсора на экране.</summary>
        ReadOnlyReactiveProperty<Vector2> PointerPosition { get; }

        // Камера
        /// <summary>Вектор движения камеры (WASD/стрелки).</summary>
        ReadOnlyReactiveProperty<Vector2> MoveCameraDirection { get; }

        /// <summary>Дельта перемещения мыши при зажатой правой кнопке.</summary>
        ReadOnlyReactiveProperty<Vector2> CameraDragDelta { get; }

        /// <summary>Значение скролла колеса мыши для зума.</summary>
        ReadOnlyReactiveProperty<float> CameraZoomDelta { get; }
    }
}