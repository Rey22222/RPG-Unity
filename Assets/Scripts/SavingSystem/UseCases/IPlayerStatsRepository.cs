public interface IPlayerStatsRepository
{
    PlayerStats Load();
    void Save(PlayerStats stats);
}
