namespace YatSocketServer;

public class GameSessionManager
{
    private Dictionary<string, GameSession> SessionList = new Dictionary<string, GameSession>();

    public int SessionCount()
    {
        return SessionList.Count;
    }
    public void AddSession(GameSession session)
    {
        SessionList[session.SessionID] = session;
        MainServer.MainLogger.Info($"AddSession({SessionCount()}) :{session.SessionID}");
    }

    public void RemoveSession(GameSession session)
    {
        if (SessionList.ContainsKey(session.SessionID))
        {
            SessionList.Remove(session.SessionID);
        }
        MainServer.MainLogger.Info($"RemoveSession({SessionCount()}) :{session.SessionID}");
    }
}