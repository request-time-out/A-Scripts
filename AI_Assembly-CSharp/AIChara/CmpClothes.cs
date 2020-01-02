// Decompiled with JetBrains decompiler
// Type: AIChara.CmpClothes
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
  public class CmpClothes : CmpBase
  {
    [Header("柄サイズ調整(固定)")]
    public Vector4 uvScalePattern = new Vector4(1f, 1f, 0.0f, 0.0f);
    [Header("基本初期設定")]
    public Color defMainColor01 = Color.get_white();
    public Color defMainColor02 = Color.get_white();
    public Color defMainColor03 = Color.get_white();
    public Color defPatternColor01 = Color.get_white();
    public Color defPatternColor02 = Color.get_white();
    public Color defPatternColor03 = Color.get_white();
    public Vector4 defLayout01 = new Vector4(10f, 10f, 0.0f, 0.0f);
    public Vector4 defLayout02 = new Vector4(10f, 10f, 0.0f, 0.0f);
    public Vector4 defLayout03 = new Vector4(10f, 10f, 0.0f, 0.0f);
    [Space]
    [Header("４色目(固定)")]
    public Color defMainColor04 = Color.get_white();
    [Header("破れフラグ")]
    public bool useBreak;
    [Header("通常パーツ")]
    public Renderer[] rendNormal01;
    public Renderer[] rendNormal02;
    public Renderer[] rendNormal03;
    public bool useColorN01;
    public bool useColorN02;
    public bool useColorN03;
    public bool useColorA01;
    public bool useColorA02;
    public bool useColorA03;
    [Header("着衣・半脱のまとめ")]
    public GameObject objTopDef;
    public GameObject objTopHalf;
    public GameObject objBotDef;
    public GameObject objBotHalf;
    [Header("オプションパーツ")]
    public GameObject[] objOpt01;
    public GameObject[] objOpt02;
    public int defPtnIndex01;
    public int defPtnIndex02;
    public int defPtnIndex03;
    [Range(0.0f, 1f)]
    public float defGloss01;
    [Range(0.0f, 1f)]
    public float defGloss02;
    [Range(0.0f, 1f)]
    public float defGloss03;
    [Range(0.0f, 1f)]
    public float defMetallic01;
    [Range(0.0f, 1f)]
    public float defMetallic02;
    [Range(0.0f, 1f)]
    public float defMetallic03;
    [Range(-1f, 1f)]
    public float defRotation01;
    [Range(-1f, 1f)]
    public float defRotation02;
    [Range(-1f, 1f)]
    public float defRotation03;
    [Range(0.0f, 1f)]
    public float defGloss04;
    [Range(0.0f, 1f)]
    public float defMetallic04;
    [Space]
    [Button("SetDefault", "初期色を設定", new object[] {})]
    public int setdefault;

    public CmpClothes()
      : base(true)
    {
    }

    public void SetDefault()
    {
      Material material = (Material) null;
      if (this.rendNormal01 != null && this.rendNormal01.Length != 0)
        material = this.rendNormal01[0].get_sharedMaterial();
      if (!Object.op_Inequality((Object) null, (Object) material))
        return;
      if (this.useColorN01 || this.useColorA01)
      {
        if (material.HasProperty("_Color"))
          this.defMainColor01 = material.GetColor("_Color");
        if (material.HasProperty("_Color1_2"))
          this.defPatternColor01 = material.GetColor("_Color1_2");
        if (material.HasProperty("_Glossiness"))
          this.defGloss01 = material.GetFloat("_Glossiness");
        if (material.HasProperty("_Metallic"))
          this.defMetallic01 = material.GetFloat("_Metallic");
        if (material.HasProperty("_patternuv1"))
          this.defLayout01 = material.GetVector("_patternuv1");
        if (material.HasProperty("_patternuv1Rotator"))
          this.defRotation01 = material.GetFloat("_patternuv1Rotator");
      }
      if (this.useColorN02 || this.useColorA02)
      {
        if (material.HasProperty("_Color2"))
          this.defMainColor02 = material.GetColor("_Color2");
        if (material.HasProperty("_Color2_2"))
          this.defPatternColor01 = material.GetColor("_Color2_2");
        if (material.HasProperty("_Glossiness2"))
          this.defGloss02 = material.GetFloat("_Glossiness2");
        if (material.HasProperty("_Metallic2"))
          this.defMetallic02 = material.GetFloat("_Metallic2");
        if (material.HasProperty("_patternuv2"))
          this.defLayout02 = material.GetVector("_patternuv2");
        if (material.HasProperty("_patternuv2Rotator"))
          this.defRotation02 = material.GetFloat("_patternuv2Rotator");
      }
      if (this.useColorN03 || this.useColorA03)
      {
        if (material.HasProperty("_Color3"))
          this.defMainColor03 = material.GetColor("_Color3");
        if (material.HasProperty("_Color3_2"))
          this.defPatternColor01 = material.GetColor("_Color3_2");
        if (material.HasProperty("_Glossiness3"))
          this.defGloss03 = material.GetFloat("_Glossiness3");
        if (material.HasProperty("_Metallic3"))
          this.defMetallic03 = material.GetFloat("_Metallic3");
        if (material.HasProperty("_patternuv3"))
          this.defLayout03 = material.GetVector("_patternuv3");
        if (material.HasProperty("_patternuv3Rotator"))
          this.defRotation03 = material.GetFloat("_patternuv3Rotator");
      }
      if (material.HasProperty("_UVScalePattern"))
        this.uvScalePattern = material.GetVector("_UVScalePattern");
      if (material.HasProperty("_Color4"))
        this.defMainColor04 = material.GetColor("_Color4");
      if (material.HasProperty("_Glossiness4"))
        this.defGloss04 = material.GetFloat("_Glossiness4");
      if (!material.HasProperty("_Metallic4"))
        return;
      this.defMetallic04 = material.GetFloat("_Metallic4");
    }

    public override void SetReferenceObject()
    {
      FindAssist findAssist = new FindAssist();
      findAssist.Initialize(((Component) this).get_transform());
      this.objTopDef = findAssist.GetObjectFromName("n_top_a");
      this.objTopHalf = findAssist.GetObjectFromName("n_top_b");
      this.objBotDef = findAssist.GetObjectFromName("n_bot_a");
      this.objBotHalf = findAssist.GetObjectFromName("n_bot_b");
      this.objOpt01 = ((IEnumerable<KeyValuePair<string, GameObject>>) findAssist.dictObjName).Where<KeyValuePair<string, GameObject>>((Func<KeyValuePair<string, GameObject>, bool>) (x => x.Key.StartsWith("op1"))).Select<KeyValuePair<string, GameObject>, GameObject>((Func<KeyValuePair<string, GameObject>, GameObject>) (x => x.Value)).ToArray<GameObject>();
      this.objOpt02 = ((IEnumerable<KeyValuePair<string, GameObject>>) findAssist.dictObjName).Where<KeyValuePair<string, GameObject>>((Func<KeyValuePair<string, GameObject>, bool>) (x => x.Key.StartsWith("op2"))).Select<KeyValuePair<string, GameObject>, GameObject>((Func<KeyValuePair<string, GameObject>, GameObject>) (x => x.Value)).ToArray<GameObject>();
    }
  }
}
