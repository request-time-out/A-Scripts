// Decompiled with JetBrains decompiler
// Type: CameraControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System.IO;
using UnityEngine;

public class CameraControl : BaseCameraControl
{
  public SmartTouch smartTouch;
  public PinchInOut pinchInOut;
  private Transform targetTex;
  public bool disableShortcut;

  public bool isOutsideTargetTex { get; set; }

  public void SetCenterSC()
  {
  }

  public void ChangeDepthOfFieldSetting()
  {
    if (!Object.op_Equality((Object) null, (Object) Singleton<Manager.Config>.Instance))
      ;
  }

  public void UpdateDepthOfFieldSetting()
  {
    if (!this.disableShortcut)
      ;
  }

  protected override void Start()
  {
    base.Start();
    this.targetTex = ((Component) this).get_transform().Find("CameraTarget");
    if (Object.op_Implicit((Object) this.targetTex))
      this.targetTex.set_localScale(Vector3.op_Multiply(Vector3.get_one(), 0.01f));
    this.isOutsideTargetTex = true;
  }

  protected new void LateUpdate()
  {
    this.UpdateDepthOfFieldSetting();
    if (Singleton<Scene>.Instance.sceneFade.IsFadeNow || Singleton<Scene>.Instance.IsOverlap)
      return;
    base.LateUpdate();
    if (this.disableShortcut || !Input.GetKeyDown((KeyCode) 54))
      return;
    this.SetCenterSC();
  }

  protected new bool InputTouchProc()
  {
    if (!base.InputTouchProc())
      return false;
    float deltaTime = Time.get_deltaTime();
    if (Object.op_Implicit((Object) this.pinchInOut))
    {
      float rate = this.pinchInOut.Rate;
      if (this.pinchInOut.NowState == PinchInOut.State.ScalUp)
      {
        ref Vector3 local = ref this.CamDat.Dir;
        local.z = (__Null) (local.z + (double) rate * (double) deltaTime * (double) this.zoomSpeed);
      }
      else if (this.pinchInOut.NowState == PinchInOut.State.ScalDown)
      {
        ref Vector3 local = ref this.CamDat.Dir;
        local.z = (__Null) (local.z - (double) rate * (double) deltaTime * (double) this.zoomSpeed);
      }
    }
    return true;
  }

  private void Save(BinaryWriter Writer)
  {
    Writer.Write((float) this.CamDat.Pos.x);
    Writer.Write((float) this.CamDat.Pos.y);
    Writer.Write((float) this.CamDat.Pos.z);
    Writer.Write((float) this.CamDat.Dir.x);
    Writer.Write((float) this.CamDat.Dir.y);
    Writer.Write((float) this.CamDat.Dir.z);
    Quaternion rotation = ((Component) this).get_transform().get_rotation();
    Vector3 eulerAngles = ((Quaternion) ref rotation).get_eulerAngles();
    Writer.Write((float) eulerAngles.x);
    Writer.Write((float) eulerAngles.y);
    Writer.Write((float) eulerAngles.z);
  }

  private void Load(BinaryReader Reader)
  {
    this.CamDat.Pos.x = (__Null) (double) Reader.ReadSingle();
    this.CamDat.Pos.y = (__Null) (double) Reader.ReadSingle();
    this.CamDat.Pos.z = (__Null) (double) Reader.ReadSingle();
    this.CamDat.Dir.x = (__Null) (double) Reader.ReadSingle();
    this.CamDat.Dir.y = (__Null) (double) Reader.ReadSingle();
    this.CamDat.Dir.z = (__Null) (double) Reader.ReadSingle();
    Quaternion rotation = ((Component) this).get_transform().get_rotation();
    Vector3 eulerAngles = ((Quaternion) ref rotation).get_eulerAngles();
    eulerAngles.x = (__Null) (double) Reader.ReadSingle();
    eulerAngles.y = (__Null) (double) Reader.ReadSingle();
    eulerAngles.z = (__Null) (double) Reader.ReadSingle();
    ((Component) this).get_transform().set_rotation(Quaternion.Euler(eulerAngles));
  }
}
