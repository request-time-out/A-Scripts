// Decompiled with JetBrains decompiler
// Type: InputSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Rewired;
using UnityEngine;

public class InputSetting
{
  public InputSetting(
    int _categoryId,
    int _layoutId,
    ControllerType _controllerType,
    ControllerElementType _elementType,
    int _elementIdentifierId,
    AxisRange _axisRange,
    KeyCode _keyboardKey,
    ModifierKeyFlags _modifierKeyFlags,
    int _actionId,
    Pole _axisContribution,
    bool _invert,
    int _elementMapId)
  {
    this.categoryId = _categoryId;
    this.layoutId = _layoutId;
    this.controllerType = _controllerType;
    this.elementType = _elementType;
    this.elementIdentifierId = _elementIdentifierId;
    this.axisRange = _axisRange;
    this.keyboardKey = _keyboardKey;
    this.modifierKeyFlags = _modifierKeyFlags;
    this.actionId = _actionId;
    this.axisContribution = _axisContribution;
    this.invert = _invert;
    this.elementMapId = _elementMapId;
  }

  public InputSetting(
    ControllerType _controllerType,
    ControllerElementType _elementType,
    int _elementIdentifierId,
    AxisRange _axisRange,
    KeyCode _keyboardKey,
    ModifierKeyFlags _modifierKeyFlags,
    int _actionId,
    Pole _axisContribution,
    bool _invert,
    int _elementMapId)
  {
    int num = 0;
    this.layoutId = num;
    this.categoryId = num;
    this.controllerType = _controllerType;
    this.elementType = _elementType;
    this.elementIdentifierId = _elementIdentifierId;
    this.axisRange = _axisRange;
    this.keyboardKey = _keyboardKey;
    this.modifierKeyFlags = _modifierKeyFlags;
    this.actionId = _actionId;
    this.axisContribution = _axisContribution;
    this.invert = _invert;
    this.elementMapId = _elementMapId;
  }

  public InputSetting(ActionElementMap _actionElementMap)
  {
    this.categoryId = _actionElementMap.get_controllerMap().get_categoryId();
    this.layoutId = _actionElementMap.get_controllerMap().get_layoutId();
    this.controllerType = _actionElementMap.get_controllerMap().get_controllerType();
    this.elementType = _actionElementMap.get_elementType();
    this.elementIdentifierId = _actionElementMap.get_elementIdentifierId();
    this.axisRange = _actionElementMap.get_axisRange();
    this.keyboardKey = _actionElementMap.get_keyCode();
    this.modifierKeyFlags = _actionElementMap.get_modifierKeyFlags();
    this.actionId = _actionElementMap.get_actionId();
    this.axisContribution = _actionElementMap.get_axisContribution();
    this.invert = _actionElementMap.get_invert();
    this.elementMapId = _actionElementMap.get_id();
  }

  public int categoryId { get; private set; }

  public int layoutId { get; private set; }

  public ControllerType controllerType { get; private set; }

  public ControllerElementType elementType { get; private set; }

  public int elementIdentifierId { get; private set; }

  public AxisRange axisRange { get; private set; }

  public KeyCode keyboardKey { get; private set; }

  public ModifierKeyFlags modifierKeyFlags { get; private set; }

  public int actionId { get; private set; }

  public Pole axisContribution { get; private set; }

  public bool invert { get; private set; }

  public int elementMapId { get; private set; }

  public ElementAssignment ToElementAssignment()
  {
    return new ElementAssignment(this.controllerType, this.elementType, this.elementIdentifierId, this.axisRange, this.keyboardKey, this.modifierKeyFlags, this.actionId, this.axisContribution, this.invert, this.elementMapId);
  }

  public ElementAssignment ToElementAssignmentCreateSetting()
  {
    return new ElementAssignment(this.controllerType, this.elementType, this.elementIdentifierId, this.axisRange, this.keyboardKey, this.modifierKeyFlags, this.actionId, this.axisContribution, this.invert, -1);
  }
}
