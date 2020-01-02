// Decompiled with JetBrains decompiler
// Type: MakeSpheres
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class MakeSpheres : MonoBehaviour
{
  public GameObject spherePrefab;
  public int numberOfSpheres;
  public float area;

  public MakeSpheres()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    for (int index = 0; index < this.numberOfSpheres; ++index)
      Object.Instantiate<GameObject>((M0) this.spherePrefab, new Vector3(Random.Range(-this.area, this.area), Random.Range(-this.area, this.area), Random.Range(-this.area, this.area)), Random.get_rotation());
  }
}
