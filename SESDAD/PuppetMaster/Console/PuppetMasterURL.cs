﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;

using SESDAD.PuppetMaster.CommonTypes;

namespace SESDAD.PuppetMaster {
    public class PuppetMasterURL : MarshalByRefObject, IPuppetMasterURL {

        public void ExecuteSiteCommand(
            String siteName,
            String parentName) {
                Console.WriteLine("Site");
        }
        public void ExecuteBrokerCommand(
            String brokerName,
            String siteName,
            String urlName) {
                Console.WriteLine("Broker");
        }
        public void ExecutePublisherCommand(
            String publisherName,
            String siteName,
            String urlName) {
                Console.WriteLine("Publisher");
        }
        public void ExecuteSubscriberCommand(
            String subscriberName,
            String siteName,
            String urlName) {
                Console.WriteLine("Subscriber");
        }
        public void ExecuteFloodingRoutingPolicyCommand() {
            Console.WriteLine("FloodingRoutingPolicy");
        }
        public void ExecuteFilterRoutingPolicyCommand() {
            Console.WriteLine("FilterRoutingPolicy");
        }
        public void ExecuteNoOrderingCommand() {
            Console.WriteLine("NoOrdering");
        }
        public void ExecuteFIFOOrderingCommand() {
            Console.WriteLine("FIFOOrdering");
        }
        public void ExecuteTotalOrderingCommand() {
            Console.WriteLine("TotalOrdering");
        }
        public void ExecuteSubscribeCommand(
            String subscriberName,
            String topicName) {
                Console.WriteLine("Subscribe");
        }
        public void ExecuteUnsubscribeCommand(
            String subscriberName,
            String topicName) {
                Console.WriteLine("Unsubscribe");
        }
        public void ExecutePublishCommand(
            String publisherName,
            int publishTimes,
            String topicName,
            int intervalTime) {
                Console.WriteLine("Publish");
        }
        public void ExecuteStatusCommand() {
            Console.WriteLine("Status");
        }
        public void ExecuteCrashCommand(String processName) {
            Console.WriteLine("Crash");
        }
        public void ExecuteFreezeCommand(String processName) {
            Console.WriteLine("Freeze");
        }
        public void ExecuteUnfreezeCommand(String processName) {
            Console.WriteLine("Unfreeze");
        }
        public void ExecuteWaitCommand(int waitingTime) {
            Console.WriteLine("Wait");
        }
        public void ExecuteFullLoggingLevelCommand() {
            Console.WriteLine("FullLoggingLevel");
        }
        public void ExecuteLightLoggingLevelCommand() {
            Console.WriteLine("LightLoggingLevel");
        }
    }
}