// Decompiled with JetBrains decompiler
// Type: ConfigScene.GraphicSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ConfigScene
{
  public class GraphicSetting : BaseSetting
  {
    private bool ChangeSlider = true;
    private bool[][] effectEnables = new bool[4][]
    {
      new bool[8]
      {
        true,
        false,
        false,
        false,
        false,
        false,
        false,
        false
      },
      new bool[8]
      {
        true,
        false,
        true,
        true,
        true,
        true,
        false,
        false
      },
      new bool[8]
      {
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        false
      },
      new bool[8]
      {
        true,
        true,
        true,
        true,
        true,
        true,
        true,
        true
      }
    };
    private Dictionary<int, List<bool>> easySettingInfo = new Dictionary<int, List<bool>>();
    [Header("描画レベルスライダー")]
    [SerializeField]
    private Slider qualitySlider;
    [Header("描画レベルスライダー数字")]
    [SerializeField]
    private Text[] qualityNumberText;
    [Header("数字の色")]
    [SerializeField]
    private Color[] qualitySelectColor;
    [Header("セルフシャドウ")]
    [SerializeField]
    private Toggle selfShadowToggle;
    [Header("被写界深度")]
    [SerializeField]
    private Toggle depthOfFieldToggle;
    [Header("ブルーム")]
    [SerializeField]
    private Toggle bloomToggle;
    [Header("大気表現")]
    [SerializeField]
    private Toggle atmosphereToggle;
    [Header("SSAO")]
    [SerializeField]
    private Toggle ssaoToggle;
    [Header("ビグネット")]
    [SerializeField]
    private Toggle vignetteToggle;
    [Header("SSR")]
    [SerializeField]
    private Toggle ssrToggle;
    [Header("雨の描画")]
    [SerializeField]
    private Toggle rainToggle;
    [Header("キャラ解像度")]
    [SerializeField]
    private Toggle[] charaLevalToggles;
    [Header("マップ解像度")]
    [SerializeField]
    private Toggle[] mapLevalToggles;
    [Header("フェイスライト")]
    [SerializeField]
    private Toggle[] faceLightToggles;
    [Header("環境ライト")]
    [SerializeField]
    private Toggle[] ambientToggles;
    [Header("遮蔽")]
    [SerializeField]
    private Toggle[] shieldToggles;
    [Header("背景色")]
    [SerializeField]
    private UI_SampleColor backGroundCololr;
    [Header("登場人数制限")]
    [SerializeField]
    private Slider charaMaxNumSlider;
    [Header("登場制限")]
    [SerializeField]
    private Toggle Entry0Toggle;
    [SerializeField]
    private Toggle Entry1Toggle;
    [SerializeField]
    private Toggle Entry2Toggle;
    [SerializeField]
    private Toggle Entry3Toggle;

    public override void Init()
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GraphicSetting.\u003CInit\u003Ec__AnonStorey0 initCAnonStorey0 = new GraphicSetting.\u003CInit\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      initCAnonStorey0.\u0024this = this;
      for (int index = 0; index < 4; ++index)
        this.easySettingInfo.Add(index + 1, ((IEnumerable<bool>) this.effectEnables[index]).ToList<bool>());
      // ISSUE: reference to a compiler-generated field
      initCAnonStorey0.data = Manager.Config.GraphicData;
      // ISSUE: reference to a compiler-generated method
      this.LinkToggle(this.selfShadowToggle, new Action<bool>(initCAnonStorey0.\u003C\u003Em__0));
      // ISSUE: reference to a compiler-generated method
      this.LinkToggle(this.depthOfFieldToggle, new Action<bool>(initCAnonStorey0.\u003C\u003Em__1));
      // ISSUE: reference to a compiler-generated method
      this.LinkToggle(this.bloomToggle, new Action<bool>(initCAnonStorey0.\u003C\u003Em__2));
      // ISSUE: reference to a compiler-generated method
      this.LinkToggle(this.atmosphereToggle, new Action<bool>(initCAnonStorey0.\u003C\u003Em__3));
      // ISSUE: reference to a compiler-generated method
      this.LinkToggle(this.ssaoToggle, new Action<bool>(initCAnonStorey0.\u003C\u003Em__4));
      // ISSUE: reference to a compiler-generated method
      this.LinkToggle(this.vignetteToggle, new Action<bool>(initCAnonStorey0.\u003C\u003Em__5));
      // ISSUE: reference to a compiler-generated method
      this.LinkToggle(this.ssrToggle, new Action<bool>(initCAnonStorey0.\u003C\u003Em__6));
      // ISSUE: reference to a compiler-generated method
      this.LinkToggle(this.rainToggle, new Action<bool>(initCAnonStorey0.\u003C\u003Em__7));
      // ISSUE: method pointer
      ((UnityEvent<float>) this.qualitySlider.get_onValueChanged()).AddListener(new UnityAction<float>((object) initCAnonStorey0, __methodptr(\u003C\u003Em__8)));
      // ISSUE: reference to a compiler-generated method
      this.LinkToggleArray(this.charaLevalToggles, new Action<int>(initCAnonStorey0.\u003C\u003Em__9));
      // ISSUE: reference to a compiler-generated method
      this.LinkToggleArray(this.mapLevalToggles, new Action<int>(initCAnonStorey0.\u003C\u003Em__A));
      // ISSUE: reference to a compiler-generated method
      this.LinkToggleArray(this.faceLightToggles, new Action<int>(initCAnonStorey0.\u003C\u003Em__B));
      // ISSUE: reference to a compiler-generated method
      this.LinkToggleArray(this.ambientToggles, new Action<int>(initCAnonStorey0.\u003C\u003Em__C));
      // ISSUE: reference to a compiler-generated method
      this.LinkToggleArray(this.shieldToggles, new Action<int>(initCAnonStorey0.\u003C\u003Em__D));
      // ISSUE: reference to a compiler-generated method
      this.backGroundCololr.actUpdateColor = new Action<Color>(initCAnonStorey0.\u003C\u003Em__E);
      // ISSUE: reference to a compiler-generated method
      this.LinkSlider(this.charaMaxNumSlider, new Action<float>(initCAnonStorey0.\u003C\u003Em__F));
      // ISSUE: reference to a compiler-generated method
      this.LinkToggle(this.Entry0Toggle, new Action<bool>(initCAnonStorey0.\u003C\u003Em__10));
      // ISSUE: reference to a compiler-generated method
      this.LinkToggle(this.Entry1Toggle, new Action<bool>(initCAnonStorey0.\u003C\u003Em__11));
      // ISSUE: reference to a compiler-generated method
      this.LinkToggle(this.Entry2Toggle, new Action<bool>(initCAnonStorey0.\u003C\u003Em__12));
      // ISSUE: reference to a compiler-generated method
      this.LinkToggle(this.Entry3Toggle, new Action<bool>(initCAnonStorey0.\u003C\u003Em__13));
      this.ChangeSlider = true;
    }

    protected override void ValueToUI()
    {
      GraphicSystem data = Manager.Config.GraphicData;
      this.selfShadowToggle.set_isOn(data.SelfShadow);
      this.depthOfFieldToggle.set_isOn(data.DepthOfField);
      this.bloomToggle.set_isOn(data.Bloom);
      this.atmosphereToggle.set_isOn(data.Atmospheric);
      this.ssaoToggle.set_isOn(data.SSAO);
      this.vignetteToggle.set_isOn(data.Vignette);
      this.ssrToggle.set_isOn(data.SSR);
      this.rainToggle.set_isOn(data.Rain);
      this.SetEasySlider();
      this.SetToggleUIArray(this.charaLevalToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index == (int) data.CharaGraphicQuality)));
      this.SetToggleUIArray(this.mapLevalToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index == (int) data.MapGraphicQuality)));
      this.SetToggleUIArray(this.faceLightToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !data.FaceLight : data.FaceLight)));
      this.SetToggleUIArray(this.ambientToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !data.AmbientLight : data.AmbientLight)));
      this.SetToggleUIArray(this.shieldToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !data.Shield : data.Shield)));
      this.backGroundCololr.SetColor(data.BackColor);
      this.charaMaxNumSlider.set_value((float) data.MaxCharaNum);
      this.Entry0Toggle.set_isOn(data.CharasEntry[0]);
      this.Entry1Toggle.set_isOn(data.CharasEntry[1]);
      this.Entry2Toggle.set_isOn(data.CharasEntry[2]);
      this.Entry3Toggle.set_isOn(data.CharasEntry[3]);
    }

    private void SetEasySlider()
    {
      GraphicSystem graphicData = Manager.Config.GraphicData;
      bool[] flagArray = new bool[8]
      {
        graphicData.SelfShadow,
        graphicData.DepthOfField,
        graphicData.Bloom,
        graphicData.Atmospheric,
        graphicData.SSAO,
        graphicData.Vignette,
        graphicData.SSR,
        graphicData.Rain
      };
      List<bool> boolList = new List<bool>();
      int length = flagArray.Length;
      foreach (KeyValuePair<int, List<bool>> keyValuePair in this.easySettingInfo)
      {
        int num = 0;
        for (int index = 0; index < length; ++index)
        {
          if (keyValuePair.Value[index] && flagArray[index] == keyValuePair.Value[index])
            ++num;
        }
        boolList.Add(num == keyValuePair.Value.Count<bool>((Func<bool, bool>) (b => b)));
      }
      int num1 = boolList.FindLastIndex((Predicate<bool>) (v => v)) + 1;
      if ((double) this.qualitySlider.get_value() == (double) num1)
        return;
      this.ChangeSlider = false;
      this.qualitySlider.set_value((float) num1);
    }

    private void SetNumberColor(int _value)
    {
      for (int index = 0; index < this.qualityNumberText.Length; ++index)
        ((Graphic) this.qualityNumberText[index]).set_color(_value != index + 1 ? this.qualitySelectColor[1] : this.qualitySelectColor[0]);
    }

    public void EntryInteractable(bool _interactable)
    {
      ((Selectable) this.Entry0Toggle).set_interactable(_interactable);
      ((Selectable) this.Entry1Toggle).set_interactable(_interactable);
      ((Selectable) this.Entry2Toggle).set_interactable(_interactable);
      ((Selectable) this.Entry3Toggle).set_interactable(_interactable);
    }
  }
}
