﻿using Anviz.SDK.Responses;

namespace EvoComms.Core.Interfaces
{
    public interface IClockingWriter
    {
        void WriteClockingFile(Record record, string filepath);
        void WriteClockingFile(string userid, string clockingDateTime, string filepath);
    }
}