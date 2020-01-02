// Decompiled with JetBrains decompiler
// Type: Housing.SettingUICtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Housing
{
  [Serializable]
  public class SettingUICtrl : UIDerived
  {
    [SerializeField]
    [Header("アイテム設定")]
    private SettingUICtrl.ItemUI itemUI = new SettingUICtrl.ItemUI();
    [SerializeField]
    [Header("フォルダー設定")]
    private SettingUICtrl.FolderUI folderUI = new SettingUICtrl.FolderUI();
    [SerializeField]
    private CanvasGroup cgRoot;
    [SerializeField]
    private Text textTitle;
    private ObjectCtrl objectCtrl;

    private bool Visible
    {
      get
      {
        return (double) this.cgRoot.get_alpha() != 0.0;
      }
      set
      {
        this.cgRoot.Enable(value, false);
      }
    }

    private OCItem OCItem
    {
      get
      {
        return this.objectCtrl as OCItem;
      }
    }

    private OIItem OIItem
    {
      get
      {
        return this.OCItem?.OIItem;
      }
    }

    private OCFolder OCFolder
    {
      get
      {
        return this.objectCtrl as OCFolder;
      }
    }

    private OIFolder OIFolder
    {
      get
      {
        return this.OCFolder?.OIFolder;
      }
    }

    public override void Init(UICtrl _uiCtrl, bool _tutorial)
    {
      base.Init(_uiCtrl, _tutorial);
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.itemUI.color1.buttonColor), (Action<M0>) (_ =>
      {
        if (this.itemUI.colorPanel.isOpen && this.itemUI.color1.IsOpen)
        {
          this.itemUI.Close(true);
        }
        else
        {
          this.itemUI.SetOpen(0);
          this.itemUI.colorPanel.Setup(this.OIItem.Color1, (Action<Color>) (_c =>
          {
            this.itemUI.color1.Color = _c;
            this.OCItem.Color1 = _c;
          }), false);
        }
      }));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.itemUI.color2.buttonColor), (Action<M0>) (_ =>
      {
        if (this.itemUI.colorPanel.isOpen && this.itemUI.color2.IsOpen)
        {
          this.itemUI.Close(true);
        }
        else
        {
          this.itemUI.SetOpen(1);
          this.itemUI.colorPanel.Setup(this.OIItem.Color2, (Action<Color>) (_c =>
          {
            this.itemUI.color2.Color = _c;
            this.OCItem.Color2 = _c;
          }), false);
        }
      }));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.itemUI.color3.buttonColor), (Action<M0>) (_ =>
      {
        if (this.itemUI.colorPanel.isOpen && this.itemUI.color3.IsOpen)
        {
          this.itemUI.Close(true);
        }
        else
        {
          this.itemUI.SetOpen(2);
          this.itemUI.colorPanel.Setup(this.OIItem.Color3, (Action<Color>) (_c =>
          {
            this.itemUI.color3.Color = _c;
            this.OCItem.Color3 = _c;
          }), false);
        }
      }));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.itemUI.emissionColor.buttonColor), (Action<M0>) (_ =>
      {
        if (this.itemUI.colorPanel.isOpen && this.itemUI.emissionColor.IsOpen)
        {
          this.itemUI.Close(true);
        }
        else
        {
          float exposure = 0.0f;
          Color32 baseLinearColor;
          SettingUICtrl.DecomposeHdrColor(this.OIItem.EmissionColor, out baseLinearColor, out exposure);
          baseLinearColor.a = (__Null) (int) byte.MaxValue;
          this.itemUI.SetOpen(3);
          this.itemUI.colorPanel.Setup(Color32.op_Implicit(baseLinearColor), (Action<Color>) (_c =>
          {
            _c = Color.op_Multiply(_c, Mathf.Pow(2f, exposure));
            this.itemUI.emissionColor.Color = _c;
            this.OCItem.EmissionColor = _c;
          }), false);
        }
      }));
      ObservableExtensions.Subscribe<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.itemUI.option.toggle), (Action<M0>) (_b =>
      {
        if (this.OCItem == null)
          return;
        this.OCItem.VisibleOption = _b;
      }));
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) UnityUIComponentExtensions.OnClickAsObservable(this.itemUI.init.buttonInit), (Action<M0>) (_ =>
      {
        if (this.OCItem == null)
          return;
        this.OCItem.ResetColor();
        this.itemUI.Close(true);
        this.itemUI.color1.Color = this.OCItem.Color1;
        this.itemUI.color2.Color = this.OCItem.Color2;
        this.itemUI.color3.Color = this.OCItem.Color3;
        this.itemUI.emissionColor.Color = this.OCItem.EmissionColor;
      }));
      this.itemUI.colorPanel.onClose += (Action) (() => this.itemUI.Close(false));
      ObservableExtensions.Subscribe<string>((IObservable<M0>) UnityUIComponentExtensions.OnEndEditAsObservable(this.folderUI.inputName), (Action<M0>) (_s =>
      {
        this.OIFolder.Name = _s;
        this.UICtrl.ListUICtrl.RefreshList();
      }));
      Singleton<Selection>.Instance.onSelectFunc += new Action<ObjectCtrl[]>(this.OnSelect);
    }

    public override void UpdateUI()
    {
    }

    private void OnSelect(ObjectCtrl[] _objectCtrls)
    {
      if (((IList<ObjectCtrl>) _objectCtrls).IsNullOrEmpty<ObjectCtrl>())
      {
        this.objectCtrl = (ObjectCtrl) null;
        this.Visible = false;
      }
      else
      {
        this.objectCtrl = _objectCtrls.SafeGet<ObjectCtrl>(0);
        switch (this.objectCtrl.Kind)
        {
          case 0:
            this.itemUI.SetActive(this.OCItem.IsColor1, this.OCItem.IsColor2, this.OCItem.IsColor3, this.OCItem.IsEmissionColor, this.OCItem.IsOption, this.OCItem.IsColor);
            this.folderUI.Active = false;
            this.textTitle.set_text("アイテム設定");
            this.itemUI.color1.Color = this.OIItem.Color1;
            this.itemUI.color2.Color = this.OIItem.Color2;
            this.itemUI.color3.Color = this.OIItem.Color3;
            this.itemUI.emissionColor.Color = this.OIItem.EmissionColor;
            this.itemUI.option.IsOn = this.OIItem.VisibleOption;
            this.Visible = this.itemUI.Active;
            break;
          case 1:
            this.Visible = true;
            this.itemUI.Active = false;
            this.folderUI.Active = true;
            this.textTitle.set_text("フォルダー設定");
            this.folderUI.Name = this.OIFolder.Name;
            break;
        }
      }
    }

    internal static void DecomposeHdrColor(
      Color linearColorHdr,
      out Color32 baseLinearColor,
      out float exposure)
    {
      baseLinearColor = Color32.op_Implicit(linearColorHdr);
      float maxColorComponent = ((Color) ref linearColorHdr).get_maxColorComponent();
      byte num1 = 191;
      if ((double) maxColorComponent == 0.0 || (double) maxColorComponent <= 1.0 && (double) maxColorComponent >= 0.00392156885936856)
      {
        exposure = 0.0f;
        baseLinearColor.r = (__Null) (int) (byte) Mathf.RoundToInt((float) (linearColorHdr.r * (double) byte.MaxValue));
        baseLinearColor.g = (__Null) (int) (byte) Mathf.RoundToInt((float) (linearColorHdr.g * (double) byte.MaxValue));
        baseLinearColor.b = (__Null) (int) (byte) Mathf.RoundToInt((float) (linearColorHdr.b * (double) byte.MaxValue));
      }
      else
      {
        float num2 = (float) num1 / maxColorComponent;
        exposure = Mathf.Log((float) byte.MaxValue / num2) / Mathf.Log(2f);
        baseLinearColor.r = (__Null) (int) (byte) Mathf.Min((int) num1, (int) (byte) Mathf.CeilToInt(num2 * (float) linearColorHdr.r));
        baseLinearColor.g = (__Null) (int) (byte) Mathf.Min((int) num1, (int) (byte) Mathf.CeilToInt(num2 * (float) linearColorHdr.g));
        baseLinearColor.b = (__Null) (int) (byte) Mathf.Min((int) num1, (int) (byte) Mathf.CeilToInt(num2 * (float) linearColorHdr.b));
      }
    }

    private class BaseClass
    {
      public GameObject objRoot;

      public virtual bool Active
      {
        set
        {
          this.objRoot.SetActiveIfDifferent(value);
        }
        get
        {
          return this.objRoot.get_activeSelf();
        }
      }
    }

    [Serializable]
    private class ColorUI : SettingUICtrl.BaseClass
    {
      public Button buttonColor;
      protected Image m_image;

      public virtual Color Color
      {
        set
        {
          ((Graphic) (this.m_image ?? (this.m_image = ((Selectable) this.buttonColor).get_image()))).set_color(value);
        }
      }

      public bool IsOpen { get; set; }
    }

    [Serializable]
    private class EmissionColorUI : SettingUICtrl.ColorUI
    {
      public override Color Color
      {
        set
        {
          value.a = (__Null) 1.0;
          ((Graphic) (this.m_image ?? (this.m_image = ((Selectable) this.buttonColor).get_image()))).set_color(value);
        }
      }
    }

    [Serializable]
    private class ToggleUI : SettingUICtrl.BaseClass
    {
      public Toggle toggle;

      public bool IsOn
      {
        set
        {
          this.toggle.set_isOn(value);
        }
      }
    }

    [Serializable]
    private class InitColorUI : SettingUICtrl.BaseClass
    {
      public Button buttonInit;
    }

    [Serializable]
    private class ItemUI : SettingUICtrl.BaseClass
    {
      public SettingUICtrl.ColorUI color1 = new SettingUICtrl.ColorUI();
      public SettingUICtrl.ColorUI color2 = new SettingUICtrl.ColorUI();
      public SettingUICtrl.ColorUI color3 = new SettingUICtrl.ColorUI();
      public SettingUICtrl.EmissionColorUI emissionColor = new SettingUICtrl.EmissionColorUI();
      public SettingUICtrl.ToggleUI option = new SettingUICtrl.ToggleUI();
      public SettingUICtrl.InitColorUI init = new SettingUICtrl.InitColorUI();
      [Header("カラーピッカー")]
      public ColorPanel colorPanel;

      public override bool Active
      {
        set
        {
          base.Active = value;
          this.Close(true);
        }
      }

      public void SetActive(params bool[] _params)
      {
        SettingUICtrl.BaseClass[] array = new SettingUICtrl.BaseClass[6]
        {
          (SettingUICtrl.BaseClass) this.color1,
          (SettingUICtrl.BaseClass) this.color2,
          (SettingUICtrl.BaseClass) this.color3,
          (SettingUICtrl.BaseClass) this.emissionColor,
          (SettingUICtrl.BaseClass) this.option,
          (SettingUICtrl.BaseClass) this.init
        };
        for (int i = 0; i < array.Length; ++i)
        {
          if (!_params.SafeProc<bool>(i, (Action<bool>) (_b => array[i].Active = _b)))
            array[i].Active = false;
        }
        this.Active = ((IEnumerable<bool>) _params).Any<bool>((Func<bool, bool>) (v => v));
      }

      public void SetOpen(int _idx)
      {
        SettingUICtrl.ColorUI[] colorUiArray = new SettingUICtrl.ColorUI[4]
        {
          this.color1,
          this.color2,
          this.color3,
          (SettingUICtrl.ColorUI) this.emissionColor
        };
        for (int index = 0; index < colorUiArray.Length; ++index)
          colorUiArray[index].IsOpen = index == _idx;
      }

      public void Close(bool _panel = true)
      {
        this.color1.IsOpen = false;
        this.color2.IsOpen = false;
        this.color3.IsOpen = false;
        this.emissionColor.IsOpen = false;
        if (!_panel)
          return;
        this.colorPanel.Close();
      }
    }

    [Serializable]
    private class FolderUI : SettingUICtrl.BaseClass
    {
      public InputField inputName;

      public string Name
      {
        set
        {
          this.inputName.set_text(value);
        }
      }
    }
  }
}
