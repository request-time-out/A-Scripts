// Decompiled with JetBrains decompiler
// Type: Housing.CraftCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Housing
{
  public class CraftCamera : VirtualCameraController
  {
    public bool bLock;
    public int nFloorCnt;
    public Vector3 initPos;
    public Vector3 initRot;
    public float initDis;
    public Vector3 limitAreaMin;
    public Vector3 limitAreaMax;
    public float zoomSpeed;

    private new void Start()
    {
      this.Init();
      this.bLock = false;
      this.nFloorCnt = 0;
      this.isLimitDir = true;
      this.limitDir = 190f;
      this.isLimitPos = true;
      this.limitPos = 50f;
      this.isLimitRotX = true;
      this.limitRotX = 180f;
      this.CamDat.Pos = this.initPos;
      this.CamDat.Dir = new Vector3(0.0f, 0.0f, this.initDis);
      this.CamDat.Rot = this.initRot;
      this.CamDat.Fov = (float) this.lens.FieldOfView;
      this.CamReset.Copy(this.CamDat, Quaternion.get_identity());
      this.craft = true;
    }

    protected override void LateUpdate()
    {
      if (this.bLock)
        return;
      this.isControlNow = false;
      if (!this.isControlNow)
      {
        VirtualCameraController.NoCtrlFunc noCtrlCondition = this.NoCtrlCondition;
        bool flag = false;
        if (noCtrlCondition != null)
        {
          foreach (VirtualCameraController.NoCtrlFunc invocation in noCtrlCondition.GetInvocationList())
            flag |= invocation();
        }
        if (!flag)
        {
          if (this.InputTouchProc())
            this.isControlNow = true;
          else if (this.InputMouseProc())
            this.isControlNow = true;
        }
      }
      if (!this.isControlNow)
      {
        VirtualCameraController.NoCtrlFunc keyCondition = this.KeyCondition;
        bool flag = true;
        if (keyCondition != null)
        {
          foreach (VirtualCameraController.NoCtrlFunc invocation in keyCondition.GetInvocationList())
            flag &= invocation();
        }
        CraftCamera craftCamera = this;
        craftCamera.isControlNow = ((craftCamera.isControlNow ? 1 : 0) | (!flag ? 0 : (this.InputKeyProc() ? 1 : 0))) != 0;
      }
      if (!this.isControlNow)
      {
        VirtualCameraController.NoCtrlFunc zoomCondition = this.ZoomCondition;
        bool flag1 = true;
        if (zoomCondition != null)
        {
          foreach (VirtualCameraController.NoCtrlFunc invocation in zoomCondition.GetInvocationList())
            flag1 &= invocation();
        }
        bool flag2 = Object.op_Implicit((Object) EventSystem.get_current()) && EventSystem.get_current().IsPointerOverGameObject();
        CraftCamera craftCamera = this;
        craftCamera.isControlNow = ((craftCamera.isControlNow ? 1 : 0) | (flag2 || !flag1 ? 0 : (this.InputMouseWheelZoomProc() ? 1 : 0))) != 0;
      }
      if (Object.op_Inequality((Object) EventSystem.get_current(), (Object) null) && !EventSystem.get_current().IsPointerOverGameObject())
        return;
      this.isControlNow = false;
    }

    public void CameraUp(int TargetFloorCnt)
    {
      if (this.isControlNow)
        return;
      this.nFloorCnt = TargetFloorCnt;
      this.CamDat.Pos.y = (__Null) (this.CamReset.Pos.y + (double) (5 * this.nFloorCnt));
      this.ForceMoveCam(0.0f);
    }

    private bool InputMouseWheelZoomProc()
    {
      bool flag = false;
      this.isZoomNow = false;
      float num = this.input.ScrollValue() * this.zoomSpeed;
      if ((double) num != 0.0)
      {
        this.CamDat.Fov -= num;
        this.CamDat.Fov = Mathf.Clamp(this.CamDat.Fov, this.CamReset.Fov, this.limitFov);
        this.Lens.FieldOfView = (__Null) (double) this.CamDat.Fov;
        ((CameraState) ref this.m_State).set_Lens(this.Lens);
        flag = true;
        this.isZoomNow = true;
      }
      return flag;
    }

    public void setLimitPos(float limit)
    {
      this.limitPos += limit;
    }

    public override void ForceMoveCam(float deltaTime = 0.0f)
    {
      if (this.isLimitDir)
        this.CamDat.Dir.z = (__Null) (double) Mathf.Clamp((float) this.CamDat.Dir.z, -this.limitDir, 10f);
      if (this.isLimitPos)
      {
        this.CamDat.Pos.x = (__Null) (double) Mathf.Clamp((float) this.CamDat.Pos.x, (float) this.limitAreaMin.x, (float) this.limitAreaMax.x);
        this.CamDat.Pos.y = (__Null) (double) Mathf.Clamp((float) this.CamDat.Pos.y, (float) this.limitAreaMin.y, (float) this.limitAreaMax.y);
        this.CamDat.Pos.z = (__Null) (double) Mathf.Clamp((float) this.CamDat.Pos.z, (float) this.limitAreaMin.z, (float) this.limitAreaMax.z);
      }
      if (this.isLimitRotX)
        this.CamDat.Rot.x = (__Null) (double) Mathf.Clamp((float) this.CamDat.Rot.x, 0.0f, this.limitRotX);
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
      bool flag = (double) Vector3.Distance(((Component) this.lookAtTarget).get_transform().get_position(), position) < 1.0 / 1000.0;
      if (Object.op_Implicit((Object) this.targetRender))
        this.targetRender.set_enabled(this.isControlNow & this.isOutsideTargetTex & this.isConfigTargetTex & !this.isZoomNow & !flag);
      if (Singleton<GameCursor>.IsInstance() && this.isCursorLock)
        Singleton<GameCursor>.Instance.SetCursorLock(this.isControlNow & this.isOutsideTargetTex);
      foreach (CinemachineCore.Stage stage in Enum.GetValues(typeof (CinemachineCore.Stage)))
        this.InvokePostPipelineStageCallback((CinemachineVirtualCameraBase) this, stage, ref this.m_State, deltaTime);
    }
  }
}
