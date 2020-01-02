// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.InputBehaviorWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
  [AddComponentMenu("")]
  public class InputBehaviorWindow : Window
  {
    private const float minSensitivity = 0.1f;
    [SerializeField]
    private RectTransform spawnTransform;
    [SerializeField]
    private Button doneButton;
    [SerializeField]
    private Button cancelButton;
    [SerializeField]
    private Button defaultButton;
    [SerializeField]
    private Text doneButtonLabel;
    [SerializeField]
    private Text cancelButtonLabel;
    [SerializeField]
    private Text defaultButtonLabel;
    [SerializeField]
    private GameObject uiControlSetPrefab;
    [SerializeField]
    private GameObject uiSliderControlPrefab;
    private List<InputBehaviorWindow.InputBehaviorInfo> inputBehaviorInfo;
    private Dictionary<int, Action<int>> buttonCallbacks;
    private int playerId;

    public override void Initialize(int id, Func<int, bool> isFocusedCallback)
    {
      if (Object.op_Equality((Object) this.spawnTransform, (Object) null) || Object.op_Equality((Object) this.doneButton, (Object) null) || (Object.op_Equality((Object) this.cancelButton, (Object) null) || Object.op_Equality((Object) this.defaultButton, (Object) null)) || (Object.op_Equality((Object) this.uiControlSetPrefab, (Object) null) || Object.op_Equality((Object) this.uiSliderControlPrefab, (Object) null) || (Object.op_Equality((Object) this.doneButtonLabel, (Object) null) || Object.op_Equality((Object) this.cancelButtonLabel, (Object) null))) || Object.op_Equality((Object) this.defaultButtonLabel, (Object) null))
      {
        Debug.LogError((object) "Rewired Control Mapper: All inspector values must be assigned!");
      }
      else
      {
        this.inputBehaviorInfo = new List<InputBehaviorWindow.InputBehaviorInfo>();
        this.buttonCallbacks = new Dictionary<int, Action<int>>();
        this.doneButtonLabel.set_text(Rewired.UI.ControlMapper.ControlMapper.GetLanguage().done);
        this.cancelButtonLabel.set_text(Rewired.UI.ControlMapper.ControlMapper.GetLanguage().cancel);
        this.defaultButtonLabel.set_text(Rewired.UI.ControlMapper.ControlMapper.GetLanguage().default_);
        base.Initialize(id, isFocusedCallback);
      }
    }

    public void SetData(int playerId, Rewired.UI.ControlMapper.ControlMapper.InputBehaviorSettings[] data)
    {
      if (!this.initialized)
        return;
      this.playerId = playerId;
      for (int index = 0; index < data.Length; ++index)
      {
        Rewired.UI.ControlMapper.ControlMapper.InputBehaviorSettings behaviorSettings = data[index];
        if (behaviorSettings != null && behaviorSettings.isValid)
        {
          InputBehavior inputBehavior = this.GetInputBehavior(behaviorSettings.inputBehaviorId);
          if (inputBehavior != null)
          {
            UIControlSet controlSet = this.CreateControlSet();
            Dictionary<int, InputBehaviorWindow.PropertyType> idToProperty = new Dictionary<int, InputBehaviorWindow.PropertyType>();
            string customEntry = Rewired.UI.ControlMapper.ControlMapper.GetLanguage().GetCustomEntry(behaviorSettings.labelLanguageKey);
            if (!string.IsNullOrEmpty(customEntry))
              controlSet.SetTitle(customEntry);
            else
              controlSet.SetTitle(inputBehavior.get_name());
            if (behaviorSettings.showJoystickAxisSensitivity)
            {
              UISliderControl slider = this.CreateSlider(controlSet, inputBehavior.get_id(), (string) null, Rewired.UI.ControlMapper.ControlMapper.GetLanguage().GetCustomEntry(behaviorSettings.joystickAxisSensitivityLabelLanguageKey), behaviorSettings.joystickAxisSensitivityIcon, behaviorSettings.joystickAxisSensitivityMin, behaviorSettings.joystickAxisSensitivityMax, new Action<int, int, float>(this.JoystickAxisSensitivityValueChanged), new Action<int, int>(this.JoystickAxisSensitivityCanceled));
              slider.slider.set_value(Mathf.Clamp(inputBehavior.get_joystickAxisSensitivity(), behaviorSettings.joystickAxisSensitivityMin, behaviorSettings.joystickAxisSensitivityMax));
              idToProperty.Add(slider.id, InputBehaviorWindow.PropertyType.JoystickAxisSensitivity);
            }
            if (behaviorSettings.showMouseXYAxisSensitivity)
            {
              UISliderControl slider = this.CreateSlider(controlSet, inputBehavior.get_id(), (string) null, Rewired.UI.ControlMapper.ControlMapper.GetLanguage().GetCustomEntry(behaviorSettings.mouseXYAxisSensitivityLabelLanguageKey), behaviorSettings.mouseXYAxisSensitivityIcon, behaviorSettings.mouseXYAxisSensitivityMin, behaviorSettings.mouseXYAxisSensitivityMax, new Action<int, int, float>(this.MouseXYAxisSensitivityValueChanged), new Action<int, int>(this.MouseXYAxisSensitivityCanceled));
              slider.slider.set_value(Mathf.Clamp(inputBehavior.get_mouseXYAxisSensitivity(), behaviorSettings.mouseXYAxisSensitivityMin, behaviorSettings.mouseXYAxisSensitivityMax));
              idToProperty.Add(slider.id, InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity);
            }
            this.inputBehaviorInfo.Add(new InputBehaviorWindow.InputBehaviorInfo(inputBehavior, controlSet, idToProperty));
          }
        }
      }
      this.defaultUIElement = ((Component) this.doneButton).get_gameObject();
    }

    public void SetButtonCallback(
      InputBehaviorWindow.ButtonIdentifier buttonIdentifier,
      Action<int> callback)
    {
      if (!this.initialized || callback == null)
        return;
      if (this.buttonCallbacks.ContainsKey((int) buttonIdentifier))
        this.buttonCallbacks[(int) buttonIdentifier] = callback;
      else
        this.buttonCallbacks.Add((int) buttonIdentifier, callback);
    }

    public override void Cancel()
    {
      if (!this.initialized)
        return;
      foreach (InputBehaviorWindow.InputBehaviorInfo inputBehaviorInfo in this.inputBehaviorInfo)
        inputBehaviorInfo.RestorePreviousData();
      Action<int> action;
      if (!this.buttonCallbacks.TryGetValue(1, out action))
      {
        if (this.cancelCallback == null)
          return;
        this.cancelCallback.Invoke();
      }
      else
        action(this.id);
    }

    public void OnDone()
    {
      Action<int> action;
      if (!this.initialized || !this.buttonCallbacks.TryGetValue(0, out action))
        return;
      action(this.id);
    }

    public void OnCancel()
    {
      this.Cancel();
    }

    public void OnRestoreDefault()
    {
      if (!this.initialized)
        return;
      foreach (InputBehaviorWindow.InputBehaviorInfo inputBehaviorInfo in this.inputBehaviorInfo)
        inputBehaviorInfo.RestoreDefaultData();
    }

    private void JoystickAxisSensitivityValueChanged(
      int inputBehaviorId,
      int controlId,
      float value)
    {
      this.GetInputBehavior(inputBehaviorId).set_joystickAxisSensitivity(value);
    }

    private void MouseXYAxisSensitivityValueChanged(
      int inputBehaviorId,
      int controlId,
      float value)
    {
      this.GetInputBehavior(inputBehaviorId).set_mouseXYAxisSensitivity(value);
    }

    private void JoystickAxisSensitivityCanceled(int inputBehaviorId, int controlId)
    {
      this.GetInputBehaviorInfo(inputBehaviorId)?.RestoreData(InputBehaviorWindow.PropertyType.JoystickAxisSensitivity, controlId);
    }

    private void MouseXYAxisSensitivityCanceled(int inputBehaviorId, int controlId)
    {
      this.GetInputBehaviorInfo(inputBehaviorId)?.RestoreData(InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity, controlId);
    }

    public override void TakeInputFocus()
    {
      base.TakeInputFocus();
    }

    private UIControlSet CreateControlSet()
    {
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.uiControlSetPrefab);
      gameObject.get_transform().SetParent((Transform) this.spawnTransform, false);
      return (UIControlSet) gameObject.GetComponent<UIControlSet>();
    }

    private UISliderControl CreateSlider(
      UIControlSet set,
      int inputBehaviorId,
      string defaultTitle,
      string overrideTitle,
      Sprite icon,
      float minValue,
      float maxValue,
      Action<int, int, float> valueChangedCallback,
      Action<int, int> cancelCallback)
    {
      UISliderControl slider = set.CreateSlider(this.uiSliderControlPrefab, icon, minValue, maxValue, (Action<int, float>) ((cId, value) => valueChangedCallback(inputBehaviorId, cId, value)), (Action<int>) (cId => cancelCallback(inputBehaviorId, cId)));
      string str = !string.IsNullOrEmpty(overrideTitle) ? overrideTitle : defaultTitle;
      if (!string.IsNullOrEmpty(str))
      {
        slider.showTitle = true;
        slider.title.set_text(str);
      }
      else
        slider.showTitle = false;
      slider.showIcon = Object.op_Inequality((Object) icon, (Object) null);
      return slider;
    }

    private InputBehavior GetInputBehavior(int id)
    {
      return ReInput.get_mapping().GetInputBehavior(this.playerId, id);
    }

    private InputBehaviorWindow.InputBehaviorInfo GetInputBehaviorInfo(
      int inputBehaviorId)
    {
      int count = this.inputBehaviorInfo.Count;
      for (int index = 0; index < count; ++index)
      {
        if (this.inputBehaviorInfo[index].inputBehavior.get_id() == inputBehaviorId)
          return this.inputBehaviorInfo[index];
      }
      return (InputBehaviorWindow.InputBehaviorInfo) null;
    }

    private class InputBehaviorInfo
    {
      private InputBehavior _inputBehavior;
      private UIControlSet _controlSet;
      private Dictionary<int, InputBehaviorWindow.PropertyType> idToProperty;
      private InputBehavior copyOfOriginal;

      public InputBehaviorInfo(
        InputBehavior inputBehavior,
        UIControlSet controlSet,
        Dictionary<int, InputBehaviorWindow.PropertyType> idToProperty)
      {
        this._inputBehavior = inputBehavior;
        this._controlSet = controlSet;
        this.idToProperty = idToProperty;
        this.copyOfOriginal = new InputBehavior(inputBehavior);
      }

      public InputBehavior inputBehavior
      {
        get
        {
          return this._inputBehavior;
        }
      }

      public UIControlSet controlSet
      {
        get
        {
          return this._controlSet;
        }
      }

      public void RestorePreviousData()
      {
        this._inputBehavior.ImportData(this.copyOfOriginal);
      }

      public void RestoreDefaultData()
      {
        this._inputBehavior.Reset();
        this.RefreshControls();
      }

      public void RestoreData(InputBehaviorWindow.PropertyType propertyType, int controlId)
      {
        switch (propertyType)
        {
          case InputBehaviorWindow.PropertyType.JoystickAxisSensitivity:
            float joystickAxisSensitivity = this.copyOfOriginal.get_joystickAxisSensitivity();
            this._inputBehavior.set_joystickAxisSensitivity(joystickAxisSensitivity);
            UISliderControl control1 = this._controlSet.GetControl<UISliderControl>(controlId);
            if (!Object.op_Inequality((Object) control1, (Object) null))
              break;
            control1.slider.set_value(joystickAxisSensitivity);
            break;
          case InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity:
            float xyAxisSensitivity = this.copyOfOriginal.get_mouseXYAxisSensitivity();
            this._inputBehavior.set_mouseXYAxisSensitivity(xyAxisSensitivity);
            UISliderControl control2 = this._controlSet.GetControl<UISliderControl>(controlId);
            if (!Object.op_Inequality((Object) control2, (Object) null))
              break;
            control2.slider.set_value(xyAxisSensitivity);
            break;
        }
      }

      public void RefreshControls()
      {
        if (Object.op_Equality((Object) this._controlSet, (Object) null) || this.idToProperty == null)
          return;
        foreach (KeyValuePair<int, InputBehaviorWindow.PropertyType> keyValuePair in this.idToProperty)
        {
          UISliderControl control = this._controlSet.GetControl<UISliderControl>(keyValuePair.Key);
          if (!Object.op_Equality((Object) control, (Object) null))
          {
            switch (keyValuePair.Value)
            {
              case InputBehaviorWindow.PropertyType.JoystickAxisSensitivity:
                control.slider.set_value(this._inputBehavior.get_joystickAxisSensitivity());
                continue;
              case InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity:
                control.slider.set_value(this._inputBehavior.get_mouseXYAxisSensitivity());
                continue;
              default:
                continue;
            }
          }
        }
      }
    }

    public enum ButtonIdentifier
    {
      Done,
      Cancel,
      Default,
    }

    private enum PropertyType
    {
      JoystickAxisSensitivity,
      MouseXYAxisSensitivity,
    }
  }
}
