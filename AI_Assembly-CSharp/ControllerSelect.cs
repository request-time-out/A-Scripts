// Decompiled with JetBrains decompiler
// Type: ControllerSelect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Rewired;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerSelect : MonoBehaviour
{
  public Dropdown dropdown;
  public Remapping remapping;
  private List<ControllerSelect.ControllerState> stateList;

  public ControllerSelect()
  {
    base.\u002Ector();
  }

  public void SetController(IList<Controller> _controllers, string _controllerName)
  {
    this.stateList.Clear();
    this.dropdown.get_options().Clear();
    this.dropdown.get_captionText().set_text(string.Empty);
    int num = 0;
    ((Selectable) this.dropdown).set_interactable(((ICollection<Controller>) _controllers).Count > 0);
    for (int index = 0; index < ((ICollection<Controller>) _controllers).Count; ++index)
    {
      Controller controller = _controllers[index];
      string name = controller.get_name();
      this.dropdown.get_options().Add(new Dropdown.OptionData(name));
      if (name == _controllerName)
      {
        this.dropdown.get_captionText().set_text(name);
        this.dropdown.set_value(index);
      }
      this.stateList.Add(new ControllerSelect.ControllerState((int) controller.get_type(), controller.get_type() == 2 ? num : 0));
      if (controller.get_type() == 2)
        ++num;
    }
  }

  public void ChangeController(Dropdown _dropdown)
  {
    if (_dropdown.get_value() < 0 || this.stateList.Count <= _dropdown.get_value())
      Debug.LogError((object) "Dropdownのvalueが範囲外です");
    else
      this.remapping.SetSelectedController(this.stateList[_dropdown.get_value()].type, this.stateList[_dropdown.get_value()].joystickIndexId);
  }

  private struct ControllerState
  {
    public int type;
    public int joystickIndexId;

    public ControllerState(int _type, int _joystickIndexId)
    {
      this.type = _type;
      this.joystickIndexId = _joystickIndexId;
    }
  }
}
