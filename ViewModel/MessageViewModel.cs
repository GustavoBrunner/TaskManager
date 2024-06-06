
namespace TaskManager.ViewModel
{
    public class MessageViewModel
    {
        public MessageType Type { get; set; }
        public string Message { get; set; }
        
        public MessageViewModel(string Message, MessageType Type = MessageType.info)
        {
            this.Type = Type;
            this.Message = Message;
        }

        public static string Serialize(string Message, MessageType Type = MessageType.info){
            var m = new MessageViewModel(Message, Type);
            Console.WriteLine(m.Message);
            
            return Newtonsoft.Json.JsonConvert.SerializeObject(m); //System.Text.Json.JsonSerializer.Serialize(m);
        }

        public static MessageViewModel Deserialize(string Message){
            Console.WriteLine(Message);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<MessageViewModel>(Message);
        }
    }
}