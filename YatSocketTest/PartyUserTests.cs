using System;
using Xunit;
using YatSocketServer;

namespace YatSocketTest;


public class PartyUserTests
{
    GameSession Session = new GameSession(); // 가짜
    private Int64 PartyUid = 1;
    private Int64 UserUid = 1;
    private Int64 HubUid = 1;
    private string HubId = "user1";
    private Int64 GuildUid = 0;
    private int Grade;
    public int UserState;
    
    
    public void SetUp()
    {
    }

    public void TearDown()
    {
    }
    [Fact(DisplayName = "마스터로 파티유저 생성해보기")]
    public void CreatePartyUserByMaster()
    {
        SetUp();

        Grade = (int) PartyUserGrade.MASTER;
        
        PartyUser partyUser = new PartyUser(Session, PartyUid, UserUid, HubUid, HubId, GuildUid, Grade);
        
        Assert.Equal(PartyUid, partyUser.PartyUID);
        Assert.Equal(UserUid, partyUser.UserUID);
        Assert.Equal(HubUid, partyUser.HubUID);
        Assert.Equal(HubId, partyUser.HubID);
        Assert.Equal(GuildUid, partyUser.GuildUID);
        Assert.Equal((PartyUserGrade) Grade, partyUser.UserGrade);
        Assert.Equal(PartyUserState.READY, partyUser.UserState);
        
        TearDown();
    }
    
    [Fact(DisplayName = "일반유저로 파티유저 생성해보기")]
    public void CreatePartyUserByNormal()
    {
        SetUp();

        Grade = (int) PartyUserGrade.NORMAL;

        PartyUser partyUser = new PartyUser(Session, PartyUid, UserUid, HubUid, HubId, GuildUid, Grade);
        
        Assert.Equal(PartyUid, partyUser.PartyUID);
        Assert.Equal(UserUid, partyUser.UserUID);
        Assert.Equal(HubUid, partyUser.HubUID);
        Assert.Equal(HubId, partyUser.HubID);
        Assert.Equal(GuildUid, partyUser.GuildUID);
        Assert.Equal((PartyUserGrade) Grade, partyUser.UserGrade);
        Assert.Equal(PartyUserState.NOT_READY, partyUser.UserState);
        
        TearDown();
    }
    [Fact(DisplayName = "파티장 유저 준비상태 바꾸기 - 파티장은 바뀌지 않음")]
    public void ChangeUserStateByMasterUser()
    {
        SetUp();

        Grade = (int) PartyUserGrade.MASTER;

        PartyUser partyUser = new PartyUser(Session, PartyUid, UserUid, HubUid, HubId, GuildUid, Grade);
        
        partyUser.SetReady(); // Ready 상태로 만들기
        
        Assert.Equal(PartyUserState.READY, partyUser.UserState);
        
        partyUser.CancelReady(); // Ready 상태 취소하기 (취소할 수 없다.)
        
        Assert.Equal(PartyUserState.READY, partyUser.UserState);
        
        TearDown();
    }
    [Fact(DisplayName = "일반유저 준비상태 바꾸기 - 일반 유저는 상태가 바뀐다.")]
    public void ChangeUserStateByNormalUser()
    {
        SetUp();

        Grade = (int) PartyUserGrade.NORMAL;
      
        PartyUser partyUser = new PartyUser(Session, PartyUid, UserUid, HubUid, HubId, GuildUid, Grade);
        
        partyUser.SetReady(); // Ready 상태로 만들기
        
        Assert.Equal(PartyUserState.READY, partyUser.UserState);
        
        partyUser.CancelReady(); // Ready 상태 취소하기
        
        Assert.Equal(PartyUserState.NOT_READY, partyUser.UserState);
        
        TearDown();
    }
}