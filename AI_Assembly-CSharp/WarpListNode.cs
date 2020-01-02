// Decompiled with JetBrains decompiler
// Type: WarpListNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using UnityEngine;
using UnityEngine.UI;

public class WarpListNode : MonoBehaviour
{
  [SerializeField]
  private Image Icon;
  [SerializeField]
  private Text Text;
  private Transform targetPos;
  public bool canWarp;
  public BasePoint basePoint;

  public WarpListNode()
  {
    base.\u002Ector();
  }

  public Transform TargetPos
  {
    get
    {
      return this.targetPos;
    }
  }

  public void Set(BasePoint _add, string _name)
  {
    this.Text.set_text(_name);
    this.targetPos = _add.WarpPoint;
    this.basePoint = _add;
  }

  public void IconSet(Sprite sprite)
  {
    this.Icon.set_sprite(sprite);
    this.canWarp = true;
  }
}
