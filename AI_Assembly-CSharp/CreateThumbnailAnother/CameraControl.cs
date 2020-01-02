// Decompiled with JetBrains decompiler
// Type: CreateThumbnailAnother.CameraControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System.IO;
using UnityEngine;

namespace CreateThumbnailAnother
{
  public class CameraControl : BaseCameraControl_Ver2
  {
    public bool isOutsideTargetTex;
    public bool isCursorLock;
    private Renderer targetRender;

    public Transform targetTex { get; private set; }

    public void SetTarget(Vector3 _pos, Vector3 _rot, float _dis, float _fov)
    {
      this.TargetPos = _pos;
      this.Rot = _rot;
      this.CameraDir = new Vector3(0.0f, 0.0f, -_dis);
      this.CameraFov = _fov;
      this.CameraUpdate();
    }

    protected new void Start()
    {
      base.Start();
      this.targetTex = ((Component) this).get_transform().Find("CameraTarget");
      if (Object.op_Implicit((Object) this.targetTex))
      {
        this.targetTex.set_localScale(Vector3.op_Multiply(Vector3.get_one(), 0.01f));
        this.targetRender = (Renderer) ((Component) this.targetTex).GetComponent<Renderer>();
      }
      this.isOutsideTargetTex = true;
      this.isCursorLock = true;
      this.isInit = true;
    }

    protected new void LateUpdate()
    {
      if (!Singleton<Scene>.IsInstance() || !Singleton<Scene>.Instance.IsNowLoading & !Singleton<Scene>.Instance.IsNowLoadingFade)
      {
        this.isControlNow = false;
        this.SetCtrlSpeed();
        if (!this.isControlNow)
        {
          BaseCameraControl_Ver2.NoCtrlFunc zoomCondition = this.ZoomCondition;
          bool flag = true;
          if (zoomCondition != null)
            flag = zoomCondition();
          CameraControl cameraControl = this;
          cameraControl.isControlNow = ((cameraControl.isControlNow ? 1 : 0) | (!flag ? 0 : (this.InputMouseWheelZoomProc() ? 1 : 0))) != 0;
        }
        if (!this.isControlNow)
        {
          BaseCameraControl_Ver2.NoCtrlFunc noCtrlCondition = this.NoCtrlCondition;
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
        if (!this.isControlNow)
        {
          BaseCameraControl_Ver2.NoCtrlFunc keyCondition = this.KeyCondition;
          bool flag = true;
          if (keyCondition != null)
            flag = keyCondition();
          CameraControl cameraControl = this;
          cameraControl.isControlNow = ((cameraControl.isControlNow ? 1 : 0) | (!flag ? 0 : (this.InputKeyProc() ? 1 : 0))) != 0;
        }
        this.CameraUpdate();
      }
      if (!Object.op_Implicit((Object) this.targetTex))
        return;
      if (Object.op_Inequality((Object) this.transBase, (Object) null))
        this.targetTex.set_position(this.transBase.TransformPoint(this.CamDat.Pos));
      else
        this.targetTex.set_position(this.CamDat.Pos);
      Vector3 position = ((Component) this).get_transform().get_position();
      position.y = this.targetTex.get_position().y;
      ((Component) this.targetTex).get_transform().LookAt(position);
      this.targetTex.Rotate(new Vector3(90f, 0.0f, 0.0f));
      if (Object.op_Implicit((Object) this.targetRender))
        this.targetRender.set_enabled(this.isControlNow & this.isOutsideTargetTex);
      if (!Singleton<GameCursor>.IsInstance() || !this.isCursorLock)
        return;
      Singleton<GameCursor>.Instance.SetCursorLock(this.isControlNow & this.isOutsideTargetTex);
    }

    protected new void CameraUpdate()
    {
      if (this.isLimitDir)
        this.CamDat.Dir.z = (__Null) (double) Mathf.Clamp((float) this.CamDat.Dir.z, -this.limitDir, 0.0f);
      if (this.isLimitPos)
        this.CamDat.Pos = Vector3.ClampMagnitude(this.CamDat.Pos, this.limitPos);
      if (Object.op_Inequality((Object) this.transBase, (Object) null))
      {
        ((Component) this).get_transform().set_rotation(Quaternion.op_Multiply(this.transBase.get_rotation(), Quaternion.Euler(this.CamDat.Rot)));
        ((Component) this).get_transform().set_position(Vector3.op_Addition(Quaternion.op_Multiply(((Component) this).get_transform().get_rotation(), this.CamDat.Dir), this.transBase.TransformPoint(this.CamDat.Pos)));
      }
      else
      {
        ((Component) this).get_transform().set_rotation(Quaternion.Euler(this.CamDat.Rot));
        ((Component) this).get_transform().set_position(Vector3.op_Addition(Quaternion.op_Multiply(((Component) this).get_transform().get_rotation(), this.CamDat.Dir), this.CamDat.Pos));
      }
    }

    public void CameraDataSaveBinary(BinaryWriter bw)
    {
      bw.Write((float) this.CamDat.Pos.x);
      bw.Write((float) this.CamDat.Pos.y);
      bw.Write((float) this.CamDat.Pos.z);
      bw.Write((float) this.CamDat.Dir.x);
      bw.Write((float) this.CamDat.Dir.y);
      bw.Write((float) this.CamDat.Dir.z);
      bw.Write((float) this.CamDat.Rot.x);
      bw.Write((float) this.CamDat.Rot.y);
      bw.Write((float) this.CamDat.Rot.z);
      bw.Write(this.CamDat.Fov);
    }

    public void CameraDataLoadBinary(BinaryReader br, bool isUpdate)
    {
      BaseCameraControl_Ver2.CameraData cameraData = new BaseCameraControl_Ver2.CameraData();
      cameraData.Pos.x = (__Null) (double) br.ReadSingle();
      cameraData.Pos.y = (__Null) (double) br.ReadSingle();
      cameraData.Pos.z = (__Null) (double) br.ReadSingle();
      cameraData.Dir.x = (__Null) (double) br.ReadSingle();
      cameraData.Dir.y = (__Null) (double) br.ReadSingle();
      cameraData.Dir.z = (__Null) (double) br.ReadSingle();
      cameraData.Rot.x = (__Null) (double) br.ReadSingle();
      cameraData.Rot.y = (__Null) (double) br.ReadSingle();
      cameraData.Rot.z = (__Null) (double) br.ReadSingle();
      cameraData.Fov = br.ReadSingle();
      this.CamReset.Copy(cameraData, Quaternion.get_identity());
      if (!isUpdate)
        return;
      this.CamDat.Copy(cameraData);
      if (Object.op_Inequality((Object) this.thisCamera, (Object) null))
        this.thisCamera.set_fieldOfView(cameraData.Fov);
      this.CameraUpdate();
      if (this.isInit)
        return;
      this.isInit = true;
    }
  }
}
