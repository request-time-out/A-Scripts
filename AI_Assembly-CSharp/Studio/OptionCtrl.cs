// Decompiled with JetBrains decompiler
// Type: Studio.OptionCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class OptionCtrl : MonoBehaviour
  {
    [SerializeField]
    private OptionCtrl.InputCombination inputCameraX;
    [SerializeField]
    private OptionCtrl.InputCombination inputCameraY;
    [SerializeField]
    private OptionCtrl.InputCombination inputCameraSpeed;
    [SerializeField]
    private OptionCtrl.InputCombination _inputSize;
    [SerializeField]
    private OptionCtrl.InputCombination inputSpeed;
    [SerializeField]
    private Toggle[] toggleInitialPosition;
    [SerializeField]
    private Toggle[] toggleSelectedState;
    [SerializeField]
    private Toggle[] toggleAutoHide;
    [SerializeField]
    private Toggle[] toggleAutoSelect;
    [SerializeField]
    private Toggle[] toggleSnap;
    [SerializeField]
    private CameraControl cameraControl;
    [SerializeField]
    private Sprite[] spriteActive;
    [SerializeField]
    private OptionCtrl.CharaFKColor charaFKColor;
    [SerializeField]
    private OptionCtrl.ItemFKColor itemFKColor;
    [SerializeField]
    private OptionCtrl.RouteSystem routeSystem;
    [SerializeField]
    private OptionCtrl.Etc etc;

    public OptionCtrl()
    {
      base.\u002Ector();
    }

    public OptionCtrl.InputCombination inputSize
    {
      get
      {
        return this._inputSize;
      }
    }

    public bool IsInit { get; private set; }

    public void UpdateUI()
    {
      this.inputCameraX.value = Studio.Studio.optionSystem.cameraSpeedY;
      this.inputCameraY.value = Studio.Studio.optionSystem.cameraSpeedX;
      this.inputCameraSpeed.value = Studio.Studio.optionSystem.cameraSpeed;
      this._inputSize.value = Studio.Studio.optionSystem.manipulateSize;
      this.inputSpeed.value = Studio.Studio.optionSystem.manipuleteSpeed;
      this.toggleInitialPosition[0].set_isOn(Studio.Studio.optionSystem.initialPosition == 0);
      this.toggleInitialPosition[1].set_isOn(Studio.Studio.optionSystem.initialPosition == 1);
      this.toggleSelectedState[0].set_isOn(Studio.Studio.optionSystem.selectedState == 0);
      this.toggleSelectedState[1].set_isOn(Studio.Studio.optionSystem.selectedState == 1);
      this.toggleAutoHide[0].set_isOn(!Studio.Studio.optionSystem.autoHide);
      this.toggleAutoHide[1].set_isOn(Studio.Studio.optionSystem.autoHide);
      this.toggleAutoSelect[0].set_isOn(Studio.Studio.optionSystem.autoSelect);
      this.toggleAutoSelect[1].set_isOn(!Studio.Studio.optionSystem.autoSelect);
      for (int index = 0; index < this.toggleSnap.Length; ++index)
        this.toggleSnap[index].set_isOn(Studio.Studio.optionSystem.snap == index);
      this.charaFKColor.UpdateInfo();
      this.itemFKColor.UpdateInfo();
      this.routeSystem.UpdateInfo();
      this.etc.UpdateInfo();
    }

    public void UpdateUIManipulateSize()
    {
      this._inputSize.value = Studio.Studio.optionSystem.manipulateSize;
    }

    private void OnValueChangedSelectedState(int _state)
    {
      Studio.Studio.optionSystem.selectedState = _state;
      this.cameraControl.ReflectOption();
    }

    private void OnValueChangedCameraX(float _value)
    {
      Studio.Studio.optionSystem.cameraSpeedY = _value;
      this.inputCameraX.value = _value;
      this.cameraControl.ReflectOption();
    }

    private void OnEndEditCameraX(string _text)
    {
      float num = Mathf.Clamp(Utility.StringToFloat(_text), this.inputCameraX.min, this.inputCameraX.max);
      Studio.Studio.optionSystem.cameraSpeedY = num;
      this.inputCameraX.value = num;
      this.cameraControl.ReflectOption();
    }

    private void OnValueChangedCameraY(float _value)
    {
      Studio.Studio.optionSystem.cameraSpeedX = _value;
      this.inputCameraY.value = _value;
      this.cameraControl.ReflectOption();
    }

    private void OnEndEditCameraY(string _text)
    {
      float num = Mathf.Clamp(Utility.StringToFloat(_text), this.inputCameraY.min, this.inputCameraY.max);
      Studio.Studio.optionSystem.cameraSpeedX = num;
      this.inputCameraY.value = num;
      this.cameraControl.ReflectOption();
    }

    private void OnValueChangedCameraSpeed(float _value)
    {
      Studio.Studio.optionSystem.cameraSpeed = _value;
      this.inputCameraSpeed.value = _value;
      this.cameraControl.ReflectOption();
    }

    private void OnEndEditCameraSpeed(string _text)
    {
      float num = Mathf.Clamp(Utility.StringToFloat(_text), this.inputCameraSpeed.min, this.inputCameraSpeed.max);
      Studio.Studio.optionSystem.cameraSpeed = num;
      this.inputCameraSpeed.value = num;
      this.cameraControl.ReflectOption();
    }

    private void OnValueChangedSize(float _value)
    {
      Studio.Studio.optionSystem.manipulateSize = _value;
      this._inputSize.value = _value;
      Singleton<GuideObjectManager>.Instance.SetScale();
    }

    private void OnEndEditSize(string _text)
    {
      float num = Mathf.Clamp(Utility.StringToFloat(_text), this._inputSize.min, this._inputSize.max);
      Studio.Studio.optionSystem.manipulateSize = num;
      this._inputSize.value = num;
      Singleton<GuideObjectManager>.Instance.SetScale();
    }

    private void OnValueChangedSpeed(float _value)
    {
      Studio.Studio.optionSystem.manipuleteSpeed = _value;
      this.inputSpeed.value = _value;
    }

    private void OnEndEditSpeed(string _text)
    {
      float num = Mathf.Clamp(Utility.StringToFloat(_text), this.inputSpeed.min, this.inputSpeed.max);
      Studio.Studio.optionSystem.manipuleteSpeed = num;
      this.inputSpeed.value = num;
    }

    public void Init()
    {
      if (this.IsInit)
        return;
      this.UpdateUI();
      // ISSUE: method pointer
      ((UnityEvent<float>) this.inputCameraX.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedCameraX)));
      // ISSUE: method pointer
      ((UnityEvent<string>) this.inputCameraX.input.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditCameraX)));
      // ISSUE: method pointer
      ((UnityEvent<float>) this.inputCameraY.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedCameraY)));
      // ISSUE: method pointer
      ((UnityEvent<string>) this.inputCameraY.input.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditCameraY)));
      // ISSUE: method pointer
      ((UnityEvent<float>) this.inputCameraSpeed.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedCameraSpeed)));
      // ISSUE: method pointer
      ((UnityEvent<string>) this.inputCameraSpeed.input.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditCameraSpeed)));
      // ISSUE: method pointer
      ((UnityEvent<float>) this._inputSize.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedSize)));
      // ISSUE: method pointer
      ((UnityEvent<string>) this._inputSize.input.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditSize)));
      // ISSUE: method pointer
      ((UnityEvent<float>) this.inputSpeed.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedSpeed)));
      // ISSUE: method pointer
      ((UnityEvent<string>) this.inputSpeed.input.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditSpeed)));
      // ISSUE: variable of the null type
      __Null onValueChanged1 = this.toggleInitialPosition[0].onValueChanged;
      // ISSUE: reference to a compiler-generated field
      if (OptionCtrl.\u003C\u003Ef__am\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        OptionCtrl.\u003C\u003Ef__am\u0024cache0 = new UnityAction<bool>((object) null, __methodptr(\u003CInit\u003Em__0));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction<bool> fAmCache0 = OptionCtrl.\u003C\u003Ef__am\u0024cache0;
      ((UnityEvent<bool>) onValueChanged1).AddListener(fAmCache0);
      // ISSUE: variable of the null type
      __Null onValueChanged2 = this.toggleInitialPosition[1].onValueChanged;
      // ISSUE: reference to a compiler-generated field
      if (OptionCtrl.\u003C\u003Ef__am\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        OptionCtrl.\u003C\u003Ef__am\u0024cache1 = new UnityAction<bool>((object) null, __methodptr(\u003CInit\u003Em__1));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction<bool> fAmCache1 = OptionCtrl.\u003C\u003Ef__am\u0024cache1;
      ((UnityEvent<bool>) onValueChanged2).AddListener(fAmCache1);
      // ISSUE: method pointer
      ((UnityEvent<bool>) this.toggleSelectedState[0].onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(\u003CInit\u003Em__2)));
      // ISSUE: method pointer
      ((UnityEvent<bool>) this.toggleSelectedState[1].onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(\u003CInit\u003Em__3)));
      // ISSUE: variable of the null type
      __Null onValueChanged3 = this.toggleAutoHide[0].onValueChanged;
      // ISSUE: reference to a compiler-generated field
      if (OptionCtrl.\u003C\u003Ef__am\u0024cache2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        OptionCtrl.\u003C\u003Ef__am\u0024cache2 = new UnityAction<bool>((object) null, __methodptr(\u003CInit\u003Em__4));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction<bool> fAmCache2 = OptionCtrl.\u003C\u003Ef__am\u0024cache2;
      ((UnityEvent<bool>) onValueChanged3).AddListener(fAmCache2);
      // ISSUE: variable of the null type
      __Null onValueChanged4 = this.toggleAutoHide[1].onValueChanged;
      // ISSUE: reference to a compiler-generated field
      if (OptionCtrl.\u003C\u003Ef__am\u0024cache3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        OptionCtrl.\u003C\u003Ef__am\u0024cache3 = new UnityAction<bool>((object) null, __methodptr(\u003CInit\u003Em__5));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction<bool> fAmCache3 = OptionCtrl.\u003C\u003Ef__am\u0024cache3;
      ((UnityEvent<bool>) onValueChanged4).AddListener(fAmCache3);
      // ISSUE: variable of the null type
      __Null onValueChanged5 = this.toggleAutoSelect[0].onValueChanged;
      // ISSUE: reference to a compiler-generated field
      if (OptionCtrl.\u003C\u003Ef__am\u0024cache4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        OptionCtrl.\u003C\u003Ef__am\u0024cache4 = new UnityAction<bool>((object) null, __methodptr(\u003CInit\u003Em__6));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction<bool> fAmCache4 = OptionCtrl.\u003C\u003Ef__am\u0024cache4;
      ((UnityEvent<bool>) onValueChanged5).AddListener(fAmCache4);
      // ISSUE: variable of the null type
      __Null onValueChanged6 = this.toggleAutoSelect[1].onValueChanged;
      // ISSUE: reference to a compiler-generated field
      if (OptionCtrl.\u003C\u003Ef__am\u0024cache5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        OptionCtrl.\u003C\u003Ef__am\u0024cache5 = new UnityAction<bool>((object) null, __methodptr(\u003CInit\u003Em__7));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction<bool> fAmCache5 = OptionCtrl.\u003C\u003Ef__am\u0024cache5;
      ((UnityEvent<bool>) onValueChanged6).AddListener(fAmCache5);
      // ISSUE: variable of the null type
      __Null onValueChanged7 = this.toggleSnap[0].onValueChanged;
      // ISSUE: reference to a compiler-generated field
      if (OptionCtrl.\u003C\u003Ef__am\u0024cache6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        OptionCtrl.\u003C\u003Ef__am\u0024cache6 = new UnityAction<bool>((object) null, __methodptr(\u003CInit\u003Em__8));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction<bool> fAmCache6 = OptionCtrl.\u003C\u003Ef__am\u0024cache6;
      ((UnityEvent<bool>) onValueChanged7).AddListener(fAmCache6);
      // ISSUE: variable of the null type
      __Null onValueChanged8 = this.toggleSnap[1].onValueChanged;
      // ISSUE: reference to a compiler-generated field
      if (OptionCtrl.\u003C\u003Ef__am\u0024cache7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        OptionCtrl.\u003C\u003Ef__am\u0024cache7 = new UnityAction<bool>((object) null, __methodptr(\u003CInit\u003Em__9));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction<bool> fAmCache7 = OptionCtrl.\u003C\u003Ef__am\u0024cache7;
      ((UnityEvent<bool>) onValueChanged8).AddListener(fAmCache7);
      // ISSUE: variable of the null type
      __Null onValueChanged9 = this.toggleSnap[2].onValueChanged;
      // ISSUE: reference to a compiler-generated field
      if (OptionCtrl.\u003C\u003Ef__am\u0024cache8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        OptionCtrl.\u003C\u003Ef__am\u0024cache8 = new UnityAction<bool>((object) null, __methodptr(\u003CInit\u003Em__A));
      }
      // ISSUE: reference to a compiler-generated field
      UnityAction<bool> fAmCache8 = OptionCtrl.\u003C\u003Ef__am\u0024cache8;
      ((UnityEvent<bool>) onValueChanged9).AddListener(fAmCache8);
      this.charaFKColor.Init(this.spriteActive);
      this.itemFKColor.Init(this.spriteActive);
      this.routeSystem.Init(this.spriteActive);
      this.etc.Init(this.spriteActive);
      this.IsInit = true;
    }

    private void Start()
    {
      this.Init();
    }

    [Serializable]
    private class CommonInfo
    {
      public GameObject root;
      public Button button;
      private Sprite[] sprite;

      public bool active
      {
        get
        {
          return this.root.get_activeSelf();
        }
        set
        {
          if (!this.root.SetActiveIfDifferent(value))
            return;
          ((Selectable) this.button).get_image().set_sprite(this.sprite[!value ? 0 : 1]);
        }
      }

      protected bool isUpdateInfo { get; set; }

      public virtual void Init(Sprite[] _sprite)
      {
        // ISSUE: method pointer
        ((UnityEvent) this.button.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(\u003CInit\u003Em__0)));
        this.sprite = _sprite;
        this.isUpdateInfo = false;
      }

      public virtual void UpdateInfo()
      {
      }
    }

    [Serializable]
    public class InputCombination
    {
      public Slider slider;
      public InputField input;

      public bool interactable
      {
        set
        {
          ((Selectable) this.input).set_interactable(value);
          ((Selectable) this.slider).set_interactable(value);
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
          this.input.set_text(value.ToString("0.00"));
        }
      }

      public float min
      {
        get
        {
          return this.slider.get_minValue();
        }
      }

      public float max
      {
        get
        {
          return this.slider.get_maxValue();
        }
      }
    }

    [Serializable]
    private class CharaFKColor : OptionCtrl.CommonInfo
    {
      public Button[] buttons;
      public Toggle toggleLine;

      public override void Init(Sprite[] _sprite)
      {
        base.Init(_sprite);
        for (int index = 0; index < this.buttons.Length; ++index)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: method pointer
          ((UnityEvent) this.buttons[index].get_onClick()).AddListener(new UnityAction((object) new OptionCtrl.CharaFKColor.\u003CInit\u003Ec__AnonStorey0()
          {
            \u0024this = this,
            no = index
          }, __methodptr(\u003C\u003Em__0)));
        }
        ObservableExtensions.Subscribe<bool>((IObservable<M0>) UnityUIComponentExtensions.OnValueChangedAsObservable(this.toggleLine), (Action<M0>) (_b => Studio.Studio.optionSystem.lineFK = _b));
      }

      public override void UpdateInfo()
      {
        this.isUpdateInfo = true;
        ((Graphic) ((Selectable) this.buttons[0]).get_image()).set_color(Studio.Studio.optionSystem.colorFKHair);
        ((Graphic) ((Selectable) this.buttons[1]).get_image()).set_color(Studio.Studio.optionSystem.colorFKNeck);
        ((Graphic) ((Selectable) this.buttons[2]).get_image()).set_color(Studio.Studio.optionSystem.colorFKBreast);
        ((Graphic) ((Selectable) this.buttons[3]).get_image()).set_color(Studio.Studio.optionSystem.colorFKBody);
        ((Graphic) ((Selectable) this.buttons[4]).get_image()).set_color(Studio.Studio.optionSystem.colorFKRightHand);
        ((Graphic) ((Selectable) this.buttons[5]).get_image()).set_color(Studio.Studio.optionSystem.colorFKLeftHand);
        ((Graphic) ((Selectable) this.buttons[6]).get_image()).set_color(Studio.Studio.optionSystem.colorFKSkirt);
        this.toggleLine.set_isOn(Studio.Studio.optionSystem.lineFK);
        this.isUpdateInfo = false;
      }

      private void OnClickColor(int _idx)
      {
        string[] strArray = new string[7]
        {
          "髪",
          "首",
          "胸",
          "体",
          "右手",
          "左手",
          "スカート"
        };
        if (Singleton<Studio.Studio>.Instance.colorPalette.Check(string.Format("FKカラー {0}", (object) strArray[_idx])))
          Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
        else
          Singleton<Studio.Studio>.Instance.colorPalette.Setup(string.Format("FKカラー {0}", (object) strArray[_idx]), Studio.Studio.optionSystem.GetFKColor(_idx), (Action<Color>) (_c => this.SetColor(_idx, _c)), false);
      }

      private void SetColor(int _idx, Color _color)
      {
        Studio.Studio.optionSystem.SetFKColor(_idx, _color);
        ((Graphic) ((Selectable) this.buttons[_idx]).get_image()).set_color(_color);
        Singleton<Studio.Studio>.Instance.UpdateCharaFKColor();
      }

      private void OnValueChangedLine(bool _value)
      {
        if (this.isUpdateInfo)
          return;
        Studio.Studio.optionSystem.lineFK = _value;
      }
    }

    [Serializable]
    private class ItemFKColor : OptionCtrl.CommonInfo
    {
      public Button buttonColor;

      public override void Init(Sprite[] _sprite)
      {
        base.Init(_sprite);
        // ISSUE: method pointer
        ((UnityEvent) this.buttonColor.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickColor)));
      }

      public override void UpdateInfo()
      {
        this.isUpdateInfo = true;
        ((Graphic) ((Selectable) this.buttonColor).get_image()).set_color(Studio.Studio.optionSystem.colorFKItem);
        this.isUpdateInfo = false;
      }

      private void OnClickColor()
      {
        if (Singleton<Studio.Studio>.Instance.colorPalette.Check("FKカラー アイテム"))
          Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
        else
          Singleton<Studio.Studio>.Instance.colorPalette.Setup("FKカラー アイテム", Studio.Studio.optionSystem.colorFKItem, new Action<Color>(this.SetColor), false);
      }

      private void SetColor(Color _color)
      {
        Studio.Studio.optionSystem.colorFKItem = _color;
        ((Graphic) ((Selectable) this.buttonColor).get_image()).set_color(_color);
        Singleton<Studio.Studio>.Instance.UpdateItemFKColor();
      }
    }

    [Serializable]
    private class RouteSystem : OptionCtrl.CommonInfo
    {
      public OptionCtrl.InputCombination inputWidth = new OptionCtrl.InputCombination();
      public Toggle[] toggleLimit;

      public override void Init(Sprite[] _sprite)
      {
        base.Init(_sprite);
        // ISSUE: method pointer
        ((UnityEvent<float>) this.inputWidth.slider.get_onValueChanged()).AddListener(new UnityAction<float>((object) this, __methodptr(OnValueChangedRouteWidth)));
        // ISSUE: method pointer
        ((UnityEvent<string>) this.inputWidth.input.get_onEndEdit()).AddListener(new UnityAction<string>((object) this, __methodptr(OnEndEditRouteWidth)));
        // ISSUE: variable of the null type
        __Null onValueChanged1 = this.toggleLimit[0].onValueChanged;
        // ISSUE: reference to a compiler-generated field
        if (OptionCtrl.RouteSystem.\u003C\u003Ef__am\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method pointer
          OptionCtrl.RouteSystem.\u003C\u003Ef__am\u0024cache0 = new UnityAction<bool>((object) null, __methodptr(\u003CInit\u003Em__0));
        }
        // ISSUE: reference to a compiler-generated field
        UnityAction<bool> fAmCache0 = OptionCtrl.RouteSystem.\u003C\u003Ef__am\u0024cache0;
        ((UnityEvent<bool>) onValueChanged1).AddListener(fAmCache0);
        // ISSUE: variable of the null type
        __Null onValueChanged2 = this.toggleLimit[1].onValueChanged;
        // ISSUE: reference to a compiler-generated field
        if (OptionCtrl.RouteSystem.\u003C\u003Ef__am\u0024cache1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method pointer
          OptionCtrl.RouteSystem.\u003C\u003Ef__am\u0024cache1 = new UnityAction<bool>((object) null, __methodptr(\u003CInit\u003Em__1));
        }
        // ISSUE: reference to a compiler-generated field
        UnityAction<bool> fAmCache1 = OptionCtrl.RouteSystem.\u003C\u003Ef__am\u0024cache1;
        ((UnityEvent<bool>) onValueChanged2).AddListener(fAmCache1);
      }

      public override void UpdateInfo()
      {
        this.isUpdateInfo = true;
        this.inputWidth.value = Studio.Studio.optionSystem._routeLineWidth;
        this.toggleLimit[!Studio.Studio.optionSystem.routePointLimit ? 1 : 0].set_isOn(true);
        this.isUpdateInfo = false;
      }

      private void OnValueChangedRouteWidth(float _value)
      {
        Studio.Studio.optionSystem._routeLineWidth = _value;
        this.inputWidth.value = _value;
        Singleton<Studio.Studio>.Instance.routeControl.ReflectOption();
      }

      private void OnEndEditRouteWidth(string _text)
      {
        float num = Mathf.Clamp(Utility.StringToFloat(_text), this.inputWidth.min, this.inputWidth.max);
        Studio.Studio.optionSystem._routeLineWidth = num;
        this.inputWidth.value = num;
        Singleton<Studio.Studio>.Instance.routeControl.ReflectOption();
      }
    }

    [Serializable]
    private class Etc : OptionCtrl.CommonInfo
    {
      public Toggle[] toggleStartup;

      public override void Init(Sprite[] _sprite)
      {
        base.Init(_sprite);
        // ISSUE: variable of the null type
        __Null onValueChanged1 = this.toggleStartup[0].onValueChanged;
        // ISSUE: reference to a compiler-generated field
        if (OptionCtrl.Etc.\u003C\u003Ef__am\u0024cache0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method pointer
          OptionCtrl.Etc.\u003C\u003Ef__am\u0024cache0 = new UnityAction<bool>((object) null, __methodptr(\u003CInit\u003Em__0));
        }
        // ISSUE: reference to a compiler-generated field
        UnityAction<bool> fAmCache0 = OptionCtrl.Etc.\u003C\u003Ef__am\u0024cache0;
        ((UnityEvent<bool>) onValueChanged1).AddListener(fAmCache0);
        // ISSUE: variable of the null type
        __Null onValueChanged2 = this.toggleStartup[1].onValueChanged;
        // ISSUE: reference to a compiler-generated field
        if (OptionCtrl.Etc.\u003C\u003Ef__am\u0024cache1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method pointer
          OptionCtrl.Etc.\u003C\u003Ef__am\u0024cache1 = new UnityAction<bool>((object) null, __methodptr(\u003CInit\u003Em__1));
        }
        // ISSUE: reference to a compiler-generated field
        UnityAction<bool> fAmCache1 = OptionCtrl.Etc.\u003C\u003Ef__am\u0024cache1;
        ((UnityEvent<bool>) onValueChanged2).AddListener(fAmCache1);
      }

      public override void UpdateInfo()
      {
        this.isUpdateInfo = true;
        this.toggleStartup[!Studio.Studio.optionSystem.startupLoad ? 1 : 0].set_isOn(true);
        this.isUpdateInfo = false;
      }
    }
  }
}
