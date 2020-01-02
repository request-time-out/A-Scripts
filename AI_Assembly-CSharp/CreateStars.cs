// Decompiled with JetBrains decompiler
// Type: CreateStars
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class CreateStars : MonoBehaviour
{
  public int numberOfStars;
  private VectorLine stars;

  public CreateStars()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    Vector3[] vector3Array = new Vector3[this.numberOfStars];
    for (int index = 0; index < this.numberOfStars; ++index)
      vector3Array[index] = Vector3.op_Multiply(Random.get_onUnitSphere(), 100f);
    float[] numArray = new float[this.numberOfStars];
    for (int index = 0; index < this.numberOfStars; ++index)
      numArray[index] = Random.Range(1.5f, 2.5f);
    Color32[] color32Array = new Color32[this.numberOfStars];
    for (int index = 0; index < this.numberOfStars; ++index)
    {
      float num = (float) ((double) Random.get_value() * 0.75 + 0.25);
      color32Array[index] = Color32.op_Implicit(new Color(num, num, num));
    }
    this.stars = new VectorLine("Stars", new List<Vector3>((IEnumerable<Vector3>) vector3Array), 1f, (LineType) 2);
    this.stars.SetColors(new List<Color32>((IEnumerable<Color32>) color32Array));
    this.stars.SetWidths(new List<float>((IEnumerable<float>) numArray));
    this.stars.Draw();
    VectorLine.SetCanvasCamera(Camera.get_main());
    VectorLine.get_canvas().set_planeDistance(Camera.get_main().get_farClipPlane() - 1f);
  }

  private void LateUpdate()
  {
    this.stars.Draw();
  }
}
