using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMM
{
    public class Task
    {
        public int Id;
        public string Description;
        public int Finished;
        public DateTime DateStarted;
        public DateTime DateFinished;
        public DateTime Deadline;
        public string SerialNumber;

        public int CategoryId;
        public int DeviceId;
        public int UserId;

        public Task() { }

        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public int TaskFinished { get; set; }
        public DateTime TaskDateStarted { get; set; }
        public DateTime TaskDateFinished { get; set; }
        public DateTime TaskDeadline { get; set; } 
        public int TasksID { get; set; }
        public int TaskDeviceId { get; set; }
        public int TaskUserId { get; set; }
        public string TaskSerial { get; set; }
        public int TaskCategoryId { get; set; }
    }
}
