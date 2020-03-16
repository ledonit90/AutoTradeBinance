using Newtonsoft.Json.Linq;
using Remibit.Utility.RabittMQ;

namespace Remitano.Helper.Helper
{
    public class LoggingHelper
    {
        private static Publisher pub = new Publisher();
        /// <summary>
        /// log for information
        /// </summary>
        /// <param name="message"></param>
        public static void LogInfo(string message)
        {
            pub.PublishMessage(string.Format("{0}_{1}", "LogInfo", message));
        }

        /// <summary>
        /// log for debugging
        /// </summary>
        /// <param name="message"></param>
        public static void LogDebug(string message)
        {
            // publish message to rabbitMQ
            //pub.PublishMessage(string.Format("{0}_{1}", "LogDebug", message));

        }

        /// <summary>
        /// log for the exception
        /// </summary>
        /// <param name="message"></param>
        public static void LogException(string message)
        {
            pub.PublishMessage(string.Format("{0}_{1}", "LogException", message));
        }

        // Save the error message to the Server log
        private static void SendToLogServer(string uriHttpSaveLog, string module, string type, JObject message)
        {
            //Send request to server
            JObject messageBody = new JObject();
            messageBody.Add("host", "ID Address");
            messageBody.Add("component", "VGR.HRM.DataHub");
            messageBody.Add("datetime", DateTimeHelper.GetCurrentDateTimeStr("yyyy/MM/dd hh:mm:ss"));
            messageBody.Add("type", type);
            messageBody.Add("message", message);

            //HTTPUtil.Send(host, messageBody);
        }
    }
}
