using RabbitMQ.Client;
using System.Text;

namespace RabbitMQ.Sender
{
    public static class Send
    {
        /// <summary>
        /// 命名队列
        /// </summary>
        public static void SendMsg()
        {
            //创建连接
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            //声明发送队列 仅当队列不存在时才会创建它 消息内容是一个字节数组
            channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            const string message = "Hello World!";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: string.Empty,
                             routingKey: "hello",
                             basicProperties: null,
                             body: body);
            Console.WriteLine($" [x] Sent {message}");

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
        /// <summary>
        /// 工作队列
        /// </summary>
        public static void Send_NewTask()
        {
            //创建连接
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            //声明发送队列 仅当队列不存在时才会创建它 消息内容是一个字节数组
            channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            string message1 = GetMessage(new string[] { "First message." });
            string message2 = GetMessage(new string[] { "Second message.." });
            string message3 = GetMessage(new string[] { "Third message..." });
            string message4 = GetMessage(new string[] { "Fourth message...." });
            string message5 = GetMessage(new string[] { "Fifth message....." });

            var body1 = Encoding.UTF8.GetBytes(message1);
            var body2 = Encoding.UTF8.GetBytes(message2);
            var body3= Encoding.UTF8.GetBytes(message3);
            var body4 = Encoding.UTF8.GetBytes(message4);
            var body5 = Encoding.UTF8.GetBytes(message5);

            channel.BasicPublish(exchange: string.Empty,
                             routingKey: "hello",
                             basicProperties: null,
                             body: body1);
            Console.WriteLine($" [x] Sent {message1}");
            channel.BasicPublish(exchange: string.Empty,
                             routingKey: "hello",
                             basicProperties: null,
                             body: body2);
            Console.WriteLine($" [x] Sent {message2}");
            channel.BasicPublish(exchange: string.Empty,
                             routingKey: "hello",
                             basicProperties: null,
                             body: body3);
            Console.WriteLine($" [x] Sent {message3}");
            channel.BasicPublish(exchange: string.Empty,
                             routingKey: "hello",
                             basicProperties: null,
                             body: body4);
            Console.WriteLine($" [x] Sent {message4}");
            channel.BasicPublish(exchange: string.Empty,
                             routingKey: "hello",
                             basicProperties: null,
                             body: body5);
            Console.WriteLine($" [x] Sent {message5}");

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
        static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
        }
        /// <summary>
        /// 发布订阅者模式
        /// </summary>
        public static void Publish()
        {
            //创建连接
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            //1.创建名为logs 的exchange
            channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);


            string message1 = GetMessage(new string[] { "First message." });
            var body1 = Encoding.UTF8.GetBytes(message1);


            channel.BasicPublish(exchange: "logs",
                             routingKey: string.Empty,
                             basicProperties: null,
                             body: body1);

            Console.WriteLine($" [x] Sent {message1}");

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
        /// <summary>
        /// Routing
        /// </summary>
        public static void Routing_Send(string severity)
        {
            //创建连接
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            //1.创建名为logs 的exchange
            channel.ExchangeDeclare(exchange: "direct_logs", type: ExchangeType.Direct);


            string message1 = GetMessage(new string[] { $"This is a {severity} message." });
            var body1 = Encoding.UTF8.GetBytes(message1);


            channel.BasicPublish(exchange: "direct_logs",
                             routingKey: severity,
                             basicProperties: null,
                             body: body1);

            Console.WriteLine($" [x] Sent '{severity}':'{message1}'");



        }
        /// <summary>
        /// Topics_Send
        /// </summary>
        /// <param name="routingKey"></param>
        /// <param name="msg"></param>
        public static void Topics_Send(string routingKey,string msg)
        {
            //创建连接
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            //1.创建名为logs 的exchange
            channel.ExchangeDeclare(exchange: "topic_logs", type: ExchangeType.Topic);


            string message1 = GetMessage(new string[] { msg });
            var body1 = Encoding.UTF8.GetBytes(message1);


            channel.BasicPublish(exchange: "topic_logs",
                             routingKey: routingKey,
                             basicProperties: null,
                             body: body1);

            Console.WriteLine($" [x] Sent '{routingKey}':'{message1}'");



        }

    }
}
