// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos.GamepadTemplateUI
{
  public class GamepadTemplateUI : MonoBehaviour
  {
    private const float stickRadius = 20f;
    public int playerId;
    [SerializeField]
    private RectTransform leftStick;
    [SerializeField]
    private RectTransform rightStick;
    [SerializeField]
    private ControllerUIElement leftStickX;
    [SerializeField]
    private ControllerUIElement leftStickY;
    [SerializeField]
    private ControllerUIElement leftStickButton;
    [SerializeField]
    private ControllerUIElement rightStickX;
    [SerializeField]
    private ControllerUIElement rightStickY;
    [SerializeField]
    private ControllerUIElement rightStickButton;
    [SerializeField]
    private ControllerUIElement actionBottomRow1;
    [SerializeField]
    private ControllerUIElement actionBottomRow2;
    [SerializeField]
    private ControllerUIElement actionBottomRow3;
    [SerializeField]
    private ControllerUIElement actionTopRow1;
    [SerializeField]
    private ControllerUIElement actionTopRow2;
    [SerializeField]
    private ControllerUIElement actionTopRow3;
    [SerializeField]
    private ControllerUIElement leftShoulder;
    [SerializeField]
    private ControllerUIElement leftTrigger;
    [SerializeField]
    private ControllerUIElement rightShoulder;
    [SerializeField]
    private ControllerUIElement rightTrigger;
    [SerializeField]
    private ControllerUIElement center1;
    [SerializeField]
    private ControllerUIElement center2;
    [SerializeField]
    private ControllerUIElement center3;
    [SerializeField]
    private ControllerUIElement dPadUp;
    [SerializeField]
    private ControllerUIElement dPadRight;
    [SerializeField]
    private ControllerUIElement dPadDown;
    [SerializeField]
    private ControllerUIElement dPadLeft;
    private Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement[] _uiElementsArray;
    private Dictionary<int, ControllerUIElement> _uiElements;
    private IList<ControllerTemplateElementTarget> _tempTargetList;
    private Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.Stick[] _sticks;

    public GamepadTemplateUI()
    {
      base.\u002Ector();
    }

    private Player player
    {
      get
      {
        return ReInput.get_players().GetPlayer(this.playerId);
      }
    }

    private void Awake()
    {
      this._uiElementsArray = new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement[23]
      {
        new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(0, this.leftStickX),
        new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(1, this.leftStickY),
        new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(17, this.leftStickButton),
        new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(2, this.rightStickX),
        new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(3, this.rightStickY),
        new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(18, this.rightStickButton),
        new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(4, this.actionBottomRow1),
        new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(5, this.actionBottomRow2),
        new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(6, this.actionBottomRow3),
        new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(7, this.actionTopRow1),
        new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(8, this.actionTopRow2),
        new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(9, this.actionTopRow3),
        new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(14, this.center1),
        new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(15, this.center2),
        new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(16, this.center3),
        new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(19, this.dPadUp),
        new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(20, this.dPadRight),
        new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(21, this.dPadDown),
        new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(22, this.dPadLeft),
        new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(10, this.leftShoulder),
        new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(11, this.leftTrigger),
        new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(12, this.rightShoulder),
        new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.UIElement(13, this.rightTrigger)
      };
      for (int index = 0; index < this._uiElementsArray.Length; ++index)
        this._uiElements.Add(this._uiElementsArray[index].id, this._uiElementsArray[index].element);
      this._sticks = new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.Stick[2]
      {
        new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.Stick(this.leftStick, 0, 1),
        new Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.Stick(this.rightStick, 2, 3)
      };
      ReInput.add_ControllerConnectedEvent(new Action<ControllerStatusChangedEventArgs>(this.OnControllerConnected));
      ReInput.add_ControllerDisconnectedEvent(new Action<ControllerStatusChangedEventArgs>(this.OnControllerDisconnected));
    }

    private void Start()
    {
      if (!ReInput.get_isReady())
        return;
      this.DrawLabels();
    }

    private void OnDestroy()
    {
      ReInput.remove_ControllerConnectedEvent(new Action<ControllerStatusChangedEventArgs>(this.OnControllerConnected));
      ReInput.remove_ControllerDisconnectedEvent(new Action<ControllerStatusChangedEventArgs>(this.OnControllerDisconnected));
    }

    private void Update()
    {
      if (!ReInput.get_isReady())
        return;
      this.DrawActiveElements();
    }

    private void DrawActiveElements()
    {
      for (int index = 0; index < this._uiElementsArray.Length; ++index)
        this._uiElementsArray[index].element.Deactivate();
      for (int index = 0; index < this._sticks.Length; ++index)
        this._sticks[index].Reset();
      IList<InputAction> actions = ReInput.get_mapping().get_Actions();
      for (int index = 0; index < ((ICollection<InputAction>) actions).Count; ++index)
        this.ActivateElements(this.player, actions[index].get_id());
    }

    private void ActivateElements(Player player, int actionId)
    {
      float axis = player.GetAxis(actionId);
      if ((double) axis == 0.0)
        return;
      IList<InputActionSourceData> currentInputSources = player.GetCurrentInputSources(actionId);
      for (int index1 = 0; index1 < ((ICollection<InputActionSourceData>) currentInputSources).Count; ++index1)
      {
        InputActionSourceData actionSourceData = currentInputSources[index1];
        IGamepadTemplate template = (IGamepadTemplate) ((InputActionSourceData) ref actionSourceData).get_controller().GetTemplate<IGamepadTemplate>();
        if (template != null)
        {
          ((IControllerTemplate) template).GetElementTargets(ControllerElementTarget.op_Implicit(((InputActionSourceData) ref actionSourceData).get_actionElementMap()), this._tempTargetList);
          for (int index2 = 0; index2 < ((ICollection<ControllerTemplateElementTarget>) this._tempTargetList).Count; ++index2)
          {
            ControllerTemplateElementTarget tempTarget = this._tempTargetList[index2];
            int id = ((ControllerTemplateElementTarget) ref tempTarget).get_element().get_id();
            ControllerUIElement uiElement = this._uiElements[id];
            if (((ControllerTemplateElementTarget) ref tempTarget).get_elementType() == null)
              uiElement.Activate(axis);
            else if (((ControllerTemplateElementTarget) ref tempTarget).get_elementType() == 1 && (player.GetButton(actionId) || player.GetNegativeButton(actionId)))
              uiElement.Activate(1f);
            this.GetStick(id)?.SetAxisPosition(id, axis * 20f);
          }
        }
      }
    }

    private void DrawLabels()
    {
      for (int index = 0; index < this._uiElementsArray.Length; ++index)
        this._uiElementsArray[index].element.ClearLabels();
      IList<InputAction> actions = ReInput.get_mapping().get_Actions();
      for (int index = 0; index < ((ICollection<InputAction>) actions).Count; ++index)
        this.DrawLabels(this.player, actions[index]);
    }

    private void DrawLabels(Player player, InputAction action)
    {
      Controller controllerWithTemplate = ((Player.ControllerHelper) player.controllers).GetFirstControllerWithTemplate<IGamepadTemplate>();
      if (controllerWithTemplate == null)
        return;
      IGamepadTemplate template = (IGamepadTemplate) controllerWithTemplate.GetTemplate<IGamepadTemplate>();
      ControllerMap map = ((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) player.controllers).maps).GetMap(controllerWithTemplate, "Default", "Default");
      if (map == null)
        return;
      for (int index = 0; index < this._uiElementsArray.Length; ++index)
      {
        ControllerUIElement element1 = this._uiElementsArray[index].element;
        int id = this._uiElementsArray[index].id;
        IControllerTemplateElement element2 = ((IControllerTemplate) template).GetElement(id);
        this.DrawLabel(element1, action, map, (IControllerTemplate) template, element2);
      }
    }

    private void DrawLabel(
      ControllerUIElement uiElement,
      InputAction action,
      ControllerMap controllerMap,
      IControllerTemplate template,
      IControllerTemplateElement element)
    {
      if (element.get_source() == null)
        return;
      if (element.get_source().get_type() == null)
      {
        IControllerTemplateAxisSource source = element.get_source() as IControllerTemplateAxisSource;
        if (source.get_splitAxis())
        {
          ActionElementMap withElementTarget1 = controllerMap.GetFirstElementMapWithElementTarget(source.get_positiveTarget(), action.get_id(), true);
          if (withElementTarget1 != null)
            uiElement.SetLabel(withElementTarget1.get_actionDescriptiveName(), (AxisRange) 1);
          ActionElementMap withElementTarget2 = controllerMap.GetFirstElementMapWithElementTarget(source.get_negativeTarget(), action.get_id(), true);
          if (withElementTarget2 == null)
            return;
          uiElement.SetLabel(withElementTarget2.get_actionDescriptiveName(), (AxisRange) 2);
        }
        else
        {
          ActionElementMap withElementTarget1 = controllerMap.GetFirstElementMapWithElementTarget(source.get_fullTarget(), action.get_id(), true);
          if (withElementTarget1 != null)
          {
            uiElement.SetLabel(withElementTarget1.get_actionDescriptiveName(), (AxisRange) 0);
          }
          else
          {
            ControllerMap controllerMap1 = controllerMap;
            ControllerElementTarget controllerElementTarget1 = new ControllerElementTarget(source.get_fullTarget());
            ((ControllerElementTarget) ref controllerElementTarget1).set_axisRange((AxisRange) 1);
            ControllerElementTarget controllerElementTarget2 = controllerElementTarget1;
            int id1 = action.get_id();
            ActionElementMap withElementTarget2 = controllerMap1.GetFirstElementMapWithElementTarget(controllerElementTarget2, id1, true);
            if (withElementTarget2 != null)
              uiElement.SetLabel(withElementTarget2.get_actionDescriptiveName(), (AxisRange) 1);
            ControllerMap controllerMap2 = controllerMap;
            ControllerElementTarget controllerElementTarget3 = new ControllerElementTarget(source.get_fullTarget());
            ((ControllerElementTarget) ref controllerElementTarget3).set_axisRange((AxisRange) 2);
            ControllerElementTarget controllerElementTarget4 = controllerElementTarget3;
            int id2 = action.get_id();
            ActionElementMap withElementTarget3 = controllerMap2.GetFirstElementMapWithElementTarget(controllerElementTarget4, id2, true);
            if (withElementTarget3 == null)
              return;
            uiElement.SetLabel(withElementTarget3.get_actionDescriptiveName(), (AxisRange) 2);
          }
        }
      }
      else
      {
        if (element.get_source().get_type() != 1)
          return;
        IControllerTemplateButtonSource source = element.get_source() as IControllerTemplateButtonSource;
        ActionElementMap withElementTarget = controllerMap.GetFirstElementMapWithElementTarget(source.get_target(), action.get_id(), true);
        if (withElementTarget == null)
          return;
        uiElement.SetLabel(withElementTarget.get_actionDescriptiveName(), (AxisRange) 0);
      }
    }

    private Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.Stick GetStick(
      int elementId)
    {
      for (int index = 0; index < this._sticks.Length; ++index)
      {
        if (this._sticks[index].ContainsElement(elementId))
          return this._sticks[index];
      }
      return (Rewired.Demos.GamepadTemplateUI.GamepadTemplateUI.Stick) null;
    }

    private void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
      this.DrawLabels();
    }

    private void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
    {
      this.DrawLabels();
    }

    private class Stick
    {
      private int _xAxisElementId = -1;
      private int _yAxisElementId = -1;
      private RectTransform _transform;
      private Vector2 _origPosition;

      public Stick(RectTransform transform, int xAxisElementId, int yAxisElementId)
      {
        if (Object.op_Equality((Object) transform, (Object) null))
          return;
        this._transform = transform;
        this._origPosition = this._transform.get_anchoredPosition();
        this._xAxisElementId = xAxisElementId;
        this._yAxisElementId = yAxisElementId;
      }

      public Vector2 position
      {
        get
        {
          return Object.op_Inequality((Object) this._transform, (Object) null) ? Vector2.op_Subtraction(this._transform.get_anchoredPosition(), this._origPosition) : Vector2.get_zero();
        }
        set
        {
          if (Object.op_Equality((Object) this._transform, (Object) null))
            return;
          this._transform.set_anchoredPosition(Vector2.op_Addition(this._origPosition, value));
        }
      }

      public void Reset()
      {
        if (Object.op_Equality((Object) this._transform, (Object) null))
          return;
        this._transform.set_anchoredPosition(this._origPosition);
      }

      public bool ContainsElement(int elementId)
      {
        if (Object.op_Equality((Object) this._transform, (Object) null))
          return false;
        return elementId == this._xAxisElementId || elementId == this._yAxisElementId;
      }

      public void SetAxisPosition(int elementId, float value)
      {
        if (Object.op_Equality((Object) this._transform, (Object) null))
          return;
        Vector2 position = this.position;
        if (elementId == this._xAxisElementId)
          position.x = (__Null) (double) value;
        else if (elementId == this._yAxisElementId)
          position.y = (__Null) (double) value;
        this.position = position;
      }
    }

    private class UIElement
    {
      public int id;
      public ControllerUIElement element;

      public UIElement(int id, ControllerUIElement element)
      {
        this.id = id;
        this.element = element;
      }
    }
  }
}
