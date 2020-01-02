// Decompiled with JetBrains decompiler
// Type: ControllerSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Rewired;
using System.Collections.Generic;

public class ControllerSetting
{
  public string controllerName = string.Empty;
  public List<InputSetting> elements = new List<InputSetting>();
  public IList<ControllerMap> maps;
  public ControllerType controllerType;

  public ControllerSetting()
  {
    this.controllerName = string.Empty;
    this.controllerType = (ControllerType) 0;
    this.elements = new List<InputSetting>();
  }

  public ControllerSetting(string _controllerName)
  {
    this.controllerName = string.Copy(_controllerName);
    this.elements = new List<InputSetting>();
  }

  public ControllerSetting(ControllerType _controllerType, int _controllerId)
  {
    this.Setting(_controllerType, _controllerId);
  }

  public ControllerSetting(string _controllerName, ControllerType _controllerType)
  {
    this.controllerName = string.Copy(_controllerName);
    this.controllerType = _controllerType;
    this.elements = new List<InputSetting>();
  }

  public ControllerSetting(
    string _controllerName,
    ControllerType _controllerType,
    IList<ControllerMap> _maps)
  {
    this.controllerName = string.Copy(_controllerName);
    this.controllerType = _controllerType;
    this.elements = new List<InputSetting>();
    this.maps = _maps;
  }

  public ControllerSetting(string _controllerName, List<InputSetting> _elements)
  {
    this.controllerName = string.Copy(_controllerName);
    this.controllerType = (ControllerType) 0;
    this.elements = _elements;
  }

  public ControllerSetting(
    string _controllerName,
    ControllerType _controllerType,
    List<InputSetting> _elements)
  {
    this.controllerName = string.Copy(_controllerName);
    this.controllerType = _controllerType;
    this.elements = _elements;
  }

  public void AddElement(InputSetting _inputSetting)
  {
    this.elements.Add(_inputSetting);
  }

  public void Clear()
  {
    this.controllerName = string.Empty;
    this.elements = new List<InputSetting>();
  }

  public void Setting(ControllerType _controllerType, int _controllerId)
  {
    this.controllerName = string.Copy(((Player.ControllerHelper) ReInput.get_players().GetPlayer(0).controllers).GetController(_controllerType, _controllerId).get_hardwareName());
    this.controllerType = _controllerType;
    this.maps = ((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) ReInput.get_players().GetPlayer(0).controllers).maps).GetMaps(_controllerType, _controllerId);
  }

  public void Setting(int _controllerId)
  {
    this.Setting(this.controllerType, _controllerId);
  }
}
