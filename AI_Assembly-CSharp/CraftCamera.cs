// Decompiled with JetBrains decompiler
// Type: CraftCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftCamera : VirtualCameraController
{
  public bool bLock;
  public int nFloorCnt;
  public Vector3 initPos;
  public float zoomSpeed;

  private new void Start()
  {
    this.Init();
  }

  private new void Init()
  {
    base.Init();
    this.bLock = false;
    this.nFloorCnt = 0;
    this.limitPos = 10f;
    this.limitDir = 50f;
    this.isLimitPos = true;
    this.isLimitDir = true;
    this.isLimitRotX = true;
    this.CamDat.Pos = this.initPos;
    this.craft = true;
  }

  protected override void LateUpdate()
  {
    if (this.bLock)
      return;
    base.LateUpdate();
    if (!this.isControlNow)
      this.isControlNow |= this.InputMouseWheelZoomProc();
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
}
