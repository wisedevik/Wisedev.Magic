using Wisedev.Magic.Logic.Data;
using Wisedev.Magic.Titam.DataStream;

namespace Wisedev.Magic.Logic.Helper;

public static class ByteStreamHelper
{
    public static void WriteDataReference(ChecksumEncoder encoder, LogicData data)
    {
        encoder.WriteInt(data != null ? data.GetGlobalID() : 0);
    }

    public static LogicData? ReadDataReference(ByteStream stream)
    {
        int id = stream.ReadInt();
        return LogicDataTables.GetDataById(id);
    }

    public static LogicData? ReadDataReference(ByteStream stream, int idx)
    {
        int id = stream.ReadInt();
        int classID = GlobalID.GetClassID(id);
        if (classID == idx + 1)
            return LogicDataTables.GetDataById(id);
        return null;
    }
}
