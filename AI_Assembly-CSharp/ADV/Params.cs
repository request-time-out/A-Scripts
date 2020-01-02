// Decompiled with JetBrains decompiler
// Type: ADV.Params
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ADV
{
  public class Params
  {
    public Params(object data)
    {
      this.data = data;
    }

    public Params(object data, string header)
      : this(data)
    {
      this.Initialize(header);
    }

    public virtual List<CommandData> list
    {
      get
      {
        return this._list;
      }
    }

    protected object data { get; }

    private List<CommandData> _list { get; } = new List<CommandData>();

    protected virtual void Initialize(string header)
    {
      // ISSUE: object of a compiler-generated type is created
      this._list.AddRange(((IEnumerable<FieldInfo>) Illusion.Utils.Type.GetPublicFields(this.data.GetType())).Select<FieldInfo, \u003C\u003E__AnonType2<FieldInfo, CommandData.Command>>((Func<FieldInfo, \u003C\u003E__AnonType2<FieldInfo, CommandData.Command>>) (info => new \u003C\u003E__AnonType2<FieldInfo, CommandData.Command>(info, CommandData.Cast(info.FieldType)))).Where<\u003C\u003E__AnonType2<FieldInfo, CommandData.Command>>((Func<\u003C\u003E__AnonType2<FieldInfo, CommandData.Command>, bool>) (p => p.command != CommandData.Command.None)).Select<\u003C\u003E__AnonType2<FieldInfo, CommandData.Command>, CommandData>((Func<\u003C\u003E__AnonType2<FieldInfo, CommandData.Command>, CommandData>) (p => new CommandData(p.command, header + p.info.Name, (Func<object>) (() => p.info.GetValue(this.data)), (Action<object>) (o => p.info.SetValue(this.data, Convert.ChangeType(o, p.info.FieldType)))))));
      // ISSUE: object of a compiler-generated type is created
      this._list.AddRange(((IEnumerable<PropertyInfo>) Illusion.Utils.Type.GetPublicProperties(this.data.GetType())).Select<PropertyInfo, \u003C\u003E__AnonType2<PropertyInfo, CommandData.Command>>((Func<PropertyInfo, \u003C\u003E__AnonType2<PropertyInfo, CommandData.Command>>) (info => new \u003C\u003E__AnonType2<PropertyInfo, CommandData.Command>(info, CommandData.Cast(info.PropertyType)))).Where<\u003C\u003E__AnonType2<PropertyInfo, CommandData.Command>>((Func<\u003C\u003E__AnonType2<PropertyInfo, CommandData.Command>, bool>) (p => p.command != CommandData.Command.None)).Select<\u003C\u003E__AnonType2<PropertyInfo, CommandData.Command>, CommandData>((Func<\u003C\u003E__AnonType2<PropertyInfo, CommandData.Command>, CommandData>) (p =>
      {
        Func<object> get = (Func<object>) null;
        Action<object> set = (Action<object>) null;
        if (p.info.CanRead)
          get = (Func<object>) (() => p.info.GetValue(this.data, (object[]) null));
        if (p.info.CanWrite)
          set = (Action<object>) (o => p.info.SetValue(this.data, o));
        return new CommandData(p.command, header + p.info.Name, get, set);
      })));
    }

    public virtual void CreateCommand(List<Program.Transfer> transfers)
    {
      CommandData.CreateCommand(transfers, (IReadOnlyCollection<CommandData>) this.list);
    }

    public virtual void Reset(string header = null)
    {
      this._list.Clear();
      this.Initialize(header);
    }

    public virtual void SetParamSync(TextScenario scenario, string key, object value)
    {
      CommandData commandData = this.list.FirstOrDefault<CommandData>((Func<CommandData, bool>) (p => p.key == key));
      if (commandData == null)
        return;
      ValData valData = new ValData(ValData.Convert(value, commandData.value.GetType()));
      commandData.value = valData.o;
      scenario.Vars[commandData.key] = valData;
      this.UpdateReplaceADV(scenario);
    }

    public virtual void SetADV(TextScenario scenario)
    {
      foreach (CommandData commandData in this.list.Where<CommandData>((Func<CommandData, bool>) (p => p.isVar)))
        scenario.Vars[commandData.key] = new ValData(ValData.Convert(commandData.value, commandData.value.GetType()));
      this.UpdateReplaceADV(scenario);
    }

    public virtual void UpdateReplaceADV(TextScenario scenario)
    {
      foreach (CommandData commandData in this.list.Where<CommandData>((Func<CommandData, bool>) (p => p.command == CommandData.Command.Replace)))
        scenario.Replaces[commandData.key] = (string) commandData.value;
    }

    public virtual void ReceiveADV(TextScenario scenario)
    {
      foreach (CommandData commandData in this.list)
        commandData.ReceiveADV(scenario);
    }
  }
}
