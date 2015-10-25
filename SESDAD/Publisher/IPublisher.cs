﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SESDAD.Processes;

namespace SESDAD.Processes {
    public interface IPublisher : IProcess {
        void Publish(String topicName, String content);
    }
}