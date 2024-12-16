using System.Collections.Generic;
using System.IO;

public class HatchingFile
{
    public const int c_FILE_VERSION = 1;

    public List<HatchingData> HatchingDatas => this.hatchingDatas;

    private int fileVersion;
    private int headerLength;
    private int recordLength;
    private int recordCount;

    private List<HatchingData> hatchingDatas;

    public static HatchingFile CreateFile(string filePath)
    {
        HatchingFile hatchingFile = new HatchingFile();

        hatchingFile.fileVersion = c_FILE_VERSION;
        hatchingFile.headerLength = 32;
        hatchingFile.recordLength = 128;
        hatchingFile.recordCount = 0;

        hatchingFile.hatchingDatas = new List<HatchingData>((int)hatchingFile.recordCount);

        new FileStream(filePath, FileMode.Create, FileAccess.Write).Close();
        hatchingFile.SaveFile(filePath);

        return hatchingFile;
    }

    public static HatchingFile LoadFile(string filePath)
    {
        string directoryPath = Path.GetDirectoryName(filePath);

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        if (!File.Exists(filePath))
            HatchingFile.CreateFile(filePath);

        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        BinaryReader rd = new BinaryReader(fs);

        HatchingFile hatchingFile = HatchingFile.ReadHeader(fs, rd);

        for (int i = 0; i < hatchingFile.recordCount; ++i)
        {
            HatchingData hatchingData = HatchingFile.ReadRecord(fs, rd, hatchingFile);
            hatchingFile.hatchingDatas.Add(hatchingData);
        }

        rd.Close();
        fs.Close();

        return hatchingFile;
    }

    private static HatchingFile ReadHeader(FileStream fs, BinaryReader rd)
    {
        long filePtrStart = fs.Position;

        HatchingFile hatchingFile = new HatchingFile();

        hatchingFile.fileVersion = rd.ReadInt32();
        hatchingFile.headerLength = rd.ReadInt32();
        hatchingFile.recordLength = rd.ReadInt32();
        hatchingFile.recordCount = rd.ReadInt32();

        hatchingFile.hatchingDatas = new List<HatchingData>((int)hatchingFile.recordCount);

        fs.Position = filePtrStart + hatchingFile.headerLength;

        return hatchingFile;
    }

    private static HatchingData ReadRecord(FileStream fs, BinaryReader rd, HatchingFile hatchingFile)
    {
        long filePtrStart = fs.Position;

        HatchingData hatchingData = new HatchingData();

        hatchingData.recordNumber = rd.ReadInt64();
        hatchingData.eggType = rd.ReadInt32();
        hatchingData.bugType = rd.ReadInt32();
        hatchingData.hatchingRequireTime = rd.ReadInt64();
        hatchingData.managingRequireTime = rd.ReadInt64();
        hatchingData.hatchingEndTime = rd.ReadInt64();
        hatchingData.managingEndTime = rd.ReadInt64();
        hatchingData.bodyLengthMin = rd.ReadSingle();
        hatchingData.bodyLengthMax = rd.ReadSingle();
        hatchingData.bodyLength = rd.ReadSingle();
        hatchingData.hatchingStatus = (HatchingStatus)rd.ReadInt32();

        fs.Position = filePtrStart + hatchingFile.recordLength;

        return hatchingData;
    }

    private static void WriteHeader(FileStream fs, BinaryWriter wr, HatchingFile hatchingFile)
    {
        long filePtrStart = fs.Position;

        wr.Write(hatchingFile.fileVersion);
        wr.Write(hatchingFile.headerLength);
        wr.Write(hatchingFile.recordLength);
        wr.Write(hatchingFile.hatchingDatas.Count);

        HatchingFile.WritePadding(fs, wr, filePtrStart, hatchingFile.headerLength);
    }

    private static void WriteRecord(FileStream fs, BinaryWriter wr, HatchingFile hatchingFile, HatchingData hatchingData)
    {
        long filePtrStart = fs.Position;

        wr.Write(hatchingData.recordNumber);
        wr.Write(hatchingData.eggType);
        wr.Write(hatchingData.bugType);
        wr.Write(hatchingData.hatchingRequireTime);
        wr.Write(hatchingData.managingRequireTime);
        wr.Write(hatchingData.hatchingEndTime);
        wr.Write(hatchingData.managingEndTime);
        wr.Write(hatchingData.bodyLengthMin);
        wr.Write(hatchingData.bodyLengthMax);
        wr.Write(hatchingData.bodyLength);
        wr.Write((int)hatchingData.hatchingStatus);

        HatchingFile.WritePadding(fs, wr, filePtrStart, hatchingFile.recordLength);
    }

    private static void WritePadding(FileStream fs, BinaryWriter wr, long filePtrStart, long sectionLength)
    {
        long writtenLength = fs.Position - filePtrStart;
        long paddingLength = sectionLength - writtenLength;

        for (int i = 0; i < paddingLength; ++i)
        {
            wr.Write((byte)0);
        }
    }

    private HatchingFile()
    {
        // NOTE: This area is intentionally left blank.
    }

    public void SaveFile(string filePath)
    {
        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Write);
        BinaryWriter wr = new BinaryWriter(fs);

        HatchingFile.WriteHeader(fs, wr, this);

        for (int i = 0; i < hatchingDatas.Count; ++i)
        {
            this.hatchingDatas[i].recordNumber = i;
            HatchingFile.WriteRecord(fs, wr, this, this.hatchingDatas[i]);
        }

        wr.Close();
        fs.Close();
    }
}