// Decompiled with JetBrains decompiler
// Type: FlyDemo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class FlyDemo : MonoBehaviour
{
  public float Speed;
  public float Height;
  private Transform t;
  private float time;

  public FlyDemo()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.t = ((Component) this).get_transform();
  }

  private void Update()
  {
    this.time += Time.get_deltaTime();
    this.t.set_localPosition(new Vector3(0.0f, 0.0f, Mathf.Cos(this.time / this.Speed) * this.Height));
  }
}
