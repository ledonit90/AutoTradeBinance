using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using ProcessDataHelper.Models;
using System;
using System.Timers;

namespace ProcessDataHelper
{
    public class Subscriber
    {
        #region Configuartion Variable
        private ConnectionFactory _cf;
        private IConnection _conn;
        private IModel _channel;
        private string queueName;
        private string messageActionQueueName;
        private string exchangeName;
        private IDatabase db;
        private string keySellerRedis;
        private string keyBuyerRedis;
        private string keyPeekRedis;
        private string keyMaxSpeedRedis;
        #endregion
        #region Calculator Variable
        private double currentSellPrice = 0;
        private double currentBuyPrice = 0;
        private int Count1s = 0;
        private int MaxSpeed1s = 0;
        private int votedTrade;  // chấm điểm theo các yếu tố: theo thang điểm 100, các thang điểm theo trọng số.
        private bool intendOfMarket = false; // đây là biến cần kết luận được là có nên mua hay bán ...
        private bool curIntendOfMarket = true; // 0 decrement - giảm xuống, 1 increment - tăng lên 
        private long currentServerTime = 0;
        #endregion

        #region Time of Estimate
        #region Redis Keywords
        private string keyMessage1mRedis;
        private string keyMessage3mRedis;
        private string keyMessage5mRedis;
        private string keyMessage10mRedis;
        private string keyMessage15mRedis;
        private string keyMessage30mRedis;
        private string keyMessage1hRedis;
        private string keyMessage5hRedis;
        private string keyMessage1DRedis;
        #endregion
        #region 100 variable
        private Message1m curMessage100 = new Message1m();
        private long cur100Time = 0;
        #endregion
        #region 200 variable
        private Message1m curMessage200 = new Message1m();
        private long cur200Time = 0;
        #endregion
        #region 500 variable
        private Message1m curMessage500 = new Message1m();
        private long cur500Time = 0;
        #endregion
        #region 1m variable
        private Message1m curMessage1m = new Message1m();
        private long cur1mTime = 0;
        private double ClosePrice1m = 0;
        #endregion
        #region 3m variable
        private Message1m curMessage3m = new Message1m();
        private long cur3mTime = 0;
        #endregion
        #region 5m variable
        private Message1m curMessage5m = new Message1m();
        private long cur5mTime = 0;
        #endregion
        #region 10m variable
        private Message1m curMessage10m = new Message1m();
        private long cur10mTime = 0;
        #endregion
        #region 15m variable
        private Message1m curMessage15m = new Message1m();
        private long cur15mTime = 0;
        #endregion
        #region 30m variable
        private Message1m curMessage30m = new Message1m();
        private long cur30mTime = 0;
        #endregion
        #region 1h variable
        private Message1m curMessage1h= new Message1m();
        private long cur1hTime = 0;
        #endregion
        #region 5h variable
        private Message1m curMessage5h = new Message1m();
        private long cur5hTime = 0;
        #endregion
        #region 1D variable
        private Message1m curMessage1d = new Message1m();
        private long cur1dTime = 0;
        #endregion
        #endregion

        public Subscriber(string hostname, int port, string exchangeName, string queueName, bool durable = true, bool exclusive = false, bool autoDelete = false, IDictionary<string, object> arguments = null)
        {
            this.queueName = queueName;
            this.exchangeName = exchangeName;
            messageActionQueueName = "message" + queueName;
            db = RedisStore.RedisCache;
            #region config key of Redis
            keySellerRedis = "S" + queueName;
            keyBuyerRedis = "B" + queueName;
            keyPeekRedis = "Peek" + queueName;
            keyMaxSpeedRedis = "MaxSpeed" + queueName;
            keyMessage1mRedis = "message1m" + queueName;
            keyMessage3mRedis = "message3m" + queueName;
            keyMessage5mRedis = "message5m" + queueName;
            keyMessage10mRedis = "message10m" + queueName;
            keyMessage15mRedis = "message15m" + queueName;
            keyMessage30mRedis = "message30m" + queueName;
            keyMessage1hRedis = "message1h" + queueName;
            keyMessage5hRedis = "message5h" + queueName;
            keyMessage1DRedis = "message1D" + queueName;
            #endregion

            _cf = new ConnectionFactory() { HostName = hostname };
            _conn = _cf.CreateConnection();
            _channel = _conn.CreateModel();
            _channel.ExchangeDeclare(exchange: exchangeName, type: "topic");

            _channel.QueueDeclare(queue: messageActionQueueName,
                                     durable: durable,
                                     exclusive: exclusive,
                                     autoDelete: autoDelete,
                                     arguments: arguments);

            _channel.QueueBind(queue: queueName,
                              exchange: exchangeName,
                              routingKey: queueName);

            #region Get Value from Redis
            var tempMaxSpeed = db.StringGet(keyMaxSpeedRedis);
            MaxSpeed1s = string.IsNullOrEmpty(tempMaxSpeed) ? 0 : int.Parse(tempMaxSpeed);
            #endregion

        }

