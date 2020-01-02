// Decompiled with JetBrains decompiler
// Type: Grid3D
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class Grid3D : MonoBehaviour
{
  public int numberOfLines;
  public float distanceBetweenLines;
  public float moveSpeed;
  public float rotateSpeed;
  public float lineWidth;

  public Grid3D()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.numberOfLines = Mathf.Clamp(this.numberOfLines, 2, 8190);
    List<Vector3> vector3List = new List<Vector3>();
    for (int index = 0; index < this.numberOfLines; ++index)
    {
      vector3List.Add(new Vector3((float) index * this.distanceBetweenLines, 0.0f, 0.0f));
      vector3List.Add(new Vector3((float) index * this.distanceBetweenLines, 0.0f, (float) (this.numberOfLines - 1) * this.distanceBetweenLines));
    }
    for (int index = 0; index < this.numberOfLines; ++index)
    {
      vector3List.Add(new Vector3(0.0f, 0.0f, (float) index * this.distanceBetweenLines));
      vector3List.Add(new Vector3((float) (this.numberOfLines - 1) * this.distanceBetweenLines, 0.0f, (float) index * this.distanceBetweenLines));
    }
    new VectorLine("Grid", vector3List, this.lineWidth).Draw3DAuto();
    Vector3 position = ((Component) this).get_transform().get_position();
    position.x = (__Null) ((double) (this.numberOfLines - 1) * (double) this.distanceBetweenLines / 2.0);
    ((Component) this).get_transform().set_position(position);
  }

  private void Update()
  {
    if (Input.GetKey((KeyCode) 304) || Input.GetKey((KeyCode) 303))
    {
      ((Component) this).get_transform().Rotate(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.get_up(), Input.GetAxis("Horizontal")), Time.get_deltaTime()), this.rotateSpeed));
      ((Component) this).get_transform().Translate(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.op_Multiply(Vector3.get_up(), Input.GetAxis("Vertical")), Time.get_deltaTime()), this.moveSpeed));
    }
    else
      ((Component) this).get_transform().Translate(new Vector3(Input.GetAxis("Horizontal") * Time.get_deltaTime() * this.moveSpeed, 0.0f, Input.GetAxis("Vertical") * Time.get_deltaTime() * this.moveSpeed));
  }

  private void OnGUI()
  {
    GUILayout.Label(" Use arrow keys to move camera. Hold Shift + arrow up/down to move vertically. Hold Shift + arrow left/right to rotate.", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
  }
}
