// Decompiled with JetBrains decompiler
// Type: ActionTable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;

public class ActionTable
{
  private static readonly IReadOnlyDictionary<ActionID, string> table_ = (IReadOnlyDictionary<ActionID, string>) new Dictionary<ActionID, string>((IEqualityComparer<ActionID>) new ActionIDComparer())
  {
    {
      ActionID.MoveHorizontal,
      "Move Horizontal"
    },
    {
      ActionID.MoveVertical,
      "Move Vertical"
    },
    {
      ActionID.CameraHorizontal,
      "Camera Horizontal"
    },
    {
      ActionID.CameraVertical,
      "Camera Vertical"
    },
    {
      ActionID.Action,
      "Action"
    },
    {
      ActionID.Jump,
      "Jump"
    },
    {
      ActionID.Attack,
      "Attack"
    },
    {
      ActionID.Guard,
      "Guard"
    },
    {
      ActionID.Special,
      "Special"
    },
    {
      ActionID.Submit,
      "Submit"
    },
    {
      ActionID.Cancel,
      "Cancel"
    },
    {
      ActionID.SelectHorizontal,
      "Select Horizontal"
    },
    {
      ActionID.SelectVertical,
      "Select Vertical"
    },
    {
      ActionID.MouseLeft,
      "Mouse Left"
    },
    {
      ActionID.MouseRight,
      "Mouse Right"
    },
    {
      ActionID.MouseCenter,
      "Mouse Center"
    },
    {
      ActionID.MouseWheel,
      "Mouse Wheel"
    },
    {
      ActionID.End,
      "End"
    },
    {
      ActionID.Angle1,
      "Angle1"
    },
    {
      ActionID.Angle2,
      "Angle2"
    },
    {
      ActionID.Angle3,
      "Angle3"
    }
  };

  public string this[int i]
  {
    get
    {
      return ActionTable.table_.ContainsKey((ActionID) i) ? ActionTable.table_.get_Item((ActionID) i) : string.Empty;
    }
  }

  public string this[ActionID i]
  {
    get
    {
      return ActionTable.table_.ContainsKey(i) ? ActionTable.table_.get_Item(i) : string.Empty;
    }
  }

  public int this[string s]
  {
    get
    {
      foreach (KeyValuePair<ActionID, string> keyValuePair in (IEnumerable<KeyValuePair<ActionID, string>>) ActionTable.table_)
      {
        if (keyValuePair.Value == s)
          return (int) keyValuePair.Key;
      }
      return 0;
    }
  }

  public static IReadOnlyDictionary<ActionID, string> Table
  {
    get
    {
      return ActionTable.table_;
    }
  }

  public static string Name(ActionID id)
  {
    return ActionTable.table_.get_Item(id);
  }

  public static int ID(string name)
  {
    foreach (KeyValuePair<ActionID, string> keyValuePair in (IEnumerable<KeyValuePair<ActionID, string>>) ActionTable.table_)
    {
      if (keyValuePair.Value == name)
        return (int) keyValuePair.Key;
    }
    return 0;
  }
}
