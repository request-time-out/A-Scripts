// Decompiled with JetBrains decompiler
// Type: AIChara.CmpBody
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace AIChara
{
  [DisallowMultipleComponent]
  public class CmpBody : CmpBase
  {
    [Header("カスタムで使用")]
    public CmpBody.TargetCustom targetCustom = new CmpBody.TargetCustom();
    [Header("その他")]
    public CmpBody.TargetEtc targetEtc = new CmpBody.TargetEtc();

    public CmpBody()
      : base(false)
    {
    }

    public override void SetReferenceObject()
    {
      FindAssist findAssist = new FindAssist();
      findAssist.Initialize(((Component) this).get_transform());
      this.targetEtc.objBody = findAssist.GetObjectFromName("o_body_cm");
      if (Object.op_Equality((Object) null, (Object) this.targetEtc.objBody))
        this.targetEtc.objBody = findAssist.GetObjectFromName("o_body_cf");
      if (Object.op_Equality((Object) null, (Object) this.targetEtc.objBody))
        this.targetEtc.objBody = findAssist.GetObjectFromName("o_silhouette_cm");
      if (Object.op_Equality((Object) null, (Object) this.targetEtc.objBody))
        this.targetEtc.objBody = findAssist.GetObjectFromName("o_silhouette_cf");
      if (Object.op_Inequality((Object) null, (Object) this.targetEtc.objBody))
        this.targetCustom.rendBody = (Renderer) this.targetEtc.objBody.GetComponent<Renderer>();
      this.targetEtc.objDanTop = findAssist.GetObjectFromName("N_dan");
      this.targetEtc.objDanTama = findAssist.GetObjectFromName("cm_o_dan_f");
      this.targetEtc.objDanSao = findAssist.GetObjectFromName("cm_o_dan00");
      this.targetEtc.objTongue = findAssist.GetObjectFromName("N_tang");
      if (Object.op_Inequality((Object) null, (Object) this.targetEtc.objTongue))
        this.targetEtc.rendTongue = (Renderer) this.targetEtc.objTongue.GetComponentInChildren<Renderer>();
      this.targetEtc.objMNPB = findAssist.GetObjectFromName("N_mnpb");
    }

    [Serializable]
    public class TargetCustom
    {
      public Renderer rendBody;
    }

    [Serializable]
    public class TargetEtc
    {
      public GameObject objBody;
      public GameObject objDanTop;
      public GameObject objDanTama;
      public GameObject objDanSao;
      public GameObject objTongue;
      public GameObject objMNPB;
      public Renderer rendTongue;
    }
  }
}
