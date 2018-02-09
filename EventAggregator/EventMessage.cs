using System.Collections.Generic;

namespace EventAggregator
{
    /// <summary>
    /// 事件消息参数
    /// </summary>
    public class EventMessage
    {
        /// <summary>
        /// 事件消息的Id
        /// </summary>
        public string EventMessageId { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="parameters"></param>
        public EventMessage(string messageId, params KeyValuePair<string, object>[] parameters)
        {
            EventMessageId = messageId;
            _parameters = new Dictionary<string, object>();

            if (_parameters != null)
            {
                foreach (KeyValuePair<string, object> keyValuePair in parameters)
                {
                    _parameters.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
        }

        /// <summary>
        /// 获取事件中的参数值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetValue<T>(string key)
        {
            return (T) _parameters[key];
        }

        /// <summary>
        /// 设置事件中的参数值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetValue(string key, object value)
        {
            if (_parameters.ContainsKey(key))
            {
                _parameters.Remove(key);
            }

            _parameters.Add(key,value);
        }


        private readonly Dictionary<string, object> _parameters;
    }
}