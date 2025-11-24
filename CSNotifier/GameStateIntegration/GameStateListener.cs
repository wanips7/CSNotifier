using System.Net;

namespace CSNotifier.GameStateIntegration;

public class GameStateListener : IDisposable
{
    public const string Host = "127.0.0.1";

    private readonly HttpListener _listener;

    public GameEventsExecutor GameEventsExecutor { get; } = new();

    private readonly GameStateParser _gameStateParser = new();

    public bool IsRunning { get; private set; } = false;

    private CancellationTokenSource _cancellationTokenSource = null!;

    public event Action? OnStart;

    public event Action? OnStop;

    public event Action<string>? OnRead;

    public event Action<string>? OnError;

    public string ErrorMessage { get; private set; } = "";

    public GameStateListener(ushort port)
    {
        var uri = $"http://{Host}:{port}/";
        
        _listener = new();
        _listener.Prefixes.Add(uri);

    }

    private void SetError(string message)
    {
        ErrorMessage = message;
        OnError?.Invoke(ErrorMessage);
    }

    public async Task ProcessRequest(HttpListenerContext context)
    {
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;

        using (var reader = new System.IO.StreamReader(request.InputStream, request.ContentEncoding))
        {
            string requestText = await reader.ReadToEndAsync(_cancellationTokenSource.Token);
            
            var gameState = await _gameStateParser.TryParseAsync(requestText);

            if (gameState is not null)
            {
                OnRead?.Invoke(requestText);               

                await GameEventsExecutor.ExecuteEventsAsync(gameState);

            }
        }

        response.Close();
    }

    public async Task StartAsync()
    {
        if (IsRunning)
            return;

        _cancellationTokenSource = new CancellationTokenSource(); 

        OnStart?.Invoke();

        IsRunning = true;

        ErrorMessage = "";

        GameEventsExecutor.ResetPrevGameState();

        try
        {
            _listener.Start();

            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                HttpListenerContext context = await _listener.GetContextAsync();
                await ProcessRequest(context);
            }

        }
        catch (OperationCanceledException)
        {

        }
        catch (Exception ex)
        {
            SetError(ex.Message);
        }

        _listener.Stop();
        _cancellationTokenSource.Dispose();

        OnStop?.Invoke();

        IsRunning = false;
    
    }

    public async Task StopAsync()
    {
        if (IsRunning)
        {
            _listener.Stop();
            await _cancellationTokenSource.CancelAsync();
        }        
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _listener.Close();
        }
    }

}
