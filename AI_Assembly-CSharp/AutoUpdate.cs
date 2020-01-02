// Decompiled with JetBrains decompiler
// Type: AutoUpdate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

[RequireComponent(typeof (ImplicitSurfaceMeshCreaterBase))]
public class AutoUpdate : MonoBehaviour
{
  private ImplicitSurfaceMeshCreaterBase _surface;

  public AutoUpdate()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    this._surface = (ImplicitSurfaceMeshCreaterBase) ((Component) this).GetComponent<ImplicitSurfaceMeshCreaterBase>();
  }

  private void Update()
  {
    this._surface.CreateMesh();
  }
}
