using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Messaging
{
    public class Message : Dictionary<string, object>
    {
        public Message(Component source)
        {
            Add("source", source);
        }
    }
}