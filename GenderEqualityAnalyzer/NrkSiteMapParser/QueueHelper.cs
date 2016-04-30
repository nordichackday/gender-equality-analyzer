using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace NrkSiteMapParser
{
    public class QueueHelper
    {
        private const string Connectionstring =
            "Endpoint=sb://genderrepresentationindex.servicebus.windows.net/;SharedAccessKeyName=WriteToQueue;SharedAccessKey=13JaZ5c3yJBtZ05fNRvOlJkZw9j0FS+VpasqbDEgCkM=;EntityPath=htmlparserqueue";
        private const string QueueName = "htmlparserqueue";

        private readonly QueueClient _client;
        public QueueHelper()
        {
            //var nsManager = new NamespaceManager(Connectionstring);

            //if (!nsManager.QueueExists(QueueName))
            //    nsManager.CreateQueue(QueueName);

            _client = QueueClient.CreateFromConnectionString(Connectionstring);
        }

        public async void Queue(ArticleInformation articleInfo)
        {
            var msg = new BrokeredMessage(articleInfo);

            await _client.SendAsync(msg);
        }

    }
}
