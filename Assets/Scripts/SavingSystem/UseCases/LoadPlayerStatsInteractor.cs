public class LoadPlayerStatsInteractor
{
    private readonly IPlayerStatsRepository _repository;

    public LoadPlayerStatsInteractor(IPlayerStatsRepository repository)
    {
        _repository = repository;
    }

    public PlayerStats Execute()
    {
        return _repository.Load();
    }
}
