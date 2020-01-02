// Decompiled with JetBrains decompiler
// Type: LiquidRotate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class LiquidRotate : MonoBehaviour
{
  private Quaternion TargetRot;
  public float RotateEverySecond;

  public LiquidRotate()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.RandomRot();
    this.InvokeRepeating("RandomRot", 0.0f, this.RotateEverySecond);
  }

  private void Update()
  {
    ((Component) this).get_transform().set_rotation(Quaternion.Lerp(((Component) this).get_transform().get_rotation(), this.TargetRot, Time.get_time() * Time.get_deltaTime()));
  }

  private void RandomRot()
  {
    this.TargetRot = Random.get_rotation();
  }
}