        public void SendMessageAction(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(   exchange: exchangeName,
                                     routingKey: messageActionQueueName,
                                     basicProperties: null,
                                     body: body);
        }

        public void CloseChannel(IModel channel)
        {
            channel.Close();
        }

        public void SubcribeAChannel()
        {
            var consumer = new EventingBasicConsumer(_channel);
            Timer aTimer;
            aTimer = new Timer();
            aTimer.Interval = 1000;

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += (source,e) => {
                if(Count1s > MaxSpeed1s)
                {
                    MaxSpeed1s = Count1s;
                    db.StringSet(keyMaxSpeedRedis, Count1s.ToString());
                    HashEntry[] redisBookHash =
                    {
                        new HashEntry(currentServerTime, MaxSpeed1s)
                    };
                    db.HashSet("MaxspeedList" + queueName, redisBookHash);
                }
                Console.WriteLine(MaxSpeed1s);
                Count1s = 0;
            };

            consumer.Received += (model, ea) =>
            {
                

                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                var trademessage = JsonConvert.DeserializeObject<TradeMessage>(message);
                currentServerTime = trademessage.T;
                #region Highest and Lowest with time
                #region 1s Check
                Count1s++;
                #endregion
                #region 1m Check
                // check neu lon hon 1 phut da dinh thi refresh lai tinh toan

                if(trademessage.T > cur1mTime + 60000)
                {
                    if (cur1mTime == 0)
                    {
                        var _tempcur1mTime = trademessage.T / 60000;
                        cur1mTime = (_tempcur1mTime - 1) * 60000;

                        cur1mTime = cur1mTime + 60000;
                        curMessage1m.HP = 0;
                        curMessage1m.HPT = 0;
                        curMessage1m.LP = 0;
                        curMessage1m.LPT = 0;
                        curMessage1m.I = false;
                        curMessage1m.T = cur1mTime;
                    }
                    else
                    {
                        curMessage1m.C = ClosePrice1m;
                        curMessage1m.I = curMessage1m.O < curMessage1m.C;
                        var strCurMessage1m = JsonConvert.SerializeObject(curMessage1m);
                        HashEntry[] redisBookHash =
                            {
                            new HashEntry(cur1mTime, strCurMessage1m)
                        };
                        db.HashSet(keyMessage1mRedis, redisBookHash);

                        cur1mTime = cur1mTime + 60000;
                        curMessage1m.HP = 0;
                        curMessage1m.HPT = 0;
                        curMessage1m.LP = 0;
                        curMessage1m.LPT = 0;
                        curMessage1m.O = trademessage.p;
                        curMessage1m.I = false;
                        curMessage1m.T = cur1mTime;
                    }
                    // luu du lieu cu vao DB
                    
                } 

                if (trademessage.p > curMessage1m.HP || curMessage1m.HP == 0)
                {
                    curMessage1m.HP = trademessage.p;
                    curMessage1m.HPT = trademessage.T;
                }

                if (trademessage.p < curMessage1m.LP || curMessage1m.LP == 0)
                {
                    curMessage1m.LP = trademessage.p;
                    curMessage1m.LPT = trademessage.T;
                }

                #endregion
                #region 3M Check
                // check neu lon hon 1 H da dinh thi refresh lai tinh toan

                if (trademessage.T > cur3mTime + 180000)
                {
                    if (cur3mTime == 0)
                    {
                        var _tempcur3mTime = trademessage.T / 180000;
                        cur3mTime = (_tempcur3mTime - 1) * 180000;

                        cur3mTime = cur3mTime + 180000;
                        curMessage3m.HP = 0;
                        curMessage3m.HPT = 0;
                        curMessage3m.LP = 0;
                        curMessage3m.LPT = 0;
                        curMessage3m.I = false;
                        curMessage3m.T = cur3mTime;
                    }
                    else
                    {
                        curMessage3m.C = ClosePrice1m;
                        curMessage3m.I = curMessage3m.O < curMessage3m.C;
                        var strcurMessage3m = JsonConvert.SerializeObject(curMessage3m);
                        HashEntry[] redisBookHash =
                            {
                            new HashEntry(cur3mTime, strcurMessage3m)
                        };
                        db.HashSet(keyMessage3mRedis, redisBookHash);

                        cur3mTime = cur3mTime + 180000;
                        curMessage3m.HP = 0;
                        curMessage3m.HPT = 0;
                        curMessage3m.LP = 0;
                        curMessage3m.LPT = 0;
                        curMessage3m.O = trademessage.p;
                        curMessage3m.I = false;
                        curMessage3m.T = cur3mTime;
                    }
                    // luu du lieu cu vao DB

                }

                if (trademessage.p > curMessage3m.HP || curMessage3m.HP == 0)
                {
                    curMessage3m.HP = trademessage.p;
                    curMessage3m.HPT = trademessage.T;
                }

                if (trademessage.p < curMessage3m.LP || curMessage3m.LP == 0)
                {
                    curMessage3m.LP = trademessage.p;
                    curMessage3m.LPT = trademessage.T;
                }
                #endregion
                #region 5M Check
                // check neu lon hon 1 H da dinh thi refresh lai tinh toan

                if (trademessage.T > cur5mTime + 300000)
                {
                    if (cur5mTime == 0)
                    {
                        var _tempcur5mTime = trademessage.T / 300000;
                        cur5mTime = (_tempcur5mTime - 1) * 300000;

                        cur5mTime = cur5mTime + 300000;
                        curMessage5m.HP = 0;
                        curMessage5m.HPT = 0;
                        curMessage5m.LP = 0;
                        curMessage5m.LPT = 0;
                        curMessage5m.I = false;
                        curMessage5m.T = cur5mTime;
                    }
                    else
                    {
                        curMessage5m.C = ClosePrice1m;
                        curMessage5m.I = curMessage5m.O < curMessage5m.C;
                        var strcurMessage5m = JsonConvert.SerializeObject(curMessage5m);
                        HashEntry[] redisBookHash =
                            {
                            new HashEntry(cur5mTime, strcurMessage5m)
                        };
                        db.HashSet(keyMessage5mRedis, redisBookHash);

                        cur5mTime = cur5mTime + 300000;
                        curMessage5m.HP = 0;
                        curMessage5m.HPT = 0;
                        curMessage5m.LP = 0;
                        curMessage5m.LPT = 0;
                        curMessage5m.O = trademessage.p;
                        curMessage5m.I = false;
                        curMessage5m.T = cur5mTime;
                    }
                    // luu du lieu cu vao DB

                }

                if (trademessage.p > curMessage5m.HP || curMessage5m.HP == 0)
                {
                    curMessage5m.HP = trademessage.p;
                    curMessage5m.HPT = trademessage.T;
                }

                if (trademessage.p < curMessage5m.LP || curMessage5m.LP == 0)
                {
                    curMessage5m.LP = trademessage.p;
                    curMessage5m.LPT = trademessage.T;
                }
                #endregion
                #region 10M Check
                // check neu lon hon 1 H da dinh thi refresh lai tinh toan

                if (trademessage.T > cur10mTime + 600000)
                {
                    if (cur10mTime == 0)
                    {
                        var _tempcur10mTime = trademessage.T / 600000;
                        cur10mTime = (_tempcur10mTime - 1) * 600000;

                        cur10mTime = cur10mTime + 600000;
                        curMessage10m.HP = 0;
                        curMessage10m.HPT = 0;
                        curMessage10m.LP = 0;
                        curMessage10m.LPT = 0;
                        curMessage10m.I = false;
                        curMessage10m.T = cur10mTime;
                    }
                    else
                    {
                        curMessage10m.C = ClosePrice1m;
                        curMessage10m.I = curMessage10m.O < curMessage10m.C;
                        var strcurMessage10m = JsonConvert.SerializeObject(curMessage10m);
                        HashEntry[] redisBookHash =
                            {
                            new HashEntry(cur10mTime, strcurMessage10m)
                        };
                        db.HashSet(keyMessage10mRedis, redisBookHash);

                        cur10mTime = cur10mTime + 600000;
                        curMessage10m.HP = 0;
                        curMessage10m.HPT = 0;
                        curMessage10m.LP = 0;
                        curMessage10m.LPT = 0;
                        curMessage10m.O = trademessage.p;
                        curMessage10m.I = false;
                        curMessage10m.T = cur10mTime;
                    }
                    // luu du lieu cu vao DB

                }

                if (trademessage.p > curMessage10m.HP || curMessage10m.HP == 0)
                {
                    curMessage10m.HP = trademessage.p;
                    curMessage10m.HPT = trademessage.T;
                }

                if (trademessage.p < curMessage10m.LP || curMessage10m.LP == 0)
                {
                    curMessage10m.LP = trademessage.p;
                    curMessage10m.LPT = trademessage.T;
                }
                #endregion
                #region 15M Check
                // check neu lon hon 1 H da dinh thi refresh lai tinh toan

                if (trademessage.T > cur15mTime + 900000)
                {
                    if (cur15mTime == 0)
                    {
                        var _tempcur15mTime = trademessage.T / 900000;
                        cur15mTime = (_tempcur15mTime - 1) * 900000;

                        cur15mTime = cur15mTime + 900000;
                        curMessage15m.HP = 0;
                        curMessage15m.HPT = 0;
                        curMessage15m.LP = 0;
                        curMessage15m.LPT = 0;
                        curMessage15m.I = false;
                        curMessage15m.T = cur15mTime;
                    }
                    else
                    {
                        curMessage15m.C = ClosePrice1m;
                        curMessage15m.I = curMessage15m.O < curMessage15m.C;
                        var strcurMessage15m = JsonConvert.SerializeObject(curMessage15m);
                        HashEntry[] redisBookHash =
                            {
                            new HashEntry(cur15mTime, strcurMessage15m)
                        };
                        db.HashSet(keyMessage15mRedis, redisBookHash);

                        cur15mTime = cur15mTime + 900000;
                        curMessage15m.HP = 0;
                        curMessage15m.HPT = 0;
                        curMessage15m.LP = 0;
                        curMessage15m.LPT = 0;
                        curMessage15m.O = trademessage.p;
                        curMessage15m.I = false;
                        curMessage15m.T = cur15mTime;
                    }
                    // luu du lieu cu vao DB

                }

                if (trademessage.p > curMessage15m.HP || curMessage15m.HP == 0)
                {
                    curMessage15m.HP = trademessage.p;
                    curMessage15m.HPT = trademessage.T;
                }

                if (trademessage.p < curMessage15m.LP || curMessage15m.LP == 0)
                {
                    curMessage15m.LP = trademessage.p;
                    curMessage15m.LPT = trademessage.T;
                }
                #endregion
                #region 30M Check
                // check neu lon hon 1 H da dinh thi refresh lai tinh toan

                if (trademessage.T > cur30mTime + 1800000)
                {
                    if (cur30mTime == 0)
                    {
                        var _tempcur30mTime = trademessage.T / 1800000;
                        cur30mTime = (_tempcur30mTime - 1) * 1800000;

                        cur30mTime = cur30mTime + 1800000;
                        curMessage30m.HP = 0;
                        curMessage30m.HPT = 0;
                        curMessage30m.LP = 0;
                        curMessage30m.LPT = 0;
                        curMessage30m.I = false;
                        curMessage30m.T = cur30mTime;
                    }
                    else
                    {
                        curMessage30m.C = ClosePrice1m;
                        curMessage30m.I = curMessage30m.O < curMessage30m.C;
                        var strcurMessage30m = JsonConvert.SerializeObject(curMessage30m);
                        HashEntry[] redisBookHash =
                            {
                            new HashEntry(cur30mTime, strcurMessage30m)
                        };
                        db.HashSet(keyMessage30mRedis, redisBookHash);

                        cur30mTime = cur30mTime + 1800000;
                        curMessage30m.HP = 0;
                        curMessage30m.HPT = 0;
                        curMessage30m.LP = 0;
                        curMessage30m.LPT = 0;
                        curMessage30m.O = trademessage.p;
                        curMessage30m.I = false;
                        curMessage30m.T = cur30mTime;
                    }
                    // luu du lieu cu vao DB

                }

                if (trademessage.p > curMessage30m.HP || curMessage30m.HP == 0)
                {
                    curMessage30m.HP = trademessage.p;
                    curMessage30m.HPT = trademessage.T;
                }

                if (trademessage.p < curMessage30m.LP || curMessage30m.LP == 0)
                {
                    curMessage30m.LP = trademessage.p;
                    curMessage30m.LPT = trademessage.T;
                }
                #endregion
                #region 1h Check
                // check neu lon hon 1 H da dinh thi refresh lai tinh toan

                if (trademessage.T > cur1hTime + 3600000)
                {
                    if (cur1hTime == 0)
                    {
                        var _tempcur1hTime = trademessage.T / 3600000;
                        cur1hTime = (_tempcur1hTime - 1) * 3600000;

                        cur1hTime = cur1hTime + 3600000;
                        curMessage1h.HP = 0;
                        curMessage1h.HPT = 0;
                        curMessage1h.LP = 0;
                        curMessage1h.LPT = 0;
                        curMessage1h.I = false;
                        curMessage1h.T = cur1hTime;
                    }
                    else
                    {
                        curMessage1h.C = ClosePrice1m;
                        curMessage1h.I = curMessage1h.O < curMessage1h.C;
                        var strcurMessage1h = JsonConvert.SerializeObject(curMessage1h);
                        HashEntry[] redisBookHash =
                            {
                            new HashEntry(cur1hTime, strcurMessage1h)
                        };
                        db.HashSet(keyMessage1hRedis, redisBookHash);

                        cur1hTime = cur1hTime + 3600000;
                        curMessage1h.HP = 0;
                        curMessage1h.HPT = 0;
                        curMessage1h.LP = 0;
                        curMessage1h.LPT = 0;
                        curMessage1h.O = trademessage.p;
                        curMessage1h.I = false;
                        curMessage1h.T = cur1hTime;
                    }
                    // luu du lieu cu vao DB

                }

                if (trademessage.p > curMessage1h.HP || curMessage1h.HP == 0)
                {
                    curMessage1h.HP = trademessage.p;
                    curMessage1h.HPT = trademessage.T;
                }

                if (trademessage.p < curMessage1h.LP || curMessage1h.LP == 0)
                {
                    curMessage1h.LP = trademessage.p;
                    curMessage1h.LPT = trademessage.T;
                }
                #endregion
                #region 5h Check
                // check neu lon hon 1 H da dinh thi refresh lai tinh toan

                if (trademessage.T > cur5hTime + 18000000)
                {
                    if (cur5hTime == 0)
                    {
                        var _tempcur5hTime = trademessage.T / 18000000;
                        cur5hTime = (_tempcur5hTime - 1) * 18000000;

                        cur5hTime = cur5hTime + 18000000;
                        curMessage5h.HP = 0;
                        curMessage5h.HPT = 0;
                        curMessage5h.LP = 0;
                        curMessage5h.LPT = 0;
                        curMessage5h.I = false;
                        curMessage5h.T = cur5hTime;
                    }
                    else
                    {
                        curMessage5h.C = ClosePrice1m;
                        curMessage5h.I = curMessage5h.O < curMessage5h.C;
                        var strcurMessage5h = JsonConvert.SerializeObject(curMessage5h);
                        HashEntry[] redisBookHash =
                            {
                            new HashEntry(cur5hTime, strcurMessage5h)
                        };
                        db.HashSet(keyMessage5hRedis, redisBookHash);

                        cur5hTime = cur5hTime + 18000000;
                        curMessage5h.HP = 0;
                        curMessage5h.HPT = 0;
                        curMessage5h.LP = 0;
                        curMessage5h.LPT = 0;
                        curMessage5h.O = trademessage.p;
                        curMessage5h.I = false;
                        curMessage5h.T = cur5hTime;
                    }
                    // luu du lieu cu vao DB

                }

                if (trademessage.p > curMessage5h.HP || curMessage5h.HP == 0)
                {
                    curMessage5h.HP = trademessage.p;
                    curMessage5h.HPT = trademessage.T;
                }

                if (trademessage.p < curMessage5h.LP || curMessage5h.LP == 0)
                {
                    curMessage5h.LP = trademessage.p;
                    curMessage5h.LPT = trademessage.T;
                }
                #endregion
                #region 1d Check
                // check neu lon hon 1 H da dinh thi refresh lai tinh toan

                if (trademessage.T > cur1dTime + 86400000)
                {
                    if (cur1dTime == 0)
                    {
                        var _tempcur1dTime = trademessage.T / 86400000;
                        cur1dTime = (_tempcur1dTime - 1) * 86400000;

                        cur1dTime = cur1dTime + 86400000;
                        curMessage1d.HP = 0;
                        curMessage1d.HPT = 0;
                        curMessage1d.LP = 0;
                        curMessage1d.LPT = 0;
                        curMessage1d.I = false;
                        curMessage1d.T = cur1dTime;
                    }
                    else
                    {
                        curMessage1d.C = ClosePrice1m;
                        curMessage1d.I = curMessage1d.O < curMessage1d.C;
                        var strcurMessage1d = JsonConvert.SerializeObject(curMessage1d);
                        HashEntry[] redisBookHash =
                            {
                            new HashEntry(cur1dTime, strcurMessage1d)
                        };
                        db.HashSet(keyMessage1DRedis, redisBookHash);

                        cur1dTime = cur1dTime + 86400000;
                        curMessage1d.HP = 0;
                        curMessage1d.HPT = 0;
                        curMessage1d.LP = 0;
                        curMessage1d.LPT = 0;
                        curMessage1d.O = trademessage.p;
                        curMessage1d.I = false;
                        curMessage1d.T = cur1dTime;
                    }
                    // luu du lieu cu vao DB

                }

                if (trademessage.p > curMessage1d.HP || curMessage1d.HP == 0)
                {
                    curMessage1d.HP = trademessage.p;
                    curMessage1d.HPT = trademessage.T;
                }

                if (trademessage.p < curMessage1d.LP || curMessage1d.LP == 0)
                {
                    curMessage1d.LP = trademessage.p;
                    curMessage1d.LPT = trademessage.T;
                }
                #endregion
                #endregion
                ClosePrice1m = trademessage.p;
                if (trademessage.m == true)
                {
                    var currentPrice = trademessage.p;
                    if(currentPrice - currentBuyPrice > 0.5 || currentPrice - currentBuyPrice < -0.5)
                    {
                        currentBuyPrice = currentPrice;
                        //GioiHan12TiengBUY.Enqueue(trademessage.T);
                        HashEntry[] redisBookHash =
                        {
                            new HashEntry(trademessage.T, trademessage.p)
                        };
                        db.HashSet(keyBuyerRedis, redisBookHash);
                        SendMessageAction(trademessage.q);
                    }
                }
                else
                {
                    var currentPrice = trademessage.p;
                    if (currentPrice - currentSellPrice > 0.5 || currentPrice - currentSellPrice < -0.5)
                    {
                        currentSellPrice = currentPrice;
                        //GioiHan12TiengSELL.Enqueue(trademessage.T);
                        HashEntry[] redisBookHash =
                        {
                            new HashEntry(trademessage.T, trademessage.p)
                        };
                        db.HashSet(keySellerRedis, redisBookHash);
                    }
                }
            };

            _channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
        #region Calculate Helper

        public void FindPeek(long time, double price)
        {
            // set follow by currentTime
            curIntendOfMarket = true;
            intendOfMarket = true;
        }

        public void Cal24hHighestAndLowest(long time,double price)
        {
            
        }

        //public bool CalculateIntendOfTrade1m(Message1m message)
        //{
        //    // tinh toan so sanh gia LP va HP voi phut truoc
        //    // neu ca 2 deu lon hon => la Green candle
        //    var phuttruoc = db.HashGet(keyMessage1mRedis, (message.T - 60000).ToString()).ToString();
        //    if(phuttruoc != null)
        //    {
        //        Message1m phutTruocObject = JsonConvert.DeserializeObject<Message1m>(phuttruoc);
        //        if (phutTruocObject.HP < message.HP && phutTruocObject.LP < message.LP)
        //        {
        //            return true;
        //        }
        //        else if(phutTruocObject.HP > message.HP && phutTruocObject.LP > message.LP)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            if (message.C > message.O)
        //            {
        //                return true;
        //            }
        //            else return false;
        //        }
        //    }

        //    return false;
        //}
        #endregion
    }
}
