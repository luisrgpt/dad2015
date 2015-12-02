﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

using SESDAD.Commons;

namespace SESDAD.Processes {


    public class EventOrderManager {
        public OrderingType Ordering { get; set; }

        private ConcurrentQueue<EventContainer> brokerQueue;
        private ConcurrentQueue<Event> subscriberQueue;
        //FIFO stuff
        private IDictionary<ProcessHeader, EventTable> fifoTables;
        private IDictionary<String, Queue<Event>> pendingDeliveryBuffer;

        public EventOrderManager() {
            brokerQueue = new ConcurrentQueue<EventContainer>();
            subscriberQueue = new ConcurrentQueue<Event>();
            fifoTables = new Dictionary<ProcessHeader, EventTable>();
            pendingDeliveryBuffer = new Dictionary<String, Queue<Event>>();
        }

        public void EnqueueEvent(EventContainer eventContainer) {
            //Console.WriteLine("EnqueueEvent Thread: " + Thread.CurrentThread.ManagedThreadId + "\n");
            if (Ordering == OrderingType.NO_ORDER) {
                lock (brokerQueue) {
                    brokerQueue.Enqueue(eventContainer);
                    subscriberQueue.Enqueue(eventContainer.Event);
                }
            }

            else if (Ordering == OrderingType.FIFO) {
                //if publisher is unknown, add it
                ProcessHeader publisher = eventContainer.Event.PublisherHeader;
                EventTable eventTable;
                lock (fifoTables) {
                    if (!fifoTables.TryGetValue(publisher, out eventTable)) {
                        eventTable = new EventTable(ref brokerQueue, ref subscriberQueue);
                        fifoTables.Add(publisher, eventTable);
                    }
                }
                eventTable.AddEvent(eventContainer);
            }
        }

        public EventContainer GetNextBrokerEvent() {
            EventContainer eventContainer;
            while (!brokerQueue.TryDequeue(out eventContainer)) {
                Thread.Sleep(10);
            }
            return eventContainer;
        }
        public Event GetNextSubscriberEvent() {
            Event @event;
            while (!subscriberQueue.TryDequeue(out @event)) {
                Thread.Sleep(10);
            }
            return @event;
        }

        public bool TrySetPendingEvent(ProcessHeader subscriber, Event @event) {
            if (Ordering == OrderingType.NO_ORDER) { }
            else if (Ordering == OrderingType.FIFO) {
                Queue<Event> pendingDeliveryList;
                lock (pendingDeliveryBuffer) {
                    if (!pendingDeliveryBuffer.TryGetValue(subscriber + @event.PublisherHeader, out pendingDeliveryList)) {
                        pendingDeliveryList = new Queue<Event>();
                        pendingDeliveryBuffer.Add(subscriber + @event.PublisherHeader, pendingDeliveryList);
                        pendingDeliveryList.Enqueue(@event);
                        return false;
                    }
                    if (pendingDeliveryList.Any()) {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool TryGetPendingEvent(ProcessHeader subscriber, ProcessHeader publisher, out Event @event) {
            if (Ordering == OrderingType.NO_ORDER) { }
            else if (Ordering == OrderingType.FIFO) {
                Queue<Event> pendingDeliveryList;
                lock (pendingDeliveryBuffer) {
                    pendingDeliveryList = pendingDeliveryBuffer[subscriber + publisher];
                    if (pendingDeliveryList.Any()) {
                        pendingDeliveryList.Dequeue();
                        @event = pendingDeliveryList.Peek();
                        return true;
                    }
                }
            }
            @event = null;
            return false;
        }

        private class EventTable {
            private ConcurrentQueue<EventContainer> BrokerQueue { get; set; }
            private ConcurrentQueue<Event> SubscriberQueue { get; set; }
            private IDictionary<int, EventContainer> EventList { get; set; }
            private int CurrentSeqNumber { get; set; }

            public EventTable(ref ConcurrentQueue<EventContainer> brokerQueue, ref ConcurrentQueue<Event> subscriberQueue) {
                BrokerQueue = brokerQueue;
                SubscriberQueue = subscriberQueue;
                CurrentSeqNumber = 0;
                EventList = new Dictionary<int, EventContainer>();
            }

            public void AddEvent(EventContainer eventContainer) {
                int newSeqNumber = eventContainer.NewSeqNumber;
                lock (EventList) {
                    EventList.Add(newSeqNumber, eventContainer);
                    OrderEvents();
                }
            }

            public void OrderEvents() {
                EventContainer eventContainer;
                while (EventList.TryGetValue(CurrentSeqNumber, out eventContainer)) {
                    BrokerQueue.Enqueue(eventContainer);
                    SubscriberQueue.Enqueue(eventContainer.Event);
                    CurrentSeqNumber++;
                }
            }
        }
    }
}