// Decompiled with JetBrains decompiler
// Type: SmartTouch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class SmartTouch : MonoBehaviour
{
  public float countTime;
  private Vector2 inPos;
  private Vector2 upPos;
  private float inTimer;
  private float outTimer;
  private int tapCount;
  private bool inFlg;

  public SmartTouch()
  {
    base.\u002Ector();
  }

  public Vector2 InPos
  {
    get
    {
      return this.inPos;
    }
  }

  public Vector2 UpPos
  {
    get
    {
      return this.upPos;
    }
  }

  public float Distance
  {
    get
    {
      if (this.Tapping)
        return 0.0f;
      Vector2 vector2 = Vector2.op_Subtraction(this.upPos, this.inPos);
      return ((Vector2) ref vector2).get_magnitude();
    }
  }

  public float TapTime
  {
    get
    {
      return this.inTimer;
    }
  }

  public bool Tapping
  {
    get
    {
      return this.inFlg;
    }
  }

  public int TapCount
  {
    get
    {
      return this.tapCount;
    }
  }

  private void Start()
  {
    this.inPos = Vector2.get_zero();
    this.upPos = Vector2.get_zero();
    this.inTimer = 0.0f;
    this.outTimer = 0.0f;
    this.tapCount = 0;
    this.inFlg = false;
  }

  private void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      this.inPos = Vector2.op_Implicit(Input.get_mousePosition());
      this.inFlg = true;
      this.inTimer = 0.0f;
      if ((double) this.outTimer < (double) this.countTime)
        ++this.tapCount;
      else
        this.tapCount = 1;
    }
    else if (Input.GetMouseButtonUp(0))
    {
      this.upPos = Vector2.op_Implicit(Input.get_mousePosition());
      this.inFlg = false;
    }
    if (this.inFlg)
    {
      this.inTimer += Time.get_deltaTime();
      this.outTimer = 0.0f;
    }
    else
    {
      this.outTimer += Time.get_deltaTime();
      this.outTimer = Mathf.Min(this.outTimer, this.countTime);
      if ((double) this.outTimer != (double) this.countTime)
        return;
      this.tapCount = 0;
    }
  }
}
