using ChatbotConsole.Models;

namespace ChatbotConsole.Services;

/// <summary>
/// Defines a minimal chat service contract used by the console app.
/// Implementations should send the conversation messages to a model
/// and return the assistant's reply as a string.
/// </summary>
public interface IChatService
{
    /// <summary>
    /// Send the provided conversation messages to the chat model and
    /// return the assistant response as a string.
    /// </summary>
    /// <param name="messages">Conversation messages with roles (system,user,assistant).</param>
    Task<string> SendAsync(List<ChatMessage> messages);
}
