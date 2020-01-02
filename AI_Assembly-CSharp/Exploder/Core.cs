// Decompiled with JetBrains decompiler
// Type: Exploder.Core
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Exploder
{
  internal class Core : Singleton<Core>
  {
    [NonSerialized]
    public ExploderParams parameters;
    [NonSerialized]
    public ExploderQueue queue;
    [NonSerialized]
    public Stopwatch explosionWatch;
    [NonSerialized]
    public Stopwatch frameWatch;
    [NonSerialized]
    public HashSet<MeshObject> meshSet;
    [NonSerialized]
    public Dictionary<int, int> targetFragments;
    [NonSerialized]
    public List<MeshObject> postList;
    [NonSerialized]
    public List<Fragment> pool;
    [NonSerialized]
    public AudioSource audioSource;
    [NonSerialized]
    public CrackManager crackManager;
    [NonSerialized]
    public int processingFrames;
    [NonSerialized]
    public int poolIdx;
    [NonSerialized]
    public bool splitMeshIslands;
    [NonSerialized]
    public BakeSkinManager bakeSkinManager;
    private ExploderTask[] tasks;
    private TaskType currTaskType;
    private bool initialized;

    public void Initialize(ExploderObject exploder)
    {
      if (this.initialized)
        return;
      this.initialized = true;
      this.parameters = new ExploderParams(exploder);
      FragmentPool.Instance.Reset(this.parameters);
      this.frameWatch = new Stopwatch();
      this.explosionWatch = new Stopwatch();
      this.crackManager = new CrackManager(this);
      this.bakeSkinManager = new BakeSkinManager(this);
      this.queue = new ExploderQueue(this);
      this.tasks = new ExploderTask[6];
      this.tasks[1] = (ExploderTask) new Preprocess(this);
      this.tasks[2] = this.parameters.ThreadOptions != ExploderObject.ThreadOptions.Disabled ? (ExploderTask) new CutterMT(this) : (ExploderTask) new CutterST(this);
      this.tasks[3] = (ExploderTask) new IsolateMeshIslands(this);
      this.tasks[4] = (ExploderTask) new PostprocessExplode(this);
      this.tasks[5] = (ExploderTask) new PostprocessCrack(this);
      this.PreAllocateBuffers();
      this.audioSource = (AudioSource) ((Component) this).get_gameObject().AddComponent<AudioSource>();
    }

    public void Enqueue(
      ExploderObject exploderObject,
      ExploderObject.OnExplosion callback,
      bool crack,
      params GameObject[] obj)
    {
      this.queue.Enqueue(exploderObject, callback, crack, obj);
    }

    public void ExplodeCracked(GameObject obj, ExploderObject.OnExplosion callback)
    {
      if (Object.op_Inequality((Object) obj, (Object) null))
      {
        long num = this.crackManager.Explode(obj);
        if (callback == null)
          return;
        callback((float) num, ExploderObject.ExplosionState.ExplosionFinished);
      }
      else
      {
        long num = this.crackManager.ExplodeAll();
        if (callback == null)
          return;
        callback((float) num, ExploderObject.ExplosionState.ExplosionFinished);
      }
    }

    public void ExplodePartial(
      GameObject obj,
      Vector3 shotDir,
      Vector3 hitPosition,
      float bulletSize,
      ExploderObject.OnExplosion callback)
    {
      if (Object.op_Inequality((Object) obj, (Object) null))
      {
        long num = this.crackManager.ExplodePartial(obj, shotDir, hitPosition, bulletSize);
        if (callback == null)
          return;
        callback((float) num, ExploderObject.ExplosionState.ExplosionFinished);
      }
      else
      {
        long num = this.crackManager.ExplodeAll();
        if (callback == null)
          return;
        callback((float) num, ExploderObject.ExplosionState.ExplosionFinished);
      }
    }

    public bool IsCracked(GameObject gm)
    {
      return this.crackManager.IsCracked(gm);
    }

    public void StartExplosionFromQueue(ExploderParams p)
    {
      this.parameters = p;
      this.processingFrames = 1;
      this.explosionWatch.Reset();
      this.explosionWatch.Start();
      AudioSource component = (AudioSource) p.ExploderGameObject.GetComponent<AudioSource>();
      if (Object.op_Implicit((Object) component))
        ExploderUtils.CopyAudioSource(component, this.audioSource);
      this.currTaskType = TaskType.Preprocess;
      this.InitTask(this.currTaskType);
      this.RunTask(this.currTaskType, 0.0f);
      if (this.parameters.ThreadOptions == ExploderObject.ThreadOptions.Disabled)
        return;
      this.currTaskType = this.NextTask(this.currTaskType);
      if (this.currTaskType != TaskType.None)
      {
        this.InitTask(this.currTaskType);
        this.RunTask(this.currTaskType, this.parameters.FrameBudget);
      }
      else
      {
        this.explosionWatch.Stop();
        this.queue.OnExplosionFinished(this.parameters.id, this.explosionWatch.ElapsedMilliseconds);
      }
    }

    public void Update()
    {
      this.frameWatch.Reset();
      this.frameWatch.Start();
      if (this.currTaskType == TaskType.None)
        return;
      while ((double) this.frameWatch.ElapsedMilliseconds < (double) this.parameters.FrameBudget)
      {
        if (this.RunTask(this.currTaskType, this.parameters.FrameBudget))
        {
          this.currTaskType = this.NextTask(this.currTaskType);
          if (this.currTaskType == TaskType.None)
          {
            this.explosionWatch.Stop();
            this.bakeSkinManager.Clear();
            this.queue.OnExplosionFinished(this.parameters.id, this.explosionWatch.ElapsedMilliseconds);
            return;
          }
          this.InitTask(this.currTaskType);
        }
      }
      ++this.processingFrames;
    }

    public override void OnDestroy()
    {
      base.OnDestroy();
      foreach (ExploderTask task in this.tasks)
        task?.OnDestroy();
    }

    private void PreAllocateBuffers()
    {
      this.meshSet = new HashSet<MeshObject>();
      for (int index = 0; index < 64; ++index)
        this.meshSet.Add(new MeshObject());
      this.InitTask(TaskType.Preprocess);
      this.RunTask(TaskType.Preprocess, 0.0f);
      if (this.meshSet.Count <= 0)
        return;
      this.InitTask(TaskType.ProcessCutter);
      this.RunTask(TaskType.ProcessCutter, 0.0f);
    }

    private bool RunTask(TaskType taskType, float budget = 0.0f)
    {
      return this.tasks[(int) taskType].Run(budget);
    }

    private void InitTask(TaskType taskType)
    {
      this.tasks[(int) taskType].Init();
    }

    private TaskType NextTask(TaskType taskType)
    {
      switch (taskType)
      {
        case TaskType.Preprocess:
          return this.meshSet.Count == 0 ? TaskType.None : TaskType.ProcessCutter;
        case TaskType.ProcessCutter:
          if (this.splitMeshIslands)
            return TaskType.IsolateMeshIslands;
          return this.parameters.Crack ? TaskType.PostprocessCrack : TaskType.PostprocessExplode;
        case TaskType.IsolateMeshIslands:
          return this.parameters.Crack ? TaskType.PostprocessCrack : TaskType.PostprocessExplode;
        case TaskType.PostprocessExplode:
          return TaskType.None;
        case TaskType.PostprocessCrack:
          return TaskType.None;
        default:
          return TaskType.None;
      }
    }
  }
}
