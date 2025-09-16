#nullable enable

using System;
using ContractsInterfaces.Infrastructure;
using R3;
using UnityEngine.InputSystem;
using VContainer.Unity;
using NVec2 = System.Numerics.Vector2;
using UVec2 = UnityEngine.Vector2;

namespace Infrastructure
{
    /// <summary>
    /// Реализация сервиса ввода на новой Input System.
    /// </summary>
    public class InputService : IInputService, IInitializable, IDisposable
    {
        // === Input Actions ===
        private readonly InputActionMap _gameplay;
        private readonly InputAction _cameraMoveAction;
        private readonly InputAction _cameraDragAction;
        private readonly InputAction _rightClickAction;
        private readonly InputAction _cameraZoomAction;
        private readonly InputAction _placeAction;
        private readonly InputAction _rotateAction;
        private readonly InputAction _deleteBuildingAction;
        private readonly InputAction _cancelAction;
        private readonly InputAction _select1Action;
        private readonly InputAction _select2Action;
        private readonly InputAction _select3Action;
        private readonly InputAction _pointerPositionAction;

        // === Reactive Properties ===
        private readonly ReactiveProperty<Unit> _onDeleteBuildingPressed = new();

        /// <inheritdoc/>
        public ReadOnlyReactiveProperty<Unit> OnDeleteBuildingPressed => this._onDeleteBuildingPressed;

        private readonly ReactiveProperty<NVec2> _moveCameraDirection = new();

        /// <inheritdoc/>
        public ReadOnlyReactiveProperty<NVec2> MoveCameraDirection => this._moveCameraDirection;

        private readonly ReactiveProperty<Unit> _onPlace = new();

        /// <inheritdoc/>
        public ReadOnlyReactiveProperty<Unit> OnPlace => this._onPlace;

        private readonly ReactiveProperty<Unit> _onRotate = new();

        /// <inheritdoc/>
        public ReadOnlyReactiveProperty<Unit> OnRotate => this._onRotate;

        private readonly ReactiveProperty<Unit> _onCancel = new();

        /// <inheritdoc/>
        public ReadOnlyReactiveProperty<Unit> OnCancel => this._onCancel;

        private readonly ReactiveProperty<int> _onSelectBuildingRequested = new(-1);

        /// <inheritdoc/>
        public ReadOnlyReactiveProperty<int> OnSelectBuildingRequested => this._onSelectBuildingRequested;

        private readonly ReactiveProperty<NVec2> _cameraDragDelta = new();

        /// <inheritdoc/>
        public ReadOnlyReactiveProperty<NVec2> CameraDragDelta => this._cameraDragDelta;

        private readonly ReactiveProperty<float> _cameraZoomDelta = new();

        /// <inheritdoc/>
        public ReadOnlyReactiveProperty<float> CameraZoomDelta => this._cameraZoomDelta;

        private readonly ReactiveProperty<NVec2> _pointerPosition = new();

        /// <inheritdoc/>
        public ReadOnlyReactiveProperty<NVec2> PointerPosition => this._pointerPosition;

        private bool _isRightMouseHeld;

        public InputService(InputActionAsset inputAsset)
        {
            this._gameplay = inputAsset.FindActionMap("Gameplay", true);
            this._cameraMoveAction = this._gameplay.FindAction("CameraMove", true);
            this._cameraDragAction = this._gameplay.FindAction("CameraDrag", true);
            this._rightClickAction = this._gameplay.FindAction("RightClick", true);
            this._cameraZoomAction = this._gameplay.FindAction("CameraZoom", true);
            this._placeAction = this._gameplay.FindAction("Place", true);
            this._rotateAction = this._gameplay.FindAction("Rotate", true);
            this._deleteBuildingAction = this._gameplay.FindAction("DeleteBuilding", true);
            this._cancelAction = this._gameplay.FindAction("Cancel", true);
            this._select1Action = this._gameplay.FindAction("SelectBuilding1", true);
            this._select2Action = this._gameplay.FindAction("SelectBuilding2", true);
            this._select3Action = this._gameplay.FindAction("SelectBuilding3", true);
            this._pointerPositionAction = this._gameplay.FindAction("PointerPosition", true);
        }

        public void Initialize()
        {
            this._gameplay.Enable();

            this._cameraMoveAction.performed += this.OnMoveCameraPerformed;
            this._cameraMoveAction.canceled += this.OnMoveCameraCanceled;

            this._rightClickAction.started += this.OnRightClickStarted;
            this._rightClickAction.canceled += this.OnRightClickCanceled;
            this._cameraDragAction.performed += this.OnCameraDragPerformed;
            this._cameraDragAction.canceled += this.OnCameraDragCanceled;

            this._cameraZoomAction.performed += this.OnCameraZoomPerformed;
            this._cameraZoomAction.canceled += this.OnCameraZoomCanceled;

            this._placeAction.performed += this.OnPlacePerformed;
            this._rotateAction.performed += this.OnRotatePerformed;
            this._deleteBuildingAction.performed += this.OnDeleteBuildingPerformed;
            this._cancelAction.performed += this.OnCancelPerformed;

            this._select1Action.performed += this.OnSelect1Performed;
            this._select2Action.performed += this.OnSelect2Performed;
            this._select3Action.performed += this.OnSelect3Performed;

            this._pointerPositionAction.performed += this.OnPointerPositionPerformed;
        }

