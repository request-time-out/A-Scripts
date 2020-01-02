// Decompiled with JetBrains decompiler
// Type: Studio.ItemComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Studio
{
  [AddComponentMenu("Studio/ItemComponent")]
  public class ItemComponent : MonoBehaviour
  {
    [Header("レンダラー管理")]
    public ItemComponent.RendererInfo[] rendererInfos;
    [Space]
    [Header("構成情報")]
    public ItemComponent.Info[] info;
    public float alpha;
    [Header("ガラス関係")]
    public Color defGlass;
    [Header("エミッション関係")]
    [ColorUsage(false, true)]
    public Color defEmissionColor;
    public float defEmissionStrength;
    public float defLightCancel;
    [Header("子の接続先")]
    public Transform childRoot;
    [Header("アニメ関係")]
    [Tooltip("アニメがあるか")]
    public bool isAnime;
    public ItemComponent.AnimeInfo[] animeInfos;
    [Header("拡縮判定")]
    public bool isScale;
    [Header("オプション")]
    public ItemComponent.OptionInfo[] optionInfos;
    [Header("海面関係")]
    public GameObject objSeaParent;
    public Renderer[] renderersSea;
    [Space]
    [Button("SetColor", "初期色を設定", new object[] {})]
    public int setcolor;

    public ItemComponent()
    {
      base.\u002Ector();
    }

    public bool check
    {
      get
      {
        return !((IList<ItemComponent.RendererInfo>) this.rendererInfos).IsNullOrEmpty<ItemComponent.RendererInfo>() && ((IEnumerable<ItemComponent.RendererInfo>) this.rendererInfos).Any<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (_ri => _ri.IsNormal || _ri.IsAlpha));
      }
    }

    public bool checkAlpha
    {
      get
      {
        return !((IList<ItemComponent.RendererInfo>) this.rendererInfos).IsNullOrEmpty<ItemComponent.RendererInfo>() && ((IEnumerable<ItemComponent.RendererInfo>) this.rendererInfos).Any<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (_ri => _ri.IsAlpha));
      }
    }

    public bool checkGlass
    {
      get
      {
        return !((IList<ItemComponent.RendererInfo>) this.rendererInfos).IsNullOrEmpty<ItemComponent.RendererInfo>() && ((IEnumerable<ItemComponent.RendererInfo>) this.rendererInfos).Any<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (_ri => _ri.IsGlass));
      }
    }

    public bool checkEmissionColor
    {
      get
      {
        return this.HasEmissionColor();
      }
    }

    public bool checkEmissionStrength
    {
      get
      {
        return this.HasEmissionStrength();
      }
    }

    public bool CheckEmission
    {
      get
      {
        return !((IList<ItemComponent.RendererInfo>) this.rendererInfos).IsNullOrEmpty<ItemComponent.RendererInfo>() && ((IEnumerable<ItemComponent.RendererInfo>) this.rendererInfos).Any<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (_ri => _ri.IsEmission));
      }
    }

    public bool checkLightCancel
    {
      get
      {
        return ((IEnumerable<ItemComponent.RendererInfo>) this.rendererInfos).Any<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (_ri => (_ri.IsNormal || _ri.IsAlpha) && ((IEnumerable<Material>) _ri.renderer.get_materials()).Any<Material>((Func<Material, bool>) (_m => _m.HasProperty(ItemShader._LightCancel)))));
      }
    }

    public bool CheckOption
    {
      get
      {
        return !((IList<ItemComponent.OptionInfo>) this.optionInfos).IsNullOrEmpty<ItemComponent.OptionInfo>();
      }
    }

    public bool CheckAnimePattern
    {
      get
      {
        return !((IList<ItemComponent.AnimeInfo>) this.animeInfos).IsNullOrEmpty<ItemComponent.AnimeInfo>() && ((IEnumerable<ItemComponent.AnimeInfo>) this.animeInfos).Any<ItemComponent.AnimeInfo>((Func<ItemComponent.AnimeInfo, bool>) (_info => _info.Check));
      }
    }

    public Color[] defColorMain
    {
      get
      {
        return ((IEnumerable<ItemComponent.Info>) this.info).Select<ItemComponent.Info, Color>((Func<ItemComponent.Info, Color>) (i => i.defColor)).ToArray<Color>();
      }
    }

    public Color[] defColorPattern
    {
      get
      {
        return ((IEnumerable<ItemComponent.Info>) this.info).Select<ItemComponent.Info, Color>((Func<ItemComponent.Info, Color>) (i => i.defColorPattern)).ToArray<Color>();
      }
    }

    public bool[] useColor
    {
      get
      {
        if (((IList<ItemComponent.RendererInfo>) this.rendererInfos).IsNullOrEmpty<ItemComponent.RendererInfo>() || !((IEnumerable<ItemComponent.RendererInfo>) this.rendererInfos).Any<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (_ri => _ri.IsColor)))
          return Enumerable.Repeat<bool>(false, 3).ToArray<bool>();
        return new bool[3]
        {
          ((IEnumerable<ItemComponent.RendererInfo>) this.rendererInfos).Any<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (_ri => _ri.IsColor1)),
          ((IEnumerable<ItemComponent.RendererInfo>) this.rendererInfos).Any<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (_ri => _ri.IsColor2)),
          ((IEnumerable<ItemComponent.RendererInfo>) this.rendererInfos).Any<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (_ri => _ri.IsColor3))
        };
      }
    }

    public bool[] useMetallic
    {
      get
      {
        return ((IEnumerable<ItemComponent.Info>) this.info).Select<ItemComponent.Info, bool>((Func<ItemComponent.Info, bool>) (i => i.useMetallic)).ToArray<bool>();
      }
    }

    public bool[] usePattern
    {
      get
      {
        if (((IList<ItemComponent.RendererInfo>) this.rendererInfos).IsNullOrEmpty<ItemComponent.RendererInfo>() || !((IEnumerable<ItemComponent.RendererInfo>) this.rendererInfos).Any<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (_ri => _ri.IsPattern)))
          return Enumerable.Repeat<bool>(false, 3).ToArray<bool>();
        return new bool[3]
        {
          ((IEnumerable<ItemComponent.RendererInfo>) this.rendererInfos).Any<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (_ri => _ri.IsPattern1)),
          ((IEnumerable<ItemComponent.RendererInfo>) this.rendererInfos).Any<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (_ri => _ri.IsPattern2)),
          ((IEnumerable<ItemComponent.RendererInfo>) this.rendererInfos).Any<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (_ri => _ri.IsPattern3))
        };
      }
    }

    public Color DefEmissionColor
    {
      get
      {
        return new Color((float) this.defEmissionColor.r, (float) this.defEmissionColor.g, (float) this.defEmissionColor.b);
      }
    }

    public ItemComponent.Info this[int _idx]
    {
      get
      {
        return this.info.SafeGet<ItemComponent.Info>(_idx);
      }
    }

    public void SetupRendererInfo()
    {
      Renderer[] componentsInChildren = (Renderer[]) ((Component) this).GetComponentsInChildren<Renderer>();
      if (((IList<Renderer>) componentsInChildren).IsNullOrEmpty<Renderer>())
        return;
      this.rendererInfos = ((IEnumerable<Renderer>) componentsInChildren).Select<Renderer, ItemComponent.RendererInfo>((Func<Renderer, ItemComponent.RendererInfo>) (_r => new ItemComponent.RendererInfo()
      {
        renderer = _r
      })).ToArray<ItemComponent.RendererInfo>();
      foreach (ItemComponent.RendererInfo rendererInfo in this.rendererInfos)
      {
        Material[] sharedMaterials = rendererInfo.renderer.get_sharedMaterials();
        rendererInfo.materials = new ItemComponent.MaterialInfo[sharedMaterials.Length];
        for (int index = 0; index < sharedMaterials.Length; ++index)
          rendererInfo.materials[index] = new ItemComponent.MaterialInfo();
      }
    }

    public void UpdateColor(OIItemInfo _info)
    {
      foreach (ItemComponent.RendererInfo rendererInfo in this.rendererInfos)
      {
        if (rendererInfo.IsNormal)
        {
          for (int index1 = 0; index1 < 3; ++index1)
          {
            ColorInfo color = _info.colors[index1];
            Material[] materials = rendererInfo.renderer.get_materials();
            for (int index2 = 0; index2 < materials.Length; ++index2)
            {
              ItemComponent.MaterialInfo materialInfo = rendererInfo.materials.SafeGet<ItemComponent.MaterialInfo>(index2);
              if (materialInfo != null)
              {
                switch (index1)
                {
                  case 0:
                    if (materialInfo.isColor1)
                    {
                      materials[index2].SetColor(ItemShader._Color, color.mainColor);
                      if (this.info[index1].useMetallic)
                      {
                        materials[index2].SetFloat(ItemShader._Metallic, color.metallic);
                        materials[index2].SetFloat(ItemShader._Glossiness, color.glossiness);
                      }
                      if (materialInfo.isPattern1)
                      {
                        materials[index2].SetColor(ItemShader._Color1_2, color.pattern.color);
                        materials[index2].SetVector(ItemShader._patternuv1, color.pattern.uv);
                        materials[index2].SetFloat(ItemShader._patternuv1Rotator, color.pattern.rot);
                        materials[index2].SetFloat(ItemShader._patternclamp1, !color.pattern.clamp ? 0.0f : 1f);
                        continue;
                      }
                      continue;
                    }
                    continue;
                  case 1:
                    if (materialInfo.isColor2)
                    {
                      materials[index2].SetColor(ItemShader._Color2, color.mainColor);
                      if (this.info[index1].useMetallic)
                      {
                        materials[index2].SetFloat(ItemShader._Metallic2, color.metallic);
                        materials[index2].SetFloat(ItemShader._Glossiness2, color.glossiness);
                      }
                      if (materialInfo.isPattern2)
                      {
                        materials[index2].SetColor(ItemShader._Color2_2, color.pattern.color);
                        materials[index2].SetVector(ItemShader._patternuv2, color.pattern.uv);
                        materials[index2].SetFloat(ItemShader._patternuv2Rotator, color.pattern.rot);
                        materials[index2].SetFloat(ItemShader._patternclamp2, !color.pattern.clamp ? 0.0f : 1f);
                        continue;
                      }
                      continue;
                    }
                    continue;
                  case 2:
                    if (materialInfo.isColor3)
                    {
                      materials[index2].SetColor(ItemShader._Color3, color.mainColor);
                      if (this.info[index1].useMetallic)
                      {
                        materials[index2].SetFloat(ItemShader._Metallic3, color.metallic);
                        materials[index2].SetFloat(ItemShader._Glossiness3, color.glossiness);
                      }
                      if (materialInfo.isPattern3)
                      {
                        materials[index2].SetColor(ItemShader._Color3_2, color.pattern.color);
                        materials[index2].SetVector(ItemShader._patternuv3, color.pattern.uv);
                        materials[index2].SetFloat(ItemShader._patternuv3Rotator, color.pattern.rot);
                        materials[index2].SetFloat(ItemShader._patternclamp3, !color.pattern.clamp ? 0.0f : 1f);
                        continue;
                      }
                      continue;
                    }
                    continue;
                  default:
                    continue;
                }
              }
            }
          }
        }
        if (rendererInfo.IsAlpha)
        {
          Material[] materials = rendererInfo.renderer.get_materials();
          for (int index = 0; index < materials.Length; ++index)
          {
            ItemComponent.MaterialInfo materialInfo = rendererInfo.materials.SafeGet<ItemComponent.MaterialInfo>(index);
            if (materialInfo != null && materialInfo.isAlpha)
              materials[index].SetFloat(ItemShader._alpha, _info.alpha);
          }
        }
        if (rendererInfo.IsNormal || rendererInfo.IsAlpha)
        {
          Material[] materials = rendererInfo.renderer.get_materials();
          for (int index = 0; index < materials.Length; ++index)
          {
            ItemComponent.MaterialInfo materialInfo = rendererInfo.materials.SafeGet<ItemComponent.MaterialInfo>(index);
            if (materialInfo != null && materialInfo.isEmission)
            {
              if (materials[index].HasProperty(ItemShader._EmissionColor))
                materials[index].SetColor(ItemShader._EmissionColor, _info.emissionColor);
              if (materials[index].HasProperty(ItemShader._EmissionStrength))
                materials[index].SetFloat(ItemShader._EmissionStrength, _info.emissionPower);
              if (materials[index].HasProperty(ItemShader._LightCancel))
                materials[index].SetFloat(ItemShader._LightCancel, _info.lightCancel);
            }
          }
        }
        if (rendererInfo.IsGlass)
        {
          Material[] materials = rendererInfo.renderer.get_materials();
          for (int index = 0; index < materials.Length; ++index)
          {
            ItemComponent.MaterialInfo materialInfo = rendererInfo.materials.SafeGet<ItemComponent.MaterialInfo>(index);
            if (materialInfo != null && materialInfo.isGlass)
            {
              ColorInfo color = _info.colors[3];
              if (materials[index].HasProperty(ItemShader._Color4))
                materials[index].SetColor(ItemShader._Color4, color.mainColor);
              else if (materials[index].HasProperty(ItemShader._Color))
                materials[index].SetColor(ItemShader._Color, color.mainColor);
              materials[index].SetColor(ItemShader._Metallic4, color.mainColor);
              materials[index].SetColor(ItemShader._Glossiness4, color.mainColor);
            }
          }
        }
      }
    }

    public void SetPatternTex(int _idx, Texture2D _texture)
    {
      int[] numArray = new int[3]
      {
        ItemShader._PatternMask1,
        ItemShader._PatternMask2,
        ItemShader._PatternMask3
      };
      foreach (ItemComponent.RendererInfo rendererInfo in ((IEnumerable<ItemComponent.RendererInfo>) this.rendererInfos).Where<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (v => v.IsNormal)))
      {
        Material[] materials = rendererInfo.renderer.get_materials();
        for (int index = 0; index < materials.Length; ++index)
        {
          ItemComponent.MaterialInfo materialInfo = rendererInfo.materials.SafeGet<ItemComponent.MaterialInfo>(index);
          if (materialInfo != null && materialInfo.CheckPattern(_idx))
            materials[index].SetTexture(numArray[_idx], (Texture) _texture);
        }
      }
    }

    public void SetOptionVisible(bool _value)
    {
      if (((IList<ItemComponent.OptionInfo>) this.optionInfos).IsNullOrEmpty<ItemComponent.OptionInfo>())
        return;
      foreach (ItemComponent.OptionInfo optionInfo in this.optionInfos)
        optionInfo.Visible = _value;
    }

    public void SetOptionVisible(int _idx, bool _value)
    {
      this.optionInfos.SafeProc<ItemComponent.OptionInfo>(_idx, (Action<ItemComponent.OptionInfo>) (_info => _info.Visible = _value));
    }

    public void SetColor()
    {
      bool[] array1 = Enumerable.Repeat<bool>(false, 7).ToArray<bool>();
      foreach (ItemComponent.RendererInfo rendererInfo in ((IEnumerable<ItemComponent.RendererInfo>) this.rendererInfos).Where<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (r => Object.op_Inequality((Object) r.renderer, (Object) null) && !((IList<ItemComponent.MaterialInfo>) r.materials).IsNullOrEmpty<ItemComponent.MaterialInfo>())).Where<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (r => ((IEnumerable<ItemComponent.MaterialInfo>) r.materials).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (_m => _m.isColor1)))))
      {
        if (!((IEnumerable<bool>) array1).Take<bool>(3).All<bool>((Func<bool, bool>) (_b => _b)))
        {
          foreach (Tuple<ItemComponent.MaterialInfo, int> tuple in ((IEnumerable<ItemComponent.MaterialInfo>) rendererInfo.materials).Select<ItemComponent.MaterialInfo, Tuple<ItemComponent.MaterialInfo, int>>((Func<ItemComponent.MaterialInfo, int, Tuple<ItemComponent.MaterialInfo, int>>) ((_m, index) => new Tuple<ItemComponent.MaterialInfo, int>(_m, index))).Where<Tuple<ItemComponent.MaterialInfo, int>>((Func<Tuple<ItemComponent.MaterialInfo, int>, bool>) (v => v.Item1.isColor1)))
          {
            Material material = rendererInfo.renderer.get_sharedMaterials().SafeGet<Material>(tuple.Item2);
            if (!Object.op_Equality((Object) material, (Object) null))
            {
              if (!array1[0] && material.HasProperty("_Color"))
              {
                this.info[0].defColor = material.GetColor("_Color");
                array1[0] = true;
              }
              if (!array1[1] && material.HasProperty("_Metallic"))
              {
                this.info[0].defMetallic = material.GetFloat("_Metallic");
                array1[1] = true;
              }
              if (!array1[2] && material.HasProperty("_Glossiness"))
              {
                this.info[0].defGlossiness = material.GetFloat("_Glossiness");
                array1[2] = true;
              }
              if (((IEnumerable<bool>) array1).Take<bool>(3).All<bool>((Func<bool, bool>) (_b => _b)))
                break;
            }
          }
        }
        if (!((IEnumerable<bool>) array1).Skip<bool>(3).All<bool>((Func<bool, bool>) (_b => _b)))
        {
          foreach (Tuple<ItemComponent.MaterialInfo, int> tuple in ((IEnumerable<ItemComponent.MaterialInfo>) rendererInfo.materials).Select<ItemComponent.MaterialInfo, Tuple<ItemComponent.MaterialInfo, int>>((Func<ItemComponent.MaterialInfo, int, Tuple<ItemComponent.MaterialInfo, int>>) ((_m, index) => new Tuple<ItemComponent.MaterialInfo, int>(_m, index))).Where<Tuple<ItemComponent.MaterialInfo, int>>((Func<Tuple<ItemComponent.MaterialInfo, int>, bool>) (v => v.Item1.isColor1 && v.Item1.isPattern1)))
          {
            Material material = rendererInfo.renderer.get_sharedMaterials().SafeGet<Material>(tuple.Item2);
            if (!Object.op_Equality((Object) material, (Object) null))
            {
              if (!array1[3] && material.HasProperty("_Color1_2"))
              {
                this.info[0].defColorPattern = material.GetColor("_Color1_2");
                array1[3] = true;
              }
              if (!array1[4] && material.HasProperty("_patternuv1"))
              {
                this.info[0].defUV = material.GetVector("_patternuv1");
                array1[4] = true;
              }
              if (!array1[5] && material.HasProperty("_patternuv1Rotator"))
              {
                this.info[0].defRot = material.GetFloat("_patternuv1Rotator");
                array1[5] = true;
              }
              if (!array1[6] && material.HasProperty("_patternclamp1"))
              {
                this.info[0].defClamp = (double) material.GetFloat("_patternclamp1") != 0.0;
                array1[6] = true;
              }
              if (((IEnumerable<bool>) array1).Skip<bool>(3).All<bool>((Func<bool, bool>) (_b => _b)))
                break;
            }
          }
        }
        if (((IEnumerable<bool>) array1).All<bool>((Func<bool, bool>) (_b => _b)))
          break;
      }
      bool[] array2 = Enumerable.Repeat<bool>(false, 7).ToArray<bool>();
      foreach (ItemComponent.RendererInfo rendererInfo in ((IEnumerable<ItemComponent.RendererInfo>) this.rendererInfos).Where<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (r => Object.op_Inequality((Object) r.renderer, (Object) null) && !((IList<ItemComponent.MaterialInfo>) r.materials).IsNullOrEmpty<ItemComponent.MaterialInfo>())).Where<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (r => ((IEnumerable<ItemComponent.MaterialInfo>) r.materials).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (_m => _m.isColor2)))))
      {
        if (!((IEnumerable<bool>) array2).Take<bool>(3).All<bool>((Func<bool, bool>) (_b => _b)))
        {
          foreach (Tuple<ItemComponent.MaterialInfo, int> tuple in ((IEnumerable<ItemComponent.MaterialInfo>) rendererInfo.materials).Select<ItemComponent.MaterialInfo, Tuple<ItemComponent.MaterialInfo, int>>((Func<ItemComponent.MaterialInfo, int, Tuple<ItemComponent.MaterialInfo, int>>) ((_m, index) => new Tuple<ItemComponent.MaterialInfo, int>(_m, index))).Where<Tuple<ItemComponent.MaterialInfo, int>>((Func<Tuple<ItemComponent.MaterialInfo, int>, bool>) (v => v.Item1.isColor2)))
          {
            Material material = rendererInfo.renderer.get_sharedMaterials().SafeGet<Material>(tuple.Item2);
            if (!Object.op_Equality((Object) material, (Object) null))
            {
              if (!array2[0] && material.HasProperty("_Color2"))
              {
                this.info[1].defColor = material.GetColor("_Color2");
                array2[0] = true;
              }
              if (!array2[1] && material.HasProperty("_Metallic2"))
              {
                this.info[1].defMetallic = material.GetFloat("_Metallic2");
                array2[1] = true;
              }
              if (!array2[2] && material.HasProperty("_Glossiness2"))
              {
                this.info[1].defGlossiness = material.GetFloat("_Glossiness2");
                array2[2] = true;
              }
              if (((IEnumerable<bool>) array2).Take<bool>(3).All<bool>((Func<bool, bool>) (_b => _b)))
                break;
            }
          }
        }
        if (!((IEnumerable<bool>) array2).Skip<bool>(3).All<bool>((Func<bool, bool>) (_b => _b)))
        {
          foreach (Tuple<ItemComponent.MaterialInfo, int> tuple in ((IEnumerable<ItemComponent.MaterialInfo>) rendererInfo.materials).Select<ItemComponent.MaterialInfo, Tuple<ItemComponent.MaterialInfo, int>>((Func<ItemComponent.MaterialInfo, int, Tuple<ItemComponent.MaterialInfo, int>>) ((_m, index) => new Tuple<ItemComponent.MaterialInfo, int>(_m, index))).Where<Tuple<ItemComponent.MaterialInfo, int>>((Func<Tuple<ItemComponent.MaterialInfo, int>, bool>) (v => v.Item1.isColor2 && v.Item1.isPattern2)))
          {
            Material material = rendererInfo.renderer.get_sharedMaterials().SafeGet<Material>(tuple.Item2);
            if (!Object.op_Equality((Object) material, (Object) null))
            {
              if (!array2[3] && material.HasProperty("_Color2_2"))
              {
                this.info[1].defColorPattern = material.GetColor("_Color2_2");
                array2[3] = true;
              }
              if (!array2[4] && material.HasProperty("_patternuv2"))
              {
                this.info[1].defUV = material.GetVector("_patternuv2");
                array2[4] = true;
              }
              if (!array2[5] && material.HasProperty("_patternuv2Rotator"))
              {
                this.info[1].defRot = material.GetFloat("_patternuv2Rotator");
                array2[5] = true;
              }
              if (!array2[6] && material.HasProperty("_patternclamp2"))
              {
                this.info[1].defClamp = (double) material.GetFloat("_patternclamp2") != 0.0;
                array2[6] = true;
              }
              if (((IEnumerable<bool>) array2).Skip<bool>(3).All<bool>((Func<bool, bool>) (_b => _b)))
                break;
            }
          }
        }
        if (((IEnumerable<bool>) array2).All<bool>((Func<bool, bool>) (_b => _b)))
          break;
      }
      bool[] array3 = Enumerable.Repeat<bool>(false, 7).ToArray<bool>();
      foreach (ItemComponent.RendererInfo rendererInfo in ((IEnumerable<ItemComponent.RendererInfo>) this.rendererInfos).Where<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (r => Object.op_Inequality((Object) r.renderer, (Object) null) && !((IList<ItemComponent.MaterialInfo>) r.materials).IsNullOrEmpty<ItemComponent.MaterialInfo>())).Where<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (r => ((IEnumerable<ItemComponent.MaterialInfo>) r.materials).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (_m => _m.isColor3)))))
      {
        if (!((IEnumerable<bool>) array3).Take<bool>(3).All<bool>((Func<bool, bool>) (_b => _b)))
        {
          foreach (Tuple<ItemComponent.MaterialInfo, int> tuple in ((IEnumerable<ItemComponent.MaterialInfo>) rendererInfo.materials).Select<ItemComponent.MaterialInfo, Tuple<ItemComponent.MaterialInfo, int>>((Func<ItemComponent.MaterialInfo, int, Tuple<ItemComponent.MaterialInfo, int>>) ((_m, index) => new Tuple<ItemComponent.MaterialInfo, int>(_m, index))).Where<Tuple<ItemComponent.MaterialInfo, int>>((Func<Tuple<ItemComponent.MaterialInfo, int>, bool>) (v => v.Item1.isColor3)))
          {
            Material material = rendererInfo.renderer.get_sharedMaterials().SafeGet<Material>(tuple.Item2);
            if (!Object.op_Equality((Object) material, (Object) null))
            {
              if (!array3[0] && material.HasProperty("_Color3"))
              {
                this.info[2].defColor = material.GetColor("_Color3");
                array3[0] = true;
              }
              if (!array3[1] && material.HasProperty("_Metallic3"))
              {
                this.info[2].defMetallic = material.GetFloat("_Metallic3");
                array3[1] = true;
              }
              if (!array3[2] && material.HasProperty("_Glossiness3"))
              {
                this.info[2].defGlossiness = material.GetFloat("_Glossiness3");
                array3[2] = true;
              }
              if (((IEnumerable<bool>) array3).Take<bool>(3).All<bool>((Func<bool, bool>) (_b => _b)))
                break;
            }
          }
        }
        if (!((IEnumerable<bool>) array3).Skip<bool>(3).All<bool>((Func<bool, bool>) (_b => _b)))
        {
          foreach (Tuple<ItemComponent.MaterialInfo, int> tuple in ((IEnumerable<ItemComponent.MaterialInfo>) rendererInfo.materials).Select<ItemComponent.MaterialInfo, Tuple<ItemComponent.MaterialInfo, int>>((Func<ItemComponent.MaterialInfo, int, Tuple<ItemComponent.MaterialInfo, int>>) ((_m, index) => new Tuple<ItemComponent.MaterialInfo, int>(_m, index))).Where<Tuple<ItemComponent.MaterialInfo, int>>((Func<Tuple<ItemComponent.MaterialInfo, int>, bool>) (v => v.Item1.isColor3 && v.Item1.isPattern3)))
          {
            Material material = rendererInfo.renderer.get_sharedMaterials().SafeGet<Material>(tuple.Item2);
            if (!Object.op_Equality((Object) material, (Object) null))
            {
              if (!array3[3] && material.HasProperty("_Color3_2"))
              {
                this.info[2].defColorPattern = material.GetColor("_Color3_2");
                array3[3] = true;
              }
              if (!array3[4] && material.HasProperty("_patternuv3"))
              {
                this.info[2].defUV = material.GetVector("_patternuv3");
                array3[4] = true;
              }
              if (!array3[5] && material.HasProperty("_patternuv3Rotator"))
              {
                this.info[2].defRot = material.GetFloat("_patternuv3Rotator");
                array3[5] = true;
              }
              if (!array3[6] && material.HasProperty("_patternclamp3"))
              {
                this.info[2].defClamp = (double) material.GetFloat("_patternclamp3") != 0.0;
                array3[6] = true;
              }
              if (((IEnumerable<bool>) array3).Skip<bool>(3).All<bool>((Func<bool, bool>) (_b => _b)))
                break;
            }
          }
        }
        if (((IEnumerable<bool>) array3).All<bool>((Func<bool, bool>) (_b => _b)))
          break;
      }
      ItemComponent.RendererInfo rendererInfo1 = ((IEnumerable<ItemComponent.RendererInfo>) this.rendererInfos).Where<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (r => Object.op_Inequality((Object) r.renderer, (Object) null) && !((IList<ItemComponent.MaterialInfo>) r.materials).IsNullOrEmpty<ItemComponent.MaterialInfo>())).FirstOrDefault<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (_i => ((IEnumerable<ItemComponent.MaterialInfo>) _i.materials).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (_m => _m.isAlpha))));
      if (rendererInfo1 != null)
      {
        Material[] sharedMaterials = rendererInfo1.renderer.get_sharedMaterials();
        for (int index = 0; index < sharedMaterials.Length; ++index)
        {
          ItemComponent.MaterialInfo materialInfo = rendererInfo1.materials.SafeGet<ItemComponent.MaterialInfo>(index);
          if (materialInfo != null && materialInfo.isAlpha && (Object.op_Inequality((Object) null, (Object) sharedMaterials[index]) && sharedMaterials[index].HasProperty("_alpha")))
            this.alpha = sharedMaterials[index].GetFloat("_alpha");
        }
      }
      this.SetGlass();
      this.SetEmission();
    }

    public void SetGlass()
    {
      if (((IList<ItemComponent.RendererInfo>) this.rendererInfos).IsNullOrEmpty<ItemComponent.RendererInfo>())
        return;
      ItemComponent.RendererInfo rendererInfo = ((IEnumerable<ItemComponent.RendererInfo>) this.rendererInfos).Where<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (r => Object.op_Inequality((Object) r.renderer, (Object) null) && !((IList<ItemComponent.MaterialInfo>) r.materials).IsNullOrEmpty<ItemComponent.MaterialInfo>())).FirstOrDefault<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (_i => ((IEnumerable<ItemComponent.MaterialInfo>) _i.materials).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (_m => _m.isGlass))));
      if (rendererInfo == null)
        return;
      Material[] sharedMaterials = rendererInfo.renderer.get_sharedMaterials();
      for (int index = 0; index < sharedMaterials.Length; ++index)
      {
        ItemComponent.MaterialInfo materialInfo = rendererInfo.materials.SafeGet<ItemComponent.MaterialInfo>(index);
        if (materialInfo != null && materialInfo.isGlass && Object.op_Inequality((Object) null, (Object) sharedMaterials[index]))
        {
          if (sharedMaterials[index].HasProperty("_Color4"))
            this.defGlass = sharedMaterials[index].GetColor("_Color4");
          else if (sharedMaterials[index].HasProperty("_Color"))
            this.defGlass = sharedMaterials[index].GetColor("_Color");
        }
      }
    }

    public void SetEmission()
    {
      bool[] flagArray = new bool[3];
      foreach (ItemComponent.RendererInfo rendererInfo in ((IEnumerable<ItemComponent.RendererInfo>) this.rendererInfos).Where<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (v => ((IEnumerable<ItemComponent.MaterialInfo>) v.materials).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (m => m.isEmission)))))
      {
        foreach (Tuple<ItemComponent.MaterialInfo, int> tuple in ((IEnumerable<ItemComponent.MaterialInfo>) rendererInfo.materials).Select<ItemComponent.MaterialInfo, Tuple<ItemComponent.MaterialInfo, int>>((Func<ItemComponent.MaterialInfo, int, Tuple<ItemComponent.MaterialInfo, int>>) ((_m, index) => new Tuple<ItemComponent.MaterialInfo, int>(_m, index))).Where<Tuple<ItemComponent.MaterialInfo, int>>((Func<Tuple<ItemComponent.MaterialInfo, int>, bool>) (v => v.Item1.isEmission)))
        {
          Material sharedMaterial = rendererInfo.renderer.get_sharedMaterials()[tuple.Item2];
          if (!Object.op_Equality((Object) sharedMaterial, (Object) null))
          {
            if (!flagArray[0] && sharedMaterial.HasProperty("_EmissionColor"))
            {
              this.defEmissionColor = sharedMaterial.GetColor("_EmissionColor");
              flagArray[0] = true;
            }
            if (!flagArray[1] && sharedMaterial.HasProperty("_EmissionStrength"))
            {
              this.defEmissionStrength = sharedMaterial.GetFloat("_EmissionStrength");
              flagArray[1] = true;
            }
            if (!flagArray[2] && sharedMaterial.HasProperty("_LightCancel"))
            {
              this.defLightCancel = sharedMaterial.GetFloat("_LightCancel");
              flagArray[2] = true;
            }
            if (((IEnumerable<bool>) flagArray).All<bool>((Func<bool, bool>) (_b => _b)))
              break;
          }
        }
        if (((IEnumerable<bool>) flagArray).All<bool>((Func<bool, bool>) (_b => _b)))
          break;
      }
    }

    public void SetSeaRenderer()
    {
      if (Object.op_Equality((Object) this.objSeaParent, (Object) null))
        return;
      this.renderersSea = (Renderer[]) this.objSeaParent.GetComponentsInChildren<Renderer>();
    }

    public void SetupSea()
    {
      if (((IList<Renderer>) this.renderersSea).IsNullOrEmpty<Renderer>())
        return;
      using (IEnumerator<Renderer> enumerator = ((IEnumerable<Renderer>) this.renderersSea).Where<Renderer>((Func<Renderer, bool>) (v => Object.op_Inequality((Object) v, (Object) null))).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          Renderer current = enumerator.Current;
          Material material = current.get_material();
          material.DisableKeyword("USINGWATERVOLUME");
          current.set_material(material);
        }
      }
    }

    private bool HasProperty(Renderer[] _renderer, int _nameID)
    {
      return ((IEnumerable<Renderer>) _renderer).Any<Renderer>((Func<Renderer, bool>) (r => ((IEnumerable<Material>) r.get_materials()).Any<Material>((Func<Material, bool>) (m => m.HasProperty(_nameID)))));
    }

    private bool HasEmissionColor()
    {
      foreach (ItemComponent.RendererInfo rendererInfo in ((IEnumerable<ItemComponent.RendererInfo>) this.rendererInfos).Where<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (v => ((IEnumerable<ItemComponent.MaterialInfo>) v.materials).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (m => m.isEmission)))))
      {
        Material[] materials = rendererInfo.renderer.get_materials();
        for (int index = 0; index < materials.Length; ++index)
        {
          ItemComponent.MaterialInfo materialInfo = rendererInfo.materials.SafeGet<ItemComponent.MaterialInfo>(index);
          if (materialInfo != null && materialInfo.isEmission && materials[index].HasProperty(ItemShader._EmissionColor))
            return true;
        }
      }
      return false;
    }

    private bool HasEmissionStrength()
    {
      foreach (ItemComponent.RendererInfo rendererInfo in ((IEnumerable<ItemComponent.RendererInfo>) this.rendererInfos).Where<ItemComponent.RendererInfo>((Func<ItemComponent.RendererInfo, bool>) (v => ((IEnumerable<ItemComponent.MaterialInfo>) v.materials).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (m => m.isEmission)))))
      {
        Material[] materials = rendererInfo.renderer.get_materials();
        for (int index = 0; index < materials.Length; ++index)
        {
          ItemComponent.MaterialInfo materialInfo = rendererInfo.materials.SafeGet<ItemComponent.MaterialInfo>(index);
          if (materialInfo != null && materialInfo.isEmission && materials[index].HasProperty(ItemShader._EmissionStrength))
            return true;
        }
      }
      return false;
    }

    [Serializable]
    public class MaterialInfo
    {
      public bool isColor1;
      public bool isPattern1;
      public bool isColor2;
      public bool isPattern2;
      public bool isColor3;
      public bool isPattern3;
      public bool isEmission;
      public bool isAlpha;
      public bool isGlass;

      public bool CheckColor(int _idx)
      {
        switch (_idx)
        {
          case 0:
            return this.isColor1;
          case 1:
            return this.isColor2;
          case 2:
            return this.isColor3;
          default:
            return false;
        }
      }

      public bool CheckPattern(int _idx)
      {
        switch (_idx)
        {
          case 0:
            return this.isPattern1;
          case 1:
            return this.isPattern2;
          case 2:
            return this.isPattern3;
          default:
            return false;
        }
      }
    }

    [Serializable]
    public class RendererInfo
    {
      public Renderer renderer;
      public ItemComponent.MaterialInfo[] materials;

      public bool IsNormal
      {
        get
        {
          return ((IEnumerable<ItemComponent.MaterialInfo>) this.materials).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (m => m.isColor1 || m.isColor2 || m.isColor3 || m.isEmission));
        }
      }

      public bool IsAlpha
      {
        get
        {
          return ((IEnumerable<ItemComponent.MaterialInfo>) this.materials).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (m => m.isAlpha));
        }
      }

      public bool IsGlass
      {
        get
        {
          return ((IEnumerable<ItemComponent.MaterialInfo>) this.materials).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (m => m.isGlass));
        }
      }

      public bool IsColor
      {
        get
        {
          return ((IEnumerable<ItemComponent.MaterialInfo>) this.materials).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (m => m.isColor1 || m.isColor2 || m.isColor3));
        }
      }

      public bool IsColor1
      {
        get
        {
          return ((IEnumerable<ItemComponent.MaterialInfo>) this.materials).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (m => m.isColor1));
        }
      }

      public bool IsColor2
      {
        get
        {
          return ((IEnumerable<ItemComponent.MaterialInfo>) this.materials).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (m => m.isColor2));
        }
      }

      public bool IsColor3
      {
        get
        {
          return ((IEnumerable<ItemComponent.MaterialInfo>) this.materials).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (m => m.isColor3));
        }
      }

      public bool IsPattern
      {
        get
        {
          return ((IEnumerable<ItemComponent.MaterialInfo>) this.materials).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (m => m.isPattern1 || m.isPattern2 || m.isPattern3));
        }
      }

      public bool IsPattern1
      {
        get
        {
          return ((IEnumerable<ItemComponent.MaterialInfo>) this.materials).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (m => m.isPattern1));
        }
      }

      public bool IsPattern2
      {
        get
        {
          return ((IEnumerable<ItemComponent.MaterialInfo>) this.materials).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (m => m.isPattern2));
        }
      }

      public bool IsPattern3
      {
        get
        {
          return ((IEnumerable<ItemComponent.MaterialInfo>) this.materials).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (m => m.isPattern3));
        }
      }

      public bool IsEmission
      {
        get
        {
          return ((IEnumerable<ItemComponent.MaterialInfo>) this.materials).Any<ItemComponent.MaterialInfo>((Func<ItemComponent.MaterialInfo, bool>) (m => m.isEmission));
        }
      }
    }

    [Serializable]
    public class Info
    {
      [Header("色替え")]
      public Color defColor = Color.get_white();
      [Header("柄")]
      public Color defColorPattern = Color.get_white();
      public bool defClamp = true;
      public Vector4 defUV = Vector4.get_zero();
      [Header("メタル")]
      public bool useMetallic;
      public float defMetallic;
      public float defGlossiness;
      public float defRot;

      public float ut
      {
        get
        {
          return (float) this.defUV.z;
        }
        set
        {
          this.defUV.z = (__Null) (double) value;
        }
      }

      public float vt
      {
        get
        {
          return (float) this.defUV.w;
        }
        set
        {
          this.defUV.w = (__Null) (double) value;
        }
      }

      public float us
      {
        get
        {
          return (float) this.defUV.x;
        }
        set
        {
          this.defUV.x = (__Null) (double) value;
        }
      }

      public float vs
      {
        get
        {
          return (float) this.defUV.y;
        }
        set
        {
          this.defUV.y = (__Null) (double) value;
        }
      }
    }

    [Serializable]
    public class OptionInfo
    {
      public GameObject[] objectsOn;
      public GameObject[] objectsOff;

      public bool Visible
      {
        set
        {
          if (value)
          {
            this.SetVisible(this.objectsOff, false);
            this.SetVisible(this.objectsOn, true);
          }
          else
          {
            this.SetVisible(this.objectsOn, false);
            this.SetVisible(this.objectsOff, true);
          }
        }
      }

      private void SetVisible(GameObject[] _objects, bool _value)
      {
        using (IEnumerator<GameObject> enumerator = ((IEnumerable<GameObject>) _objects).Where<GameObject>((Func<GameObject, bool>) (v => Object.op_Inequality((Object) v, (Object) null))).GetEnumerator())
        {
          while (((IEnumerator) enumerator).MoveNext())
            enumerator.Current.SetActiveIfDifferent(_value);
        }
      }
    }

    [Serializable]
    public class AnimeInfo
    {
      public string name = string.Empty;
      public string state = string.Empty;

      public bool Check
      {
        get
        {
          return !this.name.IsNullOrEmpty() && !this.state.IsNullOrEmpty();
        }
      }
    }
  }
}
