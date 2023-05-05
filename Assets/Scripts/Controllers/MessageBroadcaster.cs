using System.Collections.Generic;
using System.Linq;
using Mirror;
using Models;
using UnityEngine;
using Random = UnityEngine.Random;

public class MessageBroadcaster : NetworkBehaviour
{
    
    [Header("Messages")]
    [SerializeField] private TextAsset startMessagesFile;
    [SerializeField] private TextAsset gameMessageFile;
    
    [Header("Config")]
    [SerializeField] private char csvSeparator = ';';
    [SerializeField] private int minMessageDelay = 10;
    [SerializeField] private int maxMessageDelay = 180;
    
    [Header("Components")]
    [SerializeField] private MessageController messageController;
    
    private List<PhoneMessage> startMessages = new List<PhoneMessage>();
    private List<PhoneMessage> gameMessages = new List<PhoneMessage>();
    
    private Queue<PhoneMessage> messageQueue = new Queue<PhoneMessage>();
    
    public void SetMessageController(MessageController _messageController)
    {
        messageController = _messageController;
    }
    
    public override void OnStartServer()
    {
        startMessages = ReadMessages(startMessagesFile);
        gameMessages = ReadMessages(gameMessageFile);
    }

    private List<PhoneMessage> ReadMessages(TextAsset source)
    {
        var messages = new List<PhoneMessage>();
        
        using var reader = new System.IO.StringReader(source.text);
        while (reader.ReadLine() is { } line)
        {
            var values = line.Split(csvSeparator);
            messages.Add(new PhoneMessage(values[0], values[1]));
        }

        return messages;
    }
    
    private PhoneMessage RandomMessage(IReadOnlyList<PhoneMessage> messages)
    {
        return messages[Random.Range(0, messages.Count)];
    }
    
    private void ResetMessageQueue()
    {
        messageQueue.Clear();
        var rnd = new System.Random();
        messageQueue = new Queue<PhoneMessage>(
            gameMessages.OrderBy(a => rnd.Next()));
    }
    
    private void SendStartMessage()
    {
        SendMessage(RandomMessage(startMessages));
    }
    
    private void SendGameMessage()
    {
        SendMessage(messageQueue.Dequeue());
        SendGameMessageDelayed();
    }

    private void SendMessage(PhoneMessage message)
    {
        if (messageController == null)
        {
            Debug.LogError("No MessageController set, message lost...");
            return;
        }
        
        messageController.RpcReceiveMessage(message);
    }
    
    private void SendGameMessageDelayed()
    {
        // Send another message after a while
        var delay = Random.Range(minMessageDelay, maxMessageDelay);
        Invoke(nameof(SendGameMessage), delay);
    }

    public void StartMessageLoop()
    {
        ResetMessageQueue();
        SendStartMessage();
        SendGameMessageDelayed();
    }
    
}
