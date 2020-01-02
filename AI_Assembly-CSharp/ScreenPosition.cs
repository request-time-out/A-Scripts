// Decompiled with JetBrains decompiler
// Type: ScreenPosition
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

[ExecuteInEditMode]
public class ScreenPosition : MonoBehaviour
{
  [SerializeField]
  private Vector3 _position;
  private bool isChange;

  public ScreenPosition()
  {
    base.\u002Ector();
  }

  public Vector3 position
  {
    get
    {
      return this._position;
    }
    set
    {
      this.isChange = true;
      this._position = value;
      ((Component) this).get_transform().set_position(Camera.get_main().ScreenToWorldPoint(this._position));
    }
  }

  private void Update()
  {
    if (((Component) this).get_transform().get_hasChanged() && !this.isChange)
      this._position = Camera.get_main().WorldToScreenPoint(((Component) this).get_transform().get_position());
    else
      this.isChange = false;
  }

  private void OnValidate()
  {
    this.position = this._position;
  }
}
