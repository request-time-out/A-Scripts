// Decompiled with JetBrains decompiler
// Type: usmooth.DataCollector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace usmooth
{
  public class DataCollector
  {
    public static DataCollector Instance = new DataCollector();
    public static GameObject MainCamera = (GameObject) null;
    private List<FrameData> _frames = new List<FrameData>();
    private MeshLut _meshLut = new MeshLut();
    private Dictionary<Material, HashSet<GameObject>> _visibleMaterials = new Dictionary<Material, HashSet<GameObject>>();
    private Dictionary<Texture, HashSet<Material>> _visibleTextures = new Dictionary<Texture, HashSet<Material>>();
    private Dictionary<int, string> _nameLut = new Dictionary<int, string>();
    private Dictionary<Texture, int> _textureSizeLut = new Dictionary<Texture, int>();
    private FrameData _currentFrame;

    public FrameData CollectFrameData()
    {
      this._visibleMaterials.Clear();
      this._visibleTextures.Clear();
      if (Object.op_Equality((Object) DataCollector.MainCamera, (Object) null))
      {
        GameObject gameObject = GameObject.Find("MainCamera");
        if (Object.op_Inequality((Object) gameObject, (Object) null))
          DataCollector.MainCamera = gameObject;
      }
      this._currentFrame = new FrameData();
      this._currentFrame._frameCount = Time.get_frameCount();
      this._currentFrame._frameDeltaTime = Time.get_deltaTime();
      this._currentFrame._frameRealTime = Time.get_realtimeSinceStartup();
      this._currentFrame._frameStartTime = Time.get_time();
      this._meshLut.ClearLut();
      foreach (MeshRenderer meshRenderer in Object.FindObjectsOfType(typeof (MeshRenderer)) as MeshRenderer[])
      {
        if (((Renderer) meshRenderer).get_isVisible())
        {
          GameObject gameObject = ((Component) meshRenderer).get_gameObject();
          if (this._meshLut.AddMesh(gameObject))
          {
            this._currentFrame._frameMeshes.Add(((Object) gameObject).GetInstanceID());
            this._nameLut[((Object) gameObject).GetInstanceID()] = ((Object) gameObject).get_name();
            foreach (Material sharedMaterial in ((Renderer) meshRenderer).get_sharedMaterials())
            {
              this.AddVisibleMaterial(sharedMaterial, ((Component) meshRenderer).get_gameObject());
              if (Object.op_Inequality((Object) sharedMaterial, (Object) null))
                this.AddVisibleTexture(sharedMaterial.get_mainTexture(), sharedMaterial);
            }
          }
        }
      }
      this._frames.Add(this._currentFrame);
      return this._currentFrame;
    }

    public void WriteName(int instID, UsCmd cmd)
    {
      string str;
      if (!this._nameLut.TryGetValue(instID, out str))
        return;
      cmd.WriteInt32(instID);
      cmd.WriteStringStripped(str);
    }

    private void AddVisibleMaterial(Material mat, GameObject gameobject)
    {
      if (!Object.op_Inequality((Object) mat, (Object) null))
        return;
      if (!this._visibleMaterials.ContainsKey(mat))
        this._visibleMaterials.Add(mat, new HashSet<GameObject>());
      this._visibleMaterials[mat].Add(gameobject);
    }

    private void AddVisibleTexture(Texture texture, Material ownerMat)
    {
      if (!Object.op_Inequality((Object) texture, (Object) null))
        return;
      if (!this._visibleTextures.ContainsKey(texture))
        this._visibleTextures.Add(texture, new HashSet<Material>());
      this._visibleTextures[texture].Add(ownerMat);
      if (this._textureSizeLut.ContainsKey(texture))
        return;
      this._textureSizeLut[texture] = UsTextureUtil.CalculateTextureSizeBytes(texture);
    }

    public void DumpAllInfo()
    {
      Debug.Log((object) string.Format("{0} visible materials ({2}), visible textures ({3})", (object) DateTime.Now.ToLongTimeString(), (object) this.VisibleMaterials.Count, (object) this.VisibleTextures.Count));
      string empty1 = string.Empty;
      using (Dictionary<Material, HashSet<GameObject>>.Enumerator enumerator = this.VisibleMaterials.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<Material, HashSet<GameObject>> current = enumerator.Current;
          empty1 += string.Format("{0} {1} {2}\n", (object) ((Object) current.Key).get_name(), (object) ((Object) current.Key.get_shader()).get_name(), (object) current.Value.Count);
        }
      }
      Debug.Log((object) empty1);
      string empty2 = string.Empty;
      using (Dictionary<Texture, HashSet<Material>>.Enumerator enumerator = this.VisibleTextures.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<Texture, HashSet<Material>> current = enumerator.Current;
          Texture key = current.Key;
          empty2 += string.Format("{0} {1} {2} {3} {4}\n", (object) ((Object) key).get_name(), (object) key.get_width(), (object) key.get_height(), (object) current.Value.Count, (object) UsTextureUtil.FormatSizeString(this._textureSizeLut[key] / 1024));
        }
      }
      Debug.Log((object) empty2);
    }

    public UsCmd CreateMaterialCmd()
    {
      UsCmd usCmd = new UsCmd();
      usCmd.WriteNetCmd(eNetCmd.SV_FrameData_Material);
      usCmd.WriteInt32(this.VisibleMaterials.Count);
      using (Dictionary<Material, HashSet<GameObject>>.Enumerator enumerator1 = this.VisibleMaterials.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          KeyValuePair<Material, HashSet<GameObject>> current1 = enumerator1.Current;
          usCmd.WriteInt32(((Object) current1.Key).GetInstanceID());
          usCmd.WriteStringStripped(((Object) current1.Key).get_name());
          usCmd.WriteStringStripped(((Object) current1.Key.get_shader()).get_name());
          usCmd.WriteInt32(current1.Value.Count);
          using (HashSet<GameObject>.Enumerator enumerator2 = current1.Value.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              GameObject current2 = enumerator2.Current;
              usCmd.WriteInt32(((Object) current2).GetInstanceID());
            }
          }
        }
      }
      return usCmd;
    }

    public UsCmd CreateTextureCmd()
    {
      UsCmd usCmd = new UsCmd();
      usCmd.WriteNetCmd(eNetCmd.SV_FrameData_Texture);
      usCmd.WriteInt32(this.VisibleTextures.Count);
      using (Dictionary<Texture, HashSet<Material>>.Enumerator enumerator1 = this.VisibleTextures.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          KeyValuePair<Texture, HashSet<Material>> current1 = enumerator1.Current;
          usCmd.WriteInt32(((Object) current1.Key).GetInstanceID());
          usCmd.WriteStringStripped(((Object) current1.Key).get_name());
          usCmd.WriteString(string.Format("{0}x{1}", (object) current1.Key.get_width(), (object) current1.Key.get_height()));
          usCmd.WriteString(UsTextureUtil.FormatSizeString(this._textureSizeLut[current1.Key] / 1024));
          usCmd.WriteInt32(current1.Value.Count);
          using (HashSet<Material>.Enumerator enumerator2 = current1.Value.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              Material current2 = enumerator2.Current;
              usCmd.WriteInt32(((Object) current2).GetInstanceID());
            }
          }
        }
      }
      return usCmd;
    }

    public MeshLut MeshTable
    {
      get
      {
        return this._meshLut;
      }
    }

    public Dictionary<Material, HashSet<GameObject>> VisibleMaterials
    {
      get
      {
        return this._visibleMaterials;
      }
    }

    public Dictionary<Texture, HashSet<Material>> VisibleTextures
    {
      get
      {
        return this._visibleTextures;
      }
    }
  }
}
