// Decompiled with JetBrains decompiler
// Type: ConfigScene.HSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

namespace ConfigScene
{
  public class HSetting : BaseSetting
  {
    [Header("主人公の表示")]
    [SerializeField]
    private Toggle[] drawToggles;
    [Header("主人公の男根")]
    [SerializeField]
    private Toggle[] sonToggles;
    [Header("主人公の服")]
    [SerializeField]
    private Toggle[] clothToggles;
    [Header("主人公のアクセサリー")]
    [SerializeField]
    private Toggle[] accessoryToggles;
    [Header("主人公の靴")]
    [SerializeField]
    private Toggle[] shoesToggles;
    [Header("主人公を単色化")]
    [SerializeField]
    private Toggle[] silhouetteToggles;
    [Header("単色")]
    [SerializeField]
    private UI_SampleColor silhouetteCololr;
    [Header("汁")]
    [SerializeField]
    private Toggle[] siruToggles;
    [Header("尿")]
    [SerializeField]
    private Toggle[] urineToggles;
    [Header("潮吹き")]
    [SerializeField]
    private Toggle[] sioToggles;
    [Header("艶")]
    [SerializeField]
    private Toggle[] glossToggles;
    [Header("快感ゲージ")]
    [SerializeField]
    private Toggle[] gaugeToggles;
    [Header("操作ガイド")]
    [SerializeField]
    private Toggle[] guideToggles;
    [Header("メニューアイコン")]
    [SerializeField]
    private Toggle[] muneToggles;
    [Header("フィニッシュボタン")]
    [SerializeField]
    private Toggle[] finishToggles;
    [Header("カメラ初期化判断")]
    [SerializeField]
    private Toggle[] initCameraToggles;
    [Header("１人目視線")]
    [SerializeField]
    private Toggle[] eyeDir0Toggles;
    [Header("１人目首の向き")]
    [SerializeField]
    private Toggle[] neckDir0Toggles;
    [Header("２人目視線")]
    [SerializeField]
    private Toggle[] eyeDir1Toggles;
    [Header("２人目首の向き")]
    [SerializeField]
    private Toggle[] neckDir1Toggles;

    public override void Init()
    {
      HSystem hdata = Manager.Config.HData;
      GraphicSystem gdata = Manager.Config.GraphicData;
      this.LinkToggleArray(this.drawToggles, (Action<int>) (i => hdata.Visible = i == 0));
      this.LinkToggleArray(this.sonToggles, (Action<int>) (i => hdata.Son = i == 0));
      this.LinkToggleArray(this.clothToggles, (Action<int>) (i => hdata.Cloth = i == 0));
      this.LinkToggleArray(this.accessoryToggles, (Action<int>) (i => hdata.Accessory = i == 0));
      this.LinkToggleArray(this.shoesToggles, (Action<int>) (i => hdata.Shoes = i == 0));
      this.LinkToggleArray(this.silhouetteToggles, (Action<int>) (i => gdata.SimpleBody = i == 0));
      this.silhouetteCololr.actUpdateColor = (Action<Color>) (c => gdata.SilhouetteColor = c);
      this.LinkToggleArray(this.siruToggles, (Action<int>) (i => hdata.Siru = i));
      this.LinkToggleArray(this.urineToggles, (Action<int>) (i => hdata.Urine = i == 0));
      this.LinkToggleArray(this.sioToggles, (Action<int>) (i => hdata.Sio = i == 0));
      this.LinkToggleArray(this.glossToggles, (Action<int>) (i => hdata.Gloss = i == 0));
      this.LinkToggleArray(this.gaugeToggles, (Action<int>) (i => hdata.FeelingGauge = i == 0));
      this.LinkToggleArray(this.guideToggles, (Action<int>) (i => hdata.ActionGuide = i == 0));
      this.LinkToggleArray(this.muneToggles, (Action<int>) (i => hdata.MenuIcon = i == 0));
      this.LinkToggleArray(this.finishToggles, (Action<int>) (i => hdata.FinishButton = i == 0));
      this.LinkToggleArray(this.initCameraToggles, (Action<int>) (i => hdata.InitCamera = i == 0));
      this.LinkToggleArray(this.eyeDir0Toggles, (Action<int>) (i => hdata.EyeDir0 = i == 0));
      this.LinkToggleArray(this.neckDir0Toggles, (Action<int>) (i => hdata.NeckDir0 = i == 0));
      this.LinkToggleArray(this.eyeDir1Toggles, (Action<int>) (i => hdata.EyeDir1 = i == 0));
      this.LinkToggleArray(this.neckDir1Toggles, (Action<int>) (i => hdata.NeckDir1 = i == 0));
    }

    protected override void ValueToUI()
    {
      HSystem hdata = Manager.Config.HData;
      GraphicSystem gdata = Manager.Config.GraphicData;
      this.SetToggleUIArray(this.drawToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !hdata.Visible : hdata.Visible)));
      this.SetToggleUIArray(this.sonToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !hdata.Son : hdata.Son)));
      this.SetToggleUIArray(this.clothToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !hdata.Cloth : hdata.Cloth)));
      this.SetToggleUIArray(this.accessoryToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !hdata.Accessory : hdata.Accessory)));
      this.SetToggleUIArray(this.shoesToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !hdata.Shoes : hdata.Shoes)));
      this.SetToggleUIArray(this.silhouetteToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !gdata.SimpleBody : gdata.SimpleBody)));
      this.silhouetteCololr.SetColor(gdata.SilhouetteColor);
      this.SetToggleUIArray(this.siruToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index == hdata.Siru)));
      this.SetToggleUIArray(this.urineToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !hdata.Urine : hdata.Urine)));
      this.SetToggleUIArray(this.sioToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !hdata.Sio : hdata.Sio)));
      this.SetToggleUIArray(this.glossToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !hdata.Gloss : hdata.Gloss)));
      this.SetToggleUIArray(this.gaugeToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !hdata.FeelingGauge : hdata.FeelingGauge)));
      this.SetToggleUIArray(this.guideToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !hdata.ActionGuide : hdata.ActionGuide)));
      this.SetToggleUIArray(this.muneToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !hdata.MenuIcon : hdata.MenuIcon)));
      this.SetToggleUIArray(this.finishToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !hdata.FinishButton : hdata.FinishButton)));
      this.SetToggleUIArray(this.initCameraToggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !hdata.InitCamera : hdata.InitCamera)));
      this.SetToggleUIArray(this.eyeDir0Toggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !hdata.EyeDir0 : hdata.EyeDir0)));
      this.SetToggleUIArray(this.neckDir0Toggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !hdata.NeckDir0 : hdata.NeckDir0)));
      this.SetToggleUIArray(this.eyeDir1Toggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !hdata.EyeDir1 : hdata.EyeDir1)));
      this.SetToggleUIArray(this.neckDir1Toggles, (Action<Toggle, int>) ((tgl, index) => tgl.set_isOn(index != 0 ? !hdata.NeckDir1 : hdata.NeckDir1)));
    }
  }
}
