// Decompiled with JetBrains decompiler
// Type: CharaCustom.CustomClothesColorSet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using Illusion.Extensions;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CharaCustom
{
  public class CustomClothesColorSet : MonoBehaviour
  {
    [SerializeField]
    private Text title;
    [SerializeField]
    private CustomColorSet csMainColor;
    [SerializeField]
    private CustomSliderSet ssGloss;
    [SerializeField]
    private CustomSliderSet ssMetallic;
    [SerializeField]
    private CustomClothesPatternSelect clothesPtnSel;
    [SerializeField]
    private GameObject objPatternSet;
    [SerializeField]
    private Button btnPatternWin;
    [SerializeField]
    private Image imgPattern;
    [SerializeField]
    private CustomColorSet csPatternColor;
    [SerializeField]
    private CustomSliderSet ssPatternW;
    [SerializeField]
    private CustomSliderSet ssPatternH;
    [SerializeField]
    private CustomSliderSet ssPatternX;
    [SerializeField]
    private CustomSliderSet ssPatternY;
    [SerializeField]
    private CustomSliderSet ssPatternRot;
    private List<IDisposable> lstDisposable;

    public CustomClothesColorSet()
    {
      base.\u002Ector();
    }

    private CustomBase customBase
    {
      get
      {
        return Singleton<CustomBase>.Instance;
      }
    }

    private ChaControl chaCtrl
    {
      get
      {
        return this.customBase.chaCtrl;
      }
    }

    private ChaFileClothes nowClothes
    {
      get
      {
        return this.chaCtrl.nowCoordinate.clothes;
      }
    }

    private ChaFileClothes orgClothes
    {
      get
      {
        return this.chaCtrl.chaFile.coordinate.clothes;
      }
    }

    public int parts { get; set; }

    public int idx { get; set; }

    private ChaFileClothes.PartsInfo.ColorInfo nowColorInfo
    {
      get
      {
        return this.nowClothes.parts[this.parts].colorInfo[this.idx];
      }
    }

    private ChaFileClothes.PartsInfo.ColorInfo orgColorInfo
    {
      get
      {
        return this.orgClothes.parts[this.parts].colorInfo[this.idx];
      }
    }

    public void UpdateCustomUI()
    {
      if (this.parts == -1 || this.idx == -1)
        return;
      ChaFileClothes.PartsInfo.ColorInfo colorInfo = this.nowClothes.parts[this.parts].colorInfo[this.idx];
      this.csMainColor.SetColor(colorInfo.baseColor);
      this.ssGloss.SetSliderValue(colorInfo.glossPower);
      this.ssMetallic.SetSliderValue(colorInfo.metallicPower);
      this.ChangePatternImage();
      if (Object.op_Implicit((Object) this.objPatternSet))
        this.objPatternSet.SetActiveIfDifferent(0 != colorInfo.pattern);
      this.csPatternColor.SetColor(colorInfo.patternColor);
      this.ssPatternW.SetSliderValue((float) colorInfo.layout.x);
      this.ssPatternH.SetSliderValue((float) colorInfo.layout.y);
      this.ssPatternX.SetSliderValue((float) colorInfo.layout.z);
      this.ssPatternY.SetSliderValue((float) colorInfo.layout.w);
      this.ssPatternRot.SetSliderValue(colorInfo.rotation);
    }

    public void EnableColorAlpha(bool enable)
    {
      if (!Object.op_Implicit((Object) this.csMainColor))
        return;
      this.csMainColor.EnableColorAlpha(enable);
    }

    public void ChangePatternImage()
    {
      ListInfoBase listInfo = this.chaCtrl.lstCtrl.GetListInfo(ChaListDefine.CategoryNo.st_pattern, this.nowClothes.parts[this.parts].colorInfo[this.idx].pattern);
      Texture2D texture2D = CommonLib.LoadAsset<Texture2D>(listInfo.GetInfo(ChaListDefine.KeyType.ThumbAB), listInfo.GetInfo(ChaListDefine.KeyType.ThumbTex), false, string.Empty);
      if (!Object.op_Implicit((Object) texture2D))
        return;
      this.imgPattern.set_sprite(Sprite.Create(texture2D, new Rect(0.0f, 0.0f, (float) ((Texture) texture2D).get_width(), (float) ((Texture) texture2D).get_height()), new Vector2(0.5f, 0.5f)));
    }

    public CustomClothesColorSet.ClothesInfo GetDefaultClothesInfo()
    {
      float[] numArray1 = new float[3];
      float[] numArray2 = new float[3];
      Vector4[] vector4Array = new Vector4[3]
      {
        Vector4.get_zero(),
        Vector4.get_zero(),
        Vector4.get_zero()
      };
      float[] numArray3 = new float[3];
      if (Object.op_Inequality((Object) null, (Object) this.chaCtrl.cmpClothes[this.parts]))
      {
        numArray1[0] = this.chaCtrl.cmpClothes[this.parts].defGloss01;
        numArray1[1] = this.chaCtrl.cmpClothes[this.parts].defGloss02;
        numArray1[2] = this.chaCtrl.cmpClothes[this.parts].defGloss03;
        numArray2[0] = this.chaCtrl.cmpClothes[this.parts].defMetallic01;
        numArray2[1] = this.chaCtrl.cmpClothes[this.parts].defMetallic02;
        numArray2[2] = this.chaCtrl.cmpClothes[this.parts].defMetallic03;
        Vector4 vector4;
        vector4.x = (__Null) (double) Mathf.InverseLerp(20f, 1f, (float) this.chaCtrl.cmpClothes[this.parts].defLayout01.x);
        vector4.y = (__Null) (double) Mathf.InverseLerp(20f, 1f, (float) this.chaCtrl.cmpClothes[this.parts].defLayout01.y);
        vector4.z = (__Null) (double) Mathf.InverseLerp(-1f, 1f, (float) this.chaCtrl.cmpClothes[this.parts].defLayout01.z);
        vector4.w = (__Null) (double) Mathf.InverseLerp(-1f, 1f, (float) this.chaCtrl.cmpClothes[this.parts].defLayout01.w);
        vector4Array[0] = vector4;
        vector4.x = (__Null) (double) Mathf.InverseLerp(20f, 1f, (float) this.chaCtrl.cmpClothes[this.parts].defLayout02.x);
        vector4.y = (__Null) (double) Mathf.InverseLerp(20f, 1f, (float) this.chaCtrl.cmpClothes[this.parts].defLayout02.y);
        vector4.z = (__Null) (double) Mathf.InverseLerp(-1f, 1f, (float) this.chaCtrl.cmpClothes[this.parts].defLayout02.z);
        vector4.w = (__Null) (double) Mathf.InverseLerp(-1f, 1f, (float) this.chaCtrl.cmpClothes[this.parts].defLayout02.w);
        vector4Array[1] = vector4;
        vector4.x = (__Null) (double) Mathf.InverseLerp(20f, 1f, (float) this.chaCtrl.cmpClothes[this.parts].defLayout03.x);
        vector4.y = (__Null) (double) Mathf.InverseLerp(20f, 1f, (float) this.chaCtrl.cmpClothes[this.parts].defLayout03.y);
        vector4.z = (__Null) (double) Mathf.InverseLerp(-1f, 1f, (float) this.chaCtrl.cmpClothes[this.parts].defLayout03.z);
        vector4.w = (__Null) (double) Mathf.InverseLerp(-1f, 1f, (float) this.chaCtrl.cmpClothes[this.parts].defLayout03.w);
        vector4Array[2] = vector4;
        numArray3[0] = Mathf.InverseLerp(-1f, 1f, this.chaCtrl.cmpClothes[this.parts].defRotation01);
        numArray3[1] = Mathf.InverseLerp(-1f, 1f, this.chaCtrl.cmpClothes[this.parts].defRotation02);
        numArray3[2] = Mathf.InverseLerp(-1f, 1f, this.chaCtrl.cmpClothes[this.parts].defRotation03);
      }
      return new CustomClothesColorSet.ClothesInfo()
      {
        gloss = numArray1[this.idx],
        metallic = numArray2[this.idx],
        layout = vector4Array[this.idx],
        rot = numArray3[this.idx]
      };
    }

    public void Initialize(int _parts, int _idx)
    {
      this.parts = _parts;
      this.idx = _idx;
      if (this.parts == -1 || this.idx == -1)
        return;
      if (Object.op_Implicit((Object) this.title))
        this.title.set_text("カラー" + (this.idx + 1).ToString("00"));
      if (this.lstDisposable != null && this.lstDisposable.Count != 0)
      {
        int count = this.lstDisposable.Count;
        for (int index = 0; index < count; ++index)
          this.lstDisposable[index].Dispose();
      }
      this.csMainColor.actUpdateColor = (Action<Color>) (color =>
      {
        this.nowColorInfo.baseColor = color;
        this.orgColorInfo.baseColor = color;
        this.chaCtrl.ChangeCustomClothes(this.parts, true, false, false, false);
      });
      this.ssGloss.onChange = (Action<float>) (value =>
      {
        this.nowColorInfo.glossPower = value;
        this.orgColorInfo.glossPower = value;
        this.chaCtrl.ChangeCustomClothes(this.parts, true, false, false, false);
      });
      this.ssGloss.onSetDefaultValue = (Func<float>) (() => this.GetDefaultClothesInfo().gloss);
      this.ssMetallic.onChange = (Action<float>) (value =>
      {
        this.nowColorInfo.metallicPower = value;
        this.orgColorInfo.metallicPower = value;
        this.chaCtrl.ChangeCustomClothes(this.parts, true, false, false, false);
      });
      this.ssMetallic.onSetDefaultValue = (Func<float>) (() => this.GetDefaultClothesInfo().metallic);
      this.lstDisposable.Add(ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.btnPatternWin), (Action<M0>) (_ =>
      {
        this.customBase.customCtrl.showPattern = true;
        this.clothesPtnSel.ChangeLink(this.parts, this.idx);
        this.clothesPtnSel.onSelect = (Action<int, int>) ((p, i) =>
        {
          this.ChangePatternImage();
          if (!Object.op_Implicit((Object) this.objPatternSet))
            return;
          this.objPatternSet.SetActiveIfDifferent(0 != this.nowColorInfo.pattern);
        });
      })));
      this.csPatternColor.actUpdateColor = (Action<Color>) (color =>
      {
        this.nowColorInfo.patternColor = color;
        this.orgColorInfo.patternColor = color;
        this.chaCtrl.ChangeCustomClothes(this.parts, true, false, false, false);
      });
      this.ssPatternW.onChange = (Action<float>) (value =>
      {
        this.nowColorInfo.layout = new Vector4(value, (float) this.nowColorInfo.layout.y, (float) this.nowColorInfo.layout.z, (float) this.nowColorInfo.layout.w);
        this.orgColorInfo.layout = this.nowColorInfo.layout;
        this.chaCtrl.ChangeCustomClothes(this.parts, true, false, false, false);
      });
      this.ssPatternW.onSetDefaultValue = (Func<float>) (() => (float) this.GetDefaultClothesInfo().layout.x);
      this.ssPatternH.onChange = (Action<float>) (value =>
      {
        this.nowColorInfo.layout = new Vector4((float) this.nowColorInfo.layout.x, value, (float) this.nowColorInfo.layout.z, (float) this.nowColorInfo.layout.w);
        this.orgColorInfo.layout = this.nowColorInfo.layout;
        this.chaCtrl.ChangeCustomClothes(this.parts, true, false, false, false);
      });
      this.ssPatternH.onSetDefaultValue = (Func<float>) (() => (float) this.GetDefaultClothesInfo().layout.y);
      this.ssPatternX.onChange = (Action<float>) (value =>
      {
        this.nowColorInfo.layout = new Vector4((float) this.nowColorInfo.layout.x, (float) this.nowColorInfo.layout.y, value, (float) this.nowColorInfo.layout.w);
        this.orgColorInfo.layout = this.nowColorInfo.layout;
        this.chaCtrl.ChangeCustomClothes(this.parts, true, false, false, false);
      });
      this.ssPatternX.onSetDefaultValue = (Func<float>) (() => (float) this.GetDefaultClothesInfo().layout.z);
      this.ssPatternY.onChange = (Action<float>) (value =>
      {
        this.nowColorInfo.layout = new Vector4((float) this.nowColorInfo.layout.x, (float) this.nowColorInfo.layout.y, (float) this.nowColorInfo.layout.z, value);
        this.orgColorInfo.layout = this.nowColorInfo.layout;
        this.chaCtrl.ChangeCustomClothes(this.parts, true, false, false, false);
      });
      this.ssPatternY.onSetDefaultValue = (Func<float>) (() => (float) this.GetDefaultClothesInfo().layout.w);
      this.ssPatternRot.onChange = (Action<float>) (value =>
      {
        this.nowColorInfo.rotation = value;
        this.orgColorInfo.rotation = value;
        this.chaCtrl.ChangeCustomClothes(this.parts, true, false, false, false);
      });
      this.ssPatternRot.onSetDefaultValue = (Func<float>) (() => this.GetDefaultClothesInfo().rot);
      this.UpdateCustomUI();
      this.ssGloss.SetInputTextValue(CustomBase.ConvertTextFromRate(0, 100, this.nowColorInfo.glossPower));
      this.ssMetallic.SetInputTextValue(CustomBase.ConvertTextFromRate(0, 100, this.nowColorInfo.metallicPower));
      this.ssPatternW.SetInputTextValue(CustomBase.ConvertTextFromRate(0, 100, (float) this.nowColorInfo.layout.x));
      this.ssPatternH.SetInputTextValue(CustomBase.ConvertTextFromRate(0, 100, (float) this.nowColorInfo.layout.y));
      this.ssPatternX.SetInputTextValue(CustomBase.ConvertTextFromRate(0, 100, (float) this.nowColorInfo.layout.z));
      this.ssPatternY.SetInputTextValue(CustomBase.ConvertTextFromRate(0, 100, (float) this.nowColorInfo.layout.w));
      this.ssPatternRot.SetInputTextValue(CustomBase.ConvertTextFromRate(0, 100, this.nowColorInfo.rotation));
    }

    public class ClothesInfo
    {
      public Vector4 layout = Vector4.get_zero();
      public float gloss;
      public float metallic;
      public float rot;
    }
  }
}
