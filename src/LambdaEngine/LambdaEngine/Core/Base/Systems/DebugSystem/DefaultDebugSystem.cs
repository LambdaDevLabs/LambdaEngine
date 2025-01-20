using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace LambdaEngine.DebugSystem;

public partial class DefaultDebugSystem : IDebugSystem {
    private bool debugRunning = true;
    private readonly ConcurrentQueue<(string Message, int LogLevel)> logQueue = new();
    private Thread? debugThread;

    public void Initialize() {
        debugRunning = true;

        // Start the debug console in a separate thread
        debugThread = new Thread(DebuggerThread) {
            IsBackground = true
        };
        debugThread.Start();
    }

    public void Log(string message) {
        Log(message, 0); // Default log level is 0
    }

    public void Log(string message, int logLevel) {
        // Add the message and log level to the thread-safe queue
        logQueue.Enqueue((message, logLevel));
    }

    public void StartDebugConsole() {
        if (!Environment.UserInteractive) {
            // Running in IDE, use default Console
            Console.WriteLine("Debugger running in IDE console.");
        }
        else {
            FreeConsole();
            // Running as standalone, allocate a new console
            if (!AllocConsole()) {
                Console.WriteLine($"Error: {Marshal.GetLastWin32Error()}");
            }
            
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
            Console.SetIn(new StreamReader(Console.OpenStandardInput()));

            Console.WriteLine("Debugger running in a separate CMD window.");
        }
    }

    public void StopDebugConsole() {
        // Stop the debugger
        debugRunning = false;
        debugThread?.Join(); // Wait for the debugger thread to finish

        // Free the console if it was allocated
        FreeConsole();
    }

    private void DebuggerThread() {
        StartDebugConsole();

        Console.WriteLine("Debugger started. Type 'exit' to stop.");

        while (debugRunning) {
            // Process log messages
            while (logQueue.TryDequeue(out (string Message, int LogLevel) logEntry)) {
                string formattedMessage = $"[{DateTime.Now:HH:mm:ss}] [Level {logEntry.LogLevel}] {logEntry.Message}";
                Console.WriteLine(formattedMessage);
            }

            Thread.Sleep(10); // Avoid busy-waiting
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