// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.ICustomSelectable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Rewired.UI.ControlMapper
{
  public interface ICustomSelectable : ICancelHandler, IEventSystemHandler
  {
    Sprite disabledHighlightedSprite { get; set; }

    Color disabledHighlightedColor { get; set; }

    string disabledHighlightedTrigger { get; set; }

    bool autoNavUp { get; set; }

    bool autoNavDown { get; set; }

    bool autoNavLeft { get; set; }

    bool autoNavRight { get; set; }

    event UnityAction CancelEvent;
  }
}
