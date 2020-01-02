// Decompiled with JetBrains decompiler
// Type: Studio.WorkInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.IO;
using System.Text;

namespace Studio
{
  public class WorkInfo
  {
    private readonly Version version = new Version(1, 0, 1);
    public bool[] visibleFlags = new bool[6]
    {
      true,
      true,
      true,
      true,
      true,
      true
    };
    public bool visibleCenter = true;
    public bool visibleAxis = true;
    public bool visibleAxisTranslation = true;
    public bool visibleAxisCenter = true;
    public bool visibleGimmick = true;
    private const string userPath = "studio";
    private const string fileName = "work.dat";
    public bool useAlt;

    public void Init()
    {
      for (int index = 0; index < 6; ++index)
        this.visibleFlags[index] = true;
      this.visibleCenter = true;
      this.visibleAxis = true;
      this.useAlt = false;
      this.visibleAxisTranslation = true;
      this.visibleAxisCenter = true;
      this.visibleGimmick = true;
    }

    public void Save()
    {
      string path = UserData.Create("studio") + "work.dat";
      using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) fileStream, Encoding.UTF8))
        {
          try
          {
            binaryWriter.Write(this.version.ToString());
            for (int index = 0; index < 6; ++index)
              binaryWriter.Write(this.visibleFlags[index]);
            binaryWriter.Write(this.visibleCenter);
            binaryWriter.Write(this.visibleAxis);
            binaryWriter.Write(this.useAlt);
            binaryWriter.Write(this.visibleAxisTranslation);
            binaryWriter.Write(this.visibleAxisCenter);
            binaryWriter.Write(this.visibleGimmick);
          }
          catch (Exception ex)
          {
            File.Delete(path);
          }
        }
      }
    }

    public void Load()
    {
      string path = UserData.Create("studio") + "work.dat";
      if (!File.Exists(path))
      {
        this.Init();
      }
      else
      {
        using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
          using (BinaryReader binaryReader = new BinaryReader((Stream) fileStream, Encoding.UTF8))
          {
            try
            {
              Version version = new Version(binaryReader.ReadString());
              for (int index = 0; index < 6; ++index)
                this.visibleFlags[index] = binaryReader.ReadBoolean();
              this.visibleCenter = binaryReader.ReadBoolean();
              this.visibleAxis = binaryReader.ReadBoolean();
              this.useAlt = binaryReader.ReadBoolean();
              this.visibleAxisTranslation = binaryReader.ReadBoolean();
              this.visibleAxisCenter = binaryReader.ReadBoolean();
              if (version.CompareTo(new Version(1, 0, 1)) < 0)
                return;
              this.visibleGimmick = binaryReader.ReadBoolean();
            }
            catch (Exception ex)
            {
              File.Delete(path);
              this.Init();
            }
          }
        }
      }
    }
  }
}
