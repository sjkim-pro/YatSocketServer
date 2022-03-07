using MessagePack;

namespace YatSocketServer.Packet;

[MessagePackObject]
public class LoginRes
{
    [Key(0)]
    public int ResultCode { get; set; }
}