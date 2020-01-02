// Decompiled with JetBrains decompiler
// Type: AQUAS_BubbleBehaviour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class AQUAS_BubbleBehaviour : MonoBehaviour
{
  public float averageUpdrift;
  public float waterLevel;
  public GameObject mainCamera;
  public GameObject smallBubble;
  private int smallBubbleCount;
  private int maxSmallBubbleCount;
  private AQUAS_SmallBubbleBehaviour smallBubbleBehaviour;

  public AQUAS_BubbleBehaviour()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.maxSmallBubbleCount = Random.Range(20, 30);
    this.smallBubbleCount = 0;
    this.smallBubbleBehaviour = (AQUAS_SmallBubbleBehaviour) this.smallBubble.GetComponent<AQUAS_SmallBubbleBehaviour>();
  }

  private void Update()
  {
    ((Component) this).get_transform().Translate(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.get_up(), Time.get_deltaTime()), this.averageUpdrift), (Space) 0);
    this.SmallBubbleSpawner();
    if (this.mainCamera.get_transform().get_position().y <= (double) this.waterLevel && ((Component) this).get_transform().get_position().y <= (double) this.waterLevel)
      return;
    Object.Destroy((Object) ((Component) this).get_gameObject());
  }

  private void SmallBubbleSpawner()
  {
    if (this.smallBubbleCount > this.maxSmallBubbleCount)
      return;
    this.smallBubble.get_transform().set_localScale(Vector3.op_Multiply(((Component) this).get_transform().get_localScale(), Random.Range(0.05f, 0.2f)));
    this.smallBubbleBehaviour.averageUpdrift = this.averageUpdrift * 0.5f;
    this.smallBubbleBehaviour.waterLevel = this.waterLevel;
    this.smallBubbleBehaviour.mainCamera = this.mainCamera;
    Object.Instantiate<GameObject>((M0) this.smallBubble, new Vector3((float) ((Component) this).get_transform().get_position().x + Random.Range(-0.1f, 0.1f), (float) ((Component) this).get_transform().get_position().y - Random.Range(0.01f, 1f), (float) ((Component) this).get_transform().get_position().z + Random.Range(-0.1f, 0.1f)), Quaternion.get_identity());
    ++this.smallBubbleCount;
  }
}
