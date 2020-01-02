// Decompiled with JetBrains decompiler
// Type: UndoRedoMgr
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class UndoRedoMgr : Singleton<UndoRedoMgr>
{
  private Stack<ICommand> undo = new Stack<ICommand>();
  private Stack<ICommand> redo = new Stack<ICommand>();
  private bool _CanUndo;
  private bool _CanRedo;
  private const int MaxUndoRedoCnt = 100;
  [SerializeField]
  private bool bLimit;

  public event EventHandler CanUndoChange;

  public event EventHandler CanRedoChange;

  public bool CanUndo
  {
    get
    {
      return this._CanUndo;
    }
    private set
    {
      if (this._CanUndo == value)
        return;
      this._CanUndo = value;
      if (this.CanUndoChange == null)
        return;
      this.CanUndoChange((object) this, EventArgs.Empty);
    }
  }

  public bool CanRedo
  {
    get
    {
      return this._CanRedo;
    }
    private set
    {
      if (this._CanRedo == value)
        return;
      this._CanRedo = value;
      if (this.CanRedoChange == null)
        return;
      this.CanRedoChange((object) this, EventArgs.Empty);
    }
  }

  public void Undo()
  {
    if (this.undo.Count <= 0)
      return;
    ICommand command = this.undo.Pop();
    this.CanUndo = this.undo.Count > 0;
    command.Undo();
    this.redo.Push(command);
    this.CanRedo = true;
  }

  public void Redo()
  {
    if (this.redo.Count <= 0)
      return;
    ICommand command = this.redo.Pop();
    this.CanRedo = this.redo.Count > 0;
    command.Redo();
    this.undo.Push(command);
    this.CanUndo = true;
  }

  public void Push(ICommand _command)
  {
    if (_command == null)
      return;
    if (this.bLimit && this.undo.Count >= 100)
    {
      ICommand[] array = this.undo.ToArray();
      this.undo.Clear();
      for (int index = array.Length - 2; index > -1; --index)
        this.undo.Push(array[index]);
    }
    this.undo.Push(_command);
    this.CanUndo = true;
    this.redo.Clear();
    this.CanRedo = false;
  }

  public void Clear()
  {
    this.undo.Clear();
    this.redo.Clear();
    this.CanUndo = false;
    this.CanRedo = false;
  }

  private class Command : ICommand
  {
    private Delegate doMethod;
    private Delegate undoMethod;
    private object[] doParamater;
    private object[] undoParamater;

    public Command(
      Delegate _doMethod,
      object[] _doParamater,
      Delegate _undoMethod,
      object[] _undoParamater)
    {
      this.doMethod = _doMethod;
      this.doParamater = _doParamater;
      this.undoMethod = _undoMethod;
      this.undoParamater = _undoParamater;
    }

    public void Undo()
    {
      this.undoMethod.DynamicInvoke(this.undoParamater);
    }

    public void Redo()
    {
      this.doMethod.DynamicInvoke(this.doParamater);
    }
  }
}
