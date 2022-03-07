using MessagePack;

namespace YatSocketServer.Packet;

[MessagePackObject]
public class LoginReq
{
    [Key(0)]
    public int UserUid { get; set; }
}