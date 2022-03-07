using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Logging;
using SuperSocket.SocketBase.Protocol;

namespace YatSocketServer;

public class MainServer : AppServer<GameSession, EFBinaryRequestInfo>
{
    public string ServerName = "YatSocketServer";
    public IServerConfig m_Config;
    public static ILog MainLogger;

    public GameSessionManager GSessionManager;
    public MainServer()
        : base(new DefaultReceiveFilterFactory<ReceiveFilter, EFBinaryRequestInfo>())
    {
        MainLogger = new ConsoleLog(ServerName);
        GSessionManager = new GameSessionManager();
        
        NewSessionConnected += new SessionHandler<GameSession>(OnConnected);
        SessionClosed += new SessionHandler<GameSession, CloseReason>(OnClosed);
        NewRequestReceived += new RequestHandler<GameSession, EFBinaryRequestInfo>(OnPacketReceived);
        
        m_Config = new ServerConfig
        {
            Name = "YatSocketServer",
            Ip = "Any",
            Port = 32001,
            Mode = SocketMode.Tcp,
            MaxConnectionNumber = 1024,
            MaxRequestLength = 10240,
            ReceiveBufferSize = 10240,
            SendBufferSize = 10240
        };
    }

    public void StartServer()
    {
        try
        {
            bool bResult = Setup(new RootConfig(), m_Config, logFactory: new NLogLogFactory());

            if (bResult == false)
            {
                MainLogger.Error("Setup Server Fail");
                return;
            }

            MainLogger = base.Logger;
            MainLogger.Info("Start Server Success");

            Start();
        }
        catch (Exception e)
        {
            MainLogger.Error($"Start Server Fail :{e.ToString()}");
        }
    }

    public void StopServer()
    {
        Stop();
    }

    // 소켓 연결될 때 처리
    void OnConnected(GameSession session)
    {
        MainLogger.Info("OnConnected");
        GSessionManager.AddSession(session);
    }

    // 소켓 끊길 때 처리
    void OnClosed(GameSession session, CloseReason reason)
    {
        MainLogger.Info("OnClosed");
        GSessionManager.RemoveSession(session);
    }

    // 패킷 받았을 때 처리
    void OnPacketReceived(GameSession session, EFBinaryRequestInfo reqInfo)
    {
        MainLogger.Info("OnPacketReceived");
        
        // 각 패킷 분배해서 처리하게 해줘야함.
    }

    public bool SendPacket(string SessionID, byte[] sendData)
    {
        var session = GetSessionByID(SessionID);

        try
        {
            if (session == null) // session 이 없으면 보낼 수 없다.
            {
                return false;
            }
            
            session.Send(sendData, 0, sendData.Length);
        }
        catch (Exception e)
        {
            // TimeoutException 예외가 발생할 수 있다
            MainServer.MainLogger.Error($"{e.ToString()},  {e.StackTrace}");

            session.SendEndWhenSendingTimeOut(); 
            session.Close();
            return false;
        }

        return true;
    }
}