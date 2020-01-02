// Decompiled with JetBrains decompiler
// Type: DrawPath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Vectrosity;

public class DrawPath : MonoBehaviour
{
  public Texture lineTex;
  public Color lineColor;
  public int maxPoints;
  public bool continuousUpdate;
  public GameObject ballPrefab;
  public float force;
  private VectorLine pathLine;
  private int pathIndex;
  private GameObject ball;

  public DrawPath()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.pathLine = new VectorLine("Path", new List<Vector3>(), this.lineTex, 12f, (LineType) 0);
    this.pathLine.set_color(Color32.op_Implicit(Color.get_green()));
    this.pathLine.set_textureScale(1f);
    this.MakeBall();
    this.StartCoroutine(this.SamplePoints(this.ball.get_transform()));
  }

  private void MakeBall()
  {
    if (Object.op_Implicit((Object) this.ball))
      Object.Destroy((Object) this.ball);
    this.ball = (GameObject) Object.Instantiate<GameObject>((M0) this.ballPrefab, new Vector3(-2.25f, -4.4f, -1.9f), Quaternion.Euler(300f, 70f, 310f));
    ((Rigidbody) this.ball.GetComponent<Rigidbody>()).set_useGravity(true);
    ((Rigidbody) this.ball.GetComponent<Rigidbody>()).AddForce(Vector3.op_Multiply(this.ball.get_transform().get_forward(), this.force), (ForceMode) 1);
  }

  [DebuggerHidden]
  private IEnumerator SamplePoints(Transform thisTransform)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new DrawPath.\u003CSamplePoints\u003Ec__Iterator0()
    {
      thisTransform = thisTransform,
      \u0024this = this
    };
  }

  private void OnGUI()
  {
    if (GUI.Button(new Rect(10f, 10f, 100f, 30f), "Reset"))
      this.Reset();
    if (this.continuousUpdate || !GUI.Button(new Rect(10f, 45f, 100f, 30f), "Draw Path"))
      return;
    this.pathLine.Draw();
  }

  private void Reset()
  {
    this.StopAllCoroutines();
    this.MakeBall();
    this.pathLine.get_points3().Clear();
    this.pathLine.Draw();
    this.pathIndex = 0;
    this.StartCoroutine(this.SamplePoints(this.ball.get_transform()));
  }
}
