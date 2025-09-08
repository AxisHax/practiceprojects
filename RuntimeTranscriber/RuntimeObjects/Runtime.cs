//	<copyright file="Runtime.cs"  company="Alliant Technologies">
//		Copyright © 2025 Alliant Technologies, LLC. All rights reserved.
//	</copyright>
//	<summary>
//		Class file for Runtime.
//	</summary>
namespace RuntimeTranscriber.RuntimeObjects
{
    public class Runtime
    {
        public Logging Logging { get; set; }
        public string AmqpServer { get; set; }
        public string AmqpVHost { get; set; }
        public string AmqpUsername { get; set; }
        public string AmqpPassword { get; set; }
        public string AmqpExchange { get; set; }
        public string AmqpTableRoutingKeyFormat { get; set; }
        public string AmqpUpdateTimestampRoutingKey { get; set; }
        public string AmqpAlarmRoutingKeyFormat { get; set; }
        public List<ALARMDEVIATION> ALARM_DEVIATIONS { get; set; }
        public List<ALARM> ALARMS { get; set; }
        public List<APPPARAMETER> APP_PARAMETERS { get; set; }
        public List<DATACOLLECTOR> DATACOLLECTORS { get; set; }
        public List<DbReader> DbReader { get; set; }
        public List<DbWriter> DbWriter { get; set; }
        public List<SortMonitor> SortMonitor { get; set; }
        public List<TagMonitor> TagMonitor { get; set; }
        public List<DLCOLUMN> DL_COLUMNS { get; set; }
        public List<DLDEVICETYPE> DL_DEVICE_TYPES { get; set; }
        public List<DLENABLEDOBJECT> DL_ENABLED_OBJECTS { get; set; }
        public List<DLTABLE> DL_TABLES { get; set; }
        public List<DLMEMBER> DL_MEMBERS { get; set; }
        public List<DLOBJECT> DL_OBJECTS { get; set; }
        public List<ENVIRONMENT> ENVIRONMENTS { get; set; }
        public List<object> GFLINK_HEARTBEAT { get; set; }
        public List<object> GLFLINK_DOWNTIME { get; set; }
        public List<SERVER> SERVERS { get; set; }
        public List<IODEVICE> IODEVICES { get; set; }
        public List<IODEVICEADDRESS> IODEVICE_ADDRESSES { get; set; }
        public List<OBJECTTYPE> OBJECT_TYPES { get; set; }
        public List<object> PPH_RATE_TYPES { get; set; }
        public List<object> PPH_TYPE_MEMBERS { get; set; }
        public List<TYPEMEMBER> TYPE_MEMBERS { get; set; }
        public List<INSTANCE> INSTANCES { get; set; }
        public List<MEMBERDEVIATION> MEMBER_DEVIATIONS { get; set; }
        public List<MEMORYRTDB> MEMORY_RTDB { get; set; }
        public List<MEMORYFILEFORMAT> MEMORY_FILE_FORMAT { get; set; }
        public List<PROCESSRTDB> PROCESS_RTDB { get; set; }
        public List<PROCESSORVALUESRTDB> PROCESSOR_VALUES_RTDB { get; set; }
    }
}
