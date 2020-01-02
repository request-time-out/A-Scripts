// Decompiled with JetBrains decompiler
// Type: AIChara.BustSoft
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIChara
{
  public class BustSoft
  {
    private float[] bustDamping = new float[3]
    {
      0.2f,
      0.1f,
      0.1f
    };
    private float[] bustElasticity = new float[3]
    {
      0.2f,
      0.15f,
      0.05f
    };
    private float[] bustStiffness = new float[3]
    {
      1f,
      0.1f,
      0.01f
    };
    private ChaControl chaCtrl;
    private ChaInfo info;

    public BustSoft(ChaControl _ctrl)
    {
      this.chaCtrl = _ctrl;
      this.info = (ChaInfo) this.chaCtrl;
    }

    public void Change(float soft, params int[] changePtn)
    {
      if (Object.op_Equality((Object) null, (Object) this.chaCtrl) || Object.op_Equality((Object) null, (Object) this.info))
        return;
      this.info.fileBody.bustSoftness = soft;
      this.ReCalc(changePtn);
    }

    public void ReCalc(params int[] changePtn)
    {
      if (Object.op_Equality((Object) null, (Object) this.chaCtrl) || Object.op_Equality((Object) null, (Object) this.info) || changePtn.Length == 0)
        return;
      float rate = Mathf.Clamp((float) ((double) this.info.fileBody.bustSoftness * (double) this.info.fileBody.shapeValueBody[1] + 0.00999999977648258), 0.0f, 1f);
      float _stiffness = this.TreeLerp(this.bustStiffness, rate);
      float _elasticity = this.TreeLerp(this.bustElasticity, rate);
      float _damping = this.TreeLerp(this.bustDamping, rate);
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
            dynamicBoneVer02.setSoftParams(_ptn, -1, _damping, _elasticity, _stiffness, true);
        }
      }
    }

    private float TreeLerp(float[] vals, float rate)
    {
      return (double) rate < 0.5 ? Mathf.Lerp(vals[0], vals[1], rate * 2f) : Mathf.Lerp(vals[1], vals[2], (float) (((double) rate - 0.5) * 2.0));
    }
  }
}
