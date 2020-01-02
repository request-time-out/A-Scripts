// Decompiled with JetBrains decompiler
// Type: AIChara.CmpHair
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AIChara
{
  [DisallowMultipleComponent]
  public class CmpHair : CmpBase
  {
    [Tooltip("根本の色を使用")]
    public bool useTopColor = true;
    [Tooltip("毛先の色を使用")]
    public bool useUnderColor = true;
    [Header("< 髪の毛 >-------------------")]
    public Renderer[] rendHair;
    public CmpHair.BoneInfo[] boneInfo;
    [Space]
    [Button("SetDefaultPosition", "初期位置", new object[] {})]
    public int setdefaultposition;
    [Button("SetDefaultRotation", "初期回転", new object[] {})]
    public int setdefaultrotation;
    [Header("< 飾り >---------------------")]
    public bool useAcsColor01;
    public bool useAcsColor02;
    public bool useAcsColor03;
    public Renderer[] rendAccessory;
    public Color[] acsDefColor;
    [Button("SetColor", "アクセサリの初期色を設定", new object[] {})]
    public int setcolor;

    public CmpHair()
      : base(false)
    {
    }

    protected override void Reacquire()
    {
      base.Reacquire();
      if (this.boneInfo == null || this.boneInfo.Length == 0)
        return;
      FindAssist findAssist = new FindAssist();
      findAssist.Initialize(((Component) this).get_transform());
      KeyValuePair<string, GameObject> keyValuePair = ((IEnumerable<KeyValuePair<string, GameObject>>) findAssist.dictObjName).FirstOrDefault<KeyValuePair<string, GameObject>>((Func<KeyValuePair<string, GameObject>, bool>) (x => x.Key.Contains("_top")));
      if (keyValuePair.Equals((object) new KeyValuePair<string, GameObject>()))
        return;
      DynamicBone[] components = (DynamicBone[]) ((Component) this).GetComponents<DynamicBone>();
      for (int index = 0; index < this.boneInfo.Length; ++index)
      {
        Transform child = keyValuePair.Value.get_transform().GetChild(index);
        findAssist.Initialize(child);
        List<DynamicBone> dynamicBoneList = new List<DynamicBone>();
        foreach (DynamicBone dynamicBone in components)
        {
          DynamicBone n = dynamicBone;
          if (!((IEnumerable<KeyValuePair<string, GameObject>>) findAssist.dictObjName).FirstOrDefault<KeyValuePair<string, GameObject>>((Func<KeyValuePair<string, GameObject>, bool>) (x => x.Key == ((Object) n.m_Root).get_name())).Equals((object) new KeyValuePair<string, GameObject>()))
            dynamicBoneList.Add(n);
        }
        this.boneInfo[index].dynamicBone = dynamicBoneList.ToArray();
      }
    }

    public void SetColor()
    {
      if (this.rendAccessory.Length == 0)
        return;
      Material sharedMaterial = this.rendAccessory[0].get_sharedMaterial();
      if (!Object.op_Inequality((Object) null, (Object) sharedMaterial))
        return;
      this.acsDefColor = new Color[3];
      if (sharedMaterial.HasProperty("_Color"))
        this.acsDefColor[0] = sharedMaterial.GetColor("_Color");
      if (sharedMaterial.HasProperty("_Color2"))
        this.acsDefColor[1] = sharedMaterial.GetColor("_Color2");
      if (!sharedMaterial.HasProperty("_Color3"))
        return;
      this.acsDefColor[2] = sharedMaterial.GetColor("_Color3");
    }

    public void SetDefaultPosition()
    {
      if (this.boneInfo == null || this.boneInfo.Length == 0)
        return;
      foreach (CmpHair.BoneInfo boneInfo in this.boneInfo)
      {
        if (Object.op_Inequality((Object) null, (Object) boneInfo.trfCorrect))
          ((Component) boneInfo.trfCorrect).get_transform().set_localPosition(boneInfo.basePos);
      }
    }

    public void SetDefaultRotation()
    {
      if (this.boneInfo == null || this.boneInfo.Length == 0)
        return;
      foreach (CmpHair.BoneInfo boneInfo in this.boneInfo)
      {
        if (Object.op_Inequality((Object) null, (Object) boneInfo.trfCorrect))
          ((Component) boneInfo.trfCorrect).get_transform().set_localEulerAngles(boneInfo.baseRot);
      }
    }

    public override void SetReferenceObject()
    {
      FindAssist findAssist1 = new FindAssist();
      findAssist1.Initialize(((Component) this).get_transform());
      this.rendHair = ((IEnumerable<Renderer>) ((Component) this).GetComponentsInChildren<Renderer>(true)).Where<Renderer>((Func<Renderer, bool>) (x => !((Object) x).get_name().Contains("_acs"))).ToArray<Renderer>();
      DynamicBone[] components = (DynamicBone[]) ((Component) this).GetComponents<DynamicBone>();
      KeyValuePair<string, GameObject> keyValuePair1 = ((IEnumerable<KeyValuePair<string, GameObject>>) findAssist1.dictObjName).FirstOrDefault<KeyValuePair<string, GameObject>>((Func<KeyValuePair<string, GameObject>, bool>) (x => x.Key.Contains("_top")));
      if (keyValuePair1.Equals((object) new KeyValuePair<string, GameObject>()))
        return;
      this.boneInfo = new CmpHair.BoneInfo[keyValuePair1.Value.get_transform().get_childCount()];
      for (int index = 0; index < this.boneInfo.Length; ++index)
      {
        Transform child = keyValuePair1.Value.get_transform().GetChild(index);
        findAssist1.Initialize(child);
        CmpHair.BoneInfo boneInfo = new CmpHair.BoneInfo();
        KeyValuePair<string, GameObject> keyValuePair2 = ((IEnumerable<KeyValuePair<string, GameObject>>) findAssist1.dictObjName).FirstOrDefault<KeyValuePair<string, GameObject>>((Func<KeyValuePair<string, GameObject>, bool>) (x => x.Key.Contains("_s")));
        if (!keyValuePair2.Equals((object) new KeyValuePair<string, GameObject>()))
        {
          Transform transform = keyValuePair2.Value.get_transform();
          boneInfo.trfCorrect = transform;
          boneInfo.basePos = ((Component) boneInfo.trfCorrect).get_transform().get_localPosition();
          boneInfo.posMin.x = (__Null) (((Component) boneInfo.trfCorrect).get_transform().get_localPosition().x + 0.100000001490116);
          boneInfo.posMin.y = ((Component) boneInfo.trfCorrect).get_transform().get_localPosition().y;
          boneInfo.posMin.z = (__Null) (((Component) boneInfo.trfCorrect).get_transform().get_localPosition().z + 0.100000001490116);
          boneInfo.posMax.x = (__Null) (((Component) boneInfo.trfCorrect).get_transform().get_localPosition().x - 0.100000001490116);
          boneInfo.posMax.y = (__Null) (((Component) boneInfo.trfCorrect).get_transform().get_localPosition().y - 0.200000002980232);
          boneInfo.posMax.z = (__Null) (((Component) boneInfo.trfCorrect).get_transform().get_localPosition().z - 0.100000001490116);
          boneInfo.baseRot = ((Component) boneInfo.trfCorrect).get_transform().get_localEulerAngles();
          boneInfo.rotMin.x = (__Null) (((Component) boneInfo.trfCorrect).get_transform().get_localEulerAngles().x - 15.0);
          boneInfo.rotMin.y = (__Null) (((Component) boneInfo.trfCorrect).get_transform().get_localEulerAngles().y - 15.0);
          boneInfo.rotMin.z = (__Null) (((Component) boneInfo.trfCorrect).get_transform().get_localEulerAngles().z - 15.0);
          boneInfo.rotMax.x = (__Null) (((Component) boneInfo.trfCorrect).get_transform().get_localEulerAngles().x + 15.0);
          boneInfo.rotMax.y = (__Null) (((Component) boneInfo.trfCorrect).get_transform().get_localEulerAngles().y + 15.0);
          boneInfo.rotMax.z = (__Null) (((Component) boneInfo.trfCorrect).get_transform().get_localEulerAngles().z + 15.0);
          boneInfo.moveRate.x = (__Null) (double) Mathf.InverseLerp((float) boneInfo.posMin.x, (float) boneInfo.posMax.x, (float) boneInfo.basePos.x);
          boneInfo.moveRate.y = (__Null) (double) Mathf.InverseLerp((float) boneInfo.posMin.y, (float) boneInfo.posMax.y, (float) boneInfo.basePos.y);
          boneInfo.moveRate.z = (__Null) (double) Mathf.InverseLerp((float) boneInfo.posMin.z, (float) boneInfo.posMax.z, (float) boneInfo.basePos.z);
          boneInfo.rotRate.x = (__Null) (double) Mathf.InverseLerp((float) boneInfo.rotMin.x, (float) boneInfo.rotMax.x, (float) boneInfo.baseRot.x);
          boneInfo.rotRate.y = (__Null) (double) Mathf.InverseLerp((float) boneInfo.rotMin.y, (float) boneInfo.rotMax.y, (float) boneInfo.baseRot.y);
          boneInfo.rotRate.z = (__Null) (double) Mathf.InverseLerp((float) boneInfo.rotMin.z, (float) boneInfo.rotMax.z, (float) boneInfo.baseRot.z);
        }
        List<DynamicBone> dynamicBoneList = new List<DynamicBone>();
        foreach (DynamicBone dynamicBone in components)
        {
          DynamicBone n = dynamicBone;
          if (!((IEnumerable<KeyValuePair<string, GameObject>>) findAssist1.dictObjName).FirstOrDefault<KeyValuePair<string, GameObject>>((Func<KeyValuePair<string, GameObject>, bool>) (x => x.Key == ((Object) n.m_Root).get_name())).Equals((object) new KeyValuePair<string, GameObject>()))
            dynamicBoneList.Add(n);
        }
        boneInfo.dynamicBone = dynamicBoneList.ToArray();
        this.boneInfo[index] = boneInfo;
      }
      FindAssist findAssist2 = new FindAssist();
      findAssist2.Initialize(((Component) this).get_transform());
      this.rendAccessory = ((IEnumerable<KeyValuePair<string, GameObject>>) findAssist2.dictObjName).Where<KeyValuePair<string, GameObject>>((Func<KeyValuePair<string, GameObject>, bool>) (s => s.Key.Contains("_acs"))).Select<KeyValuePair<string, GameObject>, Renderer>((Func<KeyValuePair<string, GameObject>, Renderer>) (x => (Renderer) x.Value.GetComponent<Renderer>())).Where<Renderer>((Func<Renderer, bool>) (r => Object.op_Inequality((Object) null, (Object) r))).ToArray<Renderer>();
      this.SetColor();
    }

    public void ResetDynamicBonesHair(bool includeInactive = false)
    {
      if (this.boneInfo == null || this.boneInfo.Length == 0)
        return;
      foreach (CmpHair.BoneInfo boneInfo in this.boneInfo)
      {
        if (boneInfo.dynamicBone != null)
        {
          for (int index = 0; index < boneInfo.dynamicBone.Length; ++index)
          {
            if (Object.op_Inequality((Object) null, (Object) boneInfo.dynamicBone[index]) && (((Behaviour) boneInfo.dynamicBone[index]).get_enabled() || includeInactive))
              boneInfo.dynamicBone[index].ResetParticlesPosition();
          }
        }
      }
    }

    public void EnableDynamicBonesHair(bool enable, ChaFileHair.PartsInfo parts = null)
    {
      if (this.boneInfo == null || this.boneInfo.Length == 0)
        return;
      if (enable)
      {
        if (parts.dictBundle == null || parts.dictBundle.Count == 0 || this.boneInfo.Length != parts.dictBundle.Count)
          return;
        for (int key = 0; key < this.boneInfo.Length; ++key)
        {
          ChaFileHair.PartsInfo.BundleInfo bundleInfo;
          if (this.boneInfo[key].dynamicBone != null && parts.dictBundle.TryGetValue(key, out bundleInfo))
          {
            for (int index = 0; index < this.boneInfo[key].dynamicBone.Length; ++index)
            {
              DynamicBone dynamicBone = this.boneInfo[key].dynamicBone[index];
              if (!Object.op_Equality((Object) null, (Object) dynamicBone) && ((Behaviour) dynamicBone).get_enabled() != !bundleInfo.noShake)
              {
                ((Behaviour) dynamicBone).set_enabled(!bundleInfo.noShake);
                if (((Behaviour) dynamicBone).get_enabled())
                  dynamicBone.ResetParticlesPosition();
              }
            }
          }
        }
      }
      else
      {
        foreach (CmpHair.BoneInfo boneInfo in this.boneInfo)
        {
          if (boneInfo.dynamicBone != null)
          {
            for (int index = 0; index < boneInfo.dynamicBone.Length; ++index)
            {
              if (Object.op_Inequality((Object) null, (Object) boneInfo.dynamicBone[index]) && ((Behaviour) boneInfo.dynamicBone[index]).get_enabled())
                ((Behaviour) boneInfo.dynamicBone[index]).set_enabled(false);
            }
          }
        }
      }
    }

    private void Update()
    {
      if (this.boneInfo == null || this.boneInfo.Length == 0)
        return;
      foreach (CmpHair.BoneInfo boneInfo in this.boneInfo)
      {
        if (!Object.op_Equality((Object) null, (Object) boneInfo.trfCorrect))
        {
          ((Component) boneInfo.trfCorrect).get_transform().set_localPosition(new Vector3(Mathf.Lerp((float) boneInfo.posMin.x, (float) boneInfo.posMax.x, (float) boneInfo.moveRate.x), Mathf.Lerp((float) boneInfo.posMin.y, (float) boneInfo.posMax.y, (float) boneInfo.moveRate.y), Mathf.Lerp((float) boneInfo.posMin.z, (float) boneInfo.posMax.z, (float) boneInfo.moveRate.z)));
          ((Component) boneInfo.trfCorrect).get_transform().set_localEulerAngles(new Vector3(Mathf.Lerp((float) boneInfo.rotMin.x, (float) boneInfo.rotMax.x, (float) boneInfo.rotRate.x), Mathf.Lerp((float) boneInfo.rotMin.y, (float) boneInfo.rotMax.y, (float) boneInfo.rotRate.y), Mathf.Lerp((float) boneInfo.rotMin.z, (float) boneInfo.rotMax.z, (float) boneInfo.rotRate.z)));
        }
      }
    }

    [Serializable]
    public class BoneInfo
    {
      [HideInInspector]
      public Vector3 basePos = Vector3.get_zero();
      [HideInInspector]
      public Vector3 baseRot = Vector3.get_zero();
      [Header("[位置 制限]---------------------")]
      public Vector3 posMin = new Vector3(0.0f, 0.0f, 0.0f);
      public Vector3 posMax = new Vector3(0.0f, 0.0f, 0.0f);
      [Header("[回転 制限]---------------------")]
      public Vector3 rotMin = new Vector3(0.0f, 0.0f, 0.0f);
      public Vector3 rotMax = new Vector3(0.0f, 0.0f, 0.0f);
      [HideInInspector]
      public Vector3 moveRate = Vector3.get_zero();
      [HideInInspector]
      public Vector3 rotRate = Vector3.get_zero();
      public Transform trfCorrect;
      public DynamicBone[] dynamicBone;
    }
  }
}
