// Decompiled with JetBrains decompiler
// Type: ADV.CharaPackData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ADV
{
  public class CharaPackData : IPack
  {
    private List<CommandData> _commandList = new List<CommandData>();

    public void SetCommandData(params ICommandData[] commandData)
    {
      this.commandData = ((IEnumerable<ICommandData>) commandData).Where<ICommandData>((Func<ICommandData, bool>) (p => p != null)).ToArray<ICommandData>();
    }

    public void SetParam(params IParams[] param)
    {
      this.param = ((IEnumerable<IParams>) param).Where<IParams>((Func<IParams, bool>) (p => p != null)).ToArray<IParams>();
    }

    public virtual void Init()
    {
      if (!Singleton<SoundPlayer>.IsInstance())
        return;
      Singleton<SoundPlayer>.Instance.BGMPlayActive = false;
    }

    public virtual void Release()
    {
      if (!Singleton<SoundPlayer>.IsInstance())
        return;
      Singleton<SoundPlayer>.Instance.ActivateMapBGM();
    }

    public virtual List<Program.Transfer> Create()
    {
      this.Vars = (Dictionary<string, ValData>) null;
      this._commandList.Clear();
      List<Program.Transfer> transfers = Program.Transfer.NewList(true, false);
      if (this.commandData != null)
      {
        foreach (ICommandData self in this.commandData)
          self.AddList(this._commandList, "G_");
        CommandData.CreateCommand(transfers, (IReadOnlyCollection<CommandData>) this._commandList);
      }
      if (this.param != null)
      {
        foreach (IParams @params in this.param)
        {
          @params.param.Reset((string) null);
          @params.param.CreateCommand(transfers);
          this._commandList.AddRange((IEnumerable<CommandData>) @params.param.list);
        }
      }
      return transfers;
    }

    public virtual void Receive(TextScenario scenario)
    {
      foreach (CommandData command in this._commandList)
        command.ReceiveADV(scenario);
      this.Vars = scenario.Vars;
      Action onComplete = this.onComplete;
      if (onComplete == null)
        return;
      onComplete();
    }

    public Action onComplete { get; set; }

    public Dictionary<string, ValData> Vars { get; set; }

    public ICommandData[] commandData { get; private set; }

    public IParams[] param { get; private set; }

    public IReadOnlyCollection<CommandData> commandList
    {
      get
      {
        return (IReadOnlyCollection<CommandData>) this._commandList;
      }
    }

    public void CommandListVisibleEnabled(bool enabled)
    {
      this.isCommandListVisibleEnabled = enabled;
    }

    private bool isCommandListVisibleEnabled { get; set; }

    void IPack.CommandListVisibleEnabledDefault()
    {
      this.isCommandListVisibleEnabled = true;
    }

    bool IPack.isCommandListVisibleEnabled
    {
      get
      {
        return this.isCommandListVisibleEnabled;
      }
    }
  }
}
