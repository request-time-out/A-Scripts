// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomGuideAssist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace CharaCustom
{
  public class CustomGuideAssist
  {
    public static void SetCameraMoveFlag(CameraControl_Ver2 _ctrl, bool _bPlay)
    {
      if (Object.op_Equality((Object) _ctrl, (Object) null) || CustomGuideAssist.IsCameraMoveFlag(_ctrl) == _bPlay)
        return;
      _ctrl.NoCtrlCondition = (BaseCameraControl_Ver2.NoCtrlFunc) (() => !_bPlay);
    }

    public static bool IsCameraMoveFlag(CameraControl_Ver2 _ctrl)
    {
      if (Object.op_Equality((Object) _ctrl, (Object) null))
        return false;
      BaseCameraControl_Ver2.NoCtrlFunc noCtrlCondition = _ctrl.NoCtrlCondition;
      bool flag = true;
      if (noCtrlCondition != null)
        flag = noCtrlCondition();
      return !flag;
    }

    public static bool IsCameraActionFlag(CameraControl_Ver2 _ctrl)
    {
      return !Object.op_Equality((Object) _ctrl, (Object) null) && _ctrl.isControlNow;
    }
  }
}
