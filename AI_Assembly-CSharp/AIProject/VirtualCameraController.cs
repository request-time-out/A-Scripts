// Decompiled with JetBrains decompiler
// Type: AIProject.VirtualCameraController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Cinemachine;
using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AIProject
{
  public class VirtualCameraController : CinemachineVirtualCameraBase
  {
    protected Input input;
    [SerializeField]
    protected Transform follow;
    [SerializeField]
    protected Transform lookAtTarget;
    protected Renderer targetRender;
    protected CameraState m_State;
    public float xRotSpeed;
    public float yRotSpeed;
    public float moveSpeed;
    public float keyMoveSpeed;
    public float limitFov;
    public bool isLimitPos;
    public bool isLimitDir;
    public bool isLimitRotX;
    public float limitRotX;
    public float limitPos;
    public float limitDir;
    protected bool isZoomNow;
    protected Housing housingMgr;
    private const float INIT_FOV = 23f;
    protected bool isConfigVanish;
    [SerializeField]
    protected List<VirtualCameraController.VisibleObjectH> lstMapVanish;
    [SerializeField]
    protected List<Collider> listCollider;
    public readonly int CONFIG_SIZE;
    [SerializeField]
    protected VirtualCameraController.CameraData CamDat;
    protected VirtualCameraController.Config cameraType;
    protected bool[] isDrags;
    protected bool[] isButtons;
    protected VirtualCameraController.ResetData CamReset;
    protected bool isInit;
    protected CapsuleCollider viewCollider;
    protected float rateAddSpeed;
    public VirtualCameraController.NoCtrlFunc NoCtrlCondition;
    public VirtualCameraController.NoCtrlFunc ZoomCondition;
    public VirtualCameraController.NoCtrlFunc KeyCondition;
    public LensSettings Lens;
    protected bool craft;
    public const string PipelineName = "cm";

    public VirtualCameraController()
    {
      base.\u002Ector();
    }

    public Camera thisCamera { get; private set; }

    public bool isOutsideTargetTex { get; set; }

    public bool isCursorLock { get; set; }

    public bool isConfigTargetTex { get; set; }

    public bool ConfigVanish
    {
      get
      {
        return this.isConfigVanish;
      }
      set
      {
        if (this.isConfigVanish == value)
          return;
        this.isConfigVanish = value;
        this.visibleForceVanish(true);
        if (this.housingMgr == null)
          return;
        this.housingMgr.VisibleShield();
      }
    }

    public bool isControlNow { get; protected set; }

    public virtual CameraState State
    {
      get
      {
        return this.m_State;
      }
    }

    public virtual Transform LookAt
    {
      get
      {
        return this.lookAtTarget;
      }
      set
      {
        this.lookAtTarget = value;
      }
    }

    public virtual Transform Follow
    {
      get
      {
        return this.follow;
      }
      set
      {
        this.follow = value;
      }
    }

    public LensSettings lens
    {
      get
      {
        return this.Lens;
      }
      set
      {
        this.Lens = value;
      }
    }

    protected virtual void Start()
    {
      base.Start();
      this.isCursorLock = true;
      this.viewCollider = (CapsuleCollider) ((Component) this).get_gameObject().AddComponent<CapsuleCollider>();
      this.viewCollider.set_radius(0.05f);
      ((Collider) this.viewCollider).set_isTrigger(true);
      this.viewCollider.set_direction(2);
      Rigidbody rigidbody = (Rigidbody) ((Component) this).get_gameObject().AddComponent<Rigidbody>();
      rigidbody.set_useGravity(false);
      rigidbody.set_isKinematic(true);
    }

    public virtual void InternalUpdateCameraState(Vector3 worldUp, float deltaTime)
    {
      if (!((Behaviour) this).get_enabled())
        return;
      this.ForceMoveCam(deltaTime);
    }

    public virtual void ForceMoveCam(float deltaTime = 0.0f)
    {
      if (this.craft)
      {
        if (this.isLimitDir)
          this.CamDat.Dir.z = (__Null) (double) Mathf.Clamp((float) this.CamDat.Dir.z, -this.limitDir, 10f);
        if (this.isLimitPos)
        {
          this.CamDat.Pos = Vector3.ClampMagnitude(this.CamDat.Pos, this.limitPos);
          if (this.CamDat.Pos.y < 2.5)
            this.CamDat.Pos.y = (__Null) 2.5;
        }
        if (this.isLimitRotX)
          this.CamDat.Rot.x = (__Null) (double) Mathf.Clamp((float) this.CamDat.Rot.x, 0.0f, this.limitRotX);
      }
      else
      {
        if (this.isLimitDir)
          this.CamDat.Dir.z = (__Null) (double) Mathf.Clamp((float) this.CamDat.Dir.z, -this.limitDir, 10f);
        if (this.isLimitPos)
          this.CamDat.Pos = Vector3.ClampMagnitude(this.CamDat.Pos, this.limitPos);
        if (this.isLimitRotX)
          this.CamDat.Rot.x = (__Null) (double) Mathf.Clamp((float) this.CamDat.Rot.x, 0.0f, this.limitRotX);
      }
      if (Object.op_Inequality((Object) this.follow, (Object) null))
      {
        ((CameraState) ref this.m_State).set_RawOrientation(Quaternion.op_Multiply(this.follow.get_rotation(), Quaternion.Euler(this.CamDat.Rot)));
        ((CameraState) ref this.m_State).set_RawPosition(Vector3.op_Addition(Quaternion.op_Multiply(((CameraState) ref this.m_State).get_RawOrientation(), this.CamDat.Dir), this.follow.TransformPoint(this.CamDat.Pos)));
      }
      else
      {
        ((CameraState) ref this.m_State).set_RawOrientation(Quaternion.Euler(this.CamDat.Rot));
        ((CameraState) ref this.m_State).set_RawPosition(Vector3.op_Addition(Quaternion.op_Multiply(((CameraState) ref this.m_State).get_RawOrientation(), this.CamDat.Dir), this.CamDat.Pos));
      }
      ((CameraState) ref this.m_State).set_Lens(this.Lens);
      Transform transform1 = ((Component) this).get_transform();
      CameraState state1 = base.get_State();
      Vector3 rawPosition = ((CameraState) ref state1).get_RawPosition();
      transform1.set_position(rawPosition);
      Transform transform2 = ((Component) this).get_transform();
      CameraState state2 = base.get_State();
      Quaternion rawOrientation = ((CameraState) ref state2).get_RawOrientation();
      transform2.set_rotation(rawOrientation);
      if (Object.op_Inequality((Object) this.viewCollider, (Object) null))
      {
        this.viewCollider.set_height((float) this.CamDat.Dir.z);
        this.viewCollider.set_center(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_UnaryNegation(Vector3.get_forward()), (float) this.CamDat.Dir.z), 0.5f));
      }
      this.lookAtTarget.set_localPosition(this.CamDat.Pos);
      Vector3 position = ((Component) this).get_transform().get_position();
      position.y = this.lookAtTarget.get_position().y;
      ((Component) this.lookAtTarget).get_transform().LookAt(position);
      this.lookAtTarget.Rotate(new Vector3(90f, 0.0f, 0.0f));
      if (Object.op_Implicit((Object) this.targetRender))
        this.targetRender.set_enabled(this.isControlNow & this.isOutsideTargetTex & this.isConfigTargetTex & !this.isZoomNow);
      if (Singleton<GameCursor>.IsInstance() && this.isCursorLock)
        Singleton<GameCursor>.Instance.SetCursorLock(this.isControlNow & this.isOutsideTargetTex);
      this.VanishProc();
      foreach (CinemachineCore.Stage stage in Enum.GetValues(typeof (CinemachineCore.Stage)))
        this.InvokePostPipelineStageCallback((CinemachineVirtualCameraBase) this, stage, ref this.m_State, deltaTime);
    }

    public void Init()
    {
      this.thisCamera = CinemachineCore.get_Instance().GetActiveBrain(0).get_OutputCamera();
      this.housingMgr = Singleton<Housing>.Instance;
      this.CamDat.Fov = 23f;
      this.CamReset.Fov = 23f;
      ((CameraState) ref this.m_State).set_Lens(this.lens);
      this.isDrags = new bool[this.CONFIG_SIZE];
      for (int index = 0; index < this.isDrags.Length; ++index)
        this.isDrags[index] = false;
      if (this.isInit)
        return;
      this.isButtons = new bool[this.CONFIG_SIZE];
      this.targetRender = (Renderer) ((Component) this.lookAtTarget).GetComponent<Renderer>();
      this.TargetSet(this.lookAtTarget, true);
      this.isOutsideTargetTex = true;
      this.isConfigTargetTex = true;
      this.input = Singleton<Input>.Instance;
      this.isInit = true;
    }

    protected virtual void Update()
    {
    }

    protected virtual void LateUpdate()
    {
      this.isControlNow = false;
      if (!this.craft && Singleton<HSceneFlagCtrl>.Instance.BeforeHWait)
        return;
      if (!this.isControlNow)
      {
        VirtualCameraController.NoCtrlFunc noCtrlCondition = this.NoCtrlCondition;
        bool flag = false;
        if (noCtrlCondition != null)
          flag = noCtrlCondition();
        if (!flag)
        {
          if (this.InputTouchProc())
            this.isControlNow = true;
          else if (this.InputMouseProc())
            this.isControlNow = true;
        }
      }
      if (this.isControlNow)
        return;
      VirtualCameraController.NoCtrlFunc keyCondition = this.KeyCondition;
      bool flag1 = true;
      if (keyCondition != null)
        flag1 = keyCondition();
      VirtualCameraController cameraController = this;
      cameraController.isControlNow = ((cameraController.isControlNow ? 1 : 0) | (!flag1 ? 0 : (this.InputKeyProc() ? 1 : 0))) != 0;
    }

    protected bool InputTouchProc()
    {
      if (Input.get_touchCount() < 1)
        return false;
      float num1 = 10f * Time.get_deltaTime();
      if (Input.get_touchCount() == 3)
        this.Reset(0);
      else if (Input.get_touchCount() == 1)
      {
        Touch touch = Input.get_touches()[0];
        TouchPhase phase = ((Touch) ref touch).get_phase();
        if (phase != null && phase == 1)
        {
          float num2 = 0.01f;
          float num3 = 0.1f;
          Vector3 zero = Vector3.get_zero();
          if (this.cameraType == VirtualCameraController.Config.Rotation)
          {
            ref Vector3 local1 = ref zero;
            local1.y = (__Null) (local1.y + ((Touch) ref touch).get_deltaPosition().x * (double) this.xRotSpeed * (double) num1 * (double) num2);
            ref Vector3 local2 = ref zero;
            local2.x = (__Null) (local2.x - ((Touch) ref touch).get_deltaPosition().y * (double) this.yRotSpeed * (double) num1 * (double) num2);
            Vector3 vector3 = zero;
            Quaternion rotation = ((Component) this).get_transform().get_rotation();
            Vector3 eulerAngles = ((Quaternion) ref rotation).get_eulerAngles();
            ((Component) this).get_transform().set_rotation(Quaternion.Euler(Vector3.op_Addition(vector3, eulerAngles)));
          }
          else if (this.cameraType == VirtualCameraController.Config.Translation)
          {
            ref Vector3 local1 = ref this.CamDat.Dir;
            local1.z = (__Null) (local1.z - ((Touch) ref touch).get_deltaPosition().x * (double) this.xRotSpeed * (double) num1 * (double) num3);
            ref Vector3 local2 = ref this.CamDat.Pos;
            local2.y = (__Null) (local2.y + ((Touch) ref touch).get_deltaPosition().y * (double) this.yRotSpeed * (double) num1 * (double) num3);
          }
          else if (this.cameraType == VirtualCameraController.Config.MoveXY)
          {
            zero.x = (__Null) (((Touch) ref touch).get_deltaPosition().x * (double) this.xRotSpeed * (double) num1 * (double) num3);
            zero.y = (__Null) (((Touch) ref touch).get_deltaPosition().y * (double) this.yRotSpeed * (double) num1 * (double) num3);
            ref VirtualCameraController.CameraData local = ref this.CamDat;
            local.Pos = Vector3.op_Addition(local.Pos, ((Component) this).get_transform().TransformDirection(zero));
          }
          else if (this.cameraType == VirtualCameraController.Config.MoveXZ)
          {
            zero.x = (__Null) (((Touch) ref touch).get_deltaPosition().x * (double) this.xRotSpeed * (double) num1 * (double) num3);
            zero.z = (__Null) (((Touch) ref touch).get_deltaPosition().y * (double) this.yRotSpeed * (double) num1 * (double) num3);
            ref VirtualCameraController.CameraData local = ref this.CamDat;
            local.Pos = Vector3.op_Addition(local.Pos, ((Component) this).get_transform().TransformDirection(zero));
          }
        }
      }
      return true;
    }

    protected bool InputMouseProc()
    {
      bool flag1 = false;
      float deltaTime = Time.get_deltaTime();
      bool flag2 = false;
      for (int index = 0; index < this.CONFIG_SIZE; ++index)
      {
        if (this.isDrags[index])
        {
          flag2 = true;
          break;
        }
      }
      if (!flag2 && (Object.op_Equality((Object) EventSystem.get_current(), (Object) null) || EventSystem.get_current().IsPointerOverGameObject()))
      {
        if (Singleton<GameCursor>.IsInstance())
        {
          if (flag1)
            Singleton<GameCursor>.Instance.SetCursorLock(true);
          else
            Singleton<GameCursor>.Instance.UnLockCursor();
        }
        return flag1;
      }
      this.isButtons[1] = this.input.IsDown(ActionID.MouseLeft);
      this.isButtons[2] = this.input.IsDown(ActionID.MouseRight);
      this.isButtons[3] = this.input.IsDown(ActionID.MouseCenter);
      this.isButtons[0] = this.input.IsDown(ActionID.MouseLeft) && this.input.IsDown(ActionID.MouseRight);
      for (int index = 0; index < this.CONFIG_SIZE; ++index)
      {
        if (this.isButtons[index])
          this.isDrags[index] = true;
      }
      for (int index = 0; index < this.CONFIG_SIZE; ++index)
      {
        if (this.isDrags[index] && !this.isButtons[index])
          this.isDrags[index] = false;
      }
      float x = (float) this.input.MouseAxis.x;
      float y = (float) this.input.MouseAxis.y;
      for (int index = 0; index < this.CONFIG_SIZE; ++index)
      {
        if (this.isDrags[index])
        {
          Vector3 zero = Vector3.get_zero();
          switch (index)
          {
            case 0:
              zero.x = (__Null) ((double) x * (double) this.moveSpeed * (double) deltaTime * (double) this.rateAddSpeed);
              zero.z = (__Null) ((double) y * (double) this.moveSpeed * (double) deltaTime * (double) this.rateAddSpeed);
              if (Object.op_Inequality((Object) this.follow, (Object) null))
              {
                ref VirtualCameraController.CameraData local = ref this.CamDat;
                local.Pos = Vector3.op_Addition(local.Pos, this.follow.InverseTransformDirection(((Component) this).get_transform().TransformDirection(zero)));
                break;
              }
              ref VirtualCameraController.CameraData local1 = ref this.CamDat;
              local1.Pos = Vector3.op_Addition(local1.Pos, ((Component) this).get_transform().TransformDirection(zero));
              break;
            case 1:
              ref Vector3 local2 = ref zero;
              local2.y = (__Null) (local2.y + (double) x * (double) this.xRotSpeed * (double) deltaTime * (double) this.rateAddSpeed);
              ref Vector3 local3 = ref zero;
              local3.x = (__Null) (local3.x - (double) y * (double) this.yRotSpeed * (double) deltaTime * (double) this.rateAddSpeed);
              this.CamDat.Rot.y = (__Null) ((this.CamDat.Rot.y + zero.y) % 360.0);
              this.CamDat.Rot.x = (__Null) ((this.CamDat.Rot.x + zero.x) % 360.0);
              break;
            case 2:
              ref Vector3 local4 = ref this.CamDat.Pos;
              local4.y = (__Null) (local4.y + (double) y * (double) this.moveSpeed * (double) deltaTime * (double) this.rateAddSpeed);
              ref Vector3 local5 = ref this.CamDat.Dir;
              local5.z = (__Null) (local5.z - (double) x * (double) this.moveSpeed * (double) deltaTime * (double) this.rateAddSpeed);
              this.CamDat.Dir.z = (__Null) (double) Mathf.Min(0.0f, (float) this.CamDat.Dir.z);
              break;
            case 3:
              zero.x = (__Null) ((double) x * (double) this.moveSpeed * (double) deltaTime * (double) this.rateAddSpeed);
              zero.y = (__Null) ((double) y * (double) this.moveSpeed * (double) deltaTime * (double) this.rateAddSpeed);
              if (Object.op_Inequality((Object) this.follow, (Object) null))
              {
                ref VirtualCameraController.CameraData local6 = ref this.CamDat;
                local6.Pos = Vector3.op_Addition(local6.Pos, this.follow.InverseTransformDirection(((Component) this).get_transform().TransformDirection(zero)));
                break;
              }
              ref VirtualCameraController.CameraData local7 = ref this.CamDat;
              local7.Pos = Vector3.op_Addition(local7.Pos, ((Component) this).get_transform().TransformDirection(zero));
              break;
          }
          flag1 = true;
          break;
        }
      }
      return flag1;
    }

    protected bool InputKeyProc()
    {
      bool flag = false;
      if (this.input.IsDown((KeyCode) 114))
        this.Reset(0);
      else if (this.input.IsDown((KeyCode) 261))
      {
        this.CamDat.Rot.x = this.CamReset.Rot.x;
        this.CamDat.Rot.y = this.CamReset.Rot.y;
      }
      else if (this.input.IsDown((KeyCode) 47))
        this.CamDat.Rot.z = (__Null) 0.0;
      else if (this.input.IsDown((KeyCode) 59))
      {
        this.CamDat.Fov = this.CamReset.Fov;
        this.Lens.FieldOfView = (__Null) (double) this.CamDat.Fov;
      }
      float deltaTime1 = Time.get_deltaTime();
      if (this.input.IsDown((KeyCode) 278))
      {
        flag = true;
        ref Vector3 local = ref this.CamDat.Dir;
        local.z = (__Null) (local.z + (double) deltaTime1 * (double) this.keyMoveSpeed * (double) this.rateAddSpeed);
        this.CamDat.Dir.z = (__Null) (double) Mathf.Min(0.0f, (float) this.CamDat.Dir.z);
      }
      else if (this.input.IsDown((KeyCode) 279))
      {
        flag = true;
        ref Vector3 local = ref this.CamDat.Dir;
        local.z = (__Null) (local.z - (double) deltaTime1 * (double) this.keyMoveSpeed * (double) this.rateAddSpeed);
      }
      if (this.input.IsDown((KeyCode) 275))
      {
        flag = true;
        if (Object.op_Inequality((Object) this.follow, (Object) null))
        {
          ref VirtualCameraController.CameraData local = ref this.CamDat;
          local.Pos = Vector3.op_Addition(local.Pos, this.follow.InverseTransformDirection(((Component) this).get_transform().TransformDirection(new Vector3(deltaTime1 * this.keyMoveSpeed * this.rateAddSpeed, 0.0f, 0.0f))));
        }
        else
        {
          ref VirtualCameraController.CameraData local = ref this.CamDat;
          local.Pos = Vector3.op_Addition(local.Pos, ((Component) this).get_transform().TransformDirection(new Vector3(deltaTime1 * this.keyMoveSpeed * this.rateAddSpeed, 0.0f, 0.0f)));
        }
      }
      else if (this.input.IsDown((KeyCode) 276))
      {
        flag = true;
        if (Object.op_Inequality((Object) this.follow, (Object) null))
        {
          ref VirtualCameraController.CameraData local = ref this.CamDat;
          local.Pos = Vector3.op_Addition(local.Pos, this.follow.InverseTransformDirection(((Component) this).get_transform().TransformDirection(new Vector3(-deltaTime1 * this.keyMoveSpeed * this.rateAddSpeed, 0.0f, 0.0f))));
        }
        else
        {
          ref VirtualCameraController.CameraData local = ref this.CamDat;
          local.Pos = Vector3.op_Addition(local.Pos, ((Component) this).get_transform().TransformDirection(new Vector3(-deltaTime1 * this.keyMoveSpeed * this.rateAddSpeed, 0.0f, 0.0f)));
        }
      }
      if (this.input.IsDown((KeyCode) 273))
      {
        flag = true;
        if (Object.op_Inequality((Object) this.follow, (Object) null))
        {
          ref VirtualCameraController.CameraData local = ref this.CamDat;
          local.Pos = Vector3.op_Addition(local.Pos, this.follow.InverseTransformDirection(((Component) this).get_transform().TransformDirection(new Vector3(0.0f, 0.0f, deltaTime1 * this.keyMoveSpeed * this.rateAddSpeed))));
        }
        else
        {
          ref VirtualCameraController.CameraData local = ref this.CamDat;
          local.Pos = Vector3.op_Addition(local.Pos, ((Component) this).get_transform().TransformDirection(new Vector3(0.0f, 0.0f, deltaTime1 * this.keyMoveSpeed * this.rateAddSpeed)));
        }
      }
      else if (this.input.IsDown((KeyCode) 274))
      {
        flag = true;
        if (Object.op_Inequality((Object) this.follow, (Object) null))
        {
          ref VirtualCameraController.CameraData local = ref this.CamDat;
          local.Pos = Vector3.op_Addition(local.Pos, this.follow.InverseTransformDirection(((Component) this).get_transform().TransformDirection(new Vector3(0.0f, 0.0f, -deltaTime1 * this.keyMoveSpeed * this.rateAddSpeed))));
        }
        else
        {
          ref VirtualCameraController.CameraData local = ref this.CamDat;
          local.Pos = Vector3.op_Addition(local.Pos, ((Component) this).get_transform().TransformDirection(new Vector3(0.0f, 0.0f, -deltaTime1 * this.keyMoveSpeed * this.rateAddSpeed)));
        }
      }
      if (this.input.IsDown((KeyCode) 280))
      {
        flag = true;
        ref Vector3 local = ref this.CamDat.Pos;
        local.y = (__Null) (local.y + (double) deltaTime1 * (double) this.keyMoveSpeed * (double) this.rateAddSpeed);
      }
      else if (this.input.IsDown((KeyCode) 281))
      {
        flag = true;
        ref Vector3 local = ref this.CamDat.Pos;
        local.y = (__Null) (local.y - (double) deltaTime1 * (double) this.keyMoveSpeed * (double) this.rateAddSpeed);
      }
      float num = 10f * Time.get_deltaTime();
      Vector3 zero = Vector3.get_zero();
      if (this.input.IsDown((KeyCode) 46))
      {
        flag = true;
        ref Vector3 local = ref zero;
        local.z = (__Null) (local.z + (double) num);
      }
      else if (this.input.IsDown((KeyCode) 92))
      {
        flag = true;
        ref Vector3 local = ref zero;
        local.z = (__Null) (local.z - (double) num);
      }
      if (this.input.IsDown((KeyCode) 258))
      {
        flag = true;
        ref Vector3 local = ref zero;
        local.x = (__Null) (local.x - (double) num * (double) this.yRotSpeed);
      }
      else if (this.input.IsDown((KeyCode) 264))
      {
        flag = true;
        ref Vector3 local = ref zero;
        local.x = (__Null) (local.x + (double) num * (double) this.yRotSpeed);
      }
      if (this.input.IsDown((KeyCode) 260))
      {
        flag = true;
        ref Vector3 local = ref zero;
        local.y = (__Null) (local.y + (double) num * (double) this.xRotSpeed);
      }
      else if (this.input.IsDown((KeyCode) 262))
      {
        flag = true;
        ref Vector3 local = ref zero;
        local.y = (__Null) (local.y - (double) num * (double) this.xRotSpeed);
      }
      if (flag)
      {
        this.CamDat.Rot.y = (__Null) ((this.CamDat.Rot.y + zero.y) % 360.0);
        this.CamDat.Rot.x = (__Null) ((this.CamDat.Rot.x + zero.x) % 360.0);
        this.CamDat.Rot.z = (__Null) ((this.CamDat.Rot.z + zero.z) % 360.0);
      }
      float deltaTime2 = Time.get_deltaTime();
      if (this.input.IsDown((KeyCode) 61))
      {
        flag = true;
        this.CamDat.Fov = Mathf.Max(this.CamDat.Fov - deltaTime2 * 15f, 1f);
        this.Lens.FieldOfView = (__Null) (double) this.CamDat.Fov;
        ((CameraState) ref this.m_State).set_Lens(this.Lens);
      }
      else if (this.input.IsDown((KeyCode) 93))
      {
        flag = true;
        this.CamDat.Fov = Mathf.Min(this.CamDat.Fov + deltaTime2 * 15f, this.limitFov);
        this.Lens.FieldOfView = (__Null) (double) this.CamDat.Fov;
        ((CameraState) ref this.m_State).set_Lens(this.Lens);
      }
      return flag;
    }

    public void Reset(int mode)
    {
      int num1 = 0;
      int num2 = mode;
      int num3 = num1;
      int num4 = num3 + 1;
      if (num2 == num3)
      {
        this.CamDat.Copy(this.CamReset);
        this.Lens = ((CameraState) ref this.m_State).get_Lens();
        this.Lens.FieldOfView = (__Null) (double) this.CamDat.Fov;
      }
      else
      {
        int num5 = mode;
        int num6 = num4;
        int num7 = num6 + 1;
        if (num5 == num6)
        {
          this.CamDat.Pos = this.CamReset.Pos;
        }
        else
        {
          int num8 = mode;
          int num9 = num7;
          int num10 = num9 + 1;
          if (num8 == num9)
          {
            ((Component) this).get_transform().set_rotation(this.CamReset.RotQ);
          }
          else
          {
            int num11 = mode;
            int num12 = num10;
            int num13 = num12 + 1;
            if (num11 != num12)
              return;
            this.CamDat.Dir = this.CamReset.Dir;
          }
        }
      }
    }

    public void TargetSet(Transform target, bool isReset)
    {
      if (Object.op_Implicit((Object) target))
        this.lookAtTarget = target;
      if (Object.op_Implicit((Object) this.lookAtTarget))
        this.CamDat.Pos = this.lookAtTarget.get_localPosition();
      Transform transform = ((Component) this).get_transform();
      this.CamDat.Dir = Vector3.get_zero();
      this.CamDat.Dir.z = (__Null) -(double) Vector3.Distance(base.get_LookAt().get_position(), transform.get_position());
      transform.LookAt(this.CamDat.Pos);
      ref VirtualCameraController.CameraData local = ref this.CamDat;
      Quaternion rotation = ((Component) this).get_transform().get_rotation();
      Vector3 eulerAngles = ((Quaternion) ref rotation).get_eulerAngles();
      local.Rot = eulerAngles;
      if (!isReset)
        return;
      this.CamReset.Copy(this.CamDat, ((Component) this).get_transform().get_rotation());
    }

    public Vector3 TargetPos
    {
      get
      {
        return this.CamDat.Pos;
      }
      set
      {
        this.CamDat.Pos = value;
      }
    }

    public Vector3 CameraAngle
    {
      get
      {
        return this.CamDat.Rot;
      }
      set
      {
        ((Component) this).get_transform().set_rotation(Quaternion.Euler(value));
        this.CamDat.Rot = value;
      }
    }

    public void CameraDataSave(string _strCreateAssetPath, string _strFile)
    {
      string path = new FileData(string.Empty).Create(_strCreateAssetPath) + _strFile + ".txt";
      Debug.Log((object) path);
      using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.GetEncoding("UTF-8")))
      {
        streamWriter.Write((float) this.CamDat.Pos.x);
        streamWriter.Write('\n');
        streamWriter.Write((float) this.CamDat.Pos.y);
        streamWriter.Write('\n');
        streamWriter.Write((float) this.CamDat.Pos.z);
        streamWriter.Write('\n');
        streamWriter.Write((float) this.CamDat.Dir.x);
        streamWriter.Write('\n');
        streamWriter.Write((float) this.CamDat.Dir.y);
        streamWriter.Write('\n');
        streamWriter.Write((float) this.CamDat.Dir.z);
        streamWriter.Write('\n');
        streamWriter.Write((float) this.CamDat.Rot.x);
        streamWriter.Write('\n');
        streamWriter.Write((float) this.CamDat.Rot.y);
        streamWriter.Write('\n');
        streamWriter.Write((float) this.CamDat.Rot.z);
        streamWriter.Write('\n');
        streamWriter.Write(this.CamDat.Fov);
        streamWriter.Write('\n');
      }
    }

    public bool CameraDataLoad(string _assetbundleFolder, string _strFile, bool _isDirect = false)
    {
      string text = string.Empty;
      if (!_isDirect)
      {
        text = GlobalMethod.LoadAllListText(_assetbundleFolder, _strFile, (List<string>) null);
      }
      else
      {
        TextAsset textAsset = CommonLib.LoadAsset<TextAsset>(_assetbundleFolder, _strFile, false, string.Empty);
        AssetBundleManager.UnloadAssetBundle(_assetbundleFolder, true, (string) null, false);
        if (Object.op_Implicit((Object) textAsset))
          text = textAsset.get_text();
      }
      if (text == string.Empty)
      {
        GlobalMethod.DebugLog("cameraファイル読み込めません", 1);
        return false;
      }
      string[][] data;
      GlobalMethod.GetListString(text, out data);
      this.CamDat.Pos.x = (__Null) (double) float.Parse(data[0][0]);
      this.CamDat.Pos.y = (__Null) (double) float.Parse(data[1][0]);
      this.CamDat.Pos.z = (__Null) (double) float.Parse(data[2][0]);
      this.CamDat.Dir.x = (__Null) (double) float.Parse(data[3][0]);
      this.CamDat.Dir.y = (__Null) (double) float.Parse(data[4][0]);
      this.CamDat.Dir.z = (__Null) (double) float.Parse(data[5][0]);
      this.CamDat.Rot.x = (__Null) (double) float.Parse(data[6][0]);
      this.CamDat.Rot.y = (__Null) (double) float.Parse(data[7][0]);
      this.CamDat.Rot.z = (__Null) (double) float.Parse(data[8][0]);
      this.CamDat.Fov = float.Parse(data[9][0]);
      if (Object.op_Inequality((Object) this.thisCamera, (Object) null))
        this.thisCamera.set_fieldOfView(this.CamDat.Fov);
      this.CamReset.Copy(this.CamDat, Quaternion.get_identity());
      this.ForceMoveCam(0.0f);
      if (!this.isInit)
        this.isInit = true;
      return true;
    }

    public bool CameraResetDataLoad(string _assetbundleFolder, string _strFile, bool _isDirect = false)
    {
      string text = string.Empty;
      if (!_isDirect)
      {
        text = GlobalMethod.LoadAllListText(_assetbundleFolder, _strFile, (List<string>) null);
      }
      else
      {
        TextAsset textAsset = CommonLib.LoadAsset<TextAsset>(_assetbundleFolder, _strFile, false, string.Empty);
        AssetBundleManager.UnloadAssetBundle(_assetbundleFolder, true, (string) null, false);
        if (Object.op_Implicit((Object) textAsset))
          text = textAsset.get_text();
      }
      if (text == string.Empty)
      {
        GlobalMethod.DebugLog("cameraファイル読み込めません", 1);
        return false;
      }
      string[][] data;
      GlobalMethod.GetListString(text, out data);
      this.CamReset.Copy(new VirtualCameraController.CameraData()
      {
        Pos = {
          x = (__Null) (double) float.Parse(data[0][0]),
          y = (__Null) (double) float.Parse(data[1][0]),
          z = (__Null) (double) float.Parse(data[2][0])
        },
        Dir = {
          x = (__Null) (double) float.Parse(data[3][0]),
          y = (__Null) (double) float.Parse(data[4][0]),
          z = (__Null) (double) float.Parse(data[5][0])
        },
        Rot = {
          x = (__Null) (double) float.Parse(data[6][0]),
          y = (__Null) (double) float.Parse(data[7][0]),
          z = (__Null) (double) float.Parse(data[8][0])
        },
        Fov = float.Parse(data[9][0])
      }, Quaternion.get_identity());
      return true;
    }

    public void CameraDataReset()
    {
      this.CamDat.Pos = Vector3.get_zero();
      this.CamDat.Dir = Vector3.get_zero();
      this.CamDat.Rot = Vector3.get_zero();
      this.CamDat.Pos = base.get_LookAt().get_localPosition();
      this.CamDat.Dir.z = (__Null) -(double) Vector3.Distance(base.get_LookAt().get_position(), ((Component) this).get_transform().get_position());
    }

    public bool loadVanish()
    {
      this.lstMapVanish.Clear();
      List<Manager.Map.VisibleObject> lstMapVanish = Singleton<Manager.Map>.Instance.LstMapVanish;
      for (int index1 = 0; index1 < lstMapVanish.Count; ++index1)
      {
        int index2 = index1;
        if (!Object.op_Equality((Object) lstMapVanish[index2].collider, (Object) null) && ((Component) lstMapVanish[index2].collider).get_gameObject().get_activeSelf())
        {
          VirtualCameraController.VisibleObjectH visibleObjectH = new VirtualCameraController.VisibleObjectH();
          visibleObjectH.nameCollider = lstMapVanish[index2].nameCollider;
          visibleObjectH.collider = lstMapVanish[index2].collider;
          visibleObjectH.vanishObj = lstMapVanish[index2].vanishObj;
          visibleObjectH.initEnable = lstMapVanish[index2].collider.get_enabled();
          this.lstMapVanish.Add(visibleObjectH);
          visibleObjectH.collider.set_enabled(true);
        }
      }
      return true;
    }

    public void visibleForceVanish(bool _visible)
    {
      foreach (VirtualCameraController.VisibleObjectH visibleObjectH in this.lstMapVanish)
      {
        if (Object.op_Inequality((Object) visibleObjectH.vanishObj, (Object) null))
          visibleObjectH.vanishObj.SetActive(_visible);
        visibleObjectH.isVisible = _visible;
        visibleObjectH.delay = !_visible ? 0.0f : 0.3f;
      }
    }

    private void VisibleForceVanish(VirtualCameraController.VisibleObjectH _obj, bool _visible)
    {
      if (_obj == null || Object.op_Equality((Object) _obj.vanishObj, (Object) null))
        return;
      _obj.vanishObj.SetActive(_visible);
      _obj.delay = !_visible ? 0.0f : 0.3f;
      _obj.isVisible = _visible;
    }

    private bool VanishProc()
    {
      if (!this.isConfigVanish)
        return false;
      for (int i = 0; i < this.lstMapVanish.Count; ++i)
      {
        List<Collider> all = this.listCollider.FindAll((Predicate<Collider>) (x => Object.op_Inequality((Object) x, (Object) null) && this.lstMapVanish[i].nameCollider == ((Object) x).get_name()));
        if (all == null || all.Count == 0)
          this.VanishDelayVisible(this.lstMapVanish[i]);
        else if (this.lstMapVanish[i].isVisible)
          this.VisibleForceVanish(this.lstMapVanish[i], false);
      }
      if (Object.op_Inequality((Object) this.viewCollider, (Object) null) && this.housingMgr != null)
        this.housingMgr.ShieldProc((Collider) this.viewCollider);
      return true;
    }

    private bool VanishDelayVisible(VirtualCameraController.VisibleObjectH _visible)
    {
      if (_visible.isVisible)
        return false;
      _visible.delay += Time.get_deltaTime();
      if ((double) _visible.delay >= 0.300000011920929)
        this.VisibleForceVanish(_visible, true);
      return true;
    }

    protected virtual void OnDisable()
    {
      base.OnDisable();
      this.visibleForceVanish(true);
    }

    protected void OnTriggerEnter(Collider other)
    {
      if (this.listCollider.FindAll((Predicate<Collider>) (x => Object.op_Inequality((Object) x, (Object) null) && ((Object) other).get_name() == ((Object) x).get_name())) != null)
        return;
      this.listCollider.Add(other);
    }

    protected void OnTriggerStay(Collider other)
    {
      if (!Object.op_Equality((Object) this.listCollider.Find((Predicate<Collider>) (x => Object.op_Inequality((Object) x, (Object) null) && ((Object) other).get_name() == ((Object) x).get_name())), (Object) null))
        return;
      this.listCollider.Add(other);
    }

    protected void OnTriggerExit(Collider other)
    {
      this.listCollider.Clear();
    }

    public void ResetVanish()
    {
      for (int index = 0; index < this.lstMapVanish.Count; ++index)
      {
        if (!Object.op_Equality((Object) this.lstMapVanish[index].collider, (Object) null))
          this.lstMapVanish[index].collider.set_enabled(this.lstMapVanish[index].initEnable);
      }
    }

    [Serializable]
    public class VisibleObjectH : Manager.Map.VisibleObject
    {
      public bool initEnable;
    }

    [Serializable]
    protected struct CameraData
    {
      public Vector3 Pos;
      public Vector3 Dir;
      public Vector3 Rot;
      [HideInInspector]
      public float Fov;

      public void Copy(VirtualCameraController.ResetData copy)
      {
        this.Pos = copy.Pos;
        this.Dir = copy.Dir;
        this.Rot = copy.Rot;
        this.Fov = copy.Fov;
      }
    }

    protected struct ResetData
    {
      public Vector3 Pos;
      public Vector3 Dir;
      public Vector3 Rot;
      public Quaternion RotQ;
      public float Fov;

      public void Copy(VirtualCameraController.CameraData copy, Quaternion rot)
      {
        this.Pos = copy.Pos;
        this.Dir = copy.Dir;
        this.Rot = copy.Rot;
        this.RotQ = rot;
        this.Fov = copy.Fov;
      }
    }

    public enum Config
    {
      MoveXZ,
      Rotation,
      Translation,
      MoveXY,
    }

    public delegate bool NoCtrlFunc();
  }
}
