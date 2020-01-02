// Decompiled with JetBrains decompiler
// Type: Illusion.Component.UI.MouseButtonCheck
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Illusion.Component.UI
{
  public class MouseButtonCheck : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
  {
    [EnumMask]
    public MouseButtonCheck.ButtonType buttonType;
    [EnumMask]
    public MouseButtonCheck.EventType eventType;
    public MouseButtonCheck.Callback onPointerDown;
    public MouseButtonCheck.Callback onPointerUp;
    public MouseButtonCheck.Callback onBeginDrag;
    public MouseButtonCheck.Callback onDrag;
    public MouseButtonCheck.Callback onEndDrag;

    public MouseButtonCheck()
    {
      base.\u002Ector();
    }

    public bool isLeft
    {
      get
      {
        return Illusion.Utils.Enum<MouseButtonCheck.ButtonType>.Normalize(this.buttonType).HasFlag((Enum) MouseButtonCheck.ButtonType.Left);
      }
      set
      {
        this.SetButtonType(value, MouseButtonCheck.ButtonType.Left);
      }
    }

    public bool isRight
    {
      get
      {
        return Illusion.Utils.Enum<MouseButtonCheck.ButtonType>.Normalize(this.buttonType).HasFlag((Enum) MouseButtonCheck.ButtonType.Right);
      }
      set
      {
        this.SetButtonType(value, MouseButtonCheck.ButtonType.Right);
      }
    }

    public bool isCenter
    {
      get
      {
        return Illusion.Utils.Enum<MouseButtonCheck.ButtonType>.Normalize(this.buttonType).HasFlag((Enum) MouseButtonCheck.ButtonType.Center);
      }
      set
      {
        this.SetButtonType(value, MouseButtonCheck.ButtonType.Center);
      }
    }

    public bool isOnPointerDown
    {
      get
      {
        return Illusion.Utils.Enum<MouseButtonCheck.EventType>.Normalize(this.eventType).HasFlag((Enum) MouseButtonCheck.EventType.PointerDown);
      }
      set
      {
        this.SetEventType(value, MouseButtonCheck.EventType.PointerDown);
      }
    }

    public bool isOnPointerUp
    {
      get
      {
        return Illusion.Utils.Enum<MouseButtonCheck.EventType>.Normalize(this.eventType).HasFlag((Enum) MouseButtonCheck.EventType.PointerUp);
      }
      set
      {
        this.SetEventType(value, MouseButtonCheck.EventType.PointerUp);
      }
    }

    public bool isOnBeginDrag
    {
      get
      {
        return Illusion.Utils.Enum<MouseButtonCheck.EventType>.Normalize(this.eventType).HasFlag((Enum) MouseButtonCheck.EventType.BeginDrag);
      }
      set
      {
        this.SetEventType(value, MouseButtonCheck.EventType.BeginDrag);
      }
    }

    public bool isOnDrag
    {
      get
      {
        return Illusion.Utils.Enum<MouseButtonCheck.EventType>.Normalize(this.eventType).HasFlag((Enum) MouseButtonCheck.EventType.Drag);
      }
      set
      {
        this.SetEventType(value, MouseButtonCheck.EventType.Drag);
      }
    }

    public bool isOnEndDrag
    {
      get
      {
        return Illusion.Utils.Enum<MouseButtonCheck.EventType>.Normalize(this.eventType).HasFlag((Enum) MouseButtonCheck.EventType.EndDrag);
      }
      set
      {
        this.SetEventType(value, MouseButtonCheck.EventType.EndDrag);
      }
    }

    public virtual void OnPointerDown(PointerEventData data)
    {
      if (!this.isOnPointerDown || !((IEnumerable<int>) this.Indexeser).Any<int>((Func<int, bool>) (i => MouseButtonCheck.Check(i)[0])))
        return;
      this.onPointerDown.Invoke(data);
    }

    public virtual void OnPointerUp(PointerEventData data)
    {
      if (!this.isOnPointerUp || !((IEnumerable<int>) this.Indexeser).Any<int>((Func<int, bool>) (i => MouseButtonCheck.Check(i)[2])))
        return;
      this.onPointerUp.Invoke(data);
    }

    public virtual void OnBeginDrag(PointerEventData data)
    {
      if (!this.isOnBeginDrag || !((IEnumerable<int>) this.Indexeser).Any<int>((Func<int, bool>) (i => MouseButtonCheck.Check(i)[1])))
        return;
      this.onBeginDrag.Invoke(data);
    }

    public virtual void OnDrag(PointerEventData data)
    {
      if (!this.isOnDrag || !((IEnumerable<int>) this.Indexeser).Any<int>((Func<int, bool>) (i => MouseButtonCheck.Check(i)[1])))
        return;
      this.onDrag.Invoke(data);
    }

    public virtual void OnEndDrag(PointerEventData data)
    {
      if (!this.isOnEndDrag || !((IEnumerable<int>) this.Indexeser).Any<int>((Func<int, bool>) (i => MouseButtonCheck.Check(i)[2])))
        return;
      this.onEndDrag.Invoke(data);
    }

    private void SetButtonType(bool isOn, MouseButtonCheck.ButtonType type)
    {
      this.buttonType = Illusion.Utils.Enum<MouseButtonCheck.ButtonType>.Normalize(this.buttonType);
      this.buttonType = !isOn ? (MouseButtonCheck.ButtonType) this.buttonType.Sub((Enum) type) : (MouseButtonCheck.ButtonType) this.buttonType.Add((Enum) type);
    }

    private void SetEventType(bool isOn, MouseButtonCheck.EventType type)
    {
      this.eventType = Illusion.Utils.Enum<MouseButtonCheck.EventType>.Normalize(this.eventType);
      this.eventType = !isOn ? (MouseButtonCheck.EventType) this.eventType.Sub((Enum) type) : (MouseButtonCheck.EventType) this.eventType.Add((Enum) type);
    }

    private static bool[] Check(int i)
    {
      return new bool[3]
      {
        Input.GetMouseButtonDown(i),
        Input.GetMouseButton(i),
        Input.GetMouseButtonUp(i)
      };
    }

    private int[] Indexeser
    {
      get
      {
        return ((IEnumerable<int>) new int[3]
        {
          !this.isLeft ? -1 : 0,
          !this.isRight ? -1 : 1,
          !this.isCenter ? -1 : 2
        }).Where<int>((Func<int, bool>) (i => i != -1)).ToArray<int>();
      }
    }

    [Flags]
    public enum ButtonType
    {
      Left = 1,
      Right = 2,
      Center = 4,
    }

    [Flags]
    public enum EventType
    {
      PointerDown = 1,
      PointerUp = 2,
      BeginDrag = 4,
      Drag = 8,
      EndDrag = 16, // 0x00000010
    }

    [Serializable]
    public class Callback : UnityEvent<PointerEventData>
    {
      public Callback()
      {
        base.\u002Ector();
      }
    }

    private enum Key
    {
      Down,
      Hold,
      Up,
    }
  }
}
