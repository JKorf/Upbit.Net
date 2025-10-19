using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
