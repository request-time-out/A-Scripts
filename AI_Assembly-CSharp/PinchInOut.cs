// Decompiled with JetBrains decompiler
// Type: PinchInOut
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class PinchInOut : MonoBehaviour
{
  public float moveSpeed;
  private PinchInOut.State nowState;
  private float scaleRate;
  private float prevDist;

  public PinchInOut()
  {
    base.\u002Ector();
  }

  public PinchInOut.State NowState
  {
    get
    {
      return this.nowState;
    }
  }

  public float Rate
  {
    get
    {
      return this.scaleRate;
    }
  }

  private void Start()
  {
    this.nowState = PinchInOut.State.NONE;
    this.scaleRate = 0.0f;
    this.prevDist = 0.0f;
  }

  private void Update()
  {
    if (Input.get_touchCount() != 2)
    {
      this.nowState = PinchInOut.State.NONE;
    }
    else
    {
      Touch touch1 = Input.GetTouch(1);
      TouchPhase phase = ((Touch) ref touch1).get_phase();
      if (phase == null)
        return;
      if (phase != 1)
      {
        if (phase != 2)
          return;
        this.scaleRate = 0.0f;
        this.nowState = PinchInOut.State.NONE;
      }
      else
      {
        Touch touch2 = Input.GetTouch(1);
        Vector2 position1 = ((Touch) ref touch2).get_position();
        Touch touch3 = Input.GetTouch(0);
        Vector2 position2 = ((Touch) ref touch3).get_position();
        Vector2 vector2 = Vector2.op_Subtraction(position1, position2);
        float magnitude1 = ((Vector2) ref vector2).get_magnitude();
        Touch touch4 = Input.GetTouch(1);
        Vector2 deltaPosition1 = ((Touch) ref touch4).get_deltaPosition();
        double magnitude2 = (double) ((Vector2) ref deltaPosition1).get_magnitude();
        Touch touch5 = Input.GetTouch(0);
        Vector2 deltaPosition2 = ((Touch) ref touch5).get_deltaPosition();
        double magnitude3 = (double) ((Vector2) ref deltaPosition2).get_magnitude();
        float num = (float) (magnitude2 + magnitude3);
        this.nowState = (double) this.prevDist <= (double) magnitude1 ? ((double) this.prevDist >= (double) magnitude1 ? PinchInOut.State.NONE : PinchInOut.State.ScalUp) : PinchInOut.State.ScalDown;
        this.scaleRate = num * this.moveSpeed;
        this.prevDist = magnitude1;
      }
    }
  }

  public enum State
  {
    NONE,
    ScalUp,
    ScalDown,
  }
}
