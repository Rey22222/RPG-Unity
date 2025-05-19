public class SavePlayerStatsInteractor
{
    private readonly IPlayerStatsRepository _repository;

    public SavePlayerStatsInteractor(IPlayerStatsRepository repository)
    {
        _repository = repository;
    }

    public void Execute(PlayerStats stats)
    {
        _repository.Save(stats);
    }
}
