// Decompiled with JetBrains decompiler
// Type: AIChara.BustGravity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIChara
{
  public class BustGravity
  {
    private float[] range = new float[2]{ 0.0f, -0.05f };
    private ChaControl chaCtrl;
    private ChaInfo info;

    public BustGravity(ChaControl _ctrl)
    {
      this.chaCtrl = _ctrl;
      this.info = (ChaInfo) this.chaCtrl;
    }

    public void Change(float gravity, params int[] changePtn)
    {
      if (Object.op_Equality((Object) null, (Object) this.chaCtrl) || Object.op_Equality((Object) null, (Object) this.info))
        return;
      this.info.fileBody.bustWeight = gravity;
      this.ReCalc(changePtn);
    }

    public void ReCalc(params int[] changePtn)
    {
      if (Object.op_Equality((Object) null, (Object) this.chaCtrl) || Object.op_Equality((Object) null, (Object) this.info) || changePtn.Length == 0)
        return;
      float num = Mathf.Lerp(this.range[0], this.range[1], this.info.fileBody.bustWeight) * (float) ((double) this.info.fileBody.shapeValueBody[1] * (double) this.info.fileBody.bustSoftness * 0.5);
      DynamicBone_Ver02[] dynamicBoneVer02Array = new DynamicBone_Ver02[2]
      {
        this.chaCtrl.GetDynamicBoneBustAndHip(ChaControlDefine.DynamicBoneKind.BreastL),
        this.chaCtrl.GetDynamicBoneBustAndHip(ChaControlDefine.DynamicBoneKind.BreastR)
      };
      foreach (int _ptn in changePtn)
      {
        foreach (DynamicBone_Ver02 dynamicBoneVer02 in dynamicBoneVer02Array)
        {
          if (Object.op_Inequality((Object) dynamicBoneVer02, (Object) null))
            dynamicBoneVer02.setGravity(_ptn, new Vector3(0.0f, num, 0.0f), true);
        }
      }
    }
  }
}
