// Decompiled with JetBrains decompiler
// Type: TweenFragment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class TweenFragment : MonoBehaviour
{
  public Transform TargetPos;
  public float LerpTime;
  private Vector3 initPos;
  private float time;

  public TweenFragment()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.initPos = ((Component) this).get_transform().get_position();
    this.time = 0.0f;
  }

  private void Update()
  {
    this.time += (float) ((double) Time.get_deltaTime() * (double) Random.get_value() * 2.0);
    ((Component) this).get_transform().set_position(Vector3.Lerp(this.initPos, this.TargetPos.get_position(), this.time / this.LerpTime));
  }
}
