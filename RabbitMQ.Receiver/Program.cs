using RabbitMQ.Receiver;

#region 1.命名队列
//Receive.ReceiveMsg(); 
#endregion

#region 2.工作队列
//Task.Run(() =>
//{
//    Receive.Receive_NewTask("worker1");
//});
//Task.Run(() => {
//    Receive.Receive_NewTask("worker2");
//});

#endregion

#region 3.Publish/Subscribe  
//需要先启动消费者，如果没有Queue绑定到exchange 则消息会丢失
//Task.Run(() =>
//{
//    Receive.Subscribe();
//});
//Task.Run(() =>
//{
//    Receive.Subscribe();
//});
#endregion

#region 4.Routing 
//需要先启动消费者，如果没有Queue绑定到exchange 则消息会丢失
//Task.Run(() =>
//{
//    Receive.Routing_Receive(new string[] { "Info","Error","Warn"} );
//});
//Task.Run(() =>
//{
//    Receive.Routing_Receive(new string[] { "Info"});
//});
#endregion

#region 5.Topics 
//需要先启动消费者，如果没有Queue绑定到exchange 则消息会丢失
//Task.Run(() =>
//{
//    Receive.Topics_Receive(new string[] { "kern.*" });
//});
//Task.Run(() =>
//{
//    Receive.Topics_Receive(new string[] { "*.critical" });
//});
//Task.Run(() =>
//{
//    Receive.Topics_Receive(new string[] { "kern.#", "#.critical" });
//});
#endregion

#region 5.Publisher Confirms 
//需要先启动消费者，如果没有Queue绑定到exchange 则消息会丢失
Task.Run(() =>
{
    Receive.Topics_Receive(new string[] { "kern.*" });
});
#endregion
Console.ReadLine();