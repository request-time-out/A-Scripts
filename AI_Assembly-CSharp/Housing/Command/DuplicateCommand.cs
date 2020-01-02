// Decompiled with JetBrains decompiler
// Type: Housing.Command.DuplicateCommand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Battlehub.UIControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Housing.Command
{
  public class DuplicateCommand : Housing.ICommand
  {
    private DuplicateCommand.Info[] infos;
    private VirtualizingTreeView virtualizingTreeView;

    public DuplicateCommand(ObjectCtrl[] _objectCtrls)
    {
      this.infos = ((IEnumerable<ObjectCtrl>) _objectCtrls).Select<ObjectCtrl, DuplicateCommand.Info>((Func<ObjectCtrl, DuplicateCommand.Info>) (v => new DuplicateCommand.Info(v))).ToArray<DuplicateCommand.Info>();
    }

    private VirtualizingTreeView VirtualizingTreeView
    {
      get
      {
        return this.virtualizingTreeView ?? (this.virtualizingTreeView = Singleton<CraftScene>.Instance.UICtrl.ListUICtrl.VirtualizingTreeView);
      }
    }

    public void Do()
    {
      bool flag = false;
      foreach (DuplicateCommand.Info info in this.infos)
      {
        info.ObjectCtrl = Singleton<Manager.Housing>.Instance.DuplicateObject(info.Source);
        if (info.ObjectCtrl != null)
        {
          flag |= Singleton<Manager.Housing>.Instance.CheckOverlap(info.ObjectCtrl);
          this.VirtualizingTreeView.AddChild((object) info.ObjectCtrl.Parent, (object) info.ObjectCtrl);
        }
      }
      if (flag)
        this.VirtualizingTreeView.Refresh();
      this.VirtualizingTreeView.SelectedItems = (IEnumerable) ((IEnumerable<DuplicateCommand.Info>) this.infos).Select<DuplicateCommand.Info, ObjectCtrl>((Func<DuplicateCommand.Info, ObjectCtrl>) (v => v.ObjectCtrl));
      Singleton<CraftScene>.Instance.UICtrl.AddUICtrl.Reselect();
    }

    public void Redo()
    {
      bool flag = false;
      foreach (DuplicateCommand.Info info in this.infos)
      {
        if (info.ObjectCtrl != null)
        {
          flag |= Singleton<Manager.Housing>.Instance.RestoreObject(info.ObjectCtrl, info.Source.Parent, -1, true);
          this.VirtualizingTreeView?.AddChild((object) info.ObjectCtrl.Parent, (object) info.ObjectCtrl);
        }
      }
      Singleton<CraftScene>.Instance.UICtrl.AddUICtrl.Reselect();
      if (!flag)
        return;
      Singleton<CraftScene>.Instance.UICtrl.ListUICtrl.RefreshList();
    }

    public void Undo()
    {
      foreach (DuplicateCommand.Info info in this.infos)
      {
        if (info.ObjectCtrl != null)
        {
          Singleton<CraftScene>.Instance.UICtrl.ListUICtrl.isOnFuncSkip = true;
          this.VirtualizingTreeView?.RemoveChild((object) info.ObjectCtrl.Parent, (object) info.ObjectCtrl);
        }
      }
      Singleton<CraftScene>.Instance.UICtrl.AddUICtrl.Reselect();
      Singleton<CraftScene>.Instance.UICtrl.ListUICtrl.RefreshList();
    }

    private class Info
    {
      public Info(ObjectCtrl _objectCtrl)
      {
        this.Source = _objectCtrl;
      }

      public ObjectCtrl Source { get; private set; }

      public ObjectCtrl ObjectCtrl { get; set; }
    }
  }
}
