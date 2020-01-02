// Decompiled with JetBrains decompiler
// Type: ADV.CharaParams
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ADV
{
  public class CharaParams : Params
  {
    private List<CommandData> cachedList;
    private List<CommandData> _addList;

    public CharaParams(ICommandData commandData, string HEADER)
      : base((object) commandData)
    {
      this.HEADER = HEADER;
      this.Initialize(this.HEADER_PARAM);
    }

    public string HEADER { get; } = string.Empty;

    public string HEADER_PARAM
    {
      get
      {
        return this.HEADER + "_";
      }
    }

    public override List<CommandData> list
    {
      get
      {
        return this.cachedList ?? (this.cachedList = base.list.Concat<CommandData>((IEnumerable<CommandData>) this.addList).ToList<CommandData>());
      }
    }

    public List<CommandData> addList
    {
      get
      {
        if (this._addList != null)
          return this._addList;
        this._addList = new List<CommandData>(((ICommandData) this.data).CreateCommandData(this.HEADER_PARAM));
        this.CreateCommandData_Actor(this._addList);
        return this._addList;
      }
    }

    public int charaID
    {
      get
      {
        if (Object.op_Equality((Object) this.actor, (Object) null))
          return 0;
        if (this.actor is AgentActor)
        {
          if (Object.op_Inequality((Object) this.actor, (Object) null) && Object.op_Inequality((Object) this.actor.ChaControl, (Object) null) && (this.actor.ChaControl.chaFile != null && this.actor.ChaControl.chaFile.parameter != null))
            return this.actor.ChaControl.chaFile.parameter.personality;
          Debug.LogError((object) "ADV Parameter ActorChara Data none");
        }
        return this.actor.ID;
      }
    }

    public void Bind(Actor actor)
    {
      this.actor = actor;
    }

    public Actor actor { get; private set; }

    public void CreateCommandData_Actor(List<CommandData> list)
    {
      if (Object.op_Equality((Object) this.actor, (Object) null))
        return;
      list.Add(new CommandData(CommandData.Command.String, this.HEADER_PARAM + string.Format("[{0}]", (object) "CharaName"), (Func<object>) (() => (object) this.actor.CharaName), (Action<object>) null));
      list.Add(new CommandData(CommandData.Command.Replace, this.HEADER, (Func<object>) (() => (object) this.actor.CharaName), (Action<object>) null));
    }

    public override void Reset(string header = null)
    {
      this._addList = (List<CommandData>) null;
      this.cachedList = (List<CommandData>) null;
    }
  }
}