        private void OnMoveCameraPerformed(InputAction.CallbackContext ctx)
        {
            this._moveCameraDirection.Value = new NVec2(ctx.ReadValue<UVec2>().x, ctx.ReadValue<UVec2>().y);
        }

        private void OnMoveCameraCanceled(InputAction.CallbackContext _)
        {
            this._moveCameraDirection.Value = NVec2.Zero;
        }

        private void OnRightClickStarted(InputAction.CallbackContext _)
        {
            this._isRightMouseHeld = true;
        }

        private void OnRightClickCanceled(InputAction.CallbackContext _)
        {
            this._isRightMouseHeld = false;
            this._cameraDragDelta.Value = NVec2.Zero;
        }

        private void OnCameraDragPerformed(InputAction.CallbackContext ctx)
        {
            if (this._isRightMouseHeld)
            {
                this._cameraDragDelta.Value = new NVec2(ctx.ReadValue<UVec2>().x, ctx.ReadValue<UVec2>().y);
            }
        }

        private void OnCameraDragCanceled(InputAction.CallbackContext _)
        {
            this._cameraDragDelta.Value = NVec2.Zero;
        }

        private void OnCameraZoomPerformed(InputAction.CallbackContext ctx)
        {
            this._cameraZoomDelta.Value = ctx.ReadValue<UVec2>().y;
        }

        private void OnCameraZoomCanceled(InputAction.CallbackContext _)
        {
            this._cameraZoomDelta.Value = 0f;
        }

        private void OnPlacePerformed(InputAction.CallbackContext _)
        {
            this._onPlace.Value = Unit.Default;
        }

        private void OnRotatePerformed(InputAction.CallbackContext _)
        {
            this._onRotate.Value = Unit.Default;
        }

        private void OnDeleteBuildingPerformed(InputAction.CallbackContext _)
        {
            this._onDeleteBuildingPressed.Value = Unit.Default;
        }

        private void OnCancelPerformed(InputAction.CallbackContext _)
        {
            this._onCancel.Value = Unit.Default;
        }

        private void OnSelect1Performed(InputAction.CallbackContext _)
        {
            this._onSelectBuildingRequested.Value = 1;
        }

        private void OnSelect2Performed(InputAction.CallbackContext _)
        {
            this._onSelectBuildingRequested.Value = 2;
        }

        private void OnSelect3Performed(InputAction.CallbackContext _)
        {
            this._onSelectBuildingRequested.Value = 3;
        }

        private void OnPointerPositionPerformed(InputAction.CallbackContext ctx)
        {
            this._pointerPosition.Value = new NVec2(ctx.ReadValue<UVec2>().x, ctx.ReadValue<UVec2>().y);
        }

        public void Dispose()
        {
            this._gameplay?.Disable();

            this._cameraMoveAction.performed -= this.OnMoveCameraPerformed;
            this._cameraMoveAction.canceled -= this.OnMoveCameraCanceled;

            this._rightClickAction.started -= this.OnRightClickStarted;
            this._rightClickAction.canceled -= this.OnRightClickCanceled;
            this._cameraDragAction.performed -= this.OnCameraDragPerformed;
            this._cameraDragAction.canceled -= this.OnCameraDragCanceled;

            this._cameraZoomAction.performed -= this.OnCameraZoomPerformed;
            this._cameraZoomAction.canceled -= this.OnCameraZoomCanceled;

            this._placeAction.performed -= this.OnPlacePerformed;
            this._rotateAction.performed -= this.OnRotatePerformed;
            this._deleteBuildingAction.performed -= this.OnDeleteBuildingPerformed;
            this._cancelAction.performed -= this.OnCancelPerformed;

            this._select1Action.performed -= this.OnSelect1Performed;
            this._select2Action.performed -= this.OnSelect2Performed;
            this._select3Action.performed -= this.OnSelect3Performed;

            this._pointerPositionAction.performed -= this.OnPointerPositionPerformed;

            this._onDeleteBuildingPressed?.Dispose();
            this._moveCameraDirection?.Dispose();
            this._onPlace?.Dispose();
            this._onRotate?.Dispose();
            this._onCancel?.Dispose();
            this._onSelectBuildingRequested?.Dispose();
            this._cameraDragDelta?.Dispose();
            this._cameraZoomDelta?.Dispose();
            this._pointerPosition?.Dispose();
        }
    }
}