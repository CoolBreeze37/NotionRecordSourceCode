using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace RabbitMQ.Receiver
{
    public class Receive
    {
        /// <summary>
        /// 命名队列
        /// </summary>
        public static void ReceiveMsg()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            //请注意，我们也在此处声明了队列。由于我们可能在发布者之前启动消费者，因此我们希望在尝试从队列中消费消息之前确保队列存在。

            channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            Console.WriteLine(" [*] Waiting for messages.");


            //我们即将告诉服务器从队列中向我们发送消息。由于它将异步推送消息给我们，因此我们提供了一个回调。这就是EventingBasicConsumer.Received事件处理程序的作用。
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Received {message}");
            };
            channel.BasicConsume(queue: "hello",
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
        /// <summary>
        /// 工作队列
        /// </summary>
        public static void Receive_NewTask(string workerName)
        {

            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            //请注意，我们也在此处声明了队列。由于我们可能在发布者之前启动消费者，因此我们希望在尝试从队列中消费消息之前确保队列存在。

            channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            Console.WriteLine($"[{workerName}]-[*] Waiting for messages.");


            //我们即将告诉服务器从队列中向我们发送消息。由于它将异步推送消息给我们，因此我们提供了一个回调。这就是EventingBasicConsumer.Received事件处理程序的作用。
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[{workerName}]-[x] Received {message}");

                int dots = message.Split('.').Length - 1;
                Thread.Sleep(dots * 1000);

                Console.WriteLine($"[{workerName}]-{message}-[x] Done");
            };
            channel.BasicConsume(queue: "hello",
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
            
        }
        /// <summary>
        /// Publish/Subscribe
        /// </summary>
        ///
        public static void Subscribe()
        {
            //创建连接
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

            //2.我们将创建一个具有生成名称的非持久、独占、自动删除队列：
            var queueName = channel.QueueDeclare().QueueName;
            //3.将队列绑定到exchange
            channel.QueueBind(queue: queueName,
                exchange: "logs",
                routingKey: string.Empty);

            Console.WriteLine(" [*] Waiting for logs.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] {message}");
            };
            channel.BasicConsume(queue:queueName,
                autoAck:true,
                consumer:consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
        /// <summary>
        /// Routing
        /// </summary>
        public static void Routing_Receive(string[] routingKeys)
        {
            //创建连接
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "direct_logs", type: ExchangeType.Direct);

            //2.我们将创建一个具有生成名称的非持久、独占、自动删除队列：
            var queueName = channel.QueueDeclare().QueueName;
            //3.将队列绑定到exchange
            foreach (var severity in routingKeys)
            {
                channel.QueueBind(queue: queueName,
                exchange: "direct_logs",
                routingKey: severity);
            }
            

    

            Console.WriteLine($" [{queueName}] Waiting for messages.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                Console.WriteLine($" [{queueName}] Received '{routingKey}':'{message}'");
            };
            channel.BasicConsume(queue:queueName,
                autoAck:true,
                consumer:consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
        /// <summary>
        /// Topics
        /// </summary>
        /// <param name="routingKeys"></param>
        public static void Topics_Receive(string[] routingKeys)
        {
            //创建连接
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "topic_logs", type: ExchangeType.Topic);

            //2.我们将创建一个具有生成名称的非持久、独占、自动删除队列：
            var queueName = channel.QueueDeclare().QueueName;
            //3.将队列绑定到exchange
            foreach (var routingKey in routingKeys)
            {
                channel.QueueBind(queue: queueName,
                exchange: "topic_logs",
                routingKey: routingKey);
            }

            Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                Console.WriteLine($" [{queueName}] Received '{routingKey}':'{message}'");
            };
            channel.BasicConsume(queue:queueName,
                autoAck:true,
                consumer:consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
