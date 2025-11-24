namespace CSNotifier.GameStateIntegration;

public class GameEventsExecutor
{
    private Models.GameState _prevGameState = new();

    public event Action? OnPlayerDead;

    public event Action? OnRoundStart;

    public async Task ExecuteEventsAsync(Models.GameState gameState)
    {       
        if (gameState.Map.Phase != MapPhase.Live)
            return;

        if (gameState.Provider.SteamId != gameState.Player.SteamId)
            return;

        if (_prevGameState.Player.State.Health > 0 && gameState.Player.State.Health == 0)
        {
            OnPlayerDead?.Invoke();
        }

        if (_prevGameState.Round.Phase != RoundPhase.Live && gameState.Round.Phase == RoundPhase.Live)
        {
            OnRoundStart?.Invoke();
        }

        _prevGameState = gameState;
    }

    public void ResetPrevGameState()
    {
        _prevGameState = new Models.GameState();
    }

}
