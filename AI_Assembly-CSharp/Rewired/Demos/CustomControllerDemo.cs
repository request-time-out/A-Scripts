// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.CustomControllerDemo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace Rewired.Demos
{
  [AddComponentMenu("")]
  public class CustomControllerDemo : MonoBehaviour
  {
    public int playerId;
    public string controllerTag;
    public bool useUpdateCallbacks;
    private int buttonCount;
    private int axisCount;
    private float[] axisValues;
    private bool[] buttonValues;
    private TouchJoystickExample[] joysticks;
    private TouchButtonExample[] buttons;
    private CustomController controller;
    [NonSerialized]
    private bool initialized;

    public CustomControllerDemo()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      if (SystemInfo.get_deviceType() == 1 && Screen.get_orientation() != 3)
        Screen.set_orientation((ScreenOrientation) 3);
      this.Initialize();
    }

    private void Initialize()
    {
      ReInput.add_InputSourceUpdateEvent(new Action(this.OnInputSourceUpdate));
      this.joysticks = (TouchJoystickExample[]) ((Component) this).GetComponentsInChildren<TouchJoystickExample>();
      this.buttons = (TouchButtonExample[]) ((Component) this).GetComponentsInChildren<TouchButtonExample>();
      this.axisCount = this.joysticks.Length * 2;
      this.buttonCount = this.buttons.Length;
      this.axisValues = new float[this.axisCount];
      this.buttonValues = new bool[this.buttonCount];
      this.controller = (CustomController) ((Player.ControllerHelper) ReInput.get_players().GetPlayer(this.playerId).controllers).GetControllerWithTag<CustomController>(this.controllerTag);
      if (this.controller == null)
        Debug.LogError((object) ("A matching controller was not found for tag \"" + this.controllerTag + "\""));
      if (((Controller) this.controller).get_buttonCount() != this.buttonValues.Length || ((ControllerWithAxes) this.controller).get_axisCount() != this.axisValues.Length)
        Debug.LogError((object) "Controller has wrong number of elements!");
      if (this.useUpdateCallbacks && this.controller != null)
      {
        this.controller.SetAxisUpdateCallback(new Func<int, float>(this.GetAxisValueCallback));
        this.controller.SetButtonUpdateCallback(new Func<int, bool>(this.GetButtonValueCallback));
      }
      this.initialized = true;
    }

    private void Update()
    {
      if (!ReInput.get_isReady() || this.initialized)
        return;
      this.Initialize();
    }

    private void OnInputSourceUpdate()
    {
      this.GetSourceAxisValues();
      this.GetSourceButtonValues();
      if (this.useUpdateCallbacks)
        return;
      this.SetControllerAxisValues();
      this.SetControllerButtonValues();
    }

    private void GetSourceAxisValues()
    {
      for (int index = 0; index < this.axisValues.Length; ++index)
        this.axisValues[index] = index % 2 == 0 ? (float) this.joysticks[index / 2].position.x : (float) this.joysticks[index / 2].position.y;
    }

    private void GetSourceButtonValues()
    {
      for (int index = 0; index < this.buttonValues.Length; ++index)
        this.buttonValues[index] = this.buttons[index].isPressed;
    }

    private void SetControllerAxisValues()
    {
      for (int index = 0; index < this.axisValues.Length; ++index)
        this.controller.SetAxisValue(index, this.axisValues[index]);
    }

    private void SetControllerButtonValues()
    {
      for (int index = 0; index < this.buttonValues.Length; ++index)
        this.controller.SetButtonValue(index, this.buttonValues[index]);
    }

    private float GetAxisValueCallback(int index)
    {
      return index >= this.axisValues.Length ? 0.0f : this.axisValues[index];
    }

    private bool GetButtonValueCallback(int index)
    {
      return index < this.buttonValues.Length && this.buttonValues[index];
    }
  }
}
