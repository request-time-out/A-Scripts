// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.CalibrationWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Rewired.Integration.UnityUI;
using Rewired.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
  [AddComponentMenu("")]
  public class CalibrationWindow : Window
  {
    private int selectedAxis = -1;
    private int menuHorizActionId = -1;
    private int menuVertActionId = -1;
    private const float minSensitivityOtherAxes = 0.1f;
    private const float maxDeadzone = 0.8f;
    [SerializeField]
    private RectTransform rightContentContainer;
    [SerializeField]
    private RectTransform valueDisplayGroup;
    [SerializeField]
    private RectTransform calibratedValueMarker;
    [SerializeField]
    private RectTransform rawValueMarker;
    [SerializeField]
    private RectTransform calibratedZeroMarker;
    [SerializeField]
    private RectTransform deadzoneArea;
    [SerializeField]
    private Slider deadzoneSlider;
    [SerializeField]
    private Slider zeroSlider;
    [SerializeField]
    private Slider sensitivitySlider;
    [SerializeField]
    private Toggle invertToggle;
    [SerializeField]
    private RectTransform axisScrollAreaContent;
    [SerializeField]
    private Button doneButton;
    [SerializeField]
    private Button calibrateButton;
    [SerializeField]
    private Text doneButtonLabel;
    [SerializeField]
    private Text cancelButtonLabel;
    [SerializeField]
    private Text defaultButtonLabel;
    [SerializeField]
    private Text deadzoneSliderLabel;
    [SerializeField]
    private Text zeroSliderLabel;
    [SerializeField]
    private Text sensitivitySliderLabel;
    [SerializeField]
    private Text invertToggleLabel;
    [SerializeField]
    private Text calibrateButtonLabel;
    [SerializeField]
    private GameObject axisButtonPrefab;
    private Joystick joystick;
    private string origCalibrationData;
    private AxisCalibrationData origSelectedAxisCalibrationData;
    private float displayAreaWidth;
    private List<Button> axisButtons;
    private Dictionary<int, Action<int>> buttonCallbacks;
    private int playerId;
    private RewiredStandaloneInputModule rewiredStandaloneInputModule;
    private float minSensitivity;

    private bool axisSelected
    {
      get
      {
        return this.joystick != null && this.selectedAxis >= 0 && this.selectedAxis < ((ControllerWithAxes) this.joystick).get_calibrationMap().get_axisCount();
      }
    }

    private AxisCalibration axisCalibration
    {
      get
      {
        return !this.axisSelected ? (AxisCalibration) null : ((ControllerWithAxes) this.joystick).get_calibrationMap().GetAxis(this.selectedAxis);
      }
    }

    public override void Initialize(int id, Func<int, bool> isFocusedCallback)
    {
      if (Object.op_Equality((Object) this.rightContentContainer, (Object) null) || Object.op_Equality((Object) this.valueDisplayGroup, (Object) null) || (Object.op_Equality((Object) this.calibratedValueMarker, (Object) null) || Object.op_Equality((Object) this.rawValueMarker, (Object) null)) || (Object.op_Equality((Object) this.calibratedZeroMarker, (Object) null) || Object.op_Equality((Object) this.deadzoneArea, (Object) null) || (Object.op_Equality((Object) this.deadzoneSlider, (Object) null) || Object.op_Equality((Object) this.sensitivitySlider, (Object) null))) || (Object.op_Equality((Object) this.zeroSlider, (Object) null) || Object.op_Equality((Object) this.invertToggle, (Object) null) || (Object.op_Equality((Object) this.axisScrollAreaContent, (Object) null) || Object.op_Equality((Object) this.doneButton, (Object) null)) || (Object.op_Equality((Object) this.calibrateButton, (Object) null) || Object.op_Equality((Object) this.axisButtonPrefab, (Object) null) || (Object.op_Equality((Object) this.doneButtonLabel, (Object) null) || Object.op_Equality((Object) this.cancelButtonLabel, (Object) null)))) || (Object.op_Equality((Object) this.defaultButtonLabel, (Object) null) || Object.op_Equality((Object) this.deadzoneSliderLabel, (Object) null) || (Object.op_Equality((Object) this.zeroSliderLabel, (Object) null) || Object.op_Equality((Object) this.sensitivitySliderLabel, (Object) null)) || (Object.op_Equality((Object) this.invertToggleLabel, (Object) null) || Object.op_Equality((Object) this.calibrateButtonLabel, (Object) null))))
      {
        Debug.LogError((object) "Rewired Control Mapper: All inspector values must be assigned!");
      }
      else
      {
        this.axisButtons = new List<Button>();
        this.buttonCallbacks = new Dictionary<int, Action<int>>();
        this.doneButtonLabel.set_text(Rewired.UI.ControlMapper.ControlMapper.GetLanguage().done);
        this.cancelButtonLabel.set_text(Rewired.UI.ControlMapper.ControlMapper.GetLanguage().cancel);
        this.defaultButtonLabel.set_text(Rewired.UI.ControlMapper.ControlMapper.GetLanguage().default_);
        this.deadzoneSliderLabel.set_text(Rewired.UI.ControlMapper.ControlMapper.GetLanguage().calibrateWindow_deadZoneSliderLabel);
        this.zeroSliderLabel.set_text(Rewired.UI.ControlMapper.ControlMapper.GetLanguage().calibrateWindow_zeroSliderLabel);
        this.sensitivitySliderLabel.set_text(Rewired.UI.ControlMapper.ControlMapper.GetLanguage().calibrateWindow_sensitivitySliderLabel);
        this.invertToggleLabel.set_text(Rewired.UI.ControlMapper.ControlMapper.GetLanguage().calibrateWindow_invertToggleLabel);
        this.calibrateButtonLabel.set_text(Rewired.UI.ControlMapper.ControlMapper.GetLanguage().calibrateWindow_calibrateButtonLabel);
        base.Initialize(id, isFocusedCallback);
      }
    }

    public void SetJoystick(int playerId, Joystick joystick)
    {
      if (!this.initialized)
        return;
      this.playerId = playerId;
      this.joystick = joystick;
      if (joystick == null)
      {
        Debug.LogError((object) "Rewired Control Mapper: Joystick cannot be null!");
      }
      else
      {
        float num = 0.0f;
        for (int index = 0; index < ((ControllerWithAxes) joystick).get_axisCount(); ++index)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          CalibrationWindow.\u003CSetJoystick\u003Ec__AnonStorey0 joystickCAnonStorey0 = new CalibrationWindow.\u003CSetJoystick\u003Ec__AnonStorey0();
          // ISSUE: reference to a compiler-generated field
          joystickCAnonStorey0.\u0024this = this;
          // ISSUE: reference to a compiler-generated field
          joystickCAnonStorey0.index = index;
          GameObject gameObject = UITools.InstantiateGUIObject<Button>(this.axisButtonPrefab, (Transform) this.axisScrollAreaContent, "Axis" + (object) index);
          // ISSUE: reference to a compiler-generated field
          joystickCAnonStorey0.button = (Button) gameObject.GetComponent<Button>();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: method pointer
          ((UnityEvent) joystickCAnonStorey0.button.get_onClick()).AddListener(new UnityAction((object) joystickCAnonStorey0, __methodptr(\u003C\u003Em__0)));
          Text inSelfOrChildren = (Text) UnityTools.GetComponentInSelfOrChildren<Text>(gameObject);
          if (Object.op_Inequality((Object) inSelfOrChildren, (Object) null))
            inSelfOrChildren.set_text(((ControllerWithAxes) joystick).get_AxisElementIdentifiers()[index].get_name());
          if ((double) num == 0.0)
            num = ((LayoutElement) UnityTools.GetComponentInSelfOrChildren<LayoutElement>(gameObject)).get_minHeight();
          // ISSUE: reference to a compiler-generated field
          this.axisButtons.Add(joystickCAnonStorey0.button);
        }
        float spacing = ((HorizontalOrVerticalLayoutGroup) ((Component) this.axisScrollAreaContent).GetComponent<VerticalLayoutGroup>()).get_spacing();
        this.axisScrollAreaContent.set_sizeDelta(new Vector2((float) this.axisScrollAreaContent.get_sizeDelta().x, Mathf.Max((float) ((ControllerWithAxes) joystick).get_axisCount() * (num + spacing) - spacing, (float) this.axisScrollAreaContent.get_sizeDelta().y)));
        this.origCalibrationData = ((ControllerWithAxes) joystick).get_calibrationMap().ToXmlString();
        this.displayAreaWidth = (float) this.rightContentContainer.get_sizeDelta().x;
        this.rewiredStandaloneInputModule = (RewiredStandaloneInputModule) ((Component) ((Component) this).get_gameObject().get_transform().get_root()).GetComponentInChildren<RewiredStandaloneInputModule>();
        if (Object.op_Inequality((Object) this.rewiredStandaloneInputModule, (Object) null))
        {
          this.menuHorizActionId = ReInput.get_mapping().GetActionId(this.rewiredStandaloneInputModule.horizontalAxis);
          this.menuVertActionId = ReInput.get_mapping().GetActionId(this.rewiredStandaloneInputModule.verticalAxis);
        }
        if (((ControllerWithAxes) joystick).get_axisCount() > 0)
          this.SelectAxis(0);
        this.defaultUIElement = ((Component) this.doneButton).get_gameObject();
        this.RefreshControls();
        this.Redraw();
      }
    }

    public void SetButtonCallback(
      CalibrationWindow.ButtonIdentifier buttonIdentifier,
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
      if (this.joystick != null)
        ((ControllerWithAxes) this.joystick).ImportCalibrationMapFromXmlString(this.origCalibrationData);
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

    protected override void Update()
    {
      if (!this.initialized)
        return;
      base.Update();
      this.UpdateDisplay();
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
      if (!this.initialized || this.joystick == null)
        return;
      ((ControllerWithAxes) this.joystick).get_calibrationMap().Reset();
      this.RefreshControls();
      this.Redraw();
    }

    public void OnCalibrate()
    {
      Action<int> action;
      if (!this.initialized || !this.buttonCallbacks.TryGetValue(3, out action))
        return;
      action(this.selectedAxis);
    }

    public void OnInvert(bool state)
    {
      if (!this.initialized || !this.axisSelected)
        return;
      this.axisCalibration.set_invert(state);
    }

    public void OnZeroValueChange(float value)
    {
      if (!this.initialized || !this.axisSelected)
        return;
      this.axisCalibration.set_calibratedZero(value);
      this.RedrawCalibratedZero();
    }

    public void OnZeroCancel()
    {
      if (!this.initialized || !this.axisSelected)
        return;
      this.axisCalibration.set_calibratedZero((float) this.origSelectedAxisCalibrationData.zero);
      this.RedrawCalibratedZero();
      this.RefreshControls();
    }

    public void OnDeadzoneValueChange(float value)
    {
      if (!this.initialized || !this.axisSelected)
        return;
      this.axisCalibration.set_deadZone(Mathf.Clamp(value, 0.0f, 0.8f));
      if ((double) value > 0.800000011920929)
        this.deadzoneSlider.set_value(0.8f);
      this.RedrawDeadzone();
    }

    public void OnDeadzoneCancel()
    {
      if (!this.initialized || !this.axisSelected)
        return;
      this.axisCalibration.set_deadZone((float) this.origSelectedAxisCalibrationData.deadZone);
      this.RedrawDeadzone();
      this.RefreshControls();
    }

    public void OnSensitivityValueChange(float value)
    {
      if (!this.initialized || !this.axisSelected)
        return;
      this.axisCalibration.set_sensitivity(Mathf.Clamp(value, this.minSensitivity, float.PositiveInfinity));
      if ((double) value >= (double) this.minSensitivity)
        return;
      this.sensitivitySlider.set_value(this.minSensitivity);
    }

    public void OnSensitivityCancel(float value)
    {
      if (!this.initialized || !this.axisSelected)
        return;
      this.axisCalibration.set_sensitivity((float) this.origSelectedAxisCalibrationData.sensitivity);
      this.RefreshControls();
    }

    public void OnAxisScrollRectScroll(Vector2 pos)
    {
      if (this.initialized)
        ;
    }

    private void OnAxisSelected(int axisIndex, Button button)
    {
      if (!this.initialized || this.joystick == null)
        return;
      this.SelectAxis(axisIndex);
      this.RefreshControls();
      this.Redraw();
    }

    private void UpdateDisplay()
    {
      this.RedrawValueMarkers();
    }

    private void Redraw()
    {
      this.RedrawCalibratedZero();
      this.RedrawValueMarkers();
    }

    private void RefreshControls()
    {
      if (!this.axisSelected)
      {
        this.deadzoneSlider.set_value(0.0f);
        this.zeroSlider.set_value(0.0f);
        this.sensitivitySlider.set_value(0.0f);
        this.invertToggle.set_isOn(false);
      }
      else
      {
        this.deadzoneSlider.set_value(this.axisCalibration.get_deadZone());
        this.zeroSlider.set_value(this.axisCalibration.get_calibratedZero());
        this.sensitivitySlider.set_value(this.axisCalibration.get_sensitivity());
        this.invertToggle.set_isOn(this.axisCalibration.get_invert());
      }
    }

    private void RedrawDeadzone()
    {
      if (!this.axisSelected)
        return;
      this.deadzoneArea.set_sizeDelta(new Vector2(this.displayAreaWidth * this.axisCalibration.get_deadZone(), (float) this.deadzoneArea.get_sizeDelta().y));
      this.deadzoneArea.set_anchoredPosition(new Vector2(this.axisCalibration.get_calibratedZero() * (float) -((Transform) this.deadzoneArea).get_parent().get_localPosition().x, (float) this.deadzoneArea.get_anchoredPosition().y));
    }

    private void RedrawCalibratedZero()
    {
      if (!this.axisSelected)
        return;
      this.calibratedZeroMarker.set_anchoredPosition(new Vector2(this.axisCalibration.get_calibratedZero() * (float) -((Transform) this.deadzoneArea).get_parent().get_localPosition().x, (float) this.calibratedZeroMarker.get_anchoredPosition().y));
      this.RedrawDeadzone();
    }

    private void RedrawValueMarkers()
    {
      if (!this.axisSelected)
      {
        this.calibratedValueMarker.set_anchoredPosition(new Vector2(0.0f, (float) this.calibratedValueMarker.get_anchoredPosition().y));
        this.rawValueMarker.set_anchoredPosition(new Vector2(0.0f, (float) this.rawValueMarker.get_anchoredPosition().y));
      }
      else
      {
        float axis = ((ControllerWithAxes) this.joystick).GetAxis(this.selectedAxis);
        float num = Mathf.Clamp(((ControllerWithAxes) this.joystick).GetAxisRaw(this.selectedAxis), -1f, 1f);
        this.calibratedValueMarker.set_anchoredPosition(new Vector2(this.displayAreaWidth * 0.5f * axis, (float) this.calibratedValueMarker.get_anchoredPosition().y));
        this.rawValueMarker.set_anchoredPosition(new Vector2(this.displayAreaWidth * 0.5f * num, (float) this.rawValueMarker.get_anchoredPosition().y));
      }
    }

    private void SelectAxis(int index)
    {
      if (index < 0 || index >= this.axisButtons.Count || Object.op_Equality((Object) this.axisButtons[index], (Object) null))
        return;
      ((Selectable) this.axisButtons[index]).set_interactable(false);
      ((Selectable) this.axisButtons[index]).Select();
      for (int index1 = 0; index1 < this.axisButtons.Count; ++index1)
      {
        if (index1 != index)
          ((Selectable) this.axisButtons[index1]).set_interactable(true);
      }
      this.selectedAxis = index;
      this.origSelectedAxisCalibrationData = this.axisCalibration.GetData();
      this.SetMinSensitivity();
    }

    public override void TakeInputFocus()
    {
      base.TakeInputFocus();
      if (this.selectedAxis >= 0)
        this.SelectAxis(this.selectedAxis);
      this.RefreshControls();
      this.Redraw();
    }

    private void SetMinSensitivity()
    {
      if (!this.axisSelected)
        return;
      this.minSensitivity = 0.1f;
      if (!Object.op_Inequality((Object) this.rewiredStandaloneInputModule, (Object) null))
        return;
      if (this.IsMenuAxis(this.menuHorizActionId, this.selectedAxis))
      {
        this.GetAxisButtonDeadZone(this.playerId, this.menuHorizActionId, ref this.minSensitivity);
      }
      else
      {
        if (!this.IsMenuAxis(this.menuVertActionId, this.selectedAxis))
          return;
        this.GetAxisButtonDeadZone(this.playerId, this.menuVertActionId, ref this.minSensitivity);
      }
    }

    private bool IsMenuAxis(int actionId, int axisIndex)
    {
      if (Object.op_Equality((Object) this.rewiredStandaloneInputModule, (Object) null))
        return false;
      IList<Player> allPlayers = ReInput.get_players().get_AllPlayers();
      int count1 = ((ICollection<Player>) allPlayers).Count;
      for (int index1 = 0; index1 < count1; ++index1)
      {
        IList<JoystickMap> maps = (IList<JoystickMap>) ((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) allPlayers[index1].controllers).maps).GetMaps<JoystickMap>((int) ((Controller) this.joystick).id);
        if (maps != null)
        {
          int count2 = ((ICollection<JoystickMap>) maps).Count;
          for (int index2 = 0; index2 < count2; ++index2)
          {
            IList<ActionElementMap> axisMaps = ((ControllerMapWithAxes) maps[index2]).get_AxisMaps();
            if (axisMaps != null)
            {
              int count3 = ((ICollection<ActionElementMap>) axisMaps).Count;
              for (int index3 = 0; index3 < count3; ++index3)
              {
                ActionElementMap actionElementMap = axisMaps[index3];
                if (actionElementMap.get_actionId() == actionId && actionElementMap.get_elementIndex() == axisIndex)
                  return true;
              }
            }
          }
        }
      }
      return false;
    }

    private void GetAxisButtonDeadZone(int playerId, int actionId, ref float value)
    {
      InputAction action = ReInput.get_mapping().GetAction(actionId);
      if (action == null)
        return;
      int behaviorId = action.get_behaviorId();
      InputBehavior inputBehavior = ReInput.get_mapping().GetInputBehavior(playerId, behaviorId);
      if (inputBehavior == null)
        return;
      value = inputBehavior.get_buttonDeadZone() + 0.1f;
    }

    public enum ButtonIdentifier
    {
      Done,
      Cancel,
      Default,
      Calibrate,
    }
  }
}
