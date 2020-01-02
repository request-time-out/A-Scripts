// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.DualShock4SpecialFeaturesExample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Rewired.ControllerExtensions;
using Rewired.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos
{
  [AddComponentMenu("")]
  public class DualShock4SpecialFeaturesExample : MonoBehaviour
  {
    private const int maxTouches = 2;
    public int playerId;
    public Transform touchpadTransform;
    public GameObject lightObject;
    public Transform accelerometerTransform;
    private List<DualShock4SpecialFeaturesExample.Touch> touches;
    private Queue<DualShock4SpecialFeaturesExample.Touch> unusedTouches;
    private bool isFlashing;
    private GUIStyle textStyle;

    public DualShock4SpecialFeaturesExample()
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
      this.InitializeTouchObjects();
    }

    private void Update()
    {
      if (!ReInput.get_isReady())
        return;
      IDualShock4Extension firstDs4 = this.GetFirstDS4(this.player);
      if (firstDs4 != null)
      {
        ((Component) this).get_transform().set_rotation(firstDs4.GetOrientation());
        this.HandleTouchpad(firstDs4);
        this.accelerometerTransform.LookAt(Vector3.op_Addition(this.accelerometerTransform.get_position(), firstDs4.GetAccelerometerValue()));
      }
      if (this.player.GetButtonDown("CycleLight"))
        this.SetRandomLightColor();
      if (this.player.GetButtonDown("ResetOrientation"))
        this.ResetOrientation();
      if (this.player.GetButtonDown("ToggleLightFlash"))
      {
        if (this.isFlashing)
          this.StopLightFlash();
        else
          this.StartLightFlash();
        this.isFlashing = !this.isFlashing;
      }
      if (this.player.GetButtonDown("VibrateLeft"))
        ((IControllerVibrator) firstDs4).SetVibration(0, 1f, 1f);
      if (!this.player.GetButtonDown("VibrateRight"))
        return;
      ((IControllerVibrator) firstDs4).SetVibration(1, 1f, 1f);
    }

    private void OnGUI()
    {
      if (this.textStyle == null)
      {
        this.textStyle = new GUIStyle(GUI.get_skin().get_label());
        this.textStyle.set_fontSize(20);
        this.textStyle.set_wordWrap(true);
      }
      if (this.GetFirstDS4(this.player) == null)
        return;
      GUILayout.BeginArea(new Rect(200f, 100f, (float) Screen.get_width() - 400f, (float) Screen.get_height() - 200f));
      GUILayout.Label("Rotate the Dual Shock 4 to see the model rotate in sync.", this.textStyle, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      GUILayout.Label("Touch the touchpad to see them appear on the model.", this.textStyle, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      ActionElementMap elementMapWithAction1 = ((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) this.player.controllers).maps).GetFirstElementMapWithAction((ControllerType) 2, "ResetOrientation", true);
      if (elementMapWithAction1 != null)
        GUILayout.Label("Press " + elementMapWithAction1.get_elementIdentifierName() + " to reset the orientation. Hold the gamepad facing the screen with sticks pointing up and press the button.", this.textStyle, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      ActionElementMap elementMapWithAction2 = ((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) this.player.controllers).maps).GetFirstElementMapWithAction((ControllerType) 2, "CycleLight", true);
      if (elementMapWithAction2 != null)
        GUILayout.Label("Press " + elementMapWithAction2.get_elementIdentifierName() + " to change the light color.", this.textStyle, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      ActionElementMap elementMapWithAction3 = ((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) this.player.controllers).maps).GetFirstElementMapWithAction((ControllerType) 2, "ToggleLightFlash", true);
      if (elementMapWithAction3 != null)
        GUILayout.Label("Press " + elementMapWithAction3.get_elementIdentifierName() + " to start or stop the light flashing.", this.textStyle, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      ActionElementMap elementMapWithAction4 = ((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) this.player.controllers).maps).GetFirstElementMapWithAction((ControllerType) 2, "VibrateLeft", true);
      if (elementMapWithAction4 != null)
        GUILayout.Label("Press " + elementMapWithAction4.get_elementIdentifierName() + " vibrate the left motor.", this.textStyle, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      ActionElementMap elementMapWithAction5 = ((Player.ControllerHelper.MapHelper) ((Player.ControllerHelper) this.player.controllers).maps).GetFirstElementMapWithAction((ControllerType) 2, "VibrateRight", true);
      if (elementMapWithAction5 != null)
        GUILayout.Label("Press " + elementMapWithAction5.get_elementIdentifierName() + " vibrate the right motor.", this.textStyle, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      GUILayout.EndArea();
    }

    private void ResetOrientation()
    {
      this.GetFirstDS4(this.player)?.ResetOrientation();
    }

    private void SetRandomLightColor()
    {
      IDualShock4Extension firstDs4 = this.GetFirstDS4(this.player);
      if (firstDs4 == null)
        return;
      Color color;
      ((Color) ref color).\u002Ector(Random.Range(0.0f, 1f), Random.Range(0.0f, 1f), Random.Range(0.0f, 1f), 1f);
      firstDs4.SetLightColor(color);
      ((Renderer) this.lightObject.GetComponent<MeshRenderer>()).get_material().set_color(color);
    }

    private void StartLightFlash()
    {
      if (!(this.GetFirstDS4(this.player) is DualShock4Extension firstDs4))
        return;
      firstDs4.SetLightFlash(0.5f, 0.5f);
    }

    private void StopLightFlash()
    {
      if (!(this.GetFirstDS4(this.player) is DualShock4Extension firstDs4))
        return;
      firstDs4.StopLightFlash();
    }

    private IDualShock4Extension GetFirstDS4(Player player)
    {
      using (IEnumerator<Joystick> enumerator = ((IEnumerable<Joystick>) ((Player.ControllerHelper) player.controllers).get_Joysticks()).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          IDualShock4Extension extension = (IDualShock4Extension) ((Controller) enumerator.Current).GetExtension<IDualShock4Extension>();
          if (extension != null)
            return extension;
        }
      }
      return (IDualShock4Extension) null;
    }

    private void InitializeTouchObjects()
    {
      this.touches = new List<DualShock4SpecialFeaturesExample.Touch>(2);
      this.unusedTouches = new Queue<DualShock4SpecialFeaturesExample.Touch>(2);
      for (int index = 0; index < 2; ++index)
      {
        DualShock4SpecialFeaturesExample.Touch touch = new DualShock4SpecialFeaturesExample.Touch();
        touch.go = GameObject.CreatePrimitive((PrimitiveType) 0);
        touch.go.get_transform().set_localScale(new Vector3(0.1f, 0.1f, 0.1f));
        touch.go.get_transform().SetParent(this.touchpadTransform, true);
        ((Renderer) touch.go.GetComponent<MeshRenderer>()).get_material().set_color(index != 0 ? Color.get_green() : Color.get_red());
        touch.go.SetActive(false);
        this.unusedTouches.Enqueue(touch);
      }
    }

    private void HandleTouchpad(IDualShock4Extension ds4)
    {
      for (int index = this.touches.Count - 1; index >= 0; --index)
      {
        DualShock4SpecialFeaturesExample.Touch touch = this.touches[index];
        if (!ds4.IsTouchingByTouchId(touch.touchId))
        {
          touch.go.SetActive(false);
          this.unusedTouches.Enqueue(touch);
          this.touches.RemoveAt(index);
        }
      }
      for (int index = 0; index < ds4.get_maxTouches(); ++index)
      {
        if (ds4.IsTouching(index))
        {
          int touchId = ds4.GetTouchId(index);
          DualShock4SpecialFeaturesExample.Touch touch = this.touches.Find((Predicate<DualShock4SpecialFeaturesExample.Touch>) (x => x.touchId == touchId));
          if (touch == null)
          {
            touch = this.unusedTouches.Dequeue();
            this.touches.Add(touch);
          }
          touch.touchId = touchId;
          touch.go.SetActive(true);
          Vector2 vector2;
          ds4.GetTouchPosition(index, ref vector2);
          touch.go.get_transform().set_localPosition(new Vector3((float) (vector2.x - 0.5), (float) (0.5 + touch.go.get_transform().get_localScale().y * 0.5), (float) (vector2.y - 0.5)));
        }
      }
    }

    private class Touch
    {
      public int touchId = -1;
      public GameObject go;
    }
  }
}
