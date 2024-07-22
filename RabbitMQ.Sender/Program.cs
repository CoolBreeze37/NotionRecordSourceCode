using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Sender;

#region 1.命名队列
//Send.SendMsg();
#endregion

#region 2.工作队列
//Send.Send_NewTask();
#endregion

#region 3.Publish/Subscribe
//Send.Publish();
#endregion

#region 4.Routing
//Send.Routing_Send("Info");
//Send.Routing_Send("Error");
//Console.WriteLine(" Press [enter] to exit.");
//Console.ReadLine();
#endregion

//#region 5.Topic
//Send.Topics_Send("kern.info", "A kern info message");
//Send.Topics_Send("unkern.critical", "A unkern critical message");
//Send.Topics_Send("kern.critical.info", "A critical kernel error");
//Console.WriteLine(" Press [enter] to exit.");
//Console.ReadLine();
//#endregion

#region Publisher Confirms
PublisherConfirms.PublishMessagesIndividually();
PublisherConfirms.PublishMessagesInBatch();
await PublisherConfirms.HandlePublishConfirmsAsynchronously();
Console.ReadLine();
#endregion