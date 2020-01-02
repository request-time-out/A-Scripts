// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.TouchControls1_ManipulateCube
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace Rewired.Demos
{
  [AddComponentMenu("")]
  public class TouchControls1_ManipulateCube : MonoBehaviour
  {
    public float rotateSpeed;
    public float moveSpeed;
    private int currentColorIndex;
    private Color[] colors;

    public TouchControls1_ManipulateCube()
    {
      base.\u002Ector();
    }

    private void OnEnable()
    {
      if (!ReInput.get_isReady())
        return;
      Player player = ReInput.get_players().GetPlayer(0);
      if (player == null)
        return;
      player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnMoveReceivedX), (UpdateLoopType) 0, (InputActionEventType) 33, "Horizontal");
      player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnMoveReceivedX), (UpdateLoopType) 0, (InputActionEventType) 34, "Horizontal");
      player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnMoveReceivedY), (UpdateLoopType) 0, (InputActionEventType) 33, "Vertical");
      player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnMoveReceivedY), (UpdateLoopType) 0, (InputActionEventType) 34, "Vertical");
      player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnCycleColor), (UpdateLoopType) 0, (InputActionEventType) 3, "CycleColor");
      player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnCycleColorReverse), (UpdateLoopType) 0, (InputActionEventType) 3, "CycleColorReverse");
      player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnRotationReceivedX), (UpdateLoopType) 0, (InputActionEventType) 33, "RotateHorizontal");
      player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnRotationReceivedX), (UpdateLoopType) 0, (InputActionEventType) 34, "RotateHorizontal");
      player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnRotationReceivedY), (UpdateLoopType) 0, (InputActionEventType) 33, "RotateVertical");
      player.AddInputEventDelegate(new Action<InputActionEventData>(this.OnRotationReceivedY), (UpdateLoopType) 0, (InputActionEventType) 34, "RotateVertical");
    }

    private void OnDisable()
    {
      if (!ReInput.get_isReady())
        return;
      Player player = ReInput.get_players().GetPlayer(0);
      if (player == null)
        return;
      player.RemoveInputEventDelegate(new Action<InputActionEventData>(this.OnMoveReceivedX));
      player.RemoveInputEventDelegate(new Action<InputActionEventData>(this.OnMoveReceivedY));
      player.RemoveInputEventDelegate(new Action<InputActionEventData>(this.OnCycleColor));
      player.RemoveInputEventDelegate(new Action<InputActionEventData>(this.OnCycleColorReverse));
      player.RemoveInputEventDelegate(new Action<InputActionEventData>(this.OnRotationReceivedX));
      player.RemoveInputEventDelegate(new Action<InputActionEventData>(this.OnRotationReceivedY));
    }

    private void OnMoveReceivedX(InputActionEventData data)
    {
      this.OnMoveReceived(new Vector2(((InputActionEventData) ref data).GetAxis(), 0.0f));
    }

    private void OnMoveReceivedY(InputActionEventData data)
    {
      this.OnMoveReceived(new Vector2(0.0f, ((InputActionEventData) ref data).GetAxis()));
    }

    private void OnRotationReceivedX(InputActionEventData data)
    {
      this.OnRotationReceived(new Vector2(((InputActionEventData) ref data).GetAxis(), 0.0f));
    }

    private void OnRotationReceivedY(InputActionEventData data)
    {
      this.OnRotationReceived(new Vector2(0.0f, ((InputActionEventData) ref data).GetAxis()));
    }

    private void OnCycleColor(InputActionEventData data)
    {
      this.OnCycleColor();
    }

    private void OnCycleColorReverse(InputActionEventData data)
    {
      this.OnCycleColorReverse();
    }

    private void OnMoveReceived(Vector2 move)
    {
      ((Component) this).get_transform().Translate(Vector3.op_Multiply(Vector3.op_Multiply(Vector2.op_Implicit(move), Time.get_deltaTime()), this.moveSpeed), (Space) 0);
    }

    private void OnRotationReceived(Vector2 rotate)
    {
      rotate = Vector2.op_Multiply(rotate, this.rotateSpeed);
      ((Component) this).get_transform().Rotate(Vector3.get_up(), (float) -rotate.x, (Space) 0);
      ((Component) this).get_transform().Rotate(Vector3.get_right(), (float) rotate.y, (Space) 0);
    }

    private void OnCycleColor()
    {
      if (this.colors.Length == 0)
        return;
      ++this.currentColorIndex;
      if (this.currentColorIndex >= this.colors.Length)
        this.currentColorIndex = 0;
      ((Renderer) ((Component) this).GetComponent<Renderer>()).get_material().set_color(this.colors[this.currentColorIndex]);
    }

    private void OnCycleColorReverse()
    {
      if (this.colors.Length == 0)
        return;
      --this.currentColorIndex;
      if (this.currentColorIndex < 0)
        this.currentColorIndex = this.colors.Length - 1;
      ((Renderer) ((Component) this).GetComponent<Renderer>()).get_material().set_color(this.colors[this.currentColorIndex]);
    }
  }
}
