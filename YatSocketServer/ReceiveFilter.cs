
using SuperSocket.Common;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketEngine.Protocol;

namespace YatSocketServer;

public class EFBinaryRequestInfo : BinaryRequestInfo
{
    public Int16 Size { get; private set; } // bodySize
    public Int16 PacketID { get; private set; } //
    public SByte Type { get; private set; }

    public EFBinaryRequestInfo(Int16 size, Int16 packetId, SByte type, byte[] body)
        : base(null, body)
    {
        this.Size = size;
        this.PacketID = packetId;
        this.Type = type;
    }
    
}
// SuperSocket 이 이 패킷을 어떻게 파싱해야하는지를 알려주는 것
public class ReceiveFilter : FixedHeaderReceiveFilter<EFBinaryRequestInfo>
{
    public ReceiveFilter() : base(YatBaseLib.PacketDef.PACKET_HEADER_SIZE)
    {
    }

    protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
    {
        if (!BitConverter.IsLittleEndian)
        {
            Array.Reverse(header, offset, YatBaseLib.PacketDef.PACKET_HEADER_SIZE);
        }

        var packetSize = BitConverter.ToInt16(header, offset);
        var bodySize = packetSize - YatBaseLib.PacketDef.PACKET_HEADER_SIZE;
        return bodySize;
    }
    
    protected override EFBinaryRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
    {
        if (!BitConverter.IsLittleEndian)
            Array.Reverse(header.Array, 0, YatBaseLib.PacketDef.PACKET_HEADER_SIZE);

        return new EFBinaryRequestInfo(BitConverter.ToInt16(header.Array, 0),
            BitConverter.ToInt16(header.Array, 2),
            (SByte) header.Array[4],
            bodyBuffer.CloneRange(offset, length));
    }
}