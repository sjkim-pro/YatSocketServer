namespace YatSocketServer;

public class Party
{
    public const int MAX_PARTY_MEMBER = 4; 
    public Int64 GameUID { get; set; } // 입장할 때 설정
    public Int64 PartyUID { get; set; } // 파티 생성시 설정
    public Dictionary<Int64, PartyUser> PartyUsers = new Dictionary<Int64, PartyUser>();

    public Party(Int64 partyUid)
    {
        PartyUID = partyUid;
    }
    /**
    * 파티원 추가
     */
    public bool AddPartyUser(PartyUser partyUser)
    {
        if (PartyUsers.Count >= MAX_PARTY_MEMBER)
        {
            return false;
        }

        if (PartyUID != partyUser.PartyUID)
        {
            return false;
        }
        // 첫번째 추가되는 유저는 파티장이어야한다.
        if (PartyUsers.Count == 0 && partyUser.UserGrade != PartyUserGrade.MASTER)
        {
            return false;
        }

        PartyUsers[partyUser.UserUID] = partyUser;
        return true;
    }

    /**
     * 파티원 제거
     */
    public bool RemovePartyUser(PartyUser partyUser) 
    {
        if (PartyUsers.ContainsKey(partyUser.UserUID))
        {
            PartyUsers.Remove(partyUser.UserUID);
            return true;
        }
        return false;
    }

    /**
     * 파티장 위임
     */
    public int DelegatePartyMaster()
    {
        return 0;
    }
}