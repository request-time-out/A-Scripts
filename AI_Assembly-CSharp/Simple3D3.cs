// Decompiled with JetBrains decompiler
// Type: Simple3D3
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class Simple3D3 : MonoBehaviour
{
  public Simple3D3()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    List<Vector3> vector3List = new List<Vector3>();
    vector3List.Add(new Vector3(-0.5f, -0.5f, 0.5f));
    vector3List.Add(new Vector3(0.5f, -0.5f, 0.5f));
    vector3List.Add(new Vector3(-0.5f, 0.5f, 0.5f));
    vector3List.Add(new Vector3(-0.5f, -0.5f, 0.5f));
    vector3List.Add(new Vector3(0.5f, -0.5f, 0.5f));
    vector3List.Add(new Vector3(0.5f, 0.5f, 0.5f));
    vector3List.Add(new Vector3(0.5f, 0.5f, 0.5f));
    vector3List.Add(new Vector3(-0.5f, 0.5f, 0.5f));
    vector3List.Add(new Vector3(-0.5f, 0.5f, -0.5f));
    vector3List.Add(new Vector3(-0.5f, 0.5f, 0.5f));
    vector3List.Add(new Vector3(0.5f, 0.5f, 0.5f));
    vector3List.Add(new Vector3(0.5f, 0.5f, -0.5f));
    vector3List.Add(new Vector3(0.5f, 0.5f, -0.5f));
    vector3List.Add(new Vector3(-0.5f, 0.5f, -0.5f));
    vector3List.Add(new Vector3(-0.5f, -0.5f, -0.5f));
    vector3List.Add(new Vector3(-0.5f, 0.5f, -0.5f));
    vector3List.Add(new Vector3(0.5f, 0.5f, -0.5f));
    vector3List.Add(new Vector3(0.5f, -0.5f, -0.5f));
    vector3List.Add(new Vector3(0.5f, -0.5f, -0.5f));
    vector3List.Add(new Vector3(-0.5f, -0.5f, -0.5f));
    vector3List.Add(new Vector3(-0.5f, -0.5f, 0.5f));
    vector3List.Add(new Vector3(-0.5f, -0.5f, -0.5f));
    vector3List.Add(new Vector3(0.5f, -0.5f, -0.5f));
    vector3List.Add(new Vector3(0.5f, -0.5f, 0.5f));
    VectorManager.ObjectSetup(((Component) this).get_gameObject(), new VectorLine(((Object) ((Component) this).get_gameObject()).get_name(), vector3List, 3f), (Visibility) 0, (Brightness) 1, false);
    VectorManager.useDraw3D = (__Null) 1;
  }
}
