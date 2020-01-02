// Decompiled with JetBrains decompiler
// Type: AQUAS_SmallBubbleBehaviour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class AQUAS_SmallBubbleBehaviour : MonoBehaviour
{
  public float averageUpdrift;
  public float waterLevel;
  public GameObject mainCamera;
  private float updriftFactor;

  public AQUAS_SmallBubbleBehaviour()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.updriftFactor = Random.Range((float) (-(double) this.averageUpdrift * 0.75), this.averageUpdrift * 0.75f);
  }

  private void Update()
  {
    ((Component) this).get_transform().Translate(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.get_up(), Time.get_deltaTime()), this.averageUpdrift + this.updriftFactor), (Space) 0);
    if (this.mainCamera.get_transform().get_position().y <= (double) this.waterLevel && ((Component) this).get_transform().get_position().y <= (double) this.waterLevel)
      return;
    Object.Destroy((Object) ((Component) this).get_gameObject());
  }
}
