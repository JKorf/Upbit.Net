using System.Text.Json.Serialization;

namespace Upbit.Net.Objects.Internal
{
    internal class SocketTicket
    {
        [JsonPropertyName("ticket")]
        public string Ticket { get; set; }

        public SocketTicket(int id)
        {
            Ticket = id.ToString();
        }
    }
}
