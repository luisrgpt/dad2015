﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SESDAD.CommonTypes;
using SESDAD.Processes;

namespace SESDAD.Processes {
    public interface IMessageBroker : IProcess {
        ///<summary>
        /// Broker Interface name
        ///</summary>
        String ProcessName { get; }
        ///<summary>
        /// Broker Interface routing policy
        ///</summary>
        RoutingPolicyType RoutingPolicy { set; }
        ///<summary>
        /// Broker Interface ordering
        ///</summary>
        OrderingType Ordering { set; }
        void ForwardEntry(String processName, String processURL, Entry entry);
        void registerSubscription(String processName, String processURL, String topicName);
        void removeSubscription(String processName, String processURL, String topicName);

        void RegisterBroker(String processName, String processURL);
        void RegisterSubscriber(String processName, String processURL);
        void RegisterPublisher(String processName, String processURL);
    }
}