namespace Core.Interfaces;

public interface ISimulatorLatestRepository
{
    int GetLatestId();
    void SetLatestId(int value);
    void IncrementLatestId();
}
