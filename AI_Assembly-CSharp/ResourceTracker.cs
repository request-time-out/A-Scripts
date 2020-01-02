// Decompiled with JetBrains decompiler
// Type: ResourceTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResourceTracker : IDisposable
{
  private Dictionary<string, List<ResourceTracker.stackParamater>> _stackUnavailableDict = new Dictionary<string, List<ResourceTracker.stackParamater>>();
  private Dictionary<object, ResourceRequestInfo> InProgressAsyncObjects = new Dictionary<object, ResourceRequestInfo>();
  private Dictionary<int, ResourceRequestInfo> TrackedAllocInfo = new Dictionary<int, ResourceRequestInfo>();
  private Dictionary<int, int> TrackedGameObjects = new Dictionary<int, int>();
  private Dictionary<int, int> TrackedMemObjects = new Dictionary<int, int>();
  private Dictionary<int, string> Stacktraces = new Dictionary<int, string>();
  public static ResourceTracker Instance;
  private bool _enableTracking;
  private StreamWriter _logWriter;
  private int _reqSeq;
  private Dictionary<string, string> _shaderPropertyDict;

  public ResourceTracker(bool enableTracking)
  {
    if (!enableTracking)
      return;
    this.Open();
    if (!this._enableTracking)
      return;
    if (UsNet.Instance != null && UsNet.Instance.CmdExecutor != null)
    {
      UsNet.Instance.CmdExecutor.RegisterHandler(eNetCmd.CL_RequestStackData, new UsCmdHandler(this.NetHandle_RequestStackData));
      UsNet.Instance.CmdExecutor.RegisterHandler(eNetCmd.CL_RequestStackSummary, new UsCmdHandler(this.NetHandle_RequestStackSummary));
    }
    else
      Debug.LogError((object) "UsNet not available");
    this.readShaderPropertyJson();
  }

  public bool EnableTracking
  {
    get
    {
      return this._enableTracking;
    }
  }

  public Dictionary<string, string> ShaderPropertyDict
  {
    get
    {
      return this._shaderPropertyDict;
    }
  }

  private void readShaderPropertyJson()
  {
    if (this._shaderPropertyDict != null)
      return;
    try
    {
      StreamReader streamReader = new StreamReader((Stream) new FileStream(ResourceTrackerConst.shaderPropertyNameJsonPath, FileMode.Open));
      string end = streamReader.ReadToEnd();
      streamReader.Close();
      this._shaderPropertyDict = ((Serialization<string, string>) JsonUtility.FromJson<Serialization<string, string>>(end)).ToDictionary();
    }
    catch (Exception ex)
    {
      Debug.Log((object) "no ShaderPropertyNameRecord.json");
    }
  }

  public void Open()
  {
    if (this._enableTracking)
    {
      Debug.LogFormat("[ResourceTracker] info: {0} ", new object[1]
      {
        (object) "already enabled, ignored."
      });
    }
    else
    {
      try
      {
        string str = Path.Combine(Application.get_persistentDataPath(), "mem_logs");
        if (!Directory.Exists(str))
          Directory.CreateDirectory(str);
        DateTime now = DateTime.Now;
        string path2 = string.Format("{0}_{1}_alloc.txt", (object) SysUtil.FormatDateAsFileNameString(now), (object) SysUtil.FormatTimeAsFileNameString(now));
        string fileName = Path.Combine(str, path2);
        this._logWriter = new System.IO.FileInfo(fileName).CreateText();
        this._logWriter.AutoFlush = true;
        this._enableTracking = true;
        Debug.LogFormat("[ResourceTracker] tracking enabled: {0} ", new object[1]
        {
          (object) fileName
        });
      }
      catch (Exception ex)
      {
        Debug.LogErrorFormat("[ResourceTracker] Open() failed, error: {0} ", new object[1]
        {
          (object) ex.Message
        });
        if (this._logWriter != null)
        {
          this._logWriter.Close();
          this._logWriter = (StreamWriter) null;
        }
        this._enableTracking = false;
      }
    }
  }

  public void Close()
  {
    if (this._logWriter != null)
    {
      try
      {
        this._logWriter.WriteLine("--------- unfinished request: {0} --------- ", (object) this.InProgressAsyncObjects.Count);
        foreach (KeyValuePair<object, ResourceRequestInfo> progressAsyncObject in this.InProgressAsyncObjects)
          this._logWriter.WriteLine("  + {0}", (object) progressAsyncObject.Value.ToString());
        this._logWriter.Close();
      }
      catch (Exception ex)
      {
        Debug.LogErrorFormat("[ResourceTracker.ctor] error: {0} ", new object[1]
        {
          (object) ex.Message
        });
      }
      finally
      {
        this._logWriter = (StreamWriter) null;
      }
    }
    this._enableTracking = false;
  }

  public void Dispose()
  {
    if (!this._enableTracking || this._logWriter == null)
      return;
    this.Close();
  }

  public void TrackSyncRequest(Object spawned, string path)
  {
    if (!this._enableTracking)
      return;
    StackFrame sf = new StackFrame(2, true);
    ResourceRequestInfo req = this.NewRequest(path, sf);
    req.requestType = ResourceRequestType.Ordinary;
    this.TrackRequestWithObject(req, spawned);
  }

  public void TrackResourcesDotLoad(Object loaded, string path)
  {
    if (!this._enableTracking)
      return;
    StackFrame sf = new StackFrame(1, true);
    ResourceRequestInfo req = this.NewRequest(path, sf);
    req.requestType = ResourceRequestType.Ordinary;
    this.TrackRequestWithObject(req, loaded);
  }

  public void TrackAsyncRequest(object handle, string path)
  {
    if (!this._enableTracking)
      return;
    StackFrame sf = new StackFrame(2, true);
    if (sf.GetMethod().Name.Contains("SpawnAsyncOldVer"))
      sf = new StackFrame(3, true);
    this.InProgressAsyncObjects[handle] = this.NewRequest(path, sf);
  }

  public void TrackAsyncDone(object handle, Object target)
  {
    ResourceRequestInfo req;
    if (!this._enableTracking || Object.op_Equality(target, (Object) null) || !this.InProgressAsyncObjects.TryGetValue(handle, out req))
      return;
    req.requestType = ResourceRequestType.Async;
    this.TrackRequestWithObject(req, target);
    this.InProgressAsyncObjects.Remove(handle);
  }

  public void TrackSceneLoaded(string sceneName)
  {
    if (!this._enableTracking)
      return;
    Scene activeScene = SceneManager.GetActiveScene();
    foreach (Object rootGameObject in ((Scene) ref activeScene).GetRootGameObjects())
      this.TrackSyncRequest(rootGameObject, "[scene]: " + sceneName);
  }

  public void TrackObjectInstantiation(Object src, Object instantiated)
  {
    if (!this._enableTracking)
      return;
    int reqSeqID = -1;
    if (!this.TrackedGameObjects.TryGetValue(src.GetInstanceID(), out reqSeqID))
      return;
    this.ExtractObjectResources(instantiated, reqSeqID);
  }

  public ResourceRequestInfo GetAllocInfo(int instID)
  {
    if (!this._enableTracking)
      return (ResourceRequestInfo) null;
    int key = -1;
    if (!this.TrackedGameObjects.TryGetValue(instID, out key) && !this.TrackedMemObjects.TryGetValue(instID, out key))
      return (ResourceRequestInfo) null;
    ResourceRequestInfo resourceRequestInfo = (ResourceRequestInfo) null;
    return !this.TrackedAllocInfo.TryGetValue(key, out resourceRequestInfo) ? (ResourceRequestInfo) null : resourceRequestInfo;
  }

  public string GetStackTrace(ResourceRequestInfo req)
  {
    string str;
    return !this.Stacktraces.TryGetValue(req.stacktraceHash, out str) ? string.Empty : str;
  }

  private ResourceRequestInfo NewRequest(string path, StackFrame sf)
  {
    ResourceRequestInfo resourceRequestInfo = new ResourceRequestInfo();
    resourceRequestInfo.resourcePath = path;
    resourceRequestInfo.srcFile = sf.GetFileName();
    resourceRequestInfo.srcLineNum = sf.GetFileLineNumber();
    resourceRequestInfo.seqID = this._reqSeq++;
    string stackTrace = StackTraceUtility.ExtractStackTrace();
    for (int index = 10; index > 0; --index)
    {
      string str;
      if (!this.Stacktraces.TryGetValue(stackTrace.GetHashCode(), out str))
      {
        this.Stacktraces[stackTrace.GetHashCode()] = stackTrace;
        break;
      }
      if (!(stackTrace == str))
        stackTrace += ((int) ((double) Random.get_value() * 100.0)).ToString();
      else
        break;
    }
    resourceRequestInfo.stacktraceHash = stackTrace.GetHashCode();
    return resourceRequestInfo;
  }

  private void ExtractObjectResources(Object obj, int reqSeqID)
  {
    SceneGraphExtractor sceneGraphExtractor = new SceneGraphExtractor(obj);
    for (int index = 0; index < sceneGraphExtractor.GameObjectIDs.Count; ++index)
    {
      if (!this.TrackedGameObjects.ContainsKey(sceneGraphExtractor.GameObjectIDs[index]))
        this.TrackedGameObjects[sceneGraphExtractor.GameObjectIDs[index]] = reqSeqID;
    }
    foreach (KeyValuePair<string, List<int>> memObjectId in sceneGraphExtractor.MemObjectIDs)
    {
      foreach (int key in memObjectId.Value)
      {
        if (!this.TrackedMemObjects.ContainsKey(key))
          this.TrackedMemObjects[key] = reqSeqID;
      }
    }
  }

  public bool NetHandle_RequestStackSummary(eNetCmd cmd, UsCmd c)
  {
    string str = c.ReadString();
    if (string.IsNullOrEmpty(str))
      return false;
    if (str.Equals("begin"))
    {
      this._stackUnavailableDict.Clear();
      return true;
    }
    if (str.Equals("end"))
    {
      Debug.Log((object) "Stack Category Statistical Information:");
      int num1 = 0;
      int num2 = 0;
      int bytes1 = 0;
      int bytes2 = 0;
      int num3 = c.ReadInt32();
      for (int index = 0; index < num3; ++index)
      {
        string key = c.ReadString();
        List<ResourceTracker.stackParamater> stackParamaterList;
        this._stackUnavailableDict.TryGetValue(key, out stackParamaterList);
        if (stackParamaterList != null)
        {
          int num4 = c.ReadInt32();
          int bytes3 = c.ReadInt32();
          num1 += num4;
          bytes1 += bytes3;
          num2 += stackParamaterList.Count;
          int bytes4 = 0;
          foreach (ResourceTracker.stackParamater stackParamater in stackParamaterList)
            bytes4 += stackParamater.Size;
          bytes2 += bytes4;
          Debug.Log((object) string.Format("[{0} =({1}/{2},{3}/{4})]", (object) key, (object) stackParamaterList.Count, (object) num4, (object) ResourceTrackerConst.FormatBytes(bytes4), (object) ResourceTrackerConst.FormatBytes(bytes3)));
        }
      }
      Debug.Log((object) string.Format("[total =({0}/{1},{2}/{3})]", (object) num2, (object) num1, (object) ResourceTrackerConst.FormatBytes(bytes2), (object) ResourceTrackerConst.FormatBytes(bytes1)));
      return true;
    }
    string key1 = str;
    int num5 = c.ReadInt32();
    for (int index = 0; index < num5; ++index)
    {
      int instID = c.ReadInt32();
      int num1 = c.ReadInt32();
      if (ResourceTracker.Instance.GetAllocInfo(instID) == null)
      {
        if (!this._stackUnavailableDict.ContainsKey(key1))
          this._stackUnavailableDict.Add(key1, new List<ResourceTracker.stackParamater>());
        List<ResourceTracker.stackParamater> stackParamaterList;
        this._stackUnavailableDict.TryGetValue(key1, out stackParamaterList);
        stackParamaterList.Add(new ResourceTracker.stackParamater()
        {
          InstanceID = instID,
          Size = num1
        });
      }
    }
    return true;
  }

  public List<Texture2D> GetTexture2DObjsFromMaterial(Material mat)
  {
    Dictionary<string, string> shaderPropertyDict = ResourceTracker.Instance.ShaderPropertyDict;
    List<Texture2D> texture2DList = new List<Texture2D>();
    if (Object.op_Inequality((Object) mat, (Object) null))
    {
      Shader shader = mat.get_shader();
      if (Object.op_Inequality((Object) shader, (Object) null) && shaderPropertyDict != null && shaderPropertyDict.ContainsKey(((Object) shader).get_name()))
      {
        string str1;
        shaderPropertyDict.TryGetValue(((Object) shader).get_name(), out str1);
        char[] chArray = new char[1]
        {
          ResourceTrackerConst.shaderPropertyNameJsonToken
        };
        foreach (string str2 in str1.Split(chArray))
        {
          Texture2D texture = mat.GetTexture(str2) as Texture2D;
          if (Object.op_Inequality((Object) texture, (Object) null))
            texture2DList.Add(texture);
        }
      }
      Texture2D mainTexture = mat.get_mainTexture() as Texture2D;
      if (Object.op_Inequality((Object) mainTexture, (Object) null) && !texture2DList.Contains(mainTexture))
        texture2DList.Add(mainTexture);
    }
    return texture2DList;
  }

  public bool NetHandle_RequestStackData(eNetCmd cmd, UsCmd c)
  {
    int instID = c.ReadInt32();
    string str = c.ReadString();
    Debug.Log((object) string.Format("NetHandle_RequestStackData instanceID={0} className={1}", (object) instID, (object) str));
    ResourceRequestInfo allocInfo = ResourceTracker.Instance.GetAllocInfo(instID);
    UsCmd cmd1 = new UsCmd();
    cmd1.WriteNetCmd(eNetCmd.SV_QueryStacksResponse);
    if (allocInfo == null)
      cmd1.WriteString("<no_callstack_available>");
    else
      cmd1.WriteString(ResourceTracker.Instance.GetStackTrace(allocInfo));
    UsNet.Instance.SendCommand(cmd1);
    return true;
  }

  private void TrackRequestWithObject(ResourceRequestInfo req, Object obj)
  {
    if (Object.op_Equality(obj, (Object) null))
      return;
    try
    {
      req.RecordObject(obj);
      this.TrackedAllocInfo[req.seqID] = req;
      this.ExtractObjectResources(obj, req.seqID);
      if (this._logWriter == null)
        return;
      this._logWriter.WriteLine(req.ToString());
    }
    catch (Exception ex)
    {
      Debug.LogErrorFormat("[ResourceTracker.TrackRequestWithObject] error: {0} \n {1} \n {2}", new object[3]
      {
        (object) ex.Message,
        req == null ? (object) string.Empty : (object) req.ToString(),
        (object) ex.StackTrace
      });
    }
  }

  private class stackParamater
  {
    private int instanceID;
    private int size;

    public int InstanceID
    {
      get
      {
        return this.instanceID;
      }
      set
      {
        this.instanceID = value;
      }
    }

    public int Size
    {
      get
      {
        return this.size;
      }
      set
      {
        this.size = value;
      }
    }
  }
}
