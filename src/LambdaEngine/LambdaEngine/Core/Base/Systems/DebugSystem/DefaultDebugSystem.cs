// #define DEBUG_LOGGER

using System.Collections.Concurrent;
using System.Runtime.InteropServices;

using static LambdaEngine.DebugSystem.LogLevel;

namespace LambdaEngine.DebugSystem;

/// <summary>
/// Default implementation of the DebugSystem.
/// </summary>
public partial class DefaultDebugSystem : IDebugSystem {
    private readonly Dictionary<LogLevel, ConsoleColor> logColors = new() {
        { TRACE, ConsoleColor.Gray },
        { DEBUG, ConsoleColor.DarkBlue },
        { INFO, ConsoleColor.Green },
        { WARNING, ConsoleColor.Yellow },
        { ERROR, ConsoleColor.DarkRed },
        { FATAL, ConsoleColor.Red }
    };
    private readonly ConcurrentQueue<(string Message, LogLevel logLevel)> logQueue = new();
    
    private bool debugRunning = true;
    private Thread? debugThread;
    
    public LogLevel LogLevel { get; set; } 

    public void Initialize() {
        debugThread = new Thread(DebuggerThread) {
            IsBackground = true
        };
    }

    public void Start() {
        if (debugThread == null) {
            throw new InvalidOperationException("Logger not initialized");
        }
        
        debugRunning = true;
        
        debugThread.Start();
    }
    
    public void Stop() {
        debugRunning = false;
        debugThread?.Join();

        FreeConsole();
    }

    public void Log(string message) {
        Log(message, INFO);
    }

    public void Log(string message, LogLevel logLevel) {
        logQueue.Enqueue((message, logLevel));
    }

    /// <summary>
    /// Start the live-logger console.
    /// </summary>
    private static void StartDebugConsole() {
        #if DEBUG_LOGGER
        FreeConsole();
        // Running as standalone, allocate a new console
        if (!AllocConsole()) {
            Console.WriteLine($"Error: {Marshal.GetLastWin32Error()}");
        }
            
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        Console.SetIn(new StreamReader(Console.OpenStandardInput()));

        Console.WriteLine("Debugger running in a separate CMD window.");
        #else
        if (Environment.UserInteractive) {
            // Running in IDE, use default Console
            Console.WriteLine("Running debugger in integrated console.");
        }
        else {
            // Running as standalone, allocate a new console
            if (!AllocConsole()) {
                Console.WriteLine($"Error starting debugger: {Marshal.GetLastWin32Error()}");
            }
            
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });

            Console.WriteLine("Running debugger in dedicated console.");
        }
        #endif
    }

    /// <summary>
    /// Handle log messages and print them to the currently active console.
    /// </summary>
    private void DebuggerThread() {
        StartDebugConsole();

        while (debugRunning) {
            while (logQueue.TryDequeue(out (string Message, LogLevel logLevel) logEntry)) {
                if (Console.ForegroundColor != logColors[logEntry.logLevel]) {
                    Console.ForegroundColor = logColors[logEntry.logLevel];
                }
                
                string formattedMessage = $"[{DateTime.Now:HH:mm:ss}] [{logEntry.logLevel}] {logEntry.Message}";
                Console.WriteLine(formattedMessage);
            }

            Thread.Sleep(10);
        }

        Console.WriteLine("Debugger stopped.");
    }

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool AllocConsole();

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool FreeConsole();
}