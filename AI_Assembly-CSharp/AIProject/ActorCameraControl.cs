// Decompiled with JetBrains decompiler
// Type: AIProject.ActorCameraControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Scene;
using AIProject.UI;
using Cinemachine;
using Cinemachine.Utility;
using ConfigScene;
using Illusion.Extensions;
using LuxWater;
using Manager;
using PlaceholderSoftware.WetStuff;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace AIProject
{
  [RequireComponent(typeof (CinemachineBrain))]
  public class ActorCameraControl : SerializedMonoBehaviour, IActionCommand
  {
    private static Dictionary<CameraMode, Action<ActorCameraControl, CameraMode, ShotType>> _activationEventTable = new Dictionary<CameraMode, Action<ActorCameraControl, CameraMode, ShotType>>()
    {
      [CameraMode.Normal] = (Action<ActorCameraControl, CameraMode, ShotType>) ((x, y, z) =>
      {
        using (Dictionary<ShotType, CinemachineVirtualCameraBase>.Enumerator enumerator = x._virtualCameraNormalTable.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<ShotType, CinemachineVirtualCameraBase> current = enumerator.Current;
            ((Behaviour) current.Value).set_enabled(y == CameraMode.Normal && current.Key == z);
          }
        }
      }),
      [CameraMode.ActionFreeLook] = (Action<ActorCameraControl, CameraMode, ShotType>) ((x, y, z) =>
      {
        if (x._virtualCameraActionTable == null)
          return;
        using (Dictionary<ShotType, CinemachineVirtualCameraBase>.Enumerator enumerator = x._virtualCameraActionTable.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<ShotType, CinemachineVirtualCameraBase> current = enumerator.Current;
            ((Behaviour) current.Value).set_enabled(y == CameraMode.ActionFreeLook && current.Key == z);
          }
        }
      }),
      [CameraMode.Event] = (Action<ActorCameraControl, CameraMode, ShotType>) ((x, y, z) =>
      {
        if (!Object.op_Inequality((Object) x._eventCamera, (Object) null))
          return;
        ((Behaviour) x._eventCamera).set_enabled(y == CameraMode.Event);
      }),
      [CameraMode.H] = (Action<ActorCameraControl, CameraMode, ShotType>) ((x, y, z) =>
      {
        if (!Object.op_Inequality((Object) x._hCamera, (Object) null))
          return;
        ((Behaviour) x._eventCamera).set_enabled(y == CameraMode.H);
      })
    };
    [SerializeField]
    private CinemachineBrain _brain;
    [SerializeField]
    private CameraConfig _cameraConfig;
    private Camera _cameraComponent;
    [SerializeField]
    private GameScreenShot _screenShot;
    [SerializeField]
    private Dictionary<ShotType, CinemachineVirtualCameraBase> _virtualCameraNormalTable;
    [SerializeField]
    private Dictionary<ShotType, CinemachineVirtualCameraBase> _virtualCameraActionTable;
    [SerializeField]
    private CinemachineVirtualCameraBase _actionCameraNotMove;
    [SerializeField]
    private CinemachineVirtualCameraBase _advCamera;
    [SerializeField]
    private CinemachineVirtualCameraBase _advNotStandCamera;
    [SerializeField]
    private CinemachineVirtualCameraBase _eventCamera;
    [SerializeField]
    private CinemachineVirtualCameraBase _hCamera;
    [SerializeField]
    private CinemachineVirtualCameraBase _craftingCamera;
    [SerializeField]
    private Transform _advNotStandRoot;
    [SerializeField]
    private Animator _eventCameraLocator;
    [SerializeField]
    private Transform _eventCameraParent;
    [SerializeField]
    private Transform _virtualCameraRoot;
    [SerializeField]
    private CrossFade _crossFade;
    [SerializeField]
    private VanishControl _vanishControl;
    [SerializeField]
    private ActorCameraControl.LocomotionSettingData _locomotionSetting;
    [SerializeField]
    private CinemachineVirtualCameraBase _activeVirtualCamera;
    private Dictionary<ShotType, CinemachineVirtualCameraBase> _activeCameraTable;
    [SerializeField]
    private CustomAxisState _yAxis;
    [SerializeField]
    private CustomAxisState.Recentering _yAxisRecentering;
    [SerializeField]
    private CustomAxisState _xAxis;
    [SerializeField]
    private CustomAxisState.Recentering _recenterToTargetHeading;
    [SerializeField]
    private LensSettings _lensSetting;
    private LocomotionProfile.LensSettings _defaultLensSetting;
    [SerializeField]
    private GameObject _charaLightNormal;
    [SerializeField]
    private GameObject _charaLightCustom;
    private Light _enviroLight;
    public bool _updateCustomLight;
    [SerializeField]
    private Light _normalKeyLight;
    [SerializeField]
    private Light _customKeyLight;
    private List<KeyCodeDownCommand> _keyCommands;
    private List<KeyCodeCommand> _keyDownCommands;
    private Subject<bool> _isBlendingChange;
    private LuxWater_UnderWaterRendering _underWaterFX;
    private LuxWater_UnderWaterBlur _underWaterBlurFX;
    private PlaceholderSoftware.WetStuff.WetStuff _wetStuff;
    [SerializeField]
    private WetDecal _wetDecal;
    [SerializeField]
    private Vector3 _wetDecalOffset;
    private bool _prevAmb;
    private Transform _prevTarget;
    private Vector3 _lastTargetPosition;
    private Rigidbody _targetRigidbody;
    private ActorCameraControl.HeadingTracker _headingTracker;
    private CameraMode _mode;
    private ShotType _shotType;
    private ShotType _recovShotType;
    private Dictionary<ShotType, Transform> _oldNormalLookAtTransform;
    public ActionCameraData _actionCameraData;
    private Vector3 prevCamPos;
    private Quaternion prevCamrot;

    public ActorCameraControl()
    {
      base.\u002Ector();
    }

    public CinemachineBrain CinemachineBrain
    {
      get
      {
        return Object.op_Equality((Object) this._brain, (Object) null) ? (this._brain = (CinemachineBrain) ((Component) this).GetComponent<CinemachineBrain>()) : this._brain;
      }
    }

    public CameraConfig CameraConfig
    {
      get
      {
        return this._cameraConfig;
      }
    }

    public Camera CameraComponent
    {
      get
      {
        return Object.op_Equality((Object) this._cameraComponent, (Object) null) ? (this._cameraComponent = (Camera) ((Component) this).GetComponent<Camera>()) : this._cameraComponent;
      }
    }

    public GameScreenShot ScreenShot
    {
      get
      {
        return this._screenShot;
      }
    }

    public CinemachineVirtualCameraBase ActionCameraNotMove
    {
      get
      {
        return this._actionCameraNotMove;
      }
      set
      {
        this._actionCameraNotMove = value;
      }
    }

    public CinemachineVirtualCameraBase ADVCamera
    {
      get
      {
        return this._advCamera;
      }
      set
      {
        this._advCamera = value;
      }
    }

    public CinemachineVirtualCameraBase ADVNotStandCamera
    {
      get
      {
        return this._advNotStandCamera;
      }
      set
      {
        this._advNotStandCamera = value;
      }
    }

    public CinemachineVirtualCameraBase EventCamera
    {
      get
      {
        return this._eventCamera;
      }
      set
      {
        this._eventCamera = value;
      }
    }

    public CinemachineVirtualCameraBase HCamera
    {
      get
      {
        return this._hCamera;
      }
      set
      {
        this._hCamera = value;
      }
    }

    public CinemachineVirtualCameraBase FishingCamera { get; set; }

    public Transform ADVNotStandRoot
    {
      get
      {
        return this._advNotStandRoot;
      }
    }

    public Animator EventCameraLocator
    {
      get
      {
        return this._eventCameraLocator;
      }
      set
      {
        this._eventCameraLocator = value;
      }
    }

    public Transform EventCameraParent
    {
      get
      {
        return this._eventCameraParent;
      }
    }

    public Transform VirtualCameraRoot
    {
      get
      {
        return this._virtualCameraRoot;
      }
    }

    public CrossFade CrossFade
    {
      get
      {
        return this._crossFade;
      }
    }

    public VanishControl VanishControl
    {
      get
      {
        return this._vanishControl;
      }
    }

    public ActorCameraControl.LocomotionSettingData LocomotionSetting
    {
      get
      {
        return this._locomotionSetting;
      }
      set
      {
        this._locomotionSetting = value;
      }
    }

    public CinemachineFreeLook ActiveFreeLookCamera
    {
      get
      {
        return this._activeVirtualCamera as CinemachineFreeLook;
      }
    }

    public CinemachineVirtualCamera ActiveVirtualCamera
    {
      get
      {
        return this._activeVirtualCamera as CinemachineVirtualCamera;
      }
    }

    public bool IsChangeable { get; set; }

    public float XAxisValue
    {
      get
      {
        return this._xAxis.value;
      }
      set
      {
        if ((double) value > (double) this._xAxis.maxSpeed || (double) value < (double) this._xAxis.minValue)
        {
          if (this._xAxis.wrap)
          {
            if ((double) value > (double) this._xAxis.maxValue)
              this._xAxis.value = this._xAxis.minValue + (value - this._xAxis.maxValue);
            else
              this._xAxis.value = this._xAxis.maxValue + (value - this._xAxis.minValue);
          }
          else
            this._xAxis.value = Mathf.Clamp(value, this._xAxis.minValue, this._xAxis.maxValue);
        }
        else
          this._xAxis.value = value;
      }
    }

    public float YAxisValue
    {
      get
      {
        return this._yAxis.value;
      }
      set
      {
        this._yAxis.value = Mathf.Clamp(value, this._yAxis.minValue, this._yAxis.maxValue);
      }
    }

    public LensSettings LensSetting
    {
      get
      {
        return this._lensSetting;
      }
      set
      {
        this._lensSetting = value;
      }
    }

    public bool AmbientLight
    {
      set
      {
        if (Object.op_Inequality((Object) this._charaLightNormal, (Object) null))
          this._charaLightNormal.SetActiveIfDifferent(value);
        if (Object.op_Inequality((Object) this._charaLightCustom, (Object) null))
          this._charaLightCustom.SetActiveIfDifferent(!value);
        if (this._prevAmb != value && Singleton<Manager.Map>.IsInstance() && (Object.op_Inequality((Object) Singleton<Manager.Map>.Instance.Simulator, (Object) null) && Object.op_Inequality((Object) Singleton<Manager.Map>.Instance.Simulator.EnviroSky, (Object) null)))
        {
          Light component = (Light) ((Component) Singleton<Manager.Map>.Instance.Simulator.EnviroSky.Components.DirectLight).GetComponent<Light>();
          if (value)
            component.set_cullingMask(LayerMask.op_Implicit(Singleton<Resources>.Instance.DefinePack.MapDefines.EnvLightCulMask));
          else
            component.set_cullingMask(LayerMask.op_Implicit(Singleton<Resources>.Instance.DefinePack.MapDefines.EnvLightCulMaskCustom));
        }
        this._prevAmb = value;
      }
    }

    private Light EnviroLight
    {
      get
      {
        if (!Singleton<Manager.Map>.IsInstance())
          return (Light) null;
        if (Object.op_Equality((Object) this._enviroLight, (Object) null))
        {
          EnviroSky enviroSky = Singleton<Manager.Map>.Instance.Simulator.EnviroSky;
          if (Object.op_Inequality((Object) enviroSky, (Object) null))
            this._enviroLight = (Light) ((Component) enviroSky.Components.DirectLight).GetComponent<Light>();
        }
        return this._enviroLight;
      }
    }

    public void EnableUpdateCustomLight()
    {
      this._updateCustomLight = true;
    }

    public void DisableUpdateCustomLight()
    {
      this._updateCustomLight = false;
    }

    public Light NormalKeyLight
    {
      get
      {
        return this._normalKeyLight;
      }
    }

    public Light CustomKeyLight
    {
      get
      {
        return this._customKeyLight;
      }
    }

    public Action OnCameraBlended { get; set; }

    public LuxWater_UnderWaterRendering UnderWaterFX
    {
      get
      {
        if (Object.op_Equality((Object) this._underWaterFX, (Object) null))
          this._underWaterFX = (LuxWater_UnderWaterRendering) ((Component) this).GetComponent<LuxWater_UnderWaterRendering>();
        return this._underWaterFX;
      }
    }

    public LuxWater_UnderWaterBlur UnderWaterBlurFX
    {
      get
      {
        if (Object.op_Equality((Object) this._underWaterBlurFX, (Object) null))
          this._underWaterBlurFX = (LuxWater_UnderWaterBlur) ((Component) this).GetComponent<LuxWater_UnderWaterBlur>();
        return this._underWaterBlurFX;
      }
    }

    public PlaceholderSoftware.WetStuff.WetStuff WetStuff
    {
      get
      {
        if (Object.op_Equality((Object) this._wetStuff, (Object) null))
          this._wetStuff = (PlaceholderSoftware.WetStuff.WetStuff) ((Component) this).GetComponent<PlaceholderSoftware.WetStuff.WetStuff>();
        return this._wetStuff;
      }
    }

    public WetDecal WetDecal
    {
      get
      {
        return this._wetDecal;
      }
    }

    public Vector3 WetDecalOffset
    {
      get
      {
        return this._wetDecalOffset;
      }
      set
      {
        this._wetDecalOffset = value;
      }
    }

    private void Start()
    {
      this._wetStuff = (PlaceholderSoftware.WetStuff.WetStuff) ((Component) this).GetComponent<PlaceholderSoftware.WetStuff.WetStuff>();
      ObservableExtensions.Subscribe<bool>(Observable.Where<bool>(Observable.DistinctUntilChanged<bool>(Observable.TakeUntilDestroy<bool>((IObservable<M0>) this._isBlendingChange, ((Component) this).get_gameObject())), (Func<M0, bool>) (isOn => !isOn)), (Action<M0>) (_ =>
      {
        if (this.OnCameraBlended == null)
          return;
        this.OnCameraBlended();
        this.OnCameraBlended = (Action) null;
      }));
      using (Dictionary<ShotType, CinemachineVirtualCameraBase>.Enumerator enumerator = this._virtualCameraNormalTable.GetEnumerator())
      {
        while (enumerator.MoveNext())
          ((Behaviour) enumerator.Current.Value).set_enabled(false);
      }
      using (Dictionary<ShotType, CinemachineVirtualCameraBase>.Enumerator enumerator = this._virtualCameraActionTable.GetEnumerator())
      {
        while (enumerator.MoveNext())
          ((Behaviour) enumerator.Current.Value).set_enabled(false);
      }
      ((Behaviour) (this._activeVirtualCamera = (this._activeCameraTable = this._virtualCameraNormalTable)[ShotType.Near])).set_enabled(true);
      KeyCodeDownCommand keyCodeDownCommand1 = new KeyCodeDownCommand();
      keyCodeDownCommand1.KeyCode = (KeyCode) 49;
      // ISSUE: method pointer
      keyCodeDownCommand1.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__2)));
      KeyCodeDownCommand keyCodeDownCommand2 = new KeyCodeDownCommand();
      keyCodeDownCommand2.KeyCode = (KeyCode) 50;
      // ISSUE: method pointer
      keyCodeDownCommand2.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__3)));
      KeyCodeDownCommand keyCodeDownCommand3 = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 59
      };
      // ISSUE: method pointer
      keyCodeDownCommand3.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__4)));
      KeyCodeDownCommand keyCodeDownCommand4 = new KeyCodeDownCommand()
      {
        KeyCode = (KeyCode) 47
      };
      // ISSUE: method pointer
      keyCodeDownCommand4.TriggerEvent.AddListener(new UnityAction((object) this, __methodptr(\u003CStart\u003Em__5)));
      KeyCodeCommand keyCodeCommand1 = new KeyCodeCommand()
      {
        KeyCode = (KeyCode) 61
      };
      // ISSUE: method pointer
      keyCodeCommand1.TriggerEvent.AddListener(new UnityAction<bool>((object) this, __methodptr(\u003CStart\u003Em__6)));
      KeyCodeCommand keyCodeCommand2 = new KeyCodeCommand()
      {
        KeyCode = (KeyCode) 93
      };
      // ISSUE: method pointer
      keyCodeCommand2.TriggerEvent.AddListener(new UnityAction<bool>((object) this, __methodptr(\u003CStart\u003Em__7)));
      KeyCodeCommand keyCodeCommand3 = new KeyCodeCommand()
      {
        KeyCode = (KeyCode) 46
      };
      // ISSUE: method pointer
      keyCodeCommand3.TriggerEvent.AddListener(new UnityAction<bool>((object) this, __methodptr(\u003CStart\u003Em__8)));
      KeyCodeCommand keyCodeCommand4 = new KeyCodeCommand()
      {
        KeyCode = (KeyCode) 92
      };
      // ISSUE: method pointer
      keyCodeCommand4.TriggerEvent.AddListener(new UnityAction<bool>((object) this, __methodptr(\u003CStart\u003Em__9)));
      this._keyCommands.Add(keyCodeDownCommand1);
      this._keyCommands.Add(keyCodeDownCommand2);
      this._keyCommands.Add(keyCodeDownCommand3);
      this._keyCommands.Add(keyCodeDownCommand4);
      this._keyDownCommands.Add(keyCodeCommand1);
      this._keyDownCommands.Add(keyCodeCommand2);
      this._keyDownCommands.Add(keyCodeCommand3);
      this._keyDownCommands.Add(keyCodeCommand4);
      if (Singleton<Manager.Map>.IsInstance() && Object.op_Inequality((Object) Singleton<Manager.Map>.Instance.Simulator, (Object) null) && Object.op_Inequality((Object) Singleton<Manager.Map>.Instance.Simulator.EnviroSky, (Object) null))
      {
        Light component = (Light) ((Component) Singleton<Manager.Map>.Instance.Simulator.EnviroSky.Components.DirectLight).GetComponent<Light>();
        if (Manager.Config.GraphicData.AmbientLight)
          component.set_cullingMask(LayerMask.op_Implicit(Singleton<Resources>.Instance.DefinePack.MapDefines.EnvLightCulMask));
        else
          component.set_cullingMask(LayerMask.op_Implicit(Singleton<Resources>.Instance.DefinePack.MapDefines.EnvLightCulMaskCustom));
      }
      this._prevAmb = Manager.Config.GraphicData.AmbientLight;
    }

    public bool EnabledInput { get; set; }

    public void OnUpdateInput()
    {
      Input instance = Singleton<Input>.Instance;
      foreach (CommandDataBase keyCommand in this._keyCommands)
        keyCommand.Invoke(instance);
      foreach (CommandDataBase keyDownCommand in this._keyDownCommands)
        keyDownCommand.Invoke(instance);
    }

    private void Update()
    {
      if (Singleton<Manager.Scene>.Instance.IsNowLoadingFade)
        ((Behaviour) this.CinemachineBrain).set_enabled(false);
      else if (!((Behaviour) this.CinemachineBrain).get_enabled() && Singleton<MapScene>.IsInstance())
        ((Behaviour) this.CinemachineBrain).set_enabled(!Singleton<MapScene>.Instance.IsLoading);
      this.AmbientLight = Manager.Config.GraphicData.AmbientLight;
      if (this._updateCustomLight)
      {
        Light enviroLight = this.EnviroLight;
        if (Object.op_Inequality((Object) enviroLight, (Object) null))
        {
          this._customKeyLight.set_color(enviroLight.get_color());
          this._customKeyLight.set_intensity(enviroLight.get_intensity());
        }
      }
      if (this.Mode != CameraMode.Normal && this.Mode != CameraMode.ActionFreeLook)
        return;
      LocomotionProfile locomotionProfile = Singleton<Resources>.Instance.LocomotionProfile;
      Vector2 defaultCameraAxisPow = locomotionProfile.DefaultCameraAxisPow;
      ActionSystem actData = Manager.Config.ActData;
      if (this.ShotType == ShotType.PointOfView)
      {
        float t1 = Mathf.InverseLerp(0.0f, 100f, (float) actData.FPSSensitivityX);
        float t2 = Mathf.InverseLerp(0.0f, 100f, (float) actData.FPSSensitivityY);
        this._xAxis.maxSpeed = locomotionProfile.CameraPowX.Lerp(t1);
        this._yAxis.maxSpeed = locomotionProfile.CameraPowY.Lerp(t2);
      }
      else
      {
        float t1 = Mathf.InverseLerp(0.0f, 100f, (float) actData.TPSSensitivityX);
        float t2 = Mathf.InverseLerp(0.0f, 100f, (float) actData.TPSSensitivityY);
        this._xAxis.maxSpeed = locomotionProfile.CameraPowX.Lerp(t1);
        this._yAxis.maxSpeed = locomotionProfile.CameraPowY.Lerp(t2);
      }
      this._xAxis.invertInput = actData.InvertMoveX;
      this._yAxis.invertInput = !actData.InvertMoveY;
      Vector3 cameraAccelRate = locomotionProfile.CameraAccelRate;
      this._xAxis.accelTime = (float) cameraAccelRate.x;
      this._yAxis.accelTime = (float) cameraAccelRate.y;
    }

    private void LateUpdate()
    {
      this.UpdateLensSetting();
      if (Singleton<Manager.Scene>.Instance.IsNowLoadingFade)
        return;
      if (this._isBlendingChange != null)
        this._isBlendingChange.OnNext(this._brain.get_IsBlending());
      if (Object.op_Inequality((Object) this._vanishControl, (Object) null))
      {
        ((Component) this._vanishControl).get_transform().set_position(((Component) this._activeVirtualCamera).get_transform().get_position());
        ((Component) this._vanishControl).get_transform().set_rotation(((Component) this._activeVirtualCamera).get_transform().get_rotation());
      }
      if (this._brain.get_IsBlending() || !(this._activeVirtualCamera is CinemachineFreeLook))
        return;
      bool flag = false;
      float smoothDeltaTime = Time.get_smoothDeltaTime();
      if (this.EnabledInput && ((double) smoothDeltaTime >= 0.0 && CinemachineCore.get_Instance().IsLive((ICinemachineCamera) this._activeVirtualCamera)))
      {
        if (this._yAxis.Update(smoothDeltaTime))
          this._yAxisRecentering.CancelRecentering();
        flag = this._xAxis.Update(smoothDeltaTime);
      }
      CinemachineFreeLook activeVirtualCamera = this._activeVirtualCamera as CinemachineFreeLook;
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      (^(AxisState&) ref activeVirtualCamera.m_YAxis).m_MaxValue = (__Null) (double) this._yAxis.maxValue;
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      (^(AxisState&) ref activeVirtualCamera.m_YAxis).m_MinValue = (__Null) (double) this._yAxis.minValue;
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      (^(AxisState&) ref activeVirtualCamera.m_YAxis).Value = (__Null) (double) this._yAxis.value;
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      (^(AxisState&) ref activeVirtualCamera.m_XAxis).Value = (__Null) (double) this._xAxis.value;
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      (^(AxisState.Recentering&) ref activeVirtualCamera.m_RecenterToTargetHeading).m_enabled = (__Null) (this._recenterToTargetHeading.enabled ? 1 : 0);
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      (^(AxisState.Recentering&) ref activeVirtualCamera.m_RecenterToTargetHeading).m_RecenteringTime = (__Null) (double) this._recenterToTargetHeading.recenteringTime;
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      (^(AxisState.Recentering&) ref activeVirtualCamera.m_RecenterToTargetHeading).m_WaitTime = (__Null) (double) this._recenterToTargetHeading.waitTime;
      CinemachineOrbitalTransposer orbitalTransposer1 = (CinemachineOrbitalTransposer) null;
      if (flag)
      {
        orbitalTransposer1 = (CinemachineOrbitalTransposer) this.GetLiveChildOrSelf(activeVirtualCamera).GetCinemachineComponent<CinemachineOrbitalTransposer>();
        if (Object.op_Inequality((Object) orbitalTransposer1, (Object) null))
          ((AxisState.Recentering) ref orbitalTransposer1.m_RecenterToTargetHeading).CancelRecentering();
      }
      if (!Object.op_Inequality((Object) orbitalTransposer1, (Object) null))
        return;
      if (((CinemachineTransposer) orbitalTransposer1).m_BindingMode != 5)
      {
        double num1 = (double) this._xAxis.value;
        CinemachineOrbitalTransposer orbitalTransposer2 = orbitalTransposer1;
        CameraState state = this._activeVirtualCamera.get_State();
        Vector3 referenceUp = ((CameraState) ref state).get_ReferenceUp();
        Quaternion referenceOrientation = ((CinemachineTransposer) orbitalTransposer2).GetReferenceOrientation(referenceUp);
        double num2 = (double) smoothDeltaTime;
        CinemachineOrbitalTransposer transposer = orbitalTransposer1;
        float targetHeading = this.GetTargetHeading((float) num1, referenceOrientation, (float) num2, transposer);
        AxisState xaxis = (AxisState) activeVirtualCamera.m_XAxis;
        ((AxisState.Recentering) ref orbitalTransposer1.m_RecenterToTargetHeading).DoRecentering(ref xaxis, smoothDeltaTime, targetHeading);
      }
      if (((CinemachineTransposer) orbitalTransposer1).m_BindingMode != 5)
        return;
      this._xAxis.value = 0.0f;
    }

    private void UpdateLensSetting()
    {
      if (this.Mode == CameraMode.ADV || this.Mode == CameraMode.ADVExceptStand || this.Mode == CameraMode.Event)
        return;
      if (this._activeVirtualCamera is CinemachineVirtualCamera)
      {
        CinemachineVirtualCamera activeVirtualCamera = this._activeVirtualCamera as CinemachineVirtualCamera;
        LensSettings lens = (LensSettings) activeVirtualCamera.m_Lens;
        if (this._lensSetting.FieldOfView != lens.FieldOfView)
        {
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          (^(LensSettings&) ref activeVirtualCamera.m_Lens).FieldOfView = this._lensSetting.FieldOfView;
        }
        if (this._lensSetting.NearClipPlane != lens.NearClipPlane)
        {
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          (^(LensSettings&) ref activeVirtualCamera.m_Lens).NearClipPlane = this._lensSetting.NearClipPlane;
        }
        if (this._lensSetting.FarClipPlane != lens.FarClipPlane)
        {
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          (^(LensSettings&) ref activeVirtualCamera.m_Lens).FarClipPlane = this._lensSetting.FarClipPlane;
        }
        if (this._lensSetting.Dutch == lens.Dutch)
          return;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        (^(LensSettings&) ref activeVirtualCamera.m_Lens).Dutch = this._lensSetting.Dutch;
      }
      else
      {
        if (!(this._activeVirtualCamera is CinemachineFreeLook))
          return;
        CinemachineFreeLook activeVirtualCamera = this._activeVirtualCamera as CinemachineFreeLook;
        LensSettings lens = (LensSettings) activeVirtualCamera.m_Lens;
        if (this._lensSetting.FieldOfView != lens.FieldOfView)
        {
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          (^(LensSettings&) ref activeVirtualCamera.m_Lens).FieldOfView = this._lensSetting.FieldOfView;
        }
        if (this._lensSetting.NearClipPlane != lens.NearClipPlane)
        {
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          (^(LensSettings&) ref activeVirtualCamera.m_Lens).NearClipPlane = this._lensSetting.NearClipPlane;
        }
        if (this._lensSetting.FarClipPlane != lens.FarClipPlane)
        {
          // ISSUE: cast to a reference type
          // ISSUE: explicit reference operation
          (^(LensSettings&) ref activeVirtualCamera.m_Lens).FarClipPlane = this._lensSetting.FarClipPlane;
        }
        if (this._lensSetting.Dutch == lens.Dutch)
          return;
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        (^(LensSettings&) ref activeVirtualCamera.m_Lens).Dutch = this._lensSetting.Dutch;
      }
    }

    private CinemachineVirtualCamera GetLiveChildOrSelf(
      CinemachineFreeLook freeLook)
    {
      float yaxisValue = this.GetYAxisValue(freeLook);
      if ((double) yaxisValue < 0.330000013113022)
        return freeLook.GetRig(2);
      return (double) yaxisValue > 0.660000026226044 ? freeLook.GetRig(0) : freeLook.GetRig(1);
    }

    private float GetYAxisValue(CinemachineFreeLook freeLook)
    {
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      float num = (float) ((^(AxisState&) ref freeLook.m_YAxis).m_MaxValue - (^(AxisState&) ref freeLook.m_YAxis).m_MinValue);
      // ISSUE: cast to a reference type
      // ISSUE: explicit reference operation
      return (double) num > 9.99999974737875E-05 ? (float) (^(AxisState&) ref freeLook.m_YAxis).Value / num : 0.5f;
    }

    private float GetTargetHeading(
      float currentHeading,
      Quaternion targetOrientation,
      float deltaTime,
      CinemachineOrbitalTransposer transposer)
    {
      float num;
      if (((CinemachineTransposer) transposer).m_BindingMode == 5)
        num = 0.0f;
      else if (Object.op_Equality((Object) ((CinemachineComponentBase) transposer).get_FollowTarget(), (Object) null))
      {
        num = currentHeading;
      }
      else
      {
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        CinemachineOrbitalTransposer.Heading.HeadingDefinition headingDefinition = (CinemachineOrbitalTransposer.Heading.HeadingDefinition) (^(CinemachineOrbitalTransposer.Heading&) ref transposer.m_Heading).m_Definition;
        if (Object.op_Inequality((Object) this._prevTarget, (Object) ((CinemachineComponentBase) transposer).get_FollowTarget()))
        {
          this._prevTarget = ((CinemachineComponentBase) transposer).get_FollowTarget();
          this._targetRigidbody = this._prevTarget != null ? (Rigidbody) ((Component) this._prevTarget).GetComponent<Rigidbody>() : (Rigidbody) null;
          Vector3? nullable = this._prevTarget != null ? new Vector3?(this._prevTarget.get_position()) : new Vector3?();
          this._lastTargetPosition = !nullable.HasValue ? Vector3.get_zero() : nullable.Value;
        }
        if (headingDefinition == 1 && Object.op_Equality((Object) this._targetRigidbody, (Object) null))
          headingDefinition = (CinemachineOrbitalTransposer.Heading.HeadingDefinition) 0;
        Vector3 vector3_1 = Vector3.get_zero();
        switch ((int) headingDefinition)
        {
          case 0:
            vector3_1 = Vector3.op_Subtraction(((CinemachineComponentBase) transposer).get_FollowTargetPosition(), this._lastTargetPosition);
            break;
          case 1:
            vector3_1 = this._targetRigidbody.get_velocity();
            break;
          case 2:
            vector3_1 = Quaternion.op_Multiply(((CinemachineComponentBase) transposer).get_FollowTargetRotation(), Vector3.get_forward());
            break;
          case 3:
            return 0.0f;
        }
        // ISSUE: cast to a reference type
        // ISSUE: explicit reference operation
        int filterSize = (^(CinemachineOrbitalTransposer.Heading&) ref transposer.m_Heading).m_VelocityFilterStrength * 5;
        if (this._headingTracker == null || this._headingTracker.FilterSize != filterSize)
          this._headingTracker = new ActorCameraControl.HeadingTracker(filterSize);
        this._headingTracker.DecayHistory();
        Vector3 vector3_2 = Quaternion.op_Multiply(targetOrientation, Vector3.get_up());
        Vector3 vector3_3 = UnityVectorExtensions.ProjectOntoPlane(vector3_1, vector3_2);
        num = UnityVectorExtensions.AlmostZero(vector3_3) ? currentHeading : UnityVectorExtensions.SignedAngle(Quaternion.op_Multiply(targetOrientation, Vector3.get_forward()), vector3_3, vector3_2);
      }
      return num;
    }

    public CameraMode Mode
    {
      get
      {
        return this._mode;
      }
      set
      {
        if (this._mode == value)
          return;
        this._mode = value;
        CinemachineVirtualCameraBase virtualCameraBase1 = (CinemachineVirtualCameraBase) null;
        switch (value)
        {
          case CameraMode.Normal:
            this._activeCameraTable = this._virtualCameraNormalTable;
            break;
          case CameraMode.ActionFreeLook:
            this._activeCameraTable = this._virtualCameraActionTable;
            break;
          default:
            this._activeCameraTable = (Dictionary<ShotType, CinemachineVirtualCameraBase>) null;
            switch (value - 2)
            {
              case CameraMode.Normal:
                virtualCameraBase1 = this._actionCameraNotMove;
                this.prevCamPos = ((Component) this._brain).get_transform().get_position();
                this.prevCamrot = ((Component) this._brain).get_transform().get_rotation();
                break;
              case CameraMode.ActionFreeLook:
                virtualCameraBase1 = this._advCamera;
                break;
              case CameraMode.ActionNotMove:
                virtualCameraBase1 = this._advNotStandCamera;
                break;
              case CameraMode.ADV:
                virtualCameraBase1 = this._eventCamera;
                break;
              case CameraMode.ADVExceptStand:
                virtualCameraBase1 = this._hCamera;
                break;
              case CameraMode.Event:
                virtualCameraBase1 = this.FishingCamera;
                break;
              case CameraMode.H:
                virtualCameraBase1 = this._craftingCamera;
                break;
            }
            break;
        }
        ((Component) this._vanishControl).get_gameObject().SetActiveIfDifferent(value == CameraMode.ADV || value == CameraMode.ADVExceptStand);
        if (this._activeCameraTable != null)
        {
          CinemachineVirtualCameraBase virtualCameraBase2;
          this._activeCameraTable.TryGetValue(this._shotType, out virtualCameraBase2);
          virtualCameraBase1 = virtualCameraBase2;
        }
        if (this._activeVirtualCamera is CinemachineFreeLook && virtualCameraBase1 is CinemachineFreeLook)
        {
          CameraState state = ((CinemachineVirtualCameraBase) this.GetLiveChildOrSelf(this._activeVirtualCamera as CinemachineFreeLook)).get_State();
          ((Behaviour) this._activeVirtualCamera).set_enabled(false);
          this._activeVirtualCamera = virtualCameraBase1;
          this.GetLiveChildOrSelf(this._activeVirtualCamera as CinemachineFreeLook).GetComponentOwner().set_localRotation(((CameraState) ref state).get_RawOrientation());
          this._activeVirtualCamera.UpdateCameraState(((CameraState) ref state).get_ReferenceUp(), Time.get_unscaledDeltaTime());
          ((Behaviour) this._activeVirtualCamera).set_enabled(true);
        }
        else
        {
          ((Behaviour) this._activeVirtualCamera).set_enabled(false);
          ((Behaviour) (this._activeVirtualCamera = virtualCameraBase1)).set_enabled(true);
        }
      }
    }

    public void LoadInitialState()
    {
      foreach (KeyValuePair<CameraMode, Action<ActorCameraControl, CameraMode, ShotType>> keyValuePair in ActorCameraControl._activationEventTable)
      {
        Action<ActorCameraControl, CameraMode, ShotType> action = keyValuePair.Value;
        if (action != null)
          action(this, this._mode, this._shotType);
      }
    }

    public ShotType ShotType
    {
      get
      {
        return this._shotType;
      }
      set
      {
        if (this._mode == CameraMode.Event || this._mode == CameraMode.H || (this._mode == CameraMode.ActionNotMove || this._shotType == value))
          return;
        this._shotType = value;
        this.ChangeActiveCameraByShotType(value);
      }
    }

    public void SetShotTypeForce(ShotType type)
    {
      this._recovShotType = this._shotType;
      this._shotType = type;
      if (this._activeCameraTable == null)
        return;
      this.ChangeActiveCameraByShotType(type);
    }

    public void RecoverShotType()
    {
      this.SetShotTypeForce(this._recovShotType);
    }

    private void ChangeActiveCameraByShotType(ShotType type)
    {
      CameraState state = ((CinemachineVirtualCameraBase) this.GetLiveChildOrSelf(this._activeVirtualCamera as CinemachineFreeLook)).get_State();
      ((Behaviour) this._activeVirtualCamera).set_enabled(false);
      this._activeVirtualCamera = this._activeCameraTable[type];
      this.GetLiveChildOrSelf(this._activeVirtualCamera as CinemachineFreeLook).GetComponentOwner().set_localRotation(((CameraState) ref state).get_RawOrientation());
      this._activeVirtualCamera.UpdateCameraState(((CameraState) ref state).get_ReferenceUp(), Time.get_unscaledDeltaTime());
      ((Behaviour) this._activeVirtualCamera).set_enabled(true);
    }

    public void AssignCameraTable(CameraTable table, CameraTable actionTable)
    {
      if (Object.op_Inequality((Object) table, (Object) null))
      {
        foreach (ShotType key in table.Keys)
        {
          CinemachineVirtualCameraBase virtualCameraBase = table[key];
          this._virtualCameraNormalTable[key] = virtualCameraBase;
          if (key != ShotType.PointOfView)
          {
            virtualCameraBase.set_Follow(this._locomotionSetting.Follow);
            virtualCameraBase.set_LookAt(this._locomotionSetting.LookAt);
          }
          else
          {
            virtualCameraBase.set_Follow(this._locomotionSetting.LookAtPOV);
            virtualCameraBase.set_LookAt(this._locomotionSetting.LookAtPOV);
          }
        }
      }
      if (!Object.op_Inequality((Object) actionTable, (Object) null))
        return;
      foreach (ShotType key in actionTable.Keys)
      {
        CinemachineVirtualCameraBase virtualCameraBase = actionTable[key];
        this._virtualCameraActionTable[key] = virtualCameraBase;
        if (key != ShotType.PointOfView)
        {
          virtualCameraBase.set_Follow(this._locomotionSetting.ActionLookAt);
          virtualCameraBase.set_LookAt(this._locomotionSetting.ActionLookAt);
        }
        else
        {
          virtualCameraBase.set_Follow(this._locomotionSetting.ActionLookAtPOV);
          virtualCameraBase.set_LookAt(this._locomotionSetting.ActionLookAtPOV);
        }
      }
    }

    public void SetNormalLookAt(Transform lookAt)
    {
      if (Object.op_Equality((Object) lookAt, (Object) null))
        return;
      foreach (ShotType index in ((IEnumerable<ShotType>) this._virtualCameraNormalTable.Keys).ToArray<ShotType>())
      {
        CinemachineVirtualCameraBase virtualCameraBase = this._virtualCameraNormalTable[index];
        if (Object.op_Inequality((Object) virtualCameraBase, (Object) null))
          virtualCameraBase.set_LookAt(lookAt);
      }
    }

    public void SetNormalPrevLookAt()
    {
      foreach (ShotType index in ((IEnumerable<ShotType>) this._virtualCameraNormalTable.Keys).ToArray<ShotType>())
      {
        CinemachineVirtualCameraBase virtualCameraBase = this._virtualCameraNormalTable[index];
        Transform transform = this._oldNormalLookAtTransform[index];
        if (Object.op_Inequality((Object) virtualCameraBase, (Object) null))
          virtualCameraBase.set_LookAt(transform);
      }
    }

    public void SetLensSetting(LocomotionProfile.LensSettings lensSetting)
    {
      this._defaultLensSetting = lensSetting;
      this._lensSetting.FieldOfView = (__Null) (double) lensSetting.FieldOfView;
      this._lensSetting.NearClipPlane = (__Null) (double) lensSetting.NearClipPlane;
      this._lensSetting.FarClipPlane = (__Null) (double) lensSetting.FarClipPlane;
      this._lensSetting.Dutch = (__Null) (double) lensSetting.Dutch;
    }

    private void Reset()
    {
      this._brain = (CinemachineBrain) ((Component) this).GetComponent<CinemachineBrain>();
    }

    public bool LoadActionCameraFile(int _eventTypeID, int _poseID, Transform point = null)
    {
      if (this._mode != CameraMode.ActionFreeLook && this._mode != CameraMode.ActionNotMove)
        return false;
      return this._mode == CameraMode.ActionFreeLook ? this.LoadActionFreeCameraFile(_eventTypeID, _poseID) : this.LoadActionNonMoveCameraFile(_eventTypeID, _poseID, point);
    }

    private bool LoadActionFreeCameraFile(int _eventTypeID, int _poseID)
    {
      Dictionary<int, ActionCameraData> dictionary;
      if (!Singleton<Resources>.Instance.Map.ActionCameraDataTable.TryGetValue(_eventTypeID, out dictionary))
      {
        this._activeVirtualCamera.get_LookAt().set_position(this.prevCamPos);
        return false;
      }
      if (!dictionary.TryGetValue(_poseID, out this._actionCameraData))
      {
        this._activeVirtualCamera.get_LookAt().set_position(this.prevCamPos);
        return false;
      }
      this._activeVirtualCamera.get_LookAt().set_position(Vector3.op_Addition(this._actionCameraData.freePos, this.LocomotionSetting.Follow.get_position()));
      return true;
    }

    private bool LoadActionNonMoveCameraFile(int _eventTypeID, int _poseID, Transform point)
    {
      CinemachineTransposer cinemachineComponent = (CinemachineTransposer) ((CinemachineVirtualCamera) ((Component) this._activeVirtualCamera).GetComponent<CinemachineVirtualCamera>()).GetCinemachineComponent<CinemachineTransposer>();
      Dictionary<int, ActionCameraData> dictionary;
      if (!Singleton<Resources>.Instance.Map.ActionCameraDataTable.TryGetValue(_eventTypeID, out dictionary))
      {
        ((Component) this._activeVirtualCamera).get_transform().set_position(this.prevCamPos);
        ((Component) this._activeVirtualCamera).get_transform().set_rotation(this.prevCamrot);
        cinemachineComponent.m_BindingMode = (__Null) 4;
        cinemachineComponent.m_FollowOffset = (__Null) Vector3.op_Subtraction(this.prevCamPos, this._activeVirtualCamera.get_Follow().get_position());
        return false;
      }
      if (!dictionary.TryGetValue(_poseID, out this._actionCameraData))
      {
        ((Component) this._activeVirtualCamera).get_transform().set_position(this.prevCamPos);
        ((Component) this._activeVirtualCamera).get_transform().set_rotation(this.prevCamrot);
        cinemachineComponent.m_BindingMode = (__Null) 4;
        cinemachineComponent.m_FollowOffset = (__Null) Vector3.op_Subtraction(this.prevCamPos, this._activeVirtualCamera.get_Follow().get_position());
        return false;
      }
      ((Component) this._activeVirtualCamera).get_transform().set_position(Vector3.op_Addition(Quaternion.op_Multiply(point.get_rotation(), this._actionCameraData.nonMovePos), this._activeVirtualCamera.get_Follow().get_position()));
      ((Component) this._activeVirtualCamera).get_transform().set_localRotation(Quaternion.Euler(this._actionCameraData.nonMoveRot));
      ((Component) this._activeVirtualCamera).get_transform().set_rotation(Quaternion.op_Multiply(point.get_rotation(), ((Component) this._activeVirtualCamera).get_transform().get_rotation()));
      cinemachineComponent.m_BindingMode = (__Null) 0;
      cinemachineComponent.m_FollowOffset = (__Null) this._actionCameraData.nonMovePos;
      return true;
    }

    [Serializable]
    public class LocomotionSettingData
    {
      [Header("Normal")]
      [SerializeField]
      private Transform _follow;
      [SerializeField]
      private Transform _lookAt;
      [SerializeField]
      private Transform _lookAtPOV;
      [Header("Action")]
      [SerializeField]
      private Transform _actionFollow;
      [SerializeField]
      private Transform _actionLookAt;
      [SerializeField]
      private Transform _actionLookAtPOV;

      public Transform Follow
      {
        get
        {
          return this._follow;
        }
      }

      public Transform LookAt
      {
        get
        {
          return this._lookAt;
        }
      }

      public Transform LookAtPOV
      {
        get
        {
          return this._lookAtPOV;
        }
      }

      public Transform ActionFollow
      {
        get
        {
          return this._actionFollow;
        }
      }

      public Transform ActionLookAt
      {
        get
        {
          return this._actionLookAt;
        }
      }

      public Transform ActionLookAtPOV
      {
        get
        {
          return this._actionLookAtPOV;
        }
      }
    }

    private class HeadingTracker
    {
      private Vector3 mLastGoodHeading = Vector3.get_zero();
      private ActorCameraControl.HeadingTracker.Item[] mHistory;
      private int mTop;
      private int mBottom;
      private int mCount;
      private Vector3 mHeadingSum;
      private float mWeightSum;
      private float mWeightTime;
      private static float mDecayExponent;

      public HeadingTracker(int filterSize)
      {
        this.mHistory = new ActorCameraControl.HeadingTracker.Item[filterSize];
        ActorCameraControl.HeadingTracker.mDecayExponent = -Mathf.Log(2f) / ((float) filterSize / 5f);
        this.ClearHistory();
      }

      public int FilterSize
      {
        get
        {
          return this.mHistory.Length;
        }
      }

      private void ClearHistory()
      {
        this.mTop = this.mBottom = this.mCount = 0;
        this.mWeightSum = 0.0f;
        this.mHeadingSum = Vector3.get_zero();
      }

      private static float Decay(float time)
      {
        return Mathf.Exp(time * ActorCameraControl.HeadingTracker.mDecayExponent);
      }

      public void Add(Vector3 velocity)
      {
        if (this.FilterSize == 0)
        {
          this.mLastGoodHeading = velocity;
        }
        else
        {
          float magnitude = ((Vector3) ref velocity).get_magnitude();
          if ((double) magnitude <= 9.99999974737875E-05)
            return;
          ActorCameraControl.HeadingTracker.Item obj = new ActorCameraControl.HeadingTracker.Item();
          obj.velocity = velocity;
          obj.weight = magnitude;
          obj.time = Time.get_time();
          if (this.mCount == this.FilterSize)
            this.PopBottom();
          ++this.mCount;
          this.mHistory[this.mTop] = obj;
          if (++this.mTop == this.FilterSize)
            this.mTop = 0;
          this.mWeightSum *= ActorCameraControl.HeadingTracker.Decay(obj.time - this.mWeightTime);
          this.mWeightTime = obj.time;
          this.mWeightSum += magnitude;
          ActorCameraControl.HeadingTracker headingTracker = this;
          headingTracker.mHeadingSum = Vector3.op_Addition(headingTracker.mHeadingSum, obj.velocity);
        }
      }

      private void PopBottom()
      {
        if (this.mCount <= 0)
          return;
        float time = Time.get_time();
        ActorCameraControl.HeadingTracker.Item obj = this.mHistory[this.mBottom];
        if (++this.mBottom == this.FilterSize)
          this.mBottom = 0;
        --this.mCount;
        float num = ActorCameraControl.HeadingTracker.Decay(time - obj.time);
        this.mWeightSum -= obj.weight * num;
        ActorCameraControl.HeadingTracker headingTracker = this;
        headingTracker.mHeadingSum = Vector3.op_Subtraction(headingTracker.mHeadingSum, Vector3.op_Multiply(obj.velocity, num));
        if ((double) this.mWeightSum > 9.99999974737875E-05 && this.mCount != 0)
          return;
        this.ClearHistory();
      }

      public void DecayHistory()
      {
        float time = Time.get_time();
        float num = ActorCameraControl.HeadingTracker.Decay(time - this.mWeightTime);
        this.mWeightSum *= num;
        this.mWeightTime = time;
        if ((double) this.mWeightSum < 9.99999974737875E-05)
        {
          this.ClearHistory();
        }
        else
        {
          ActorCameraControl.HeadingTracker headingTracker = this;
          headingTracker.mHeadingSum = Vector3.op_Multiply(headingTracker.mHeadingSum, num);
        }
      }

      public Vector3 GetReliableHeading()
      {
        if ((double) this.mWeightSum > 9.99999974737875E-05 && (this.mCount == this.mHistory.Length || UnityVectorExtensions.AlmostZero(this.mLastGoodHeading)))
        {
          Vector3 vector3 = Vector3.op_Division(this.mHeadingSum, this.mWeightSum);
          if (!UnityVectorExtensions.AlmostZero(vector3))
            this.mLastGoodHeading = ((Vector3) ref vector3).get_normalized();
        }
        return this.mLastGoodHeading;
      }

      private struct Item
      {
        public Vector3 velocity;
        public float weight;
        public float time;
      }
    }
  }
}
