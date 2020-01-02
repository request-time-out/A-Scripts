// Decompiled with JetBrains decompiler
// Type: RadialFillCursor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

public class RadialFillCursor : MonoBehaviour
{
  [Header("Radial Data")]
  public float radius;
  public float strenght;
  public float strenghtMax;
  public float angle;
  public float rotationAngle;
  [Header("Input Speed")]
  public float radiusSpeed;
  public float strenghtSpeed;
  public float rotationSpeed;
  public float angleSpeed;
  [Header("Mesh Data")]
  public int meshAngleSeparation;
  public MeshFilter meshFilter;
  public Color centerColor;
  public Color externalColor;
  private Vector3[] vertices;
  private Color[] colors;
  private int[] triangles;
  private Vector3 tempV3;

  public RadialFillCursor()
  {
    base.\u002Ector();
  }

  private void Update()
  {
    Vector3 mousePosition = Input.get_mousePosition();
    mousePosition.z = (__Null) 10.0;
    ((Component) this).get_transform().set_position(Camera.get_main().ScreenToWorldPoint(mousePosition));
    if (Input.GetKey((KeyCode) 119))
      this.radius += Time.get_deltaTime() * this.radiusSpeed;
    else if (Input.GetKey((KeyCode) 115))
      this.radius -= Time.get_deltaTime() * this.radiusSpeed;
    if ((double) this.radius < 0.0)
      this.radius = 0.0f;
    if (Input.GetKey((KeyCode) 97))
      this.rotationAngle += Time.get_deltaTime() * this.rotationSpeed;
    else if (Input.GetKey((KeyCode) 100))
      this.rotationAngle -= Time.get_deltaTime() * this.rotationSpeed;
    if (Input.GetKey((KeyCode) 113))
      this.angle += Time.get_deltaTime() * this.angleSpeed;
    else if (Input.GetKey((KeyCode) 101))
      this.angle -= Time.get_deltaTime() * this.angleSpeed;
    this.angle = Mathf.Clamp(this.angle, 0.0f, 360f);
    if (Input.GetKey((KeyCode) 120))
      this.strenght += Time.get_deltaTime() * this.strenghtSpeed;
    else if (Input.GetKey((KeyCode) 122))
      this.strenght -= Time.get_deltaTime() * this.strenghtSpeed;
    this.strenght = Mathf.Clamp(this.strenght, 0.1f, this.strenghtMax);
    Color centerColor = this.centerColor;
    centerColor.a = (__Null) (0.340000003576279 + (double) this.strenght / (double) this.strenghtMax * 0.660000026226044);
    this.centerColor = centerColor;
    if (this.vertices == null || this.vertices.Length != this.meshAngleSeparation + 1)
    {
      this.vertices = new Vector3[this.meshAngleSeparation + 2];
      this.colors = new Color[this.meshAngleSeparation + 2];
      this.triangles = new int[this.meshAngleSeparation * 3 + 3];
    }
    Mesh mesh = this.meshFilter.get_mesh();
    if (Object.op_Equality((Object) mesh, (Object) null))
    {
      mesh = new Mesh();
      this.meshFilter.set_mesh(mesh);
    }
    this.vertices[0] = Vector3.get_zero();
    this.colors[0] = this.centerColor;
    for (int index = 1; index < this.meshAngleSeparation + 2; ++index)
    {
      float num1 = this.angle / (float) this.meshAngleSeparation * (float) (index - 1) + this.rotationAngle;
      float num2 = Mathf.Cos((float) Math.PI / 180f * num1);
      float num3 = Mathf.Sin((float) Math.PI / 180f * num1);
      float num4 = this.radius * num2;
      float num5 = this.radius * num3;
      this.tempV3.x = (__Null) (double) num4;
      this.tempV3.y = (__Null) (double) num5;
      this.tempV3.z = (__Null) 0.0;
      this.vertices[index] = this.tempV3;
      this.colors[index] = this.externalColor;
    }
    int num = 0;
    for (int index = 0; index < this.meshAngleSeparation * 3; index += 3)
    {
      this.triangles[index] = 0;
      this.triangles[index + 1] = num + 2;
      this.triangles[index + 2] = num + 1;
      ++num;
    }
    mesh.Clear();
    mesh.set_vertices(this.vertices);
    mesh.set_triangles(this.triangles);
    mesh.set_colors(this.colors);
  }

  public void Show(bool b)
  {
    ((Component) this.meshFilter).get_gameObject().SetActive(b);
  }
}
