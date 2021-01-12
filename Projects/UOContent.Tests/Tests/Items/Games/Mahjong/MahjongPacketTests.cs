using System;
using Server;
using Server.Engines.Mahjong;
using Server.Tests;
using Server.Tests.Network;
using Xunit;

namespace UOContent.Tests
{
    public class MahjongPacketTests : IClassFixture<ServerFixture>
    {
        [Fact]
        public void TestMahjongJoinGame()
        {
            Serial game = 0x1024u;

            var expected = new MahjongJoinGame(game).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendMahjongJoinGame(game);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestMahjongPlayersInfo(bool showScores)
        {
            var m = new Mobile(0x1);
            m.DefaultMobileInit();

            var game = new MahjongGame { ShowScores = showScores };
            game.Players.Join(m);

            var expected = new MahjongPlayersInfo(game, m).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendMahjongPlayersInfo(game, m);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void TestMahjongGeneralInfo(bool showScores, bool spectatorVision)
        {
            var game = new MahjongGame { ShowScores = showScores, SpectatorVision = spectatorVision};

            var expected = new MahjongGeneralInfo(game).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendMahjongGeneralInfo(game);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestMahjongTilesInfo(bool spectatorVision)
        {
            var m = new Mobile(0x1);
            m.DefaultMobileInit();

            var game = new MahjongGame { SpectatorVision = spectatorVision };
            game.Players.Join(m);

            var expected = new MahjongTilesInfo(game, m).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendMahjongTilesInfo(game, m);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestMahjongTileInfo(bool spectatorVision)
        {
            var m = new Mobile(0x1);
            m.DefaultMobileInit();

            var game = new MahjongGame { SpectatorVision = spectatorVision };
            game.Players.Join(m);

            var expected = new MahjongTileInfo(game.Tiles[0], m).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendMahjongTileInfo(game.Tiles[0], m);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }

        [Fact]
        public void TestMahjongRelieve()
        {
            Serial game = 0x1024u;

            var expected = new MahjongRelieve(game).Compile();

            using var ns = PacketTestUtilities.CreateTestNetState();
            ns.SendMahjongRelieve(game);

            var result = ns.SendPipe.Reader.TryRead();
            AssertThat.Equal(result.Buffer[0].AsSpan(0), expected);
        }
    }
}
