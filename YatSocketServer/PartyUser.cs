namespace YatSocketServer;

public class PartyUser
{
    public GameSession Session { get; private set; }
    public Int64 UserUID { get; private set; }
    public Int64 HubUID { get; private set; }
    public string HubID { get; private set; }
    public Int64 GuildUID { get; private set; }
    
    public Int64 PartyUID { get; private set; }
    
    public PartyUserGrade UserGrade { get; private set; }
    public PartyUserState UserState { get; private set; }

    public PartyUser(GameSession session, Int64 partyUid, Int64 userUid, Int64 hubUid, string hubId, Int64 guildUid, int grade)
    {
        Session = session;
        PartyUID = partyUid;
        UserUID = userUid;
        HubUID = hubUid;
        HubID = hubId;
        GuildUID = guildUid;

        UserGrade = (PartyUserGrade) grade;
        if (UserGrade == PartyUserGrade.MASTER)
        {
            UserState = PartyUserState.READY; // 파티장은 무조건 레디상태로 시작한다.
        }
        else
        {
            UserState = PartyUserState.NOT_READY;
        }
        
    }

    public void SetReady()
    {   // 유저가 준비 버튼을 눌렀을 때, 상태를 바꿔준다. 
        UserState = PartyUserState.READY;
    }
    
    public void CancelReady()
    {   // 유저가 준비 취소를 하면, 상태를 바꿔준다. (파티장은 적용되지 않는다.)
        if (UserGrade == PartyUserGrade.MASTER)
            return;
        UserState = PartyUserState.NOT_READY;
    }
}