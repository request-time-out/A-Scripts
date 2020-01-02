// Decompiled with JetBrains decompiler
// Type: AIChara.CmpFace
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace AIChara
{
  [DisallowMultipleComponent]
  public class CmpFace : CmpBase
  {
    [Header("カスタムで使用")]
    public CmpFace.TargetCustom targetCustom = new CmpFace.TargetCustom();
    [Header("その他")]
    public CmpFace.TargetEtc targetEtc = new CmpFace.TargetEtc();

    public CmpFace()
      : base(false)
    {
    }

    public override void SetReferenceObject()
    {
      FindAssist findAssist = new FindAssist();
      findAssist.Initialize(((Component) this).get_transform());
      this.targetCustom.rendEyes = new Renderer[2];
      GameObject objectFromName1 = findAssist.GetObjectFromName("o_eyebase_L");
      if (Object.op_Inequality((Object) null, (Object) objectFromName1))
        this.targetCustom.rendEyes[0] = (Renderer) objectFromName1.GetComponent<Renderer>();
      GameObject objectFromName2 = findAssist.GetObjectFromName("o_eyebase_R");
      if (Object.op_Inequality((Object) null, (Object) objectFromName2))
        this.targetCustom.rendEyes[1] = (Renderer) objectFromName2.GetComponent<Renderer>();
      GameObject objectFromName3 = findAssist.GetObjectFromName("o_eyelashes");
      if (Object.op_Inequality((Object) null, (Object) objectFromName3))
        this.targetCustom.rendEyelashes = (Renderer) objectFromName3.GetComponent<Renderer>();
      GameObject objectFromName4 = findAssist.GetObjectFromName("o_eyeshadow");
      if (Object.op_Inequality((Object) null, (Object) objectFromName4))
        this.targetCustom.rendShadow = (Renderer) objectFromName4.GetComponent<Renderer>();
      GameObject objectFromName5 = findAssist.GetObjectFromName("o_head");
      if (Object.op_Inequality((Object) null, (Object) objectFromName5))
        this.targetCustom.rendHead = (Renderer) objectFromName5.GetComponent<Renderer>();
      GameObject objectFromName6 = findAssist.GetObjectFromName("o_namida");
      if (Object.op_Inequality((Object) null, (Object) objectFromName6))
        this.targetEtc.rendTears = (Renderer) objectFromName6.GetComponent<Renderer>();
      this.targetEtc.objTongue = findAssist.GetObjectFromName("o_tang");
    }

    [Serializable]
    public class TargetCustom
    {
      public Renderer[] rendEyes;
      public Renderer rendEyelashes;
      public Renderer rendShadow;
      public Renderer rendHead;
    }

    [Serializable]
    public class TargetEtc
    {
      public Renderer rendTears;
      public GameObject objTongue;
    }
  }
}
