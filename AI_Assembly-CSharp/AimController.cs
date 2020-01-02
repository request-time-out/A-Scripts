// Decompiled with JetBrains decompiler
// Type: AimController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class AimController : MonoBehaviour
{
  public AimController()
  {
    base.\u002Ector();
  }

  private void Start()
  {
  }

  private void Update()
  {
    ((Component) this).get_transform().set_position(Camera.get_main().ScreenToWorldPoint(new Vector3((float) Input.get_mousePosition().x, (float) Input.get_mousePosition().y, 100f)));
  }
}
