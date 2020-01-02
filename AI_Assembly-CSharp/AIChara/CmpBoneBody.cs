// Decompiled with JetBrains decompiler
// Type: AIChara.CmpBoneBody
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace AIChara
{
  [DisallowMultipleComponent]
  public class CmpBoneBody : CmpBase
  {
    [Header("アクセサリの親")]
    public CmpBoneBody.TargetAccessory targetAccessory = new CmpBoneBody.TargetAccessory();
    [Header("その他ターゲット")]
    public CmpBoneBody.TargetEtc targetEtc = new CmpBoneBody.TargetEtc();
    private DynamicBone_Ver02[] dynamicBonesBustAndHip;
    [Header("男無効用胸の判定")]
    public DynamicBoneCollider[] dbcBust;

    public CmpBoneBody()
      : base(false)
    {
    }

    public override void SetReferenceObject()
    {
      FindAssist findAssist = new FindAssist();
      findAssist.Initialize(((Component) this).get_transform());
      this.targetAccessory.acs_Neck = findAssist.GetTransformFromName("N_Neck");
      this.targetAccessory.acs_Chest_f = findAssist.GetTransformFromName("N_Chest_f");
      this.targetAccessory.acs_Chest = findAssist.GetTransformFromName("N_Chest");
      this.targetAccessory.acs_Tikubi_L = findAssist.GetTransformFromName("N_Tikubi_L");
      this.targetAccessory.acs_Tikubi_R = findAssist.GetTransformFromName("N_Tikubi_R");
      this.targetAccessory.acs_Back = findAssist.GetTransformFromName("N_Back");
      this.targetAccessory.acs_Back_L = findAssist.GetTransformFromName("N_Back_L");
      this.targetAccessory.acs_Back_R = findAssist.GetTransformFromName("N_Back_R");
      this.targetAccessory.acs_Waist = findAssist.GetTransformFromName("N_Waist");
      this.targetAccessory.acs_Waist_f = findAssist.GetTransformFromName("N_Waist_f");
      this.targetAccessory.acs_Waist_b = findAssist.GetTransformFromName("N_Waist_b");
      this.targetAccessory.acs_Waist_L = findAssist.GetTransformFromName("N_Waist_L");
      this.targetAccessory.acs_Waist_R = findAssist.GetTransformFromName("N_Waist_R");
      this.targetAccessory.acs_Leg_L = findAssist.GetTransformFromName("N_Leg_L");
      this.targetAccessory.acs_Leg_R = findAssist.GetTransformFromName("N_Leg_R");
      this.targetAccessory.acs_Knee_L = findAssist.GetTransformFromName("N_Knee_L");
      this.targetAccessory.acs_Knee_R = findAssist.GetTransformFromName("N_Knee_R");
      this.targetAccessory.acs_Ankle_L = findAssist.GetTransformFromName("N_Ankle_L");
      this.targetAccessory.acs_Ankle_R = findAssist.GetTransformFromName("N_Ankle_R");
      this.targetAccessory.acs_Foot_L = findAssist.GetTransformFromName("N_Foot_L");
      this.targetAccessory.acs_Foot_R = findAssist.GetTransformFromName("N_Foot_R");
      this.targetAccessory.acs_Shoulder_L = findAssist.GetTransformFromName("N_Shoulder_L");
      this.targetAccessory.acs_Shoulder_R = findAssist.GetTransformFromName("N_Shoulder_R");
      this.targetAccessory.acs_Elbo_L = findAssist.GetTransformFromName("N_Elbo_L");
      this.targetAccessory.acs_Elbo_R = findAssist.GetTransformFromName("N_Elbo_R");
      this.targetAccessory.acs_Arm_L = findAssist.GetTransformFromName("N_Arm_L");
      this.targetAccessory.acs_Arm_R = findAssist.GetTransformFromName("N_Arm_R");
      this.targetAccessory.acs_Wrist_L = findAssist.GetTransformFromName("N_Wrist_L");
      this.targetAccessory.acs_Wrist_R = findAssist.GetTransformFromName("N_Wrist_R");
      this.targetAccessory.acs_Hand_L = findAssist.GetTransformFromName("N_Hand_L");
      this.targetAccessory.acs_Hand_R = findAssist.GetTransformFromName("N_Hand_R");
      this.targetAccessory.acs_Index_L = findAssist.GetTransformFromName("N_Index_L");
      this.targetAccessory.acs_Index_R = findAssist.GetTransformFromName("N_Index_R");
      this.targetAccessory.acs_Middle_L = findAssist.GetTransformFromName("N_Middle_L");
      this.targetAccessory.acs_Middle_R = findAssist.GetTransformFromName("N_Middle_R");
      this.targetAccessory.acs_Ring_L = findAssist.GetTransformFromName("N_Ring_L");
      this.targetAccessory.acs_Ring_R = findAssist.GetTransformFromName("N_Ring_R");
      this.targetAccessory.acs_Dan = findAssist.GetTransformFromName("N_Dan");
      this.targetAccessory.acs_Kokan = findAssist.GetTransformFromName("N_Kokan");
      this.targetAccessory.acs_Ana = findAssist.GetTransformFromName("N_Ana");
      this.targetEtc.trfRoot = findAssist.GetTransformFromName("cf_J_Hips");
      this.targetEtc.trfHeadParent = findAssist.GetTransformFromName("cf_J_Head_s");
      this.targetEtc.trfNeckLookTarget = findAssist.GetTransformFromName("cf_J_Spine03");
      this.targetEtc.trfAnaCorrect = findAssist.GetTransformFromName("cf_J_Ana");
      this.targetEtc.trf_k_shoulderL_00 = findAssist.GetTransformFromName("k_f_shoulderL_00");
      this.targetEtc.trf_k_shoulderR_00 = findAssist.GetTransformFromName("k_f_shoulderR_00");
      this.targetEtc.trf_k_handL_00 = findAssist.GetTransformFromName("k_f_handL_00");
      this.targetEtc.trf_k_handR_00 = findAssist.GetTransformFromName("k_f_handR_00");
      this.dbcBust = new DynamicBoneCollider[4];
      this.dbcBust[0] = (DynamicBoneCollider) findAssist.GetObjectFromName("cf_hit_Mune02_s_L").GetComponent<DynamicBoneCollider>();
      this.dbcBust[1] = (DynamicBoneCollider) findAssist.GetObjectFromName("cf_hit_Mune021_s_L").GetComponent<DynamicBoneCollider>();
      this.dbcBust[2] = (DynamicBoneCollider) findAssist.GetObjectFromName("cf_hit_Mune02_s_R").GetComponent<DynamicBoneCollider>();
      this.dbcBust[3] = (DynamicBoneCollider) findAssist.GetObjectFromName("cf_hit_Mune021_s_R").GetComponent<DynamicBoneCollider>();
    }

    public void InactiveBustDynamicBoneCollider()
    {
      foreach (DynamicBoneCollider dynamicBoneCollider in this.dbcBust)
      {
        if (Object.op_Inequality((Object) null, (Object) dynamicBoneCollider))
          ((Behaviour) dynamicBoneCollider).set_enabled(false);
      }
    }

    public void InitDynamicBonesBustAndHip()
    {
      DynamicBone_Ver02[] componentsInChildren = (DynamicBone_Ver02[]) ((Component) this).GetComponentsInChildren<DynamicBone_Ver02>(true);
      this.dynamicBonesBustAndHip = new DynamicBone_Ver02[Enum.GetNames(typeof (ChaControlDefine.DynamicBoneKind)).Length];
      foreach (DynamicBone_Ver02 dynamicBoneVer02 in componentsInChildren)
      {
        if (dynamicBoneVer02.Comment == "Mune_L")
          this.dynamicBonesBustAndHip[0] = dynamicBoneVer02;
        else if (dynamicBoneVer02.Comment == "Mune_R")
          this.dynamicBonesBustAndHip[1] = dynamicBoneVer02;
        else if (dynamicBoneVer02.Comment == "Siri_L")
          this.dynamicBonesBustAndHip[2] = dynamicBoneVer02;
        else if (dynamicBoneVer02.Comment == "Siri_R")
          this.dynamicBonesBustAndHip[3] = dynamicBoneVer02;
      }
    }

    public DynamicBone_Ver02 GetDynamicBoneBustAndHip(
      ChaControlDefine.DynamicBoneKind area)
    {
      return this.GetDynamicBoneBustAndHip((int) area);
    }

    public DynamicBone_Ver02 GetDynamicBoneBustAndHip(int area)
    {
      return area >= this.dynamicBonesBustAndHip.Length ? (DynamicBone_Ver02) null : this.dynamicBonesBustAndHip[area];
    }

    public void ResetDynamicBonesBustAndHip(bool includeInactive = false)
    {
      if (this.dynamicBonesBustAndHip == null)
        return;
      foreach (DynamicBone_Ver02 dynamicBoneVer02 in this.dynamicBonesBustAndHip)
      {
        if (((Behaviour) dynamicBoneVer02).get_enabled() || includeInactive)
          dynamicBoneVer02.ResetParticlesPosition();
      }
    }

    public void EnableDynamicBonesBustAndHip(bool enable, int area)
    {
      if (this.dynamicBonesBustAndHip == null || area >= this.dynamicBonesBustAndHip.Length || (Object.op_Equality((Object) null, (Object) this.dynamicBonesBustAndHip[area]) || ((Behaviour) this.dynamicBonesBustAndHip[area]).get_enabled() == enable))
        return;
      ((Behaviour) this.dynamicBonesBustAndHip[area]).set_enabled(enable);
      if (!enable)
        return;
      this.dynamicBonesBustAndHip[area].ResetParticlesPosition();
    }

    [Serializable]
    public class TargetAccessory
    {
      public Transform acs_Neck;
      public Transform acs_Chest_f;
      public Transform acs_Chest;
      public Transform acs_Tikubi_L;
      public Transform acs_Tikubi_R;
      public Transform acs_Back;
      public Transform acs_Back_L;
      public Transform acs_Back_R;
      public Transform acs_Waist;
      public Transform acs_Waist_f;
      public Transform acs_Waist_b;
      public Transform acs_Waist_L;
      public Transform acs_Waist_R;
      public Transform acs_Leg_L;
      public Transform acs_Leg_R;
      public Transform acs_Knee_L;
      public Transform acs_Knee_R;
      public Transform acs_Ankle_L;
      public Transform acs_Ankle_R;
      public Transform acs_Foot_L;
      public Transform acs_Foot_R;
      public Transform acs_Shoulder_L;
      public Transform acs_Shoulder_R;
      public Transform acs_Elbo_L;
      public Transform acs_Elbo_R;
      public Transform acs_Arm_L;
      public Transform acs_Arm_R;
      public Transform acs_Wrist_L;
      public Transform acs_Wrist_R;
      public Transform acs_Hand_L;
      public Transform acs_Hand_R;
      public Transform acs_Index_L;
      public Transform acs_Index_R;
      public Transform acs_Middle_L;
      public Transform acs_Middle_R;
      public Transform acs_Ring_L;
      public Transform acs_Ring_R;
      public Transform acs_Dan;
      public Transform acs_Kokan;
      public Transform acs_Ana;
    }

    [Serializable]
    public class TargetEtc
    {
      public Transform trfRoot;
      public Transform trfHeadParent;
      public Transform trfNeckLookTarget;
      public Transform trfAnaCorrect;
      public Transform trf_k_shoulderL_00;
      public Transform trf_k_shoulderR_00;
      public Transform trf_k_handL_00;
      public Transform trf_k_handR_00;
    }
  }
}
