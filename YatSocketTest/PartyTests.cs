using System;
using Xunit;
using YatSocketServer;

namespace YatSocketTest;

public class PartyTests
{
    public Int64 partyUid;
    public PartyUser[] Party1Users;
    public PartyUser[] Party2Users;
    
    public void SetUp()
    {
        partyUid = 1;
        Party1Users = new PartyUser[5];
        Party2Users = new PartyUser[5];

        for (int i = 0; i < 5; i++)
        {
            GameSession session = new GameSession();
            int grade = 0;
            
            if (i == 0)
            {
                grade = (int) PartyUserGrade.MASTER;
            }
            else
            {
                grade = (int) PartyUserGrade.NORMAL;
            }
            
            Party1Users[i] = new PartyUser(session, partyUid, i, i, "user"+i,0, grade);
            Party2Users[i] = new PartyUser(session, 2, i*10, i*10, "user"+i*10,0, grade);
        }
        
    }

    public void TearDown()
    {
        Party1Users = null;
    }

    [Fact(DisplayName = "파티생성테스트")]
    public void CreatePartyTest()
    {
        SetUp();

        Party party = new Party(partyUid);
        Assert.Equal(partyUid, party.PartyUID);
        
        TearDown();
    }
    
    [Fact(DisplayName = "정상적으로 4명까지 파티원 추가하고, 다섯번째는 실패하기")]
    public void AddPartyMemberTests()
    {
        SetUp();

        Party party = new Party(partyUid);
        Assert.Equal(partyUid, party.PartyUID);

        int i = 0;
        foreach (var partyUser in Party1Users)
        {
            bool result = party.AddPartyUser(partyUser);
            
            if (i < Party.MAX_PARTY_MEMBER) // 인원제한을 초과하지 않았을 때는 성공하기
            {
                Assert.True(result);
                i++;
            }
            else // 인원제한을 초과하면 실패하기
            {
                Assert.False(result);
            }
            
            Assert.Equal(i, party.PartyUsers.Count);
        }
        
        TearDown();
    }

    [Fact(DisplayName = "첫번째 추가되는 멤버가 파티장이 아니라서 실패하기")]
    public void AddFirstMemberFailNotMasterTests()
    {
        SetUp();

        Party party = new Party(partyUid);
        Assert.Equal(partyUid, party.PartyUID);

        PartyUser partyUser = Party1Users[1];
        bool result = party.AddPartyUser(partyUser);

        Assert.Equal(PartyUserGrade.NORMAL, partyUser.UserGrade); // 첫등록 유저가 파티장이 아님
        Assert.False(result); // 그래서 실패함
        TearDown();
    }
    
    [Fact(DisplayName = "파티ID가 다른 유저 등록 실패하기")]
    public void AddMemberFailPartyUidIsDifferentTests()
    {
        SetUp();

        Party party = new Party(partyUid);
        Assert.Equal(partyUid, party.PartyUID);

        PartyUser partyUser = Party2Users[0]; // 2번파티의 파티장
        bool result = party.AddPartyUser(partyUser);

        Assert.Equal(PartyUserGrade.MASTER, partyUser.UserGrade); // 첫등록 유저가 파티장이지만, partyId가 다름
        Assert.NotEqual(party.PartyUID, partyUser.PartyUID);
        Assert.False(result); // 그래서 실패함
        TearDown();
    }
    
    [Fact(DisplayName = "파티장이 아닌 멤버 제거하기")]
    public void RemoveNormalMemberSuccessTests()
    {
        SetUp();

        bool result = false;
        Party party = new Party(partyUid);
        
        for (int i = 0; i < 4; i++)
        {
            PartyUser partyUser = Party1Users[i];
            result = party.AddPartyUser(partyUser); 
        }

        PartyUser removeUser = Party1Users[1]; // 일반 멤버 제거하기

        result = party.RemovePartyUser(removeUser);
        
        Assert.True(result);
        TearDown();
    }
    
    [Fact(DisplayName = "파티에 소속되지 않은 유저 제거 시도시 실패하기")]
    public void RemoveNormalMemberFailNotExsitUserTests()
    {
        SetUp();

        bool result = false;
        Party party = new Party(partyUid);
        
        for (int i = 0; i < 4; i++)
        {
            PartyUser partyUser = Party1Users[i];
            result = party.AddPartyUser(partyUser); 
        }

        PartyUser removeUser = Party1Users[4]; // 파티에 속하지 않은 멤버 제거 시도하기

        result = party.RemovePartyUser(removeUser);
        
        Assert.False(result);
        TearDown();
    }
}