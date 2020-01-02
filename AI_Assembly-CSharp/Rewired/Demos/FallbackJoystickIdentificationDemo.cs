// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.FallbackJoystickIdentificationDemo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos
{
  [AddComponentMenu("")]
  public class FallbackJoystickIdentificationDemo : MonoBehaviour
  {
    private const float windowWidth = 250f;
    private const float windowHeight = 250f;
    private const float inputDelay = 1f;
    private bool identifyRequired;
    private Queue<Joystick> joysticksToIdentify;
    private float nextInputAllowedTime;
    private GUIStyle style;

    public FallbackJoystickIdentificationDemo()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      if (!ReInput.get_unityJoystickIdentificationRequired())
        return;
      ReInput.add_ControllerConnectedEvent(new Action<ControllerStatusChangedEventArgs>(this.JoystickConnected));
      ReInput.add_ControllerDisconnectedEvent(new Action<ControllerStatusChangedEventArgs>(this.JoystickDisconnected));
      this.IdentifyAllJoysticks();
    }

    private void JoystickConnected(ControllerStatusChangedEventArgs args)
    {
      this.IdentifyAllJoysticks();
    }

    private void JoystickDisconnected(ControllerStatusChangedEventArgs args)
    {
      this.IdentifyAllJoysticks();
    }

    public void IdentifyAllJoysticks()
    {
      this.Reset();
      if (ReInput.get_controllers().get_joystickCount() == 0)
        return;
      Joystick[] joysticks = ReInput.get_controllers().GetJoysticks();
      if (joysticks == null)
        return;
      this.identifyRequired = true;
      this.joysticksToIdentify = new Queue<Joystick>((IEnumerable<Joystick>) joysticks);
      this.SetInputDelay();
    }

    private void SetInputDelay()
    {
      this.nextInputAllowedTime = Time.get_time() + 1f;
    }

    private void OnGUI()
    {
      if (!this.identifyRequired)
        return;
      if (this.joysticksToIdentify == null || this.joysticksToIdentify.Count == 0)
      {
        this.Reset();
      }
      else
      {
        Rect rect;
        ((Rect) ref rect).\u002Ector((float) ((double) Screen.get_width() * 0.5 - 125.0), (float) ((double) Screen.get_height() * 0.5 - 125.0), 250f, 250f);
        // ISSUE: method pointer
        GUILayout.Window(0, rect, new GUI.WindowFunction((object) this, __methodptr(DrawDialogWindow)), "Joystick Identification Required", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
        GUI.FocusWindow(0);
        if ((double) Time.get_time() < (double) this.nextInputAllowedTime || !ReInput.get_controllers().SetUnityJoystickIdFromAnyButtonOrAxisPress((int) ((Controller) this.joysticksToIdentify.Peek()).id, 0.8f, false))
          return;
        this.joysticksToIdentify.Dequeue();
        this.SetInputDelay();
        if (this.joysticksToIdentify.Count != 0)
          return;
        this.Reset();
      }
    }

    private void DrawDialogWindow(int windowId)
    {
      if (!this.identifyRequired)
        return;
      if (this.style == null)
      {
        this.style = new GUIStyle(GUI.get_skin().get_label());
        this.style.set_wordWrap(true);
      }
      GUILayout.Space(15f);
      GUILayout.Label("A joystick has been attached or removed. You will need to identify each joystick by pressing a button on the controller listed below:", this.style, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      GUILayout.Label("Press any button on \"" + ((Controller) this.joysticksToIdentify.Peek()).get_name() + "\" now.", this.style, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      GUILayout.FlexibleSpace();
      if (!GUILayout.Button("Skip", (GUILayoutOption[]) Array.Empty<GUILayoutOption>()))
        return;
      this.joysticksToIdentify.Dequeue();
    }

    private void Reset()
    {
      this.joysticksToIdentify = (Queue<Joystick>) null;
      this.identifyRequired = false;
    }
  }
}
