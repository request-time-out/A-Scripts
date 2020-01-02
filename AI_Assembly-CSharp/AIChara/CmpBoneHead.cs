// Decompiled with JetBrains decompiler
// Type: AIChara.CmpBoneHead
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace AIChara
{
  [DisallowMultipleComponent]
  public class CmpBoneHead : CmpBase
  {
    [Header("アクセサリの親")]
    public CmpBoneHead.TargetAccessory targetAccessory = new CmpBoneHead.TargetAccessory();
    [Header("その他ターゲット")]
    public CmpBoneHead.TargetEtc targetEtc = new CmpBoneHead.TargetEtc();

    public CmpBoneHead()
      : base(false)
    {
    }

    public override void SetReferenceObject()
    {
      FindAssist findAssist = new FindAssist();
      findAssist.Initialize(((Component) this).get_transform());
      this.targetAccessory.acs_Hair_pony = findAssist.GetTransformFromName("N_Hair_pony");
      this.targetAccessory.acs_Hair_twin_L = findAssist.GetTransformFromName("N_Hair_twin_L");
      this.targetAccessory.acs_Hair_twin_R = findAssist.GetTransformFromName("N_Hair_twin_R");
      this.targetAccessory.acs_Hair_pin_L = findAssist.GetTransformFromName("N_Hair_pin_L");
      this.targetAccessory.acs_Hair_pin_R = findAssist.GetTransformFromName("N_Hair_pin_R");
      this.targetAccessory.acs_Head_top = findAssist.GetTransformFromName("N_Head_top");
      this.targetAccessory.acs_Head = findAssist.GetTransformFromName("N_Head");
      this.targetAccessory.acs_Hitai = findAssist.GetTransformFromName("N_Hitai");
      this.targetAccessory.acs_Face = findAssist.GetTransformFromName("N_Face");
      this.targetAccessory.acs_Megane = findAssist.GetTransformFromName("N_Megane");
      this.targetAccessory.acs_Earring_L = findAssist.GetTransformFromName("N_Earring_L");
      this.targetAccessory.acs_Earring_R = findAssist.GetTransformFromName("N_Earring_R");
      this.targetAccessory.acs_Nose = findAssist.GetTransformFromName("N_Nose");
      this.targetAccessory.acs_Mouth = findAssist.GetTransformFromName("N_Mouth");
      this.targetEtc.trfHairParent = findAssist.GetTransformFromName("N_hair_Root");
      this.targetEtc.trfMouthAdjustWidth = findAssist.GetTransformFromName("cf_J_MouthMove");
    }

    [Serializable]
    public class TargetAccessory
    {
      public Transform acs_Hair_pony;
      public Transform acs_Hair_twin_L;
      public Transform acs_Hair_twin_R;
      public Transform acs_Hair_pin_L;
      public Transform acs_Hair_pin_R;
      public Transform acs_Head_top;
      public Transform acs_Head;
      public Transform acs_Hitai;
      public Transform acs_Face;
      public Transform acs_Megane;
      public Transform acs_Earring_L;
      public Transform acs_Earring_R;
      public Transform acs_Nose;
      public Transform acs_Mouth;
    }

    [Serializable]
    public class TargetEtc
    {
      public Transform trfHairParent;
      public Transform trfMouthAdjustWidth;
    }
  }
}
