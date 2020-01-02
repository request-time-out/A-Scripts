// Decompiled with JetBrains decompiler
// Type: LazyLoad
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class LazyLoad : MonoBehaviour
{
  public GameObject GO;
  public float TimeDelay;

  public LazyLoad()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    this.GO.SetActive(false);
  }

  private void LazyEnable()
  {
    this.GO.SetActive(true);
  }

  private void OnEnable()
  {
    this.Invoke("LazyEnable", this.TimeDelay);
  }

  private void OnDisable()
  {
    this.CancelInvoke("LazyEnable");
    this.GO.SetActive(false);
  }
}
