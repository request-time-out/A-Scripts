// Decompiled with JetBrains decompiler
// Type: AIChara.CmpAccessory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIChara
{
  [DisallowMultipleComponent]
  public class CmpAccessory : CmpBase
  {
    public bool useGloss01 = true;
    public bool useMetallic01 = true;
    public Color defColor01 = Color.get_white();
    public float defGlossPower01 = 0.5f;
    public float defMetallicPower01 = 0.5f;
    public bool useGloss02 = true;
    public bool useMetallic02 = true;
    public Color defColor02 = Color.get_white();
    public float defGlossPower02 = 0.5f;
    public float defMetallicPower02 = 0.5f;
    public bool useGloss03 = true;
    public bool useMetallic03 = true;
    public Color defColor03 = Color.get_white();
    public float defGlossPower03 = 0.5f;
    public float defMetallicPower03 = 0.5f;
    public bool useGloss04 = true;
    public bool useMetallic04 = true;
    public Color defColor04 = Color.get_white();
    public float defGlossPower04 = 0.5f;
    public float defMetallicPower04 = 0.5f;
    [Header("< 髪タイプ >-------------------------")]
    public bool typeHair;
    [Header("< 通常パーツ >-----------------------")]
    public Renderer[] rendNormal;
    [Header("01 or BaseColor")]
    public bool useColor01;
    [Header("02 or TopColor")]
    public bool useColor02;
    [Header("03 or UnderColor")]
    public bool useColor03;
    [Header("< 半透明パーツ >---------------------")]
    public Renderer[] rendAlpha;
    [Header("< 調整NULL >-------------------------")]
    public Transform trfMove01;
    public Transform trfMove02;
    [Space]
    [Button("SetColor", "初期色を設定", new object[] {})]
    public int setcolor;

    public CmpAccessory()
      : base(true)
    {
    }

    public override void SetReferenceObject()
    {
      FindAssist findAssist = new FindAssist();
      findAssist.Initialize(((Component) this).get_transform());
      this.trfMove01 = findAssist.GetTransformFromName("N_move");
      this.trfMove02 = findAssist.GetTransformFromName("N_move2");
      this.SetColor();
    }

    public void SetColor()
    {
      if (this.rendNormal != null && this.rendNormal.Length != 0)
      {
        Material sharedMaterial = this.rendNormal[0].get_sharedMaterial();
        if (Object.op_Inequality((Object) null, (Object) sharedMaterial))
        {
          if (sharedMaterial.HasProperty("_Color"))
            this.defColor01 = sharedMaterial.GetColor("_Color");
          if (sharedMaterial.HasProperty("_Glossiness"))
            this.defGlossPower01 = sharedMaterial.GetFloat("_Glossiness");
          if (sharedMaterial.HasProperty("_Metallic"))
            this.defMetallicPower01 = sharedMaterial.GetFloat("_Metallic");
          if (sharedMaterial.HasProperty("_Color2"))
            this.defColor02 = sharedMaterial.GetColor("_Color2");
          if (sharedMaterial.HasProperty("_Glossiness2"))
            this.defGlossPower02 = sharedMaterial.GetFloat("_Glossiness2");
          if (sharedMaterial.HasProperty("_Metallic2"))
            this.defMetallicPower02 = sharedMaterial.GetFloat("_Metallic2");
          if (sharedMaterial.HasProperty("_Color3"))
            this.defColor03 = sharedMaterial.GetColor("_Color3");
          if (sharedMaterial.HasProperty("_Glossiness3"))
            this.defGlossPower03 = sharedMaterial.GetFloat("_Glossiness3");
          if (sharedMaterial.HasProperty("_Metallic3"))
            this.defMetallicPower03 = sharedMaterial.GetFloat("_Metallic3");
        }
      }
      if (this.rendAlpha == null || this.rendAlpha.Length == 0)
        return;
      Material sharedMaterial1 = this.rendAlpha[0].get_sharedMaterial();
      if (!Object.op_Inequality((Object) null, (Object) sharedMaterial1))
        return;
      if (sharedMaterial1.HasProperty("_Color"))
        this.defColor04 = sharedMaterial1.GetColor("_Color");
      if (sharedMaterial1.HasProperty("_Glossiness4"))
        this.defGlossPower04 = sharedMaterial1.GetFloat("_Glossiness4");
      if (!sharedMaterial1.HasProperty("_Metallic4"))
        return;
      this.defMetallicPower04 = sharedMaterial1.GetFloat("_Metallic4");
    }
  }
}
