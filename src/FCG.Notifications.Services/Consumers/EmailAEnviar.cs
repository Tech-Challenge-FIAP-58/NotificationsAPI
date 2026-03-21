using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Notifications.Services.Consumers
{
    public class EmailAEnviar
    {
        public string destinatario { get; set; }
        public string assunto { get; set; }
        public string corpo { get; set; }
    }
}
