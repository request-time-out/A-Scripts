// Decompiled with JetBrains decompiler
// Type: AIProject.Player.Photo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Cinemachine;
using Manager;
using System;
using UniRx;
using UnityEngine;

namespace AIProject.Player
{
  public class Photo : PlayerStateBase
  {
    private float _prevFieldOfView = 60f;
    private Subject<Unit> _onEndAction = new Subject<Unit>();
    private bool _prevCameraChangeable = true;
    private Vector3 _prevCameraLocalPosition = Vector3.get_zero();
    private Quaternion _prevCameraLocalRotation = Quaternion.get_identity();
    private Vector3 _offset = Vector3.get_zero();
    private LayerMask _cameraLayer = LayerMask.op_Implicit(0);
    private float _prevDutch;
    private ShotType _prevShotType;
    private bool _updatable;
    private bool _reversi;
    private float _prevScrollValue;
    private Transform _followRoot;
    private Camera _camera;
    private ActorCameraControl _actorCamera;

    protected override void OnAwake(PlayerActor player)
    {
      this._actorCamera = player.CameraControl;
      this._camera = this._actorCamera.CameraComponent;
      this._prevShotType = this._actorCamera.ShotType;
      if (this._prevShotType != ShotType.PointOfView)
        this._actorCamera.ShotType = ShotType.PointOfView;
      this._prevCameraChangeable = this._actorCamera.IsChangeable;
      if (this._prevCameraChangeable)
        this._actorCamera.IsChangeable = false;
      this._prevFieldOfView = (float) this._actorCamera.LensSetting.FieldOfView;
      this._prevDutch = (float) this._actorCamera.LensSetting.Dutch;
      this._followRoot = ((CinemachineVirtualCameraBase) this._actorCamera.ActiveFreeLookCamera)?.get_Follow();
      if (Object.op_Inequality((Object) this._followRoot, (Object) null))
      {
        this._prevCameraLocalPosition = this._followRoot.get_localPosition();
        this._prevCameraLocalRotation = this._followRoot.get_localRotation();
      }
      this._cameraLayer = Singleton<Resources>.Instance.DefinePack.MapDefines.MapLayer;
      MapUIContainer.SetCommandLabelAcception(CommandLabel.AcceptionState.None);
      MapUIContainer.SetVisibleHUD(false);
      if (Singleton<Resources>.IsInstance())
      {
        LocomotionProfile.LensSettings defaultLensSetting = Singleton<Resources>.Instance.LocomotionProfile.DefaultLensSetting;
        LocomotionProfile.PhotoShotSetting photoShot = Singleton<Resources>.Instance.LocomotionProfile.PhotoShot;
        MapUIContainer.PhotoShotUI.SetZoomValue(Mathf.InverseLerp(defaultLensSetting.MinFOV, defaultLensSetting.MaxFOV, this._prevFieldOfView));
      }
      else
        MapUIContainer.PhotoShotUI.SetZoomValue(0.5f);
      MapUIContainer.SetActivePhotoShotUI(true);
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) this._onEndAction, 1), (Action<M0>) (_ =>
      {
        this._updatable = false;
        player.PlayerController.ChangeState("Normal");
      }));
      this._updatable = true;
    }

    protected override void OnUpdate(PlayerActor player, ref Actor.InputInfo info)
    {
      if (!this._updatable || !Singleton<Input>.IsInstance() || (!Singleton<Resources>.IsInstance() || !Singleton<Game>.IsInstance()) || Object.op_Inequality((Object) Singleton<Game>.Instance.MapShortcutUI, (Object) null))
        return;
      float deltaTime = Time.get_deltaTime();
      Input instance = Singleton<Input>.Instance;
      LocomotionProfile locomotionProfile = Singleton<Resources>.Instance.LocomotionProfile;
      LocomotionProfile.PhotoShotSetting photoShot = locomotionProfile.PhotoShot;
      LocomotionProfile.LensSettings defaultLensSetting = locomotionProfile.DefaultLensSetting;
      float num1 = instance.ScrollValue();
      float num2 = (float) (((double) num1 + (double) this._prevScrollValue) / 2.0);
      this._prevScrollValue = num1;
      Transform transform = ((Component) this._actorCamera).get_transform();
      Vector2 moveAxis = instance.MoveAxis;
      Quaternion rotation = transform.get_rotation();
      Vector3 vector3 = Quaternion.op_Multiply(Quaternion.Euler(0.0f, (float) ((Quaternion) ref rotation).get_eulerAngles().y, 0.0f), Vector3.ClampMagnitude(new Vector3((float) moveAxis.x, 0.0f, (float) moveAxis.y), 1f));
      info.move = vector3;
      info.lookPos = Vector3.op_Addition(((Component) player).get_transform().get_position(), Vector3.op_Multiply(transform.get_forward(), 100f));
      if (instance.IsPressedKey((KeyCode) 324))
      {
        this._onEndAction.OnNext(Unit.get_Default());
      }
      else
      {
        if (instance.IsPressedKey((KeyCode) 325))
        {
          this.PlaySE(SoundPack.SystemSE.Photo);
          this._actorCamera.ScreenShot.Capture(string.Empty);
        }
        if (!Mathf.Approximately(num2, 0.0f))
        {
          float num3 = num2 * photoShot.mouseZoomScale * deltaTime;
          ref Vector3 local = ref this._offset;
          local.z = (__Null) (local.z + (double) num3 * (!this._reversi ? 1.0 : -1.0));
        }
        if (Object.op_Inequality((Object) this._followRoot, (Object) null) && Object.op_Inequality((Object) this._camera, (Object) null))
        {
          Vector3 offsetMoveValue = photoShot.offsetMoveValue;
          Vector3 maxOffset = photoShot.maxOffset;
          Vector3 minOffset = photoShot.minOffset;
          if (Input.GetKey((KeyCode) 273))
          {
            ref Vector3 local = ref this._offset;
            local.y = (__Null) (local.y + offsetMoveValue.y * (double) deltaTime);
          }
          if (Input.GetKey((KeyCode) 274))
          {
            ref Vector3 local = ref this._offset;
            local.y = (__Null) (local.y - offsetMoveValue.y * (double) deltaTime);
          }
          if (Input.GetKey((KeyCode) 275))
          {
            ref Vector3 local = ref this._offset;
            local.x = (__Null) (local.x + offsetMoveValue.x * (double) deltaTime);
          }
          if (Input.GetKey((KeyCode) 276))
          {
            ref Vector3 local = ref this._offset;
            local.x = (__Null) (local.x - offsetMoveValue.x * (double) deltaTime);
          }
          this._offset.x = (__Null) (double) Mathf.Clamp((float) this._offset.x, (float) minOffset.x, (float) maxOffset.x);
          this._offset.y = (__Null) (double) Mathf.Clamp((float) this._offset.y, (float) minOffset.y, (float) maxOffset.y);
          this._offset.z = (__Null) (double) Mathf.Clamp((float) this._offset.z, (float) minOffset.z, (float) maxOffset.z);
          this._followRoot.set_localPosition(this._prevCameraLocalPosition);
          Vector3 position = this._followRoot.get_position();
          Vector3 next = position;
          ref Vector3 local1 = ref next;
          local1.y = local1.y + this._offset.y;
          next = Vector3.op_Addition(next, Vector3.op_Multiply(((Component) this._camera).get_transform().get_right(), (float) this._offset.x));
          Vector3 current = next = this.CheckNextPosition(position, next, defaultLensSetting.NearClipPlane, this._cameraLayer);
          next = Vector3.op_Addition(next, Vector3.op_Multiply(((Component) this._camera).get_transform().get_forward(), (float) this._offset.z));
          this._followRoot.set_position(this.CheckNextPosition(current, next, defaultLensSetting.NearClipPlane, this._cameraLayer));
        }
        MapUIContainer.PhotoShotUI.SetZoomValue(Mathf.InverseLerp((float) photoShot.maxOffset.z, (float) photoShot.minOffset.z, (float) this._offset.z));
      }
    }

    protected override void OnAfterUpdate(PlayerActor player, Actor.InputInfo info)
    {
      player.CharacterTPS.UpdateState(info, ActorLocomotion.UpdateType.Update);
    }

    protected override void OnRelease(PlayerActor player)
    {
      if (Object.op_Inequality((Object) this._followRoot, (Object) null))
      {
        this._followRoot.set_localPosition(this._prevCameraLocalPosition);
        this._followRoot.set_localRotation(this._prevCameraLocalRotation);
      }
      if (Object.op_Inequality((Object) this._actorCamera, (Object) null))
      {
        LensSettings lensSetting = this._actorCamera.LensSetting;
        lensSetting.FieldOfView = (__Null) (double) this._prevFieldOfView;
        lensSetting.Dutch = (__Null) (double) this._prevDutch;
        this._actorCamera.LensSetting = lensSetting;
        if (this._prevCameraChangeable)
          this._actorCamera.IsChangeable = true;
        if (this._prevShotType != this._actorCamera.ShotType)
          this._actorCamera.ShotType = this._prevShotType;
      }
      MapUIContainer.SetActivePhotoShotUI(false);
      MapUIContainer.SetVisibleHUD(true);
    }

    private Vector3 CheckNextPosition(
      Vector3 current,
      Vector3 next,
      float nearClip,
      LayerMask layer)
    {
      Vector3 vector3 = Vector3.op_Subtraction(next, current);
      Vector3 normalized = ((Vector3) ref vector3).get_normalized();
      RaycastHit raycastHit;
      current = !Physics.Raycast(current, normalized, ref raycastHit, ((Vector3) ref vector3).get_magnitude() + nearClip, LayerMask.op_Implicit(this._cameraLayer)) ? next : Vector3.op_Subtraction(((RaycastHit) ref raycastHit).get_point(), Vector3.op_Multiply(normalized, nearClip));
      return current;
    }

    private void PlaySE(SoundPack.SystemSE se)
    {
      SoundPack soundPack = !Singleton<Resources>.IsInstance() ? (SoundPack) null : Singleton<Resources>.Instance.SoundPack;
      if (Object.op_Equality((Object) soundPack, (Object) null))
        return;
      soundPack.Play(se);
    }

    ~Photo()
    {
    }
  }
}
