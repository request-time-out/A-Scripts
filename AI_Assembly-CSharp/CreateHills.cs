// Decompiled with JetBrains decompiler
// Type: CreateHills
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class CreateHills : MonoBehaviour
{
  public Texture hillTexture;
  public PhysicsMaterial2D hillPhysicsMaterial;
  public int numberOfPoints;
  public int numberOfHills;
  public GameObject ball;
  private Vector3 storedPosition;
  private VectorLine hills;
  private Vector2[] splinePoints;

  public CreateHills()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.storedPosition = this.ball.get_transform().get_position();
    this.splinePoints = new Vector2[this.numberOfHills * 2 + 1];
    this.hills = new VectorLine("Hills", new List<Vector2>(this.numberOfPoints), this.hillTexture, 12f, (LineType) 0, (Joins) 1);
    this.hills.set_useViewportCoords(true);
    this.hills.set_collider(true);
    this.hills.set_physicsMaterial(this.hillPhysicsMaterial);
    Random.InitState(95);
    this.CreateHillLine();
  }

  private void OnGUI()
  {
    if (!GUI.Button(new Rect(10f, 10f, 150f, 40f), "Make new hills"))
      return;
    this.CreateHillLine();
    this.ball.get_transform().set_position(this.storedPosition);
    ((Rigidbody2D) this.ball.GetComponent<Rigidbody2D>()).set_velocity(Vector2.get_zero());
    ((Rigidbody2D) this.ball.GetComponent<Rigidbody2D>()).WakeUp();
  }

  private void CreateHillLine()
  {
    this.splinePoints[0] = new Vector2(-0.02f, Random.Range(0.1f, 0.6f));
    float num1 = 0.0f;
    float num2 = 1f / (float) (this.numberOfHills * 2);
    int index;
    for (index = 1; index < this.splinePoints.Length; index += 2)
    {
      float num3 = num1 + num2;
      this.splinePoints[index] = new Vector2(num3, Random.Range(0.3f, 0.7f));
      num1 = num3 + num2;
      this.splinePoints[index + 1] = new Vector2(num1, Random.Range(0.1f, 0.6f));
    }
    this.splinePoints[index - 1] = new Vector2(1.02f, Random.Range(0.1f, 0.6f));
    this.hills.MakeSpline(this.splinePoints);
    this.hills.Draw();
  }
}
