// Decompiled with JetBrains decompiler
// Type: Studio.MPItemCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class MPItemCtrl : MonoBehaviour
  {
    [SerializeField]
    private MPItemCtrl.ColorInfo[] colorInfo;
    [SerializeField]
    private Sprite[] spriteBack;
    [SerializeField]
    private MPItemCtrl.PanelInfo panelInfo;
    [SerializeField]
    private MPItemCtrl.PanelList panelList;
    [SerializeField]
    private MPItemCtrl.ColorCombination colorShadow;
    [SerializeField]
    private MPItemCtrl.InputCombination inputAlpha;
    [SerializeField]
    private MPItemCtrl.EmissionInfo emissionInfo;
    [SerializeField]
    private MPItemCtrl.InputCombination inputLightCancel;
    [SerializeField]
    private MPItemCtrl.LineInfo lineInfo;
    [SerializeField]
    [Header("キネマティクス関係")]
    private MPItemCtrl.KinematicInfo kinematicInfo;
    [SerializeField]
    [Header("オプション関係")]
    private MPItemCtrl.OptionInfo optionInfo;
    [SerializeField]
    [Header("パターン関係")]
    private CanvasGroup cgPattern;
    [SerializeField]
    [Header("アニメ関係")]
    private MPItemCtrl.AnimeInfo animeInfo;
    [SerializeField]
    private AnimeControl animeControl;
    [SerializeField]
    private MPCharCtrl mpCharCtrl;
    private OCIItem m_OCIItem;
    private bool m_Active;
    private bool isUpdateInfo;
    private bool isColorFunc;

    public MPItemCtrl()
    {
      base.\u002Ector();
    }

    public OCIItem ociItem
    {
      get
      {
        return this.m_OCIItem;
      }
      set
      {
        this.m_OCIItem = value;
        if (this.m_OCIItem == null)
          return;
        this.UpdateInfo();
      }
    }

    public bool active
    {
      get
      {
        return this.m_Active;
      }
      set
      {
        this.m_Active = value;
        if (this.m_Active)
        {
          ((Component) this).get_gameObject().SetActive(this.m_OCIItem != null && this.m_OCIItem.CheckAnim);
          this.animeControl.active = this.m_OCIItem != null && this.m_OCIItem.isAnime;
        }
        else
        {
          if (!this.mpCharCtrl.active)
            this.animeControl.active = false;
          ((Component) this).get_gameObject().SetActive(false);
          if (this.isColorFunc)
            Singleton<Studio.Studio>.Instance.colorPalette.Close();
          this.isColorFunc = false;
          Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
          this.panelList.active = false;
          this.cgPattern.Enable(false, false);
        }
      }
    }

    public bool Deselect(OCIItem _ociItem)
    {
      if (this.m_OCIItem != _ociItem)
        return false;
      this.ociItem = (OCIItem) null;
      this.active = false;
      return true;
    }

    public void UpdateInfo()
    {
      if (this.m_OCIItem == null)
        return;
      this.isUpdateInfo = true;
      bool[] useColor = this.m_OCIItem.useColor;
      bool[] useMetallic = this.m_OCIItem.useMetallic;
      bool[] usePattern = this.m_OCIItem.usePattern;
      for (int index = 0; index < 3; ++index)
      {
        this.colorInfo[index].enable = useColor[index];
        if (useColor[index])
        {
          Studio.ColorInfo color = this.m_OCIItem.itemInfo.colors[index];
          this.colorInfo[index].colorMain = color.mainColor;
          this.colorInfo[index].EnableMetallic = useMetallic[index];
          if (useMetallic[index])
          {
            this.colorInfo[index].inputMetallic.value = color.metallic;
            this.colorInfo[index].inputGlossiness.value = color.glossiness;
          }
          this.colorInfo[index].EnablePattern = usePattern[index];
          if (usePattern[index])
          {
            this.colorInfo[index].textPattern = color.pattern.name;
            this.colorInfo[index].colorPattern = color.pattern.color;
            this.colorInfo[index].isOn = !color.pattern.clamp;
            this.colorInfo[index][0].value = color.pattern.ut;
            this.colorInfo[index][1].value = color.pattern.vt;
            this.colorInfo[index][2].value = color.pattern.us;
            this.colorInfo[index][3].value = color.pattern.vs;
            this.colorInfo[index][4].value = color.pattern.rot;
          }
        }
      }
      this.colorInfo[3].enable = this.m_OCIItem.useColor4;
      if (this.colorInfo[3].enable)
      {
        this.colorInfo[3].colorMain = this.m_OCIItem.itemInfo.colors[3].mainColor;
        this.colorInfo[3].EnableMetallic = false;
        this.colorInfo[3].EnablePattern = false;
      }
      this.panelInfo.enable = this.m_OCIItem.checkPanel;
      this.panelList.active = false;
      this.SetPanelTexName(this.m_OCIItem.itemInfo.panel.filePath);
      this.panelInfo.color = this.m_OCIItem.itemInfo.colors[0].mainColor;
      this.panelInfo.isOn = !this.m_OCIItem.itemInfo.colors[0].pattern.clamp;
      this.panelInfo[0].value = this.m_OCIItem.itemInfo.colors[0].pattern.ut;
      this.panelInfo[1].value = this.m_OCIItem.itemInfo.colors[0].pattern.vt;
      this.panelInfo[2].value = this.m_OCIItem.itemInfo.colors[0].pattern.us;
      this.panelInfo[3].value = this.m_OCIItem.itemInfo.colors[0].pattern.vs;
      this.panelInfo[4].value = this.m_OCIItem.itemInfo.colors[0].pattern.rot;
      this.inputAlpha.value = this.m_OCIItem.itemInfo.alpha;
      this.inputAlpha.active = this.m_OCIItem.CheckAlpha;
      this.emissionInfo.active = this.m_OCIItem.CheckEmission;
      if (this.m_OCIItem.CheckEmissionColor)
      {
        this.emissionInfo.color.interactable = true;
        this.emissionInfo.color.color = this.m_OCIItem.itemInfo.EmissionColor;
      }
      else
      {
        this.emissionInfo.color.interactable = false;
        this.emissionInfo.color.color = Color.get_white();
      }
      if (this.m_OCIItem.CheckEmissionPower)
      {
        this.emissionInfo.input.interactable = true;
        this.emissionInfo.input.value = this.m_OCIItem.itemInfo.emissionPower;
      }
      else
      {
        this.emissionInfo.input.interactable = false;
        this.emissionInfo.input.value = 0.0f;
      }
      this.inputLightCancel.active = this.m_OCIItem.CheckLightCancel;
      this.inputLightCancel.value = this.m_OCIItem.itemInfo.lightCancel;
      this.kinematicInfo.Active = this.m_OCIItem.isFK || this.m_OCIItem.isDynamicBone;
      ((Selectable) this.kinematicInfo.toggleFK).set_interactable(this.m_OCIItem.isFK);
      this.kinematicInfo.toggleFK.set_isOn(this.m_OCIItem.itemInfo.enableFK);
      ((Selectable) this.kinematicInfo.toggleDynamicBone).set_interactable(this.m_OCIItem.isDynamicBone);
      this.kinematicInfo.toggleDynamicBone.set_isOn(this.m_OCIItem.itemInfo.enableDynamicBone);
      this.optionInfo.Active = this.m_OCIItem.CheckOption;
      this.optionInfo.toggleAll.set_isOn(this.m_OCIItem.itemInfo.option.SafeGet<bool>(0));
      this.animeInfo.Active = this.m_OCIItem.CheckAnimePattern;
      this.animeInfo.InitDropdown(this.m_OCIItem);
      this.animeControl.objectCtrlInfo = (ObjectCtrlInfo) this.m_OCIItem;
      this.cgPattern.Enable(false, false);
      this.isUpdateInfo = false;
    }

    private void OnClickColorMain(int _idx)
    {
      string[] strArray = new string[3]
      {
        "アイテム カラー１",
        "アイテム カラー２",
        "アイテム カラー３"
      };
      if (Singleton<Studio.Studio>.Instance.colorPalette.Check(strArray[_idx]))
      {
        this.isColorFunc = false;
        Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
      }
      else
      {
        IEnumerable<OCIItem> array = ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem));
        Singleton<Studio.Studio>.Instance.colorPalette.Setup(strArray[_idx], this.m_OCIItem.itemInfo.colors[_idx].mainColor, (Action<Color>) (_c =>
        {
          foreach (OCIItem ociItem in array)
            ociItem.SetColor(!ociItem.IsParticle ? new Color((float) _c.r, (float) _c.g, (float) _c.b, 1f) : _c, _idx);
          this.colorInfo[_idx].colorMain = _c;
        }), this.m_OCIItem.IsParticle);
        this.isColorFunc = true;
      }
    }

    private void OnClickColor4()
    {
      if (Singleton<Studio.Studio>.Instance.colorPalette.Check("アイテム カラー４"))
      {
        this.isColorFunc = false;
        Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
      }
      else
      {
        IEnumerable<OCIItem> array = ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem));
        Singleton<Studio.Studio>.Instance.colorPalette.Setup("アイテム カラー４", this.m_OCIItem.itemInfo.colors[3].mainColor, (Action<Color>) (_c =>
        {
          foreach (OCIItem ociItem in array)
            ociItem.SetColor(_c, 3);
          this.colorInfo[3].colorMain = _c;
        }), true);
        this.isColorFunc = true;
      }
    }

    private void OnClickColorMainDef(int _idx)
    {
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
      {
        if (ociItem.isChangeColor && ociItem.useColor.SafeGet<bool>(_idx))
          ociItem.SetColor(ociItem.defColor[_idx], _idx);
      }
      this.m_OCIItem.defColor.SafeProc<Color>(_idx, (Action<Color>) (_c => this.colorInfo[_idx].colorMain = _c));
      this.isColorFunc = false;
      Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
    }

    private void OnClickColor4Def()
    {
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
        ociItem.SetColor(ociItem.itemComponent.defGlass, 3);
      this.colorInfo[3].colorMain = this.m_OCIItem.itemComponent.defGlass;
      this.isColorFunc = false;
      Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
    }

    private void OnValueChangeMetallic(int _idx, float _value)
    {
      if (this.isUpdateInfo)
        return;
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
        ociItem.SetMetallic(_idx, _value);
      this.colorInfo[_idx].inputMetallic.value = _value;
    }

    private void OnEndEditMetallic(int _idx, string _text)
    {
      float num = Mathf.Clamp(Utility.StringToFloat(_text), 0.0f, 1f);
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
        ociItem.SetMetallic(_idx, num);
      this.colorInfo[_idx].inputMetallic.value = num;
    }

    private void OnClickMetallicDef(int _idx)
    {
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
      {
        OCIItem v = ociItem;
        v.itemComponent[_idx].SafeProc<ItemComponent.Info>((Action<ItemComponent.Info>) (info => v.SetMetallic(_idx, info.defMetallic)));
      }
      this.m_OCIItem.itemComponent[_idx].SafeProc<ItemComponent.Info>((Action<ItemComponent.Info>) (info => this.colorInfo[_idx].inputMetallic.value = info.defMetallic));
    }

    private void OnValueChangeGlossiness(int _idx, float _value)
    {
      if (this.isUpdateInfo)
        return;
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
        ociItem.SetGlossiness(_idx, _value);
      this.colorInfo[_idx].inputGlossiness.value = _value;
    }

    private void OnEndEditGlossiness(int _idx, string _text)
    {
      float num = Mathf.Clamp(Utility.StringToFloat(_text), 0.0f, 1f);
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
        ociItem.SetGlossiness(_idx, num);
      this.colorInfo[_idx].inputGlossiness.value = num;
    }

    private void OnClickGlossinessDef(int _idx)
    {
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
      {
        OCIItem v = ociItem;
        v.itemComponent[_idx].SafeProc<ItemComponent.Info>((Action<ItemComponent.Info>) (info => v.SetGlossiness(_idx, info.defGlossiness)));
      }
      this.m_OCIItem.itemComponent[_idx].SafeProc<ItemComponent.Info>((Action<ItemComponent.Info>) (info => this.colorInfo[_idx].inputGlossiness.value = info.defGlossiness));
    }

    private void OnClickColorPattern(int _idx)
    {
      string[] strArray = new string[3]
      {
        "柄の色１",
        "柄の色２",
        "柄の色３"
      };
      if (Singleton<Studio.Studio>.Instance.colorPalette.Check(strArray[_idx]))
      {
        this.isColorFunc = false;
        Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
      }
      else
      {
        IEnumerable<OCIItem> array = ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem));
        Singleton<Studio.Studio>.Instance.colorPalette.Setup(strArray[_idx], this.m_OCIItem.itemInfo.colors[_idx].pattern.color, (Action<Color>) (_c =>
        {
          foreach (OCIItem ociItem in array)
            ociItem.SetPatternColor(_idx, _c);
          this.colorInfo[_idx].colorPattern = _c;
        }), true);
        this.isColorFunc = true;
      }
    }

    private void OnClickColorPatternDef(int _idx)
    {
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
        ociItem.SetPatternColor(_idx, ociItem.itemComponent.defColorPattern[_idx]);
      this.colorInfo[_idx].colorPattern = this.m_OCIItem.itemComponent.defColorPattern[_idx];
      this.isColorFunc = false;
      Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
    }

    private void OnToggleColor(int _idx, bool _flag)
    {
      if (this.m_OCIItem == null)
        return;
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
        ociItem.SetPatternClamp(_idx, !_flag);
    }

    private void OnValueChangeUT(int _idx, float _value)
    {
      if (this.isUpdateInfo)
        return;
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
        ociItem.SetPatternUT(_idx, _value);
      this.colorInfo[_idx][0].value = _value;
    }

    private void OnEndEditUT(int _idx, string _text)
    {
      float num = Mathf.Clamp(Utility.StringToFloat(_text), -1f, 1f);
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
        ociItem.SetPatternUT(_idx, num);
      this.colorInfo[_idx][0].value = num;
    }

    private void OnClickUTDef(int _idx)
    {
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
      {
        OCIItem v = ociItem;
        v.itemComponent[_idx].SafeProc<ItemComponent.Info>((Action<ItemComponent.Info>) (info => v.SetPatternUT(_idx, info.ut)));
      }
      this.m_OCIItem.itemComponent[_idx].SafeProc<ItemComponent.Info>((Action<ItemComponent.Info>) (info => this.colorInfo[_idx][0].value = info.ut));
    }

    private void OnValueChangeVT(int _idx, float _value)
    {
      if (this.isUpdateInfo)
        return;
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
        ociItem.SetPatternVT(_idx, _value);
      this.colorInfo[_idx][1].value = _value;
    }

    private void OnEndEditVT(int _idx, string _text)
    {
      float num = Mathf.Clamp(Utility.StringToFloat(_text), -1f, 1f);
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
        ociItem.SetPatternVT(_idx, num);
      this.colorInfo[_idx][1].value = num;
    }

    private void OnClickVTDef(int _idx)
    {
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
      {
        OCIItem v = ociItem;
        v.itemComponent[_idx].SafeProc<ItemComponent.Info>((Action<ItemComponent.Info>) (info => v.SetPatternVT(_idx, info.vt)));
      }
      this.m_OCIItem.itemComponent[_idx].SafeProc<ItemComponent.Info>((Action<ItemComponent.Info>) (info => this.colorInfo[_idx][1].value = info.vt));
    }

    private void OnValueChangeUS(int _idx, float _value)
    {
      if (this.isUpdateInfo)
        return;
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
        ociItem.SetPatternUS(_idx, _value);
      this.colorInfo[_idx][2].value = _value;
    }

    private void OnEndEditUS(int _idx, string _text)
    {
      float num = Mathf.Clamp(Utility.StringToFloat(_text), 0.01f, 20f);
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
        ociItem.SetPatternUS(_idx, num);
      this.colorInfo[_idx][2].value = num;
    }

    private void OnClickUSDef(int _idx)
    {
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
      {
        OCIItem v = ociItem;
        v.itemComponent[_idx].SafeProc<ItemComponent.Info>((Action<ItemComponent.Info>) (info => v.SetPatternUS(_idx, info.us)));
      }
      this.m_OCIItem.itemComponent[_idx].SafeProc<ItemComponent.Info>((Action<ItemComponent.Info>) (info => this.colorInfo[_idx][2].value = info.us));
    }

    private void OnValueChangeVS(int _idx, float _value)
    {
      if (this.isUpdateInfo)
        return;
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
        ociItem.SetPatternVS(_idx, _value);
      this.colorInfo[_idx][3].value = _value;
    }

    private void OnEndEditVS(int _idx, string _text)
    {
      float num = Mathf.Clamp(Utility.StringToFloat(_text), 0.01f, 20f);
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
        ociItem.SetPatternVS(_idx, num);
      this.colorInfo[_idx][3].value = num;
    }

    private void OnClickVSDef(int _idx)
    {
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
      {
        OCIItem v = ociItem;
        v.itemComponent[_idx].SafeProc<ItemComponent.Info>((Action<ItemComponent.Info>) (info => v.SetPatternVS(_idx, info.vs)));
      }
      this.m_OCIItem.itemComponent[_idx].SafeProc<ItemComponent.Info>((Action<ItemComponent.Info>) (info => this.colorInfo[_idx][3].value = info.vs));
    }

    private void OnValueChangeRot(int _idx, float _value)
    {
      if (this.isUpdateInfo)
        return;
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
        ociItem.SetPatternRot(_idx, _value);
      this.colorInfo[_idx][4].value = _value;
    }

    private void OnEndEditRot(int _idx, string _text)
    {
      float num = Mathf.Clamp(Utility.StringToFloat(_text), -1f, 1f);
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
        ociItem.SetPatternRot(_idx, num);
      this.colorInfo[_idx][4].value = num;
    }

    private void OnClickPanel()
    {
      if (this.panelList.active)
      {
        this.panelList.active = false;
      }
      else
      {
        this.panelList.Setup(this.m_OCIItem.itemInfo.panel.filePath, new Action<string>(this.SetMainTex));
        this.isColorFunc = true;
      }
    }

    private void SetMainTex(string _file)
    {
      this.SetPanelTexName(_file);
      this.m_OCIItem.SetMainTex(_file);
    }

    private void SetPanelTexName(string _str)
    {
      this.panelInfo.textTex = !_str.IsNullOrEmpty() ? Path.GetFileNameWithoutExtension(_str) : "なし";
    }

    private void OnClickColorPanel()
    {
      if (Singleton<Studio.Studio>.Instance.colorPalette.Check("画像板"))
      {
        this.isColorFunc = false;
        Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
      }
      else
      {
        Singleton<Studio.Studio>.Instance.colorPalette.Setup("画像板", this.m_OCIItem.itemInfo.colors[0].mainColor, (Action<Color>) (_c =>
        {
          Color _color;
          ((Color) ref _color).\u002Ector((float) _c.r, (float) _c.g, (float) _c.b, 1f);
          this.m_OCIItem.SetColor(_color, 0);
          this.panelInfo.color = _color;
        }), false);
        this.isColorFunc = true;
      }
    }

    private void OnToggleColor(bool _flag)
    {
      if (this.m_OCIItem == null)
        return;
      this.m_OCIItem.SetPatternClamp(0, !_flag);
    }

    private void OnValueChangeUT(float _value)
    {
      if (this.isUpdateInfo)
        return;
      this.m_OCIItem.SetPatternUT(0, _value);
      this.panelInfo[0].value = _value;
    }

    private void OnEndEditUT(string _text)
    {
      float num = Mathf.Clamp(Utility.StringToFloat(_text), -1f, 1f);
      this.m_OCIItem.SetPatternUT(0, num);
      this.panelInfo[0].value = num;
    }

    private void OnClickUTDef()
    {
      this.panelInfo[0].value = 0.0f;
    }

    private void OnValueChangeVT(float _value)
    {
      if (this.isUpdateInfo)
        return;
      this.m_OCIItem.SetPatternVT(0, _value);
      this.panelInfo[1].value = _value;
    }

    private void OnEndEditVT(string _text)
    {
      float num = Mathf.Clamp(Utility.StringToFloat(_text), -1f, 1f);
      this.m_OCIItem.SetPatternVT(0, num);
      this.panelInfo[1].value = num;
    }

    private void OnClickVTDef()
    {
      this.panelInfo[1].value = 0.0f;
    }

    private void OnValueChangeUS(float _value)
    {
      if (this.isUpdateInfo)
        return;
      this.m_OCIItem.SetPatternUS(0, _value);
      this.panelInfo[2].value = _value;
    }

    private void OnEndEditUS(string _text)
    {
      float num = Mathf.Clamp(Utility.StringToFloat(_text), 0.01f, 20f);
      this.m_OCIItem.SetPatternUS(0, num);
      this.panelInfo[2].value = num;
    }

    private void OnClickUSDef()
    {
      this.panelInfo[2].value = 1f;
    }

    private void OnValueChangeVS(float _value)
    {
      if (this.isUpdateInfo)
        return;
      this.m_OCIItem.SetPatternVS(0, _value);
      this.panelInfo[3].value = _value;
    }

    private void OnEndEditVS(string _text)
    {
      float num = Mathf.Clamp(Utility.StringToFloat(_text), 0.01f, 20f);
      this.m_OCIItem.SetPatternVS(0, num);
      this.panelInfo[3].value = num;
    }

    private void OnClickVSDef()
    {
      this.panelInfo[3].value = 1f;
    }

    private void OnValueChangeRot(float _value)
    {
      if (this.isUpdateInfo)
        return;
      this.m_OCIItem.SetPatternRot(0, _value);
      this.panelInfo[4].value = _value;
    }

    private void OnEndEditRot(string _text)
    {
      float num = Mathf.Clamp(Utility.StringToFloat(_text), -1f, 1f);
      this.m_OCIItem.SetPatternRot(0, num);
      this.panelInfo[4].value = num;
    }

    private void OnValueChangeAlpha(float _value)
    {
      if (this.isUpdateInfo)
        return;
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
        ociItem.SetAlpha(_value);
      this.inputAlpha.value = _value;
    }

    private void OnEndEditAlpha(string _text)
    {
      float num = Mathf.Clamp(Utility.StringToFloat(_text), 0.0f, 1f);
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
        ociItem.SetAlpha(num);
      this.inputAlpha.value = num;
    }

    private void OnClickEmissionColor()
    {
      if (Singleton<Studio.Studio>.Instance.colorPalette.Check("発光色"))
      {
        this.isColorFunc = false;
        Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
      }
      else
      {
        IEnumerable<OCIItem> array = ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem));
        Singleton<Studio.Studio>.Instance.colorPalette.Setup("発光色", this.m_OCIItem.itemInfo.EmissionColor, (Action<Color>) (_c =>
        {
          foreach (OCIItem ociItem in array)
          {
            ociItem.itemInfo.EmissionColor = _c;
            ociItem.SetEmissionColor(ociItem.itemInfo.emissionColor);
          }
          this.emissionInfo.color.color = _c;
        }), false);
        this.isColorFunc = true;
      }
    }

    private void OnClickEmissionColorDef()
    {
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
        ociItem.SetEmissionColor(ociItem.itemComponent.DefEmissionColor);
      this.emissionInfo.color.color = this.m_OCIItem.itemComponent.DefEmissionColor;
      this.isColorFunc = false;
      Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
    }

    private void OnValueChangeEmissionPower(float _value)
    {
      if (this.isUpdateInfo)
        return;
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
        ociItem.SetEmissionPower(_value);
      this.emissionInfo.input.value = _value;
    }

    private void OnEndEditEmissionPower(string _text)
    {
      float num = Mathf.Clamp(Utility.StringToFloat(_text), 0.0f, 1f);
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
        ociItem.SetEmissionPower(num);
      this.emissionInfo.input.value = num;
    }

    private void OnClickEmissionPowerDef()
    {
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
        ociItem.SetEmissionPower(ociItem.itemComponent.defEmissionStrength);
      this.emissionInfo.input.value = this.m_OCIItem.itemComponent.defEmissionStrength;
    }

    private void OnValueChangeLightCancel(float _value)
    {
      if (this.isUpdateInfo)
        return;
      this.m_OCIItem.SetLightCancel(_value);
      this.inputLightCancel.value = _value;
    }

    private void OnEndEditLightCancel(string _text)
    {
      float num = Mathf.Clamp(Utility.StringToFloat(_text), 0.0f, 1f);
      this.m_OCIItem.SetLightCancel(num);
      this.inputLightCancel.value = num;
    }

    private void OnClickLightCancelDef()
    {
      this.m_OCIItem.SetLightCancel(this.m_OCIItem.itemComponent.defLightCancel);
      this.inputLightCancel.value = this.m_OCIItem.itemComponent.defLightCancel;
    }

    private void OnValueChangedFK(bool _value)
    {
      if (this.isUpdateInfo)
        return;
      this.m_OCIItem.ActiveFK(_value);
      ((Selectable) this.kinematicInfo.toggleDynamicBone).set_interactable(this.m_OCIItem.isDynamicBone);
    }

    private void OnValueChangedDynamicBone(bool _value)
    {
      if (this.isUpdateInfo)
        return;
      this.m_OCIItem.ActiveDynamicBone(_value);
    }

    private void OnValueChangedOption(bool _value)
    {
      if (this.isUpdateInfo)
        return;
      foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
        ociItem.SetOptionVisible(_value);
    }

    private void OnClickPattern(int _idx)
    {
      if ((double) this.cgPattern.get_alpha() != 0.0)
      {
        this.cgPattern.Enable(false, false);
      }
      else
      {
        Singleton<Studio.Studio>.Instance.patternSelectListCtrl.onChangeItemFunc = (PatternSelectListCtrl.OnChangeItemFunc) (_index =>
        {
          string str1 = string.Empty;
          foreach (OCIItem ociItem in ((IEnumerable<ObjectCtrlInfo>) Studio.Studio.GetSelectObjectCtrl()).Where<ObjectCtrlInfo>((Func<ObjectCtrlInfo, bool>) (v => v.kind == 1)).Select<ObjectCtrlInfo, OCIItem>((Func<ObjectCtrlInfo, OCIItem>) (v => v as OCIItem)))
          {
            string str2 = ociItem.SetPatternTex(_idx, _index);
            if (str1.IsNullOrEmpty())
              str1 = str2;
          }
          this.colorInfo[_idx].textPattern = Path.GetFileNameWithoutExtension(str1);
          this.cgPattern.Enable(false, false);
        });
        this.cgPattern.Enable(true, false);
      }
    }

    private void OnValueChangedAnimePattern(int _value)
    {
      if (this.isUpdateInfo)
        return;
      this.m_OCIItem.SetAnimePattern(_value);
    }

    private string ConvertString(float _t)
    {
      return ((int) Mathf.Lerp(0.0f, 100f, _t)).ToString();
    }

    private void Awake()
    {
      this.panelList.Init();
      for (int index = 0; index < 3; ++index)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        MPItemCtrl.\u003CAwake\u003Ec__AnonStorey12 awakeCAnonStorey12 = new MPItemCtrl.\u003CAwake\u003Ec__AnonStorey12();
        // ISSUE: reference to a compiler-generated field
        awakeCAnonStorey12.\u0024this = this;
        // ISSUE: reference to a compiler-generated field
        awakeCAnonStorey12.no = index;
        // ISSUE: reference to a compiler-generated method
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.colorInfo[index]._colorMain.buttonColor), (Action<M0>) new Action<Unit>(awakeCAnonStorey12.\u003C\u003Em__0));
        // ISSUE: reference to a compiler-generated method
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.colorInfo[index]._colorMain.buttonColorDefault), (Action<M0>) new Action<Unit>(awakeCAnonStorey12.\u003C\u003Em__1));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.colorInfo[index].inputMetallic.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) awakeCAnonStorey12, __methodptr(\u003C\u003Em__2)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.colorInfo[index].inputMetallic.input.get_onEndEdit()).AddListener(new UnityAction<string>((object) awakeCAnonStorey12, __methodptr(\u003C\u003Em__3)));
        // ISSUE: reference to a compiler-generated method
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.colorInfo[index].inputMetallic.buttonDefault), (Action<M0>) new Action<Unit>(awakeCAnonStorey12.\u003C\u003Em__4));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.colorInfo[index].inputGlossiness.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) awakeCAnonStorey12, __methodptr(\u003C\u003Em__5)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.colorInfo[index].inputGlossiness.input.get_onEndEdit()).AddListener(new UnityAction<string>((object) awakeCAnonStorey12, __methodptr(\u003C\u003Em__6)));
        // ISSUE: reference to a compiler-generated method
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.colorInfo[index].inputGlossiness.buttonDefault), (Action<M0>) new Action<Unit>(awakeCAnonStorey12.\u003C\u003Em__7));
        // ISSUE: reference to a compiler-generated method
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.colorInfo[index]._buttonPattern), (Action<M0>) new Action<Unit>(awakeCAnonStorey12.\u003C\u003Em__8));
        // ISSUE: reference to a compiler-generated method
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.colorInfo[index]._colorPattern.buttonColor), (Action<M0>) new Action<Unit>(awakeCAnonStorey12.\u003C\u003Em__9));
        // ISSUE: reference to a compiler-generated method
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.colorInfo[index]._colorPattern.buttonColorDefault), (Action<M0>) new Action<Unit>(awakeCAnonStorey12.\u003C\u003Em__A));
        // ISSUE: reference to a compiler-generated method
        ObservableExtensions.Subscribe<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.colorInfo[index]._toggleClamp), (Action<M0>) new Action<bool>(awakeCAnonStorey12.\u003C\u003Em__B));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.colorInfo[index][0].slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) awakeCAnonStorey12, __methodptr(\u003C\u003Em__C)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.colorInfo[index][0].input.get_onEndEdit()).AddListener(new UnityAction<string>((object) awakeCAnonStorey12, __methodptr(\u003C\u003Em__D)));
        // ISSUE: reference to a compiler-generated method
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.colorInfo[index][0].buttonDefault), (Action<M0>) new Action<Unit>(awakeCAnonStorey12.\u003C\u003Em__E));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.colorInfo[index][1].slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) awakeCAnonStorey12, __methodptr(\u003C\u003Em__F)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.colorInfo[index][1].input.get_onEndEdit()).AddListener(new UnityAction<string>((object) awakeCAnonStorey12, __methodptr(\u003C\u003Em__10)));
        // ISSUE: reference to a compiler-generated method
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.colorInfo[index][1].buttonDefault), (Action<M0>) new Action<Unit>(awakeCAnonStorey12.\u003C\u003Em__11));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.colorInfo[index][2].slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) awakeCAnonStorey12, __methodptr(\u003C\u003Em__12)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.colorInfo[index][2].input.get_onEndEdit()).AddListener(new UnityAction<string>((object) awakeCAnonStorey12, __methodptr(\u003C\u003Em__13)));
        // ISSUE: reference to a compiler-generated method
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.colorInfo[index][2].buttonDefault), (Action<M0>) new Action<Unit>(awakeCAnonStorey12.\u003C\u003Em__14));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.colorInfo[index][3].slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) awakeCAnonStorey12, __methodptr(\u003C\u003Em__15)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.colorInfo[index][3].input.get_onEndEdit()).AddListener(new UnityAction<string>((object) awakeCAnonStorey12, __methodptr(\u003C\u003Em__16)));
        // ISSUE: reference to a compiler-generated method
        ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.colorInfo[index][3].buttonDefault), (Action<M0>) new Action<Unit>(awakeCAnonStorey12.\u003C\u003Em__17));
        // ISSUE: method pointer
        ((UnityEvent<float>) this.colorInfo[index][4].slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) awakeCAnonStorey12, __methodptr(\u003C\u003Em__18)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.colorInfo[index][4].input.get_onEndEdit()).AddListener(new UnityAction<string>((object) awakeCAnonStorey12, __methodptr(\u003C\u003Em__19)));
      }
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.colorInfo[3]._colorMain.buttonColor), (Action<M0>) (_ => this.OnClickColor4()));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.colorInfo[3]._colorMain.buttonColorDefault), (Action<M0>) (_ => this.OnClickColor4Def()));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.panelInfo._buttonTex), (Action<M0>) (_ => this.OnClickPanel()));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.panelInfo._color.buttonColor), (Action<M0>) (_ => this.OnClickColorPanel()));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.panelInfo._toggleClamp), (Action<M0>) (f => this.OnToggleColor(f)));
      // ISSUE: method pointer
      ((UnityEvent<float>) this.panelInfo[0].slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(\u003CAwake\u003Em__4C)));
      // ISSUE: method pointer
      ((UnityEvent<string>) this.panelInfo[0].input.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(\u003CAwake\u003Em__4D)));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.panelInfo[0].buttonDefault), (Action<M0>) (_ => this.OnClickUTDef()));
      // ISSUE: method pointer
      ((UnityEvent<float>) this.panelInfo[1].slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(\u003CAwake\u003Em__4F)));
      // ISSUE: method pointer
      ((UnityEvent<string>) this.panelInfo[1].input.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(\u003CAwake\u003Em__50)));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.panelInfo[1].buttonDefault), (Action<M0>) (_ => this.OnClickVTDef()));
      // ISSUE: method pointer
      ((UnityEvent<float>) this.panelInfo[2].slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(\u003CAwake\u003Em__52)));
      // ISSUE: method pointer
      ((UnityEvent<string>) this.panelInfo[2].input.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(\u003CAwake\u003Em__53)));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.panelInfo[2].buttonDefault), (Action<M0>) (_ => this.OnClickUSDef()));
      // ISSUE: method pointer
      ((UnityEvent<float>) this.panelInfo[3].slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(\u003CAwake\u003Em__55)));
      // ISSUE: method pointer
      ((UnityEvent<string>) this.panelInfo[3].input.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(\u003CAwake\u003Em__56)));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.panelInfo[3].buttonDefault), (Action<M0>) (_ => this.OnClickVSDef()));
      // ISSUE: method pointer
      ((UnityEvent<float>) this.panelInfo[4].slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(\u003CAwake\u003Em__58)));
      // ISSUE: method pointer
      ((UnityEvent<string>) this.panelInfo[4].input.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(\u003CAwake\u003Em__59)));
      // ISSUE: method pointer
      ((UnityEvent<float>) this.inputAlpha.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(\u003CAwake\u003Em__5A)));
      // ISSUE: method pointer
      ((UnityEvent<string>) this.inputAlpha.input.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(\u003CAwake\u003Em__5B)));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.emissionInfo.color.buttonColor), (Action<M0>) (_ => this.OnClickEmissionColor()));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.emissionInfo.color.buttonColorDefault), (Action<M0>) (_ => this.OnClickEmissionColorDef()));
      // ISSUE: method pointer
      ((UnityEvent<float>) this.emissionInfo.input.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(\u003CAwake\u003Em__5E)));
      // ISSUE: method pointer
      ((UnityEvent<string>) this.emissionInfo.input.input.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(\u003CAwake\u003Em__5F)));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.emissionInfo.input.buttonDefault), (Action<M0>) (_ => this.OnClickEmissionPowerDef()));
      // ISSUE: method pointer
      ((UnityEvent<float>) this.inputLightCancel.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(\u003CAwake\u003Em__61)));
      // ISSUE: method pointer
      ((UnityEvent<string>) this.inputLightCancel.input.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(\u003CAwake\u003Em__62)));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.inputLightCancel.buttonDefault), (Action<M0>) (_ => this.OnClickLightCancelDef()));
      // ISSUE: method pointer
      ((UnityEvent<bool>) this.kinematicInfo.toggleFK.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangedFK)));
      // ISSUE: method pointer
      ((UnityEvent<bool>) this.kinematicInfo.toggleDynamicBone.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangedDynamicBone)));
      // ISSUE: method pointer
      ((UnityEvent<bool>) this.optionInfo.toggleAll.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(OnValueChangedOption)));
      // ISSUE: method pointer
      ((UnityEvent<int>) this.animeInfo.dropdownAnime.get_onValueChanged()).AddListener(new UnityAction<int>((object) this, __methodptr(OnValueChangedAnimePattern)));
      this.isUpdateInfo = false;
      this.m_Active = false;
      ((Component) this).get_gameObject().SetActive(false);
    }

    [Serializable]
    private class ColorCombination
    {
      public GameObject objRoot;
      public Image imageColor;
      public Button buttonColor;
      public Button buttonColorDefault;

      public bool interactable
      {
        set
        {
          ((Selectable) this.buttonColor).set_interactable(value);
          if (!Object.op_Implicit((Object) this.buttonColorDefault))
            return;
          ((Selectable) this.buttonColorDefault).set_interactable(value);
        }
      }

      public Color color
      {
        get
        {
          return ((Graphic) this.imageColor).get_color();
        }
        set
        {
          ((Graphic) this.imageColor).set_color(value);
        }
      }

      public bool active
      {
        set
        {
          this.objRoot.SetActiveIfDifferent(value);
        }
      }
    }

    [Serializable]
    private class InputCombination
    {
      public GameObject objRoot;
      public TMP_InputField input;
      public Slider slider;
      public Button buttonDefault;

      public bool interactable
      {
        set
        {
          ((Selectable) this.input).set_interactable(value);
          ((Selectable) this.slider).set_interactable(value);
          if (!Object.op_Implicit((Object) this.buttonDefault))
            return;
          ((Selectable) this.buttonDefault).set_interactable(value);
        }
      }

      public string text
      {
        get
        {
          return this.input.get_text();
        }
        set
        {
          this.input.set_text(value);
          this.slider.set_value(Utility.StringToFloat(value));
        }
      }

      public float value
      {
        get
        {
          return this.slider.get_value();
        }
        set
        {
          this.slider.set_value(value);
          this.input.set_text(value.ToString("0.0"));
        }
      }

      public bool active
      {
        set
        {
          this.objRoot.SetActiveIfDifferent(value);
        }
      }
    }

    [Serializable]
    private class ColorInfo
    {
      public MPItemCtrl.ColorCombination _colorMain = new MPItemCtrl.ColorCombination();
      public MPItemCtrl.InputCombination inputMetallic = new MPItemCtrl.InputCombination();
      public MPItemCtrl.InputCombination inputGlossiness = new MPItemCtrl.InputCombination();
      public MPItemCtrl.ColorCombination _colorPattern = new MPItemCtrl.ColorCombination();
      public GameObject objRoot;
      [Header("メタリック関係")]
      public GameObject objMetallic;
      [Header("柄関係")]
      public GameObject objPattern;
      public Button _buttonPattern;
      public TextMeshProUGUI _textPattern;
      public Toggle _toggleClamp;
      public MPItemCtrl.InputCombination[] _input;

      public Color colorMain
      {
        set
        {
          ((Graphic) this._colorMain.imageColor).set_color(value);
        }
      }

      public string textPattern
      {
        set
        {
          ((TMP_Text) this._textPattern).set_text(value);
        }
      }

      public Color colorPattern
      {
        set
        {
          ((Graphic) this._colorPattern.imageColor).set_color(value);
        }
      }

      public bool isOn
      {
        set
        {
          this._toggleClamp.set_isOn(value);
        }
      }

      public MPItemCtrl.InputCombination this[int _idx]
      {
        get
        {
          return this._input.SafeGet<MPItemCtrl.InputCombination>(_idx);
        }
      }

      public bool enable
      {
        get
        {
          return this.objRoot.get_activeSelf();
        }
        set
        {
          if (this.objRoot.get_activeSelf() == value)
            return;
          this.objRoot.SetActive(value);
        }
      }

      public bool EnableMetallic
      {
        set
        {
          this.objMetallic.SetActiveIfDifferent(value);
        }
      }

      public bool EnablePattern
      {
        set
        {
          this.objPattern.SetActiveIfDifferent(value);
        }
      }
    }

    [Serializable]
    private class ColorInputCombination
    {
      public MPItemCtrl.ColorCombination color = new MPItemCtrl.ColorCombination();
      public MPItemCtrl.InputCombination input = new MPItemCtrl.InputCombination();
      public GameObject objRoot;

      public bool active
      {
        set
        {
          this.objRoot.SetActiveIfDifferent(value);
        }
      }
    }

    [Serializable]
    private class EmissionInfo : MPItemCtrl.ColorInputCombination
    {
    }

    [Serializable]
    private class LineInfo : MPItemCtrl.ColorInputCombination
    {
    }

    [Serializable]
    private class KinematicInfo
    {
      public GameObject objRoot;
      public Toggle toggleFK;
      public Toggle toggleDynamicBone;

      public bool Active
      {
        get
        {
          return this.objRoot.get_activeSelf();
        }
        set
        {
          this.objRoot.SetActiveIfDifferent(value);
        }
      }
    }

    [Serializable]
    private class PanelInfo
    {
      public GameObject objRoot;
      public Button _buttonTex;
      public TextMeshProUGUI _textTex;
      public MPItemCtrl.ColorCombination _color;
      public Toggle _toggleClamp;
      public MPItemCtrl.InputCombination[] _input;

      public string textTex
      {
        set
        {
          ((TMP_Text) this._textTex).set_text(value);
        }
      }

      public Color color
      {
        set
        {
          ((Graphic) this._color.imageColor).set_color(value);
        }
      }

      public bool isOn
      {
        set
        {
          this._toggleClamp.set_isOn(value);
        }
      }

      public MPItemCtrl.InputCombination this[int _idx]
      {
        get
        {
          return this._input.SafeGet<MPItemCtrl.InputCombination>(_idx);
        }
      }

      public bool enable
      {
        get
        {
          return this.objRoot.get_activeSelf();
        }
        set
        {
          this.objRoot.SetActiveIfDifferent(value);
        }
      }
    }

    [Serializable]
    private class PanelList
    {
      private List<string> listPath = new List<string>();
      private Dictionary<int, StudioNode> dicNode = new Dictionary<int, StudioNode>();
      private int select = -1;
      [SerializeField]
      private GameObject objRoot;
      [SerializeField]
      private GameObject objectNode;
      [SerializeField]
      private Transform transformRoot;
      public Action<string> actUpdatePath;

      public bool active
      {
        get
        {
          return this.objRoot.get_activeSelf();
        }
        set
        {
          this.objRoot.SetActiveIfDifferent(value);
        }
      }

      public void Init()
      {
        for (int index = 0; index < this.transformRoot.get_childCount(); ++index)
          Object.Destroy((Object) ((Component) this.transformRoot.GetChild(index)).get_gameObject());
        this.transformRoot.DetachChildren();
        string[] files = Directory.GetFiles(UserData.Create(BackgroundList.dirName), "*.png");
        // ISSUE: reference to a compiler-generated field
        if (MPItemCtrl.PanelList.\u003C\u003Ef__mg\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          MPItemCtrl.PanelList.\u003C\u003Ef__mg\u0024cache0 = new Func<string, string>(Path.GetFileName);
        }
        // ISSUE: reference to a compiler-generated field
        Func<string, string> fMgCache0 = MPItemCtrl.PanelList.\u003C\u003Ef__mg\u0024cache0;
        this.listPath = ((IEnumerable<string>) files).Select<string, string>(fMgCache0).ToList<string>();
        this.CreateNode(-1, "なし");
        int count = this.listPath.Count;
        for (int _idx = 0; _idx < count; ++_idx)
          this.CreateNode(_idx, Path.GetFileNameWithoutExtension(this.listPath[_idx]));
      }

      public void Setup(string _file, Action<string> _actUpdate)
      {
        this.SetSelect(this.select, false);
        this.select = this.listPath.FindIndex((Predicate<string>) (s => s == _file));
        this.SetSelect(this.select, true);
        this.actUpdatePath = _actUpdate;
        this.active = true;
      }

      private void OnClickSelect(int _idx)
      {
        this.SetSelect(this.select, false);
        this.select = _idx;
        this.SetSelect(this.select, true);
        if (this.actUpdatePath != null)
          this.actUpdatePath(this.select == -1 ? string.Empty : this.listPath[_idx]);
        this.active = false;
      }

      private void SetSelect(int _idx, bool _flag)
      {
        StudioNode studioNode = (StudioNode) null;
        if (!this.dicNode.TryGetValue(_idx, out studioNode))
          return;
        studioNode.select = _flag;
      }

      private void CreateNode(int _idx, string _text)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        MPItemCtrl.PanelList.\u003CCreateNode\u003Ec__AnonStorey1 nodeCAnonStorey1 = new MPItemCtrl.PanelList.\u003CCreateNode\u003Ec__AnonStorey1();
        // ISSUE: reference to a compiler-generated field
        nodeCAnonStorey1._idx = _idx;
        // ISSUE: reference to a compiler-generated field
        nodeCAnonStorey1.\u0024this = this;
        GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.objectNode);
        gameObject.get_transform().SetParent(this.transformRoot, false);
        StudioNode component = (StudioNode) gameObject.GetComponent<StudioNode>();
        component.active = true;
        // ISSUE: method pointer
        component.addOnClick = new UnityAction((object) nodeCAnonStorey1, __methodptr(\u003C\u003Em__0));
        component.text = _text;
        // ISSUE: reference to a compiler-generated field
        this.dicNode.Add(nodeCAnonStorey1._idx, component);
      }
    }

    [Serializable]
    private class OptionInfo
    {
      public GameObject objRoot;
      public Toggle toggleAll;

      public bool Active
      {
        get
        {
          return this.objRoot.get_activeSelf();
        }
        set
        {
          this.objRoot.SetActiveIfDifferent(value);
        }
      }
    }

    [Serializable]
    private class AnimeInfo
    {
      public GameObject objRoot;
      public TMP_Dropdown dropdownAnime;

      public bool Active
      {
        get
        {
          return this.objRoot.get_activeSelf();
        }
        set
        {
          this.objRoot.SetActiveIfDifferent(value);
        }
      }

      public void InitDropdown(OCIItem _ocii)
      {
        this.dropdownAnime.ClearOptions();
        if (_ocii == null || Object.op_Equality((Object) _ocii.itemComponent, (Object) null))
          return;
        ItemComponent.AnimeInfo[] animeInfos = _ocii.itemComponent.animeInfos;
        this.dropdownAnime.set_options(!((IList<ItemComponent.AnimeInfo>) animeInfos).IsNullOrEmpty<ItemComponent.AnimeInfo>() ? ((IEnumerable<ItemComponent.AnimeInfo>) animeInfos).Select<ItemComponent.AnimeInfo, TMP_Dropdown.OptionData>((Func<ItemComponent.AnimeInfo, TMP_Dropdown.OptionData>) (c => new TMP_Dropdown.OptionData(c.name))).ToList<TMP_Dropdown.OptionData>() : new List<TMP_Dropdown.OptionData>());
        this.dropdownAnime.set_value(_ocii.itemInfo.animePattern);
      }
    }
  }
}
