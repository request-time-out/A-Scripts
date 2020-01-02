// Decompiled with JetBrains decompiler
// Type: RouteNode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class RouteNode : MonoBehaviour
{
  public Button buttonSelect;
  [SerializeField]
  private Text textName;
  public Button buttonPlay;
  public Sprite[] spritePlay;

  public RouteNode()
  {
    base.\u002Ector();
  }

  public string text
  {
    get
    {
      return this.textName.get_text();
    }
    set
    {
      this.textName.set_text(value);
    }
  }

  public RouteNode.State state
  {
    set
    {
      switch (value)
      {
        case RouteNode.State.Stop:
          ((Graphic) ((Selectable) this.buttonPlay).get_image()).set_color(Color.get_white());
          ((Selectable) this.buttonPlay).get_image().set_sprite(this.spritePlay[0]);
          break;
        case RouteNode.State.Play:
          ((Graphic) ((Selectable) this.buttonPlay).get_image()).set_color(Color.get_green());
          ((Selectable) this.buttonPlay).get_image().set_sprite(this.spritePlay[1]);
          break;
        case RouteNode.State.End:
          ((Graphic) ((Selectable) this.buttonPlay).get_image()).set_color(Color.get_red());
          ((Selectable) this.buttonPlay).get_image().set_sprite(this.spritePlay[1]);
          break;
      }
    }
  }

  public enum State
  {
    Stop,
    Play,
    End,
  }
}
